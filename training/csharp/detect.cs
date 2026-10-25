using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenCvSharp;
using OpenCvSharp.Dnn;
using OpenCvSharp.Extensions;

public partial class MainForm : Form
{
    private VideoCapture capture;
    private InferenceSession yoloSession;
    private InferenceSession resnetSession;
    private SerialPort serialPort;
    private Timer timer;

    // Các tham số nhận diện YOLO
    private const float YOLO_CONF_THRESH = 0.35f;
    private const float YOLO_IOU_THRESH = 0.45f;
    private const int YOLO_INPUT_SIZE = 640;

    // Các tham số phân loại ResNet-34
    private const int RESNET_INPUT_SIZE = 224;
    private readonly string[] classNames = {
        "damaged_shield", // Lớp 0: Hỏng nắp chắn bụi (NG)
        "normal",         // Lớp 1: Bình thường (OK)
        "rusted",         // Lớp 2: Gỉ sét (NG)
        "surface_damage"  // Lớp 3: Hư hỏng bề mặt (NG)
    };

    // Chuẩn hóa ImageNet cho ResNet
    private readonly float[] imagenetMean = { 0.485f, 0.456f, 0.406f };
    private readonly float[] imagenetStd = { 0.229f, 0.224f, 0.225f };

    public MainForm()
    {
        InitializeComponent();
        InitModelsAndHardware();
    }

