using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

using WinFormTimer = System.Windows.Forms.Timer;

namespace DashBoardBearing
{
    // Kết quả của từng vòng bi
    public class BearingResult
    {
        public Rect Box { get; set; }
        public string Label { get; set; }
        public float Confidence { get; set; }
        public bool IsOk { get; set; }

        // Ảnh crop của vòng bi, đã vẽ bbox + nhãn (DetectionResult.Dispose sẽ dọn).
        public Bitmap Image { get; set; }
    }

    // Frame live gửi cho form (chưa detect)
    public class FrameReadyEventArgs : EventArgs
    {
        // Bitmap MỚI cho mỗi frame -> form nhận phải Dispose ảnh cũ.
        public Bitmap Frame { get; set; }
    }

    // Kết quả của 1 lần detect (do DetectAsync trả về)
    public class DetectionResult : IDisposable
    {
        public List<BearingResult> Results { get; set; }

        /// <summary>
        /// Ảnh crop (có bbox + nhãn) của vòng bi gần tâm ảnh nhất.
        /// Null nếu không phát hiện vòng bi nào. Ảnh thuộc về DetectionResult.
        /// </summary>
        public Bitmap Frame =>
            Results != null && Results.Count > 0 ? Results[0].Image : null;
        public double YoloMs { get; set; }
        public double ResnetMs { get; set; }
        public double TotalMs { get; set; }

        /// <summary>True nếu có ít nhất 1 vòng bi và tất cả đều đạt.</summary>
        public bool AllOk =>
            Results != null && Results.Count > 0 && Results.All(r => r.IsOk);

        public void Dispose()
        {
            if (Results == null) return;

            foreach (BearingResult r in Results)
            {
                r.Image?.Dispose();
                r.Image = null;
            }
        }
    }

    public class CameraManager : IDisposable
    {
        // ==================== EVENTS ====================

        /// <summary>Raise trên UI thread mỗi frame (live preview, KHÔNG detect).</summary>
        public event EventHandler<FrameReadyEventArgs> FrameReady;

        /// <summary>Raise khi có lỗi (camera, model, inference).</summary>
        public event EventHandler<string> ErrorOccurred;

        public Action<bool> CameraStatus;

        // ==================== CAMERA ====================

        private VideoCapture videoCapture;
        private readonly WinFormTimer timer;

        private volatile bool stopRequested;
        private bool disposed;

        // Frame mới nhất, dùng làm nguồn cho DetectAsync
        private Mat latestFrame;
        private readonly object frameLock = new object();

        // Detect
        private int detecting;                       // 0 = rảnh, 1 = đang detect
        private Task detectTask = Task.CompletedTask;

        public bool IsRunning { get; private set; }

        /// <summary>True khi đang chạy một lần detect.</summary>
        public bool IsDetecting => Volatile.Read(ref detecting) == 1;

        /// <summary>Index camera đang mở, -1 nếu chưa mở.</summary>
        public int CurrentCameraIndex { get; private set; } = -1;

        /// <summary>URL stream đang mở (camera IP), null nếu đang dùng webcam.</summary>
        public string CurrentUrl { get; private set; }

        /// <summary>True khi đang chạy bằng camera IP.</summary>
        public bool IsIpCamera => IsRunning && CurrentUrl != null;

        private int connecting;                      // chặn gọi StartIpAsync chồng nhau

        /// <summary>
        /// Dò các index camera dùng được (0..maxIndex-1).
        /// Camera đang bị CameraManager chiếm sẽ không xuất hiện trong list,
        /// nên form cần tự thêm CurrentCameraIndex nếu IsRunning.
        /// </summary>
        public static List<int> GetAvailableCameras(int maxIndex = 10)
        {
            var list = new List<int>();

            for (int i = 0; i < maxIndex; i++)
            {
                try
                {
                    using (var cap = new VideoCapture(i, VideoCaptureAPIs.DSHOW))
                    {
                        if (cap.IsOpened())
                            list.Add(i);
                    }
                }
                catch { /* bỏ qua index lỗi */ }
            }

            return list;
        }