    /// <summary>
    /// Khởi tạo các mô hình ONNX, camera và cổng Serial ESP32
    /// </summary>
    private void InitModelsAndHardware()
    {
        try
        {
            // 1. Nạp 2 mô hình ONNX đã xuất từ Python
            string yoloPath = FindModelPath("yolo_bearing.onnx");
            string resnetPath = FindModelPath("resnet34_bearing.onnx");

            yoloSession = new InferenceSession(yoloPath);
            resnetSession = new InferenceSession(resnetPath);

            // 2. Mở Camera (Index 0 hoặc 1)
            capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            capture.Set(VideoCaptureProperties.FrameWidth, 640);
            capture.Set(VideoCaptureProperties.FrameHeight, 480);
            capture.Set(VideoCaptureProperties.BufferSize, 1);

            if (!capture.IsOpened())
            {
                MessageBox.Show("Không thể mở Camera! Vui lòng kiểm tra kết nối.", "Lỗi Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 3. Kết nối Serial với ESP32 điều khiển Robot 4-DOF (bỏ comment khi cắm phần cứng)
            /*
            serialPort = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
            serialPort.Open();
            */

            // 4. Khởi động Timer quét khung hình (~30 FPS)
            timer = new Timer { Interval = 33 };
            timer.Tick += ProcessFrame;
            timer.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Lỗi khởi tạo hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Tìm file mô hình ONNX trong thư mục hiện tại, thư mục csharp hoặc weights/onnx
    /// </summary>
    private string FindModelPath(string fileName)
    {
        string[] candidates = {
            fileName,
            System.IO.Path.Combine("csharp", fileName),
            System.IO.Path.Combine("weights", "onnx", fileName),
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName),
            System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "csharp", fileName)
        };

        foreach (var path in candidates)
        {
            if (System.IO.File.Exists(path))
                return path;
        }

        throw new System.IO.FileNotFoundException($"Không tìm thấy mô hình ONNX: {fileName}");
    }

    /// <summary>
    /// Vòng lặp xử lý từng khung hình từ Camera
    /// </summary>
    private void ProcessFrame(object sender, EventArgs e)
    {
        using (Mat frame = new Mat())
        {
            if (!capture.Read(frame) || frame.Empty())
                return;

            // ========================================================
            // BƯỚC 1: DÙNG YOLOv8 PHÁT HIỆN TẤT CẢ VÒNG BI TRÊN KHUNG HÌNH
            // ========================================================
            List<Rect> bearingBoxes = DetectBearings(frame);

            foreach (var bbox in bearingBoxes)
            {
                if (bbox.Width < 16 || bbox.Height < 16)
                    continue;

                // Thêm 5% viền đệm (padding margin) quanh bounding box để lấy trọn vẹn viền vòng bi
                int padX = (int)(bbox.Width * 0.05);
                int padY = (int)(bbox.Height * 0.05);
                int cropX = Math.Max(0, bbox.X - padX);
                int cropY = Math.Max(0, bbox.Y - padY);
                int cropW = Math.Min(frame.Width - cropX, bbox.Width + padX * 2);
                int cropH = Math.Min(frame.Height - cropY, bbox.Height + padY * 2);

                using (Mat crop = new Mat(frame, new Rect(cropX, cropY, cropW, cropH)))
                {
                    // ========================================================
                    // BƯỚC 2: DÙNG RESNET-34 PHÂN LOẠI CHẤT LƯỢNG (OK / NG)
                    // ========================================================
                    var (label, conf) = ClassifyBearingCrop(crop);
                    bool isOk = (label == "normal");

                    // ========================================================
                    // BƯỚC 3: HIỂN THỊ TRỰC QUAN TRÊN GIAO DIỆN
                    // ========================================================
                    Scalar boxColor = isOk ? Scalar.Green : Scalar.Red;
                    string statusText = isOk 
                        ? $"[OK] DAT ({conf:P0})" 
                        : $"[NG] {label.ToUpper()} ({conf:P0})";

                    // Vẽ khung Bounding Box và nhãn tình trạng
                    Cv2.Rectangle(frame, bbox, boxColor, 2);
                    Cv2.PutText(
                        frame,
                        statusText,
                        new OpenCvSharp.Point(bbox.X, Math.Max(25, bbox.Y - 10)),
                        HersheyFonts.HersheySimplex,
                        0.6,
                        boxColor,
                        2
                    );

                    // Vẽ tâm vòng bi
                    int centerX = bbox.X + bbox.Width / 2;
                    int centerY = bbox.Y + bbox.Height / 2;
                    Cv2.Circle(frame, new OpenCvSharp.Point(centerX, centerY), 4, boxColor, -1);

                    // ========================================================
                    // BƯỚC 4: GỬI LỆNH ĐIỀU KHIỂN TỚI ROBOT 4-DOF (ESP32)
                    // ========================================================
                    if (serialPort != null && serialPort.IsOpen)
                    {
                        // Định dạng chuỗi: CMD:OK,X,Y hoặc CMD:NG,X,Y
                        string cmd = $"CMD:{(isOk ? "OK" : "NG")},{centerX},{centerY}\n";
                        serialPort.WriteLine(cmd);
                    }
                }
            }

            // Hiển thị frame hoàn chỉnh lên PictureBox của WinForms
            pictureBoxCam.Image?.Dispose();
            pictureBoxCam.Image = BitmapConverter.ToBitmap(frame);
        }
    }

    /// <summary>
    /// Hàm 1: Phát hiện vị trí các vòng bi (YOLOv8 Detection với Letterbox và NMS)
    /// </summary>
    private List<Rect> DetectBearings(Mat frame)
    {
        var resultBoxes = new List<Rect>();
        int origW = frame.Width;
        int origH = frame.Height;

        // 1. Chuẩn bị ảnh Letterbox (640x640) giữ nguyên tỷ lệ tránh méo hình tròn của vòng bi
        float scale = Math.Min((float)YOLO_INPUT_SIZE / origW, (float)YOLO_INPUT_SIZE / origH);
        int newW = (int)(origW * scale);
        int newH = (int)(origH * scale);
        int padX = (YOLO_INPUT_SIZE - newW) / 2;
        int padY = (YOLO_INPUT_SIZE - newH) / 2;

        using (Mat resized = new Mat())
        using (Mat letterbox = new Mat(new OpenCvSharp.Size(YOLO_INPUT_SIZE, YOLO_INPUT_SIZE), MatType.CV_8UC3, new Scalar(114, 114, 114)))
        using (Mat rgb = new Mat())
        {
            Cv2.Resize(frame, resized, new OpenCvSharp.Size(newW, newH));
            Mat roi = new Mat(letterbox, new Rect(padX, padY, newW, newH));
            resized.CopyTo(roi);
            Cv2.CvtColor(letterbox, rgb, ColorConversionCodes.BGR2RGB);

            // 2. Chuyển sang Tensor [1, 3, 640, 640] chuẩn hóa 0.0 -> 1.0
            var inputTensor = new DenseTensor<float>(new[] { 1, 3, YOLO_INPUT_SIZE, YOLO_INPUT_SIZE });
            for (int y = 0; y < YOLO_INPUT_SIZE; y++)
            {
                for (int x = 0; x < YOLO_INPUT_SIZE; x++)
                {
                    Vec3b pixel = rgb.At<Vec3b>(y, x);
                    inputTensor[0, 0, y, x] = pixel.Item0 / 255.0f;
                    inputTensor[0, 1, y, x] = pixel.Item1 / 255.0f;
                    inputTensor[0, 2, y, x] = pixel.Item2 / 255.0f;
                }
            }

            // 3. Thực thi YOLO Inference
            var inputs = new[] { NamedOnnxValue.CreateFromTensor("images", inputTensor) };
            using (var results = yoloSession.Run(inputs))
            {
                var outputTensor = results.First().AsTensor<float>();
                // Output shape của YOLOv8 1 class: [1, 5, 8400]
                // 5 dòng: cx, cy, w, h, conf
                int dimensions = outputTensor.Dimensions[1]; // thường là 5 (4 bbox coords + 1 class conf)
                int numAnchors = outputTensor.Dimensions[2];  // 8400

                var boxes = new List<Rect2d>();
                var confidences = new List<float>();

                for (int i = 0; i < numAnchors; i++)
                {
                    // Lấy confidence của class 'bearing' (dòng thứ 4)
                    float conf = outputTensor[0, 4, i];
                    if (conf >= YOLO_CONF_THRESH)
                    {
                        float cx = outputTensor[0, 0, i];
                        float cy = outputTensor[0, 1, i];
                        float w = outputTensor[0, 2, i];
                        float h = outputTensor[0, 3, i];

                        // Quy đổi tọa độ từ Letterbox 640x640 về khung hình gốc
                        float x1 = (cx - w / 2 - padX) / scale;
                        float y1 = (cy - h / 2 - padY) / scale;
                        float boxW = w / scale;
                        float boxH = h / scale;

                        // Giới hạn tọa độ trong kích thước frame gốc
                        x1 = Math.Max(0, Math.Min(x1, origW - 1));
                        y1 = Math.Max(0, Math.Min(y1, origH - 1));
                        boxW = Math.Min(boxW, origW - x1);
                        boxH = Math.Min(boxH, origH - y1);

                        boxes.Add(new Rect2d(x1, y1, boxW, boxH));
                        confidences.Add(conf);
                    }
                }

                // 4. Áp dụng Non-Maximum Suppression (NMS) lọc bớt các box trùng lặp
                if (boxes.Count > 0)
                {
                    CvDnn.NMSBoxes(boxes, confidences, YOLO_CONF_THRESH, YOLO_IOU_THRESH, out int[] indices);
                    foreach (int idx in indices)
                    {
                        var b = boxes[idx];
                        resultBoxes.Add(new Rect((int)b.X, (int)b.Y, (int)b.Width, (int)b.Height));
                    }
                }
            }
        }

        return resultBoxes;
    }

    /// <summary>
    /// Hàm 2: Phân loại chất lượng vòng bi (ResNet-34 Classification)
    /// </summary>
    private (string label, float conf) ClassifyBearingCrop(Mat crop)
    {
        using (Mat rgb = new Mat())
        using (Mat resized = new Mat())
        {
            // 1. Chuyển sang RGB và Resize về 224x224
            Cv2.CvtColor(crop, rgb, ColorConversionCodes.BGR2RGB);
            Cv2.Resize(rgb, resized, new OpenCvSharp.Size(RESNET_INPUT_SIZE, RESNET_INPUT_SIZE));

            // 2. Chuẩn hóa ImageNet: (pixel / 255.0 - mean) / std
            var inputTensor = new DenseTensor<float>(new[] { 1, 3, RESNET_INPUT_SIZE, RESNET_INPUT_SIZE });
            for (int y = 0; y < RESNET_INPUT_SIZE; y++)
            {
                for (int x = 0; x < RESNET_INPUT_SIZE; x++)
                {
                    Vec3b pixel = resized.At<Vec3b>(y, x);
                    inputTensor[0, 0, y, x] = (pixel.Item0 / 255.0f - imagenetMean[0]) / imagenetStd[0];
                    inputTensor[0, 1, y, x] = (pixel.Item1 / 255.0f - imagenetMean[1]) / imagenetStd[1];
                    inputTensor[0, 2, y, x] = (pixel.Item2 / 255.0f - imagenetMean[2]) / imagenetStd[2];
                }
            }

            // 3. Inference qua resnet34_bearing.onnx
            var inputs = new[] { NamedOnnxValue.CreateFromTensor("input", inputTensor) };
            using (var results = resnetSession.Run(inputs))
            {
                var logits = results.First().AsEnumerable<float>().ToArray();

                // 4. Tính hàm Softmax để quy đổi Logits sang xác suất (%)
                float maxVal = logits.Max();
                float[] exp = logits.Select(v => (float)Math.Exp(v - maxVal)).ToArray();
                float sum = exp.Sum();
                float[] probs = exp.Select(v => v / sum).ToArray();

                // Tìm class có xác suất lớn nhất
                int bestIdx = 0;
                float bestProb = probs[0];
                for (int i = 1; i < probs.Length; i++)
                {
                    if (probs[i] > bestProb)
                    {
                        bestProb = probs[i];
                        bestIdx = i;
                    }
                }

                return (classNames[bestIdx], bestProb);
            }
        }
    }

    /// <summary>
    /// Giải phóng tài nguyên khi đóng Form
    /// </summary>
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        timer?.Stop();
        capture?.Release();
        yoloSession?.Dispose();
        resnetSession?.Dispose();
        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
        base.OnFormClosing(e);
    }
}