        // ==================== MODEL ====================

        private InferenceSession yolo;
        private InferenceSession resnet;

        private const int YOLO_INPUT_SIZE = 640;
        private const float YOLO_CONF_THRESH = 0.35f;
        private const float YOLO_IOU_THRESH = 0.45f;
        private const int RESNET_INPUT_SIZE = 224;

        // Kích thước cạnh dài của ảnh crop khi hiển thị + thanh nhãn phía trên
        private const int DISPLAY_SIZE = 360;
        private const int LABEL_BAR_HEIGHT = 28;

        private readonly string[] classNames =
        {
            "damaged_shield", "normal", "rusted", "surface_damage"
        };

        private readonly float[] imagenetMean = { 0.485f, 0.456f, 0.406f };
        private readonly float[] imagenetStd = { 0.229f, 0.224f, 0.225f };

        // =====================================================
        // KHỞI TẠO
        // =====================================================

        public CameraManager(string yoloPath, string resnetPath)
        {
            InitModels(yoloPath, resnetPath);

            timer = new WinFormTimer();
            timer.Interval = 10;
            timer.Tick += Timer_Tick;
        }

        private void InitModels(string yoloPath, string resnetPath)
        {
            var options = new SessionOptions
            {
                GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
                ExecutionMode = ExecutionMode.ORT_SEQUENTIAL,
                IntraOpNumThreads = 4,
                InterOpNumThreads = 1
            };

            try
            {
                yolo = new InferenceSession(yoloPath, options);
                resnet = new InferenceSession(resnetPath, options);
            }
            catch
            {
                yolo?.Dispose();
                resnet?.Dispose();
                throw;
            }
            finally
            {
                options.Dispose();
            }

            Debug.WriteLine("YOLO input: " + string.Join(", ", yolo.InputNames));
            Debug.WriteLine("ResNet input: " + string.Join(", ", resnet.InputNames));
        }

        // =====================================================
        // START / STOP
        // =====================================================

        /// <summary>Mở webcam theo index và bắt đầu live preview. Trả về false nếu lỗi.</summary>
        public bool Start(int idx)
        {
            if (disposed) throw new ObjectDisposedException(nameof(CameraManager));

            // Đóng camera cũ nếu đang chạy.
            StopInternal();

            try
            {
                // DSHOW: giống AForge, thấy được webcam ảo như DroidCam.
                // (Backend mặc định MSMF thường không thấy thiết bị ảo này.)
                var cap = new VideoCapture(idx, VideoCaptureAPIs.DSHOW);

                if (!cap.IsOpened())
                {
                    cap.Dispose();
                    throw new InvalidOperationException("Không mở được camera " + idx);
                }

                cap.Set(VideoCaptureProperties.FrameWidth, 640);
                cap.Set(VideoCaptureProperties.FrameHeight, 480);

                BeginCapture(cap, idx, null);
                return true;
            }
            catch (Exception ex)
            {
                IsRunning = false;
                ErrorOccurred?.Invoke(this, "Khởi tạo camera không thành công: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Kết nối camera qua mạng (DroidCam, IP camera MJPEG...).
        /// ipOrUrl: "192.168.1.5" (tự thành http://192.168.1.5:port/video)
        /// hoặc URL đầy đủ như "http://192.168.1.5:4747/video".
        /// Mở kết nối chạy nền nên UI không bị đứng; quá timeoutMs thì báo lỗi.
        /// </summary>
        public async Task<bool> StartIpAsync(string ipOrUrl, int port = 4747, int timeoutMs = 8000)
        {
            if (disposed) throw new ObjectDisposedException(nameof(CameraManager));

            if (string.IsNullOrWhiteSpace(ipOrUrl))
            {
                ErrorOccurred?.Invoke(this, "Chưa nhập địa chỉ IP.");
                return false;
            }

            // Đang kết nối dở thì bỏ qua lần gọi này.
            if (Interlocked.Exchange(ref connecting, 1) == 1)
                return false;

            string url = BuildStreamUrl(ipOrUrl.Trim(), port);

            // Đóng camera cũ nếu đang chạy.
            StopInternal();

            try
            {
                // new VideoCapture(url) có thể treo vài giây (thậm chí lâu hơn)
                // nếu IP sai -> mở ở worker thread + giới hạn thời gian chờ.
                Task<VideoCapture> openTask =
                    Task.Run(() => new VideoCapture(url, VideoCaptureAPIs.FFMPEG));

                Task finished = await Task.WhenAny(openTask, Task.Delay(timeoutMs));

                if (finished != openTask)
                {
                    // Nếu về sau mở được thì tự dọn, tránh giữ kết nối mồ côi.
                    _ = openTask.ContinueWith(
                        t => { if (t.Status == TaskStatus.RanToCompletion) t.Result.Dispose(); },
                        TaskScheduler.Default);

                    throw new TimeoutException(
                        "Hết thời gian chờ kết nối " + url);
                }

                VideoCapture cap = await openTask;

                if (!cap.IsOpened())
                {
                    cap.Dispose();
                    throw new InvalidOperationException("Không mở được stream " + url);
                }

                // Form/CameraManager đã đóng trong lúc chờ.
                if (disposed)
                {
                    cap.Dispose();
                    return false;
                }

                BeginCapture(cap, -1, url);
                return true;
            }
            catch (Exception ex)
            {
                IsRunning = false;
                ErrorOccurred?.Invoke(this, "Kết nối camera IP không thành công: " + ex.Message);
                return false;
            }
            finally
            {
                Interlocked.Exchange(ref connecting, 0);
            }
        }

        private static string BuildStreamUrl(string ipOrUrl, int port)
        {
            // Đã là URL đầy đủ (http://, https://, rtsp://...) -> giữ nguyên.
            if (ipOrUrl.Contains("://"))
                return ipOrUrl;

            // "192.168.1.5" hoặc "192.168.1.5:4747" -> thêm giao thức + đường dẫn.
            string hostPort = ipOrUrl.Contains(":") ? ipOrUrl : ipOrUrl + ":" + port;
            return "http://" + hostPort + "/video";
        }

        // Nhận VideoCapture đã mở thành công và bật live preview.
        private void BeginCapture(VideoCapture cap, int index, string url)
        {
            videoCapture = cap;

            stopRequested = false;
            IsRunning = true;
            CurrentCameraIndex = index;
            CurrentUrl = url;

            timer.Start();
            CameraStatus?.Invoke(true);
        }

        /// <summary>
        /// Dừng camera. Detect đang chạy (nếu có) làm việc trên bản sao frame
        /// nên không bị ảnh hưởng.
        /// </summary>
        public Task StopAsync()
        {
            bool wasRunning = IsRunning;

            StopInternal();

            if (wasRunning)
                CameraStatus?.Invoke(false);

            return Task.CompletedTask;
        }

        private void StopInternal()
        {
            stopRequested = true;
            timer.Stop();
            ReleaseCamera();
        }

        private void ReleaseCamera()
        {
            if (videoCapture != null)
            {
                videoCapture.Release();
                videoCapture.Dispose();
                videoCapture = null;
            }

            lock (frameLock)
            {
                latestFrame?.Dispose();
                latestFrame = null;
            }

            IsRunning = false;
            CurrentCameraIndex = -1;
            CurrentUrl = null;
        }

        // =====================================================
        // LIVE PREVIEW: chỉ đọc frame, KHÔNG inference
        // =====================================================

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (stopRequested || videoCapture == null)
                return;

            Mat frame = new Mat();

            try
            {
                if (!videoCapture.Read(frame) || frame.Empty())
                    return;

                Bitmap bmp = frame.ToBitmap();

                // Chuyển quyền sở hữu Mat sang latestFrame (không cần Clone).
                lock (frameLock)
                {
                    latestFrame?.Dispose();
                    latestFrame = frame;
                }
                frame = null;

                var handler = FrameReady;
                if (handler != null)
                    handler(this, new FrameReadyEventArgs { Frame = bmp });
                else
                    bmp.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Frame error: " + ex);
                ErrorOccurred?.Invoke(this, "Lỗi đọc frame: " + ex.Message);
            }
            finally
            {
                frame?.Dispose();
            }
        }

        // =====================================================
        // DETECT THEO YÊU CẦU
        // =====================================================

        /// <summary>
        /// Chụp frame mới nhất rồi chạy YOLO + ResNet trên worker thread.
        /// Trả về null nếu camera chưa chạy, chưa có frame, đang có lần detect khác
        /// hoặc bị lỗi. Nếu chạy được nhưng không thấy vòng bi thì Results rỗng
        /// và Frame = null. Người gọi phải Dispose kết quả.
        /// Live preview vẫn chạy bình thường trong lúc detect.
        /// </summary>
        public async Task<DetectionResult> DetectAsync()
        {
            if (disposed) throw new ObjectDisposedException(nameof(CameraManager));

            if (!IsRunning)
                return null;

            // Chỉ cho 1 lần detect tại một thời điểm.
            if (Interlocked.Exchange(ref detecting, 1) == 1)
                return null;

            Mat snapshot = null;

            try
            {
                lock (frameLock)
                {
                    if (latestFrame == null || latestFrame.Empty())
                        return null;

                    snapshot = latestFrame.Clone();
                }

                Mat work = snapshot;
                Task<DetectionResult> task = Task.Run(() => ProcessFrame(work));
                detectTask = task;

                return await task;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Detect error: " + ex);
                ErrorOccurred?.Invoke(this, "Lỗi detect: " + ex.Message);
                return null;
            }
            finally
            {
                snapshot?.Dispose();
                detectTask = Task.CompletedTask;
                Interlocked.Exchange(ref detecting, 0);
            }
        }

        // =====================================================
        // XỬ LÝ 1 FRAME (chạy trên worker thread, vẽ lên bản sao)
        // =====================================================

        private DetectionResult ProcessFrame(Mat frame)
        {
            Stopwatch totalWatch = Stopwatch.StartNew();
            Stopwatch yoloWatch = Stopwatch.StartNew();

            List<Rect> boxes = DetectBearings(frame);
            boxes = FilterBoundingBoxes(boxes, frame.Width, frame.Height);
            yoloWatch.Stop();

            var results = new List<BearingResult>();
            double totalResnetMs = 0;

            try
            {
                foreach (Rect bbox in boxes)
                {
                    if (bbox.Width < 16 || bbox.Height < 16)
                        continue;

                    int padX = (int)(bbox.Width * 0.05);
                    int padY = (int)(bbox.Height * 0.05);

                    int cropX = Math.Max(0, bbox.X - padX);
                    int cropY = Math.Max(0, bbox.Y - padY);
                    int cropRight = Math.Min(frame.Width, bbox.X + bbox.Width + padX);
                    int cropBottom = Math.Min(frame.Height, bbox.Y + bbox.Height + padY);

                    int cropW = cropRight - cropX;
                    int cropH = cropBottom - cropY;
                    if (cropW <= 0 || cropH <= 0)
                        continue;

                    using (Mat crop = new Mat(frame, new Rect(cropX, cropY, cropW, cropH)))
                    {
                        Stopwatch rw = Stopwatch.StartNew();
                        var (label, confidence) = ClassifyBearingCrop(crop);
                        rw.Stop();
                        totalResnetMs += rw.Elapsed.TotalMilliseconds;

                        bool isOk = label == "normal";
                        Scalar color = isOk ? Scalar.Green : Scalar.Red;

                        string status = isOk
                            ? $"[OK] DAT ({confidence:P0})"
                            : $"[NG] {label.ToUpperInvariant()} ({confidence:P0})";

                        // Vẽ SAU khi đã classify (crop là view trên frame).
                        Bitmap image = RenderCrop(crop, bbox, cropX, cropY, status, color);

                        results.Add(new BearingResult
                        {
                            Box = bbox,
                            Label = label,
                            Confidence = confidence,
                            IsOk = isOk,
                            Image = image
                        });
                    }
                }
            }
            catch
            {
                // Lỗi giữa chừng -> dọn các ảnh đã tạo.
                foreach (BearingResult r in results)
                    r.Image?.Dispose();
                throw;
            }

            totalWatch.Stop();

            return new DetectionResult
            {
                Results = results,
                YoloMs = yoloWatch.Elapsed.TotalMilliseconds,
                ResnetMs = totalResnetMs,
                TotalMs = totalWatch.Elapsed.TotalMilliseconds
            };
        }

        // Phóng/thu ảnh crop về DISPLAY_SIZE, vẽ bbox, thêm thanh nhãn phía trên.
        private Bitmap RenderCrop(
            Mat crop, Rect bbox, int cropX, int cropY,
            string status, Scalar color)
        {
            double s = (double)DISPLAY_SIZE / Math.Max(crop.Width, crop.Height);

            int w = Math.Max(1, (int)Math.Round(crop.Width * s));
            int h = Math.Max(1, (int)Math.Round(crop.Height * s));

            using (Mat view = new Mat())
            using (Mat withBar = new Mat())
            {
                Cv2.Resize(crop, view, new OpenCvSharp.Size(w, h));

                // bbox đổi sang tọa độ của ảnh crop đã scale
                var box = new Rect(
                    (int)Math.Round((bbox.X - cropX) * s),
                    (int)Math.Round((bbox.Y - cropY) * s),
                    (int)Math.Round(bbox.Width * s),
                    (int)Math.Round(bbox.Height * s));

                Cv2.Rectangle(view, box, color, 2);

                Cv2.CopyMakeBorder(
                    view, withBar,
                    LABEL_BAR_HEIGHT, 0, 0, 0,
                    BorderTypes.Constant, Scalar.Black);

                Cv2.PutText(
                    withBar, status,
                    new OpenCvSharp.Point(5, 20),
                    HersheyFonts.HersheySimplex, 0.55, color, 2);

                return withBar.ToBitmap();
            }
        }

        // =====================================================
        // TIỀN XỬ LÝ TENSOR
        // =====================================================

        private static byte[] MatToBytes(Mat rgb, int pixelCount)
        {
            byte[] pixels = new byte[pixelCount * 3];

            if (!rgb.IsContinuous())
            {
                using (Mat continuous = rgb.Clone())
                    Marshal.Copy(continuous.Data, pixels, 0, pixels.Length);
            }
            else
            {
                Marshal.Copy(rgb.Data, pixels, 0, pixels.Length);
            }
            return pixels;
        }

        private DenseTensor<float> CreateYoloTensor(Mat rgb)
        {
            int size = YOLO_INPUT_SIZE;
            int pixelCount = size * size;
            byte[] pixels = MatToBytes(rgb, pixelCount);

            float[] data = new float[pixelCount * 3];
            int g = pixelCount, b = pixelCount * 2;

            for (int i = 0, j = 0; i < pixelCount; i++, j += 3)
            {
                data[i] = pixels[j] / 255.0f;
                data[g + i] = pixels[j + 1] / 255.0f;
                data[b + i] = pixels[j + 2] / 255.0f;
            }

            return new DenseTensor<float>(data, new[] { 1, 3, size, size });
        }

        private DenseTensor<float> CreateResnetTensor(Mat rgb)
        {
            int size = RESNET_INPUT_SIZE;
            int pixelCount = size * size;
            byte[] pixels = MatToBytes(rgb, pixelCount);

            float[] data = new float[pixelCount * 3];
            int g = pixelCount, b = pixelCount * 2;

            for (int i = 0, j = 0; i < pixelCount; i++, j += 3)
            {
                data[i] = (pixels[j] / 255.0f - imagenetMean[0]) / imagenetStd[0];
                data[g + i] = (pixels[j + 1] / 255.0f - imagenetMean[1]) / imagenetStd[1];
                data[b + i] = (pixels[j + 2] / 255.0f - imagenetMean[2]) / imagenetStd[2];
            }

            return new DenseTensor<float>(data, new[] { 1, 3, size, size });
        }

        // =====================================================
        // YOLOV8 DETECTION
        // =====================================================

        private List<Rect> DetectBearings(Mat frame)
        {
            var resultBoxes = new List<Rect>();

            int origW = frame.Width;
            int origH = frame.Height;

            float scale = Math.Min((float)YOLO_INPUT_SIZE / origW, (float)YOLO_INPUT_SIZE / origH);
            int newW = (int)(origW * scale);
            int newH = (int)(origH * scale);
            int padX = (YOLO_INPUT_SIZE - newW) / 2;
            int padY = (YOLO_INPUT_SIZE - newH) / 2;

            using (Mat resized = new Mat())
            using (Mat letterbox = new Mat(
                new OpenCvSharp.Size(YOLO_INPUT_SIZE, YOLO_INPUT_SIZE),
                MatType.CV_8UC3, new Scalar(114, 114, 114)))
            using (Mat rgb = new Mat())
            {
                Cv2.Resize(frame, resized, new OpenCvSharp.Size(newW, newH));

                using (Mat roi = new Mat(letterbox, new Rect(padX, padY, newW, newH)))
                    resized.CopyTo(roi);

                Cv2.CvtColor(letterbox, rgb, ColorConversionCodes.BGR2RGB);

                DenseTensor<float> inputTensor = CreateYoloTensor(rgb);
                var inputs = new[] { NamedOnnxValue.CreateFromTensor("images", inputTensor) };

                using (var results = yolo.Run(inputs))
                {
                    var output = results.First().AsTensor<float>();

                    if (output.Dimensions.Length != 3 ||
                        output.Dimensions[0] != 1 ||
                        output.Dimensions[1] != 5)
                    {
                        throw new InvalidOperationException(
                            "YOLO output không phải [1,5,N]. Hãy kiểm tra shape của model ONNX.");
                    }

                    int numAnchors = output.Dimensions[2];
                    var boxes = new List<Rect>();
                    var confidences = new List<float>();

                    for (int i = 0; i < numAnchors; i++)
                    {
                        float conf = output[0, 4, i];
                        if (conf < YOLO_CONF_THRESH)
                            continue;

                        float cx = output[0, 0, i];
                        float cy = output[0, 1, i];
                        float w = output[0, 2, i];
                        float h = output[0, 3, i];

                        float left = (cx - w / 2 - padX) / scale;
                        float top = (cy - h / 2 - padY) / scale;
                        float right = (cx + w / 2 - padX) / scale;
                        float bottom = (cy + h / 2 - padY) / scale;

                        int x1 = Math.Max(0, Math.Min((int)Math.Floor(left), origW));
                        int y1 = Math.Max(0, Math.Min((int)Math.Floor(top), origH));
                        int x2 = Math.Max(0, Math.Min((int)Math.Ceiling(right), origW));
                        int y2 = Math.Max(0, Math.Min((int)Math.Ceiling(bottom), origH));

                        if (x2 - x1 <= 0 || y2 - y1 <= 0)
                            continue;

                        boxes.Add(new Rect(x1, y1, x2 - x1, y2 - y1));
                        confidences.Add(conf);
                    }

                    if (boxes.Count > 0)
                    {
                        CvDnn.NMSBoxes(boxes, confidences,
                            YOLO_CONF_THRESH, YOLO_IOU_THRESH, out int[] indices);

                        foreach (int index in indices)
                            resultBoxes.Add(boxes[index]);
                    }
                }
            }

            return resultBoxes;
        }

        // =====================================================
        // RESNET-34 CLASSIFICATION
        // =====================================================

        private (string label, float conf) ClassifyBearingCrop(Mat crop)
        {
            using (Mat rgb = new Mat())
            using (Mat resized = new Mat())
            {
                Cv2.CvtColor(crop, rgb, ColorConversionCodes.BGR2RGB);
                Cv2.Resize(rgb, resized,
                    new OpenCvSharp.Size(RESNET_INPUT_SIZE, RESNET_INPUT_SIZE));

                DenseTensor<float> inputTensor = CreateResnetTensor(resized);
                var inputs = new[] { NamedOnnxValue.CreateFromTensor("input", inputTensor) };

                using (var results = resnet.Run(inputs))
                {
                    float[] logits = results.First().AsEnumerable<float>().ToArray();

                    if (logits.Length != classNames.Length)
                    {
                        throw new InvalidOperationException(
                            $"ResNet trả về {logits.Length} logits, " +
                            $"nhưng đang có {classNames.Length} class.");
                    }

                    float maxVal = logits.Max();
                    float[] exp = logits.Select(v => (float)Math.Exp(v - maxVal)).ToArray();
                    float sum = exp.Sum();

                    int bestIdx = 0;
                    float bestProb = -1;

                    for (int i = 0; i < exp.Length; i++)
                    {
                        float p = exp[i] / sum;
                        if (p > bestProb) { bestProb = p; bestIdx = i; }
                    }

                    return (classNames[bestIdx], bestProb);
                }
            }
        }

        // =====================================================
        // LỌC BOUNDING BOX
        // =====================================================

        private List<Rect> FilterBoundingBoxes(List<Rect> boxes, int imageWidth, int imageHeight)
        {
            if (boxes == null || boxes.Count == 0)
                return new List<Rect>();

            float imageCenterX = imageWidth / 2.0f;
            float imageCenterY = imageHeight / 2.0f;

            int marginX = (int)(imageWidth * 0.03f);
            int marginY = (int)(imageHeight * 0.03f);

            float maxDistance = 0.35f * (float)Math.Sqrt(
                imageWidth * imageWidth + imageHeight * imageHeight);

            var filtered = new List<Rect>();

            foreach (Rect box in boxes)
            {
                // 1. Loại bbox sát rìa ảnh
                if (box.X < marginX || box.Y < marginY ||
                    box.X + box.Width > imageWidth - marginX ||
                    box.Y + box.Height > imageHeight - marginY)
                    continue;

                // 2. Loại bbox quá lớn (>= 1/3 ảnh)
                //if (box.Width >= imageWidth / 3.0f || box.Height >= imageHeight / 3.0f)
                //    continue;

                // 3. Chỉ giữ bbox trong vùng trung tâm
                float dx = box.X + box.Width / 2.0f - imageCenterX;
                float dy = box.Y + box.Height / 2.0f - imageCenterY;

                if (Math.Sqrt(dx * dx + dy * dy) > maxDistance)
                    continue;

                filtered.Add(box);
            }

            // Ưu tiên bbox gần tâm ảnh nhất
            return filtered.OrderBy(box =>
            {
                float dx = box.X + box.Width / 2.0f - imageCenterX;
                float dy = box.Y + box.Height / 2.0f - imageCenterY;
                return dx * dx + dy * dy;
            }).ToList();
        }

        // =====================================================
        // DISPOSE
        // =====================================================

        public void Dispose()
        {
            if (disposed) return;
            disposed = true;

            stopRequested = true;
            timer.Stop();
            timer.Dispose();
            FrameReady = null;

            // Nếu đang detect, đợi xong rồi mới giải phóng ONNX session
            // (tránh crash do Dispose khi yolo.Run đang chạy).
            detectTask.ContinueWith(_ => ReleaseAll(), TaskScheduler.Default);
        }

        private void ReleaseAll()
        {
            videoCapture?.Release();
            videoCapture?.Dispose();
            videoCapture = null;

            lock (frameLock)
            {
                latestFrame?.Dispose();
                latestFrame = null;
            }

            yolo?.Dispose();
            resnet?.Dispose();
            yolo = null;
            resnet = null;
        }
    }
}