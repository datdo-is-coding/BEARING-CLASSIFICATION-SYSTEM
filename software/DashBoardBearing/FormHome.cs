using System;



using System.Collections.Generic;



using System.Drawing;



using System.Threading.Tasks;



using System.Windows.Forms;



using RobotView;



using FontAwesome.Sharp;



namespace DashBoardBearing



{



    public partial class FormHome : Form



    {



        private readonly CameraManager _cam;

        private readonly SerialManager? _serial;



        // Global system running status for header synchronization



        public static bool IsSystemRunning { get; private set; } = false;



        public static event Action<bool>? SystemRunStatusChanged;



        private bool _isSystemRunning = false;



        private bool _isDetecting = false;



        private readonly System.Windows.Forms.Timer _autoTimer;



        // Statistics counters



        private int _totalCount = 0;



        private int _okCount = 0;



        private int _ngCount = 0;



        private bool? _lastResultOk = null; // null: ready, true: OK, false: NG



        private string _lastStatusMsg = "SẴN SÀNG";



        // Tên class của model -> tên hiển thị



        private static readonly Dictionary<string, (string DisplayName, Color TextColor)> ClassInfo =



            new Dictionary<string, (string DisplayName, Color TextColor)>



            {



                { "normal",         ("Bình thường (Đạt)", Color.FromArgb(16, 185, 129)) },



                { "rusted",         ("Gỉ sét bề mặt",     Color.FromArgb(239, 68, 68)) },



                { "damaged_shield", ("Hư nắp che",       Color.FromArgb(245, 158, 11)) },



                { "surface_damage", ("Trầy xước bề mặt", Color.FromArgb(239, 68, 68)) }



            };



        public FormHome(CameraManager cameraManager, SerialManager? serialManager = null, Robot4DOF? robotView = null)



        {



            InitializeComponent();



            _cam = cameraManager;

            _serial = serialManager;



            if (_serial != null)

            {

                _serial.DetectTriggerReceived += OnSerialDetectTrigger;

            }



            _autoTimer = new System.Windows.Forms.Timer();



            _autoTimer.Interval = 500; // Check every 500ms



            _autoTimer.Tick += AutoTimer_Tick;



            if (_cam != null && _cam.IsRunning)



            {



                _cam.FrameReady += Camera_FrameReady;



                UpdateLiveBadge(true);



            }



            else



            {



                UpdateLiveBadge(false);



            }



            UITheme.ThemeChanged += ApplyTheme;



        }



        private void FormHome_Load(object sender, EventArgs e)



        {



            ApplyTheme();



            // Khôi phục thống kê nhận diện hôm nay từ bộ lưu trữ bền vững



            var todayStats = BearingLogManager.GetTodayStats();



            _totalCount = todayStats.Total;



            _okCount = todayStats.Ok;



            _ngCount = todayStats.Ng;



            UpdateStatsUI();



            var todayEntries = BearingLogManager.GetToday();



            if (todayEntries.Count > 0)



            {



                var lastEntry = todayEntries.Last();



                SetStatus(lastEntry.IsOk);



                var info = GetClassInfo(lastEntry.Label);



                lblDefect.Text = info.DisplayName;



                lblDefect.ForeColor = info.TextColor;



                lblConfidence.Text = lastEntry.Confidence.ToString("P1");



                lblTime.Text = $"{lastEntry.ProcessTimeMs:F0} ms";



            }



            else



            {



                ClearResult("SẴN SÀNG");



            }



            _isSystemRunning = IsSystemRunning;



            btnStart.Enabled = !_isSystemRunning;



            btnStop.Enabled = _isSystemRunning;



            if (_isSystemRunning)



            {



                _autoTimer.Start();



            }



            // Visual docking order: Header Top, Log Bottom, Actions above Log, Webcam Fill



            pnlCameraHeader.SendToBack();



            pnlLogContainer.SendToBack();



            pnlCameraActions.SendToBack();



            pnlWebcamContainer.BringToFront();



            panelLeftCard.Resize += PanelLeftCard_Resize;



            AdjustSplitter();



            InitInspectionGrid();



            AppendLog("Hệ thống khởi tạo thành công. Sẵn sàng hoạt động.");



        }



        private void FormHome_Resize(object sender, EventArgs e)



        {



            AdjustSplitter();



        }



        private void PanelLeftCard_Resize(object? sender, EventArgs e)



        {



            try



            {



                if (pnlCameraHeader != null && lblLiveBadge != null)



                {



                    lblLiveBadge.Left = pnlCameraHeader.ClientSize.Width - lblLiveBadge.Width - 4;



                }



                if (pnlCameraActions != null && btnResetStats != null)



                {



                    btnResetStats.Left = pnlCameraActions.ClientSize.Width - btnResetStats.Width;



                }



            }



            catch { }



        }



        private void AdjustSplitter()



        {



            try



            {



                if (splitContainerMain != null && splitContainerMain.Width > 500)



                {



                    int target = (int)(splitContainerMain.Width * 0.55);



                    int minLeft = Math.Max(350, splitContainerMain.Panel1MinSize);



                    int maxLeft = splitContainerMain.Width - Math.Max(300, splitContainerMain.Panel2MinSize);



                    if (target >= minLeft && target <= maxLeft)



                    {



                        splitContainerMain.SplitterDistance = target;



                    }



                }



            }



            catch { }



        }



        private void ApplyTheme()



        {



            BackColor = UITheme.BgDark;



            // Top KPI Cards



            cardTotal.BackgroundColor = UITheme.BgCard;



            cardTotal.BorderColor = UITheme.BorderCard;



            cardOk.BackgroundColor = UITheme.BgCard;



            cardOk.BorderColor = UITheme.BorderCard;



            cardNg.BackgroundColor = UITheme.BgCard;



            cardNg.BorderColor = UITheme.BorderCard;



            cardYield.BackgroundColor = UITheme.BgCard;



            cardYield.BorderColor = UITheme.BorderCard;



            lblStatTotalVal.ForeColor = UITheme.TextPrimary;



            lblStatTotalTitle.ForeColor = UITheme.TextSecondary;



            lblStatOkTitle.ForeColor = UITheme.TextSecondary;



            lblStatNgTitle.ForeColor = UITheme.TextSecondary;



            lblStatYieldTitle.ForeColor = UITheme.TextSecondary;



            // Left & Right Main Cards



            panelLeftCard.BackgroundColor = UITheme.BgCard;



            panelLeftCard.BorderColor = UITheme.BorderCard;



            panelRightCard.BackgroundColor = UITheme.BgCard;



            panelRightCard.BorderColor = UITheme.BorderCard;



            tableTopCards.BackColor = Color.Transparent;



            lblCameraTitle.ForeColor = UITheme.TextPrimary;



            lblLogTitle.ForeColor = UITheme.TextSecondary;



            lblResultTitle.ForeColor = UITheme.TextPrimary;



            pnlWebcamContainer.BackColor = UITheme.BgViewport;



            pnlCameraActions.BackColor = UITheme.BgCard;



            ApplyGridTheme();



            btnResetStats.BackColor = UITheme.BgCardAlt;



            btnResetStats.ForeColor = UITheme.TextSecondary;



            btnResetStats.IconColor = UITheme.TextSecondary;



            btnResetStats.FlatAppearance.BorderColor = UITheme.BorderCard;



            // Metrics Group



            pnlDefectMetric.BackgroundColor = UITheme.BgCardAlt;



            pnlDefectMetric.BorderColor = UITheme.BorderCard;



            labelDefectTitle.ForeColor = UITheme.TextSecondary;



            pnlConfMetric.BackgroundColor = UITheme.BgCardAlt;



            pnlConfMetric.BorderColor = UITheme.BorderCard;



            labelConfTitle.ForeColor = UITheme.TextSecondary;



            pnlTimeMetric.BackgroundColor = UITheme.BgCardAlt;



            pnlTimeMetric.BorderColor = UITheme.BorderCard;



            labelTimeTitle.ForeColor = UITheme.TextSecondary;



            pnlResultCrop.BackgroundColor = UITheme.BgCardAlt;



            pnlResultCrop.BorderColor = UITheme.BorderCard;



            lblResultCropTitle.ForeColor = UITheme.TextSecondary;



            // Refresh Status Banner colors



            if (_lastResultOk == true)



                SetStatus(true);



            else if (_lastResultOk == false)



                SetStatus(false);



            else



                ClearResult(_lastStatusMsg);



            UpdateLiveBadge(_cam != null && _cam.IsRunning);



        }



        private void UpdateLiveBadge(bool isLive)



        {



            if (isLive)



            {



                lblLiveBadge.Text = "● LIVE STREAM";



                lblLiveBadge.ForeColor = UITheme.Success;



                lblLiveBadge.BackColor = UITheme.SuccessBg;



            }



            else



            {



                lblLiveBadge.Text = "● CAMERA TẮT";



                lblLiveBadge.ForeColor = UITheme.Danger;



                lblLiveBadge.BackColor = UITheme.DangerBg;



            }



        }



        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_cam == null || !_cam.IsRunning)
            {
                var choice = MessageBox.Show(
                    "Camera chưa được mở! Bạn có muốn tiếp tục chạy Băng tải và Robot không?",
                    "Cảnh báo Camera",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (choice != DialogResult.Yes)
                    return;
            }

            _isSystemRunning = true;
            IsSystemRunning = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            // Gửi lệnh kích hoạt chu trình tự động tới Robot và Băng tải qua Serial (Dat Protocol)
            if (_serial != null && _serial.IsOpen)
            {
                bool sent = _serial.SendAutoStart(180);
                if (sent)
                {
                    AppendLog("▶ [TX_AUTO] Đã gửi lệnh BẮT ĐẦU (CMD_AUTO_START, Tốc độ 180) tới Robot và Băng tải!");
                }
                else
                {
                    AppendLog("❌ [LỖI] Không thể truyền gói tin CMD_AUTO_START qua cổng Serial.");
                }
            }
            else
            {
                AppendLog("⚠ [CẢNH BÁO] Cổng Serial chưa kết nối với ESP32! Băng tải và Robot sẽ không chạy. Vui lòng vào tab 'Cài đặt cấu hình' để kết nối COM.");
                _autoTimer.Start();
            }

            SystemRunStatusChanged?.Invoke(true);
            AppendLog("▶ ĐÃ BẮT ĐẦU: Hệ thống đang tự động nhận diện và phân loại.");
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _isSystemRunning = false;
            IsSystemRunning = false;
            _autoTimer.Stop();
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            // Gửi lệnh dừng chu trình tự động tới Robot và Băng tải qua Serial (Dat Protocol)
            if (_serial != null && _serial.IsOpen)
            {
                bool sent = _serial.SendAutoStop();
                if (sent)
                {
                    AppendLog("⏹ [TX_AUTO] Đã gửi lệnh DỪNG (CMD_AUTO_STOP) tới Robot và Băng tải!");
                }
            }

            SystemRunStatusChanged?.Invoke(false);
            AppendLog("⏹ ĐÃ DỪNG: Hệ thống đã tạm dừng phân loại.");
        }

        private async void AutoTimer_Tick(object sender, EventArgs e)



        {



            if (!_isSystemRunning || _isDetecting || _cam == null || !_cam.IsRunning)



                return;

            // Nếu Serial đang kết nối, cảm biến quang CB1/CB2 trên phần cứng sẽ kích hoạt nhận diện
            if (_serial != null && _serial.IsOpen)
                return;



            _isDetecting = true;



            try



            {



                using (DetectionResult result = await _cam.DetectAsync())



                {



                    if (result == null || result.Frame == null || result.Results == null || result.Results.Count == 0)



                    {



                        // Chưa có vòng bi đi qua khung hình



                        return;



                    }



                    // Vòng bi gần tâm nhất



                    BearingResult best = result.Results[0];



                    Image old = picResult.Image;



                    picResult.Image = (Image)result.Frame.Clone();



                    old?.Dispose();



                    // Cập nhật thống kê



                    _totalCount++;



                    if (best.IsOk)



                        _okCount++;



                    else



                        _ngCount++;



                    UpdateStatsUI();



                    // OK / NG



                    SetStatus(best.IsOk);



                    // Thông tin chi tiết



                    var info = GetClassInfo(best.Label);



                    lblDefect.Text = info.DisplayName;



                    lblDefect.ForeColor = info.TextColor;



                    lblConfidence.Text = best.Confidence.ToString("P1");



                    lblTime.Text = $"{result.TotalMs:F0} ms";



                    // Lưu lịch sử vào cơ sở dữ liệu bền vững và cập nhật DataGridView



                    var logEntry = BearingLogManager.Add(best.Label, info.DisplayName, best.IsOk, best.Confidence, result.TotalMs);



                    AddEntryToHomeGrid(logEntry);



                }



            }



            catch (Exception ex)



            {



                AppendLog($"Lỗi nhận diện tự động: {ex.Message}");



            }



            finally



            {



                _isDetecting = false;



            }



        }



        private void Camera_FrameReady(object sender, FrameReadyEventArgs e)



        {



            if (IsDisposed || Disposing || picWebcam.IsDisposed)



            {



                e.Frame?.Dispose();



                return;



            }



            Image oldImage = picWebcam.Image;



            picWebcam.Image = e.Frame;



            oldImage?.Dispose();



        }



        private void FormHome_FormClosing(object sender, FormClosingEventArgs e)



        {



            _autoTimer.Stop();



            UITheme.ThemeChanged -= ApplyTheme;

            if (_serial != null)
            {
                _serial.DetectTriggerReceived -= OnSerialDetectTrigger;
            }



            if (_cam != null)



            {



                _cam.FrameReady -= Camera_FrameReady;



                if (picWebcam.Image != null)



                {



                    Image old = picWebcam.Image;



                    picWebcam.Image = null;



                    old.Dispose();



                }



            }



        }



        private static (string DisplayName, Color TextColor) GetClassInfo(string label)



        {



            if (ClassInfo.TryGetValue(label, out var info))



                return info;



            return (label, UITheme.TextPrimary);



        }



        private void SetStatus(bool isOk)



        {



            _lastResultOk = isOk;



            if (isOk)



            {



                lblStatus.Text = "✔  ĐẠT CHUẨN (PASS)";



                lblStatus.ForeColor = UITheme.Success;



                pnlStatusBanner.BackgroundColor = UITheme.SuccessBg;



                pnlStatusBanner.BorderColor = UITheme.Success;



            }



            else



            {



                lblStatus.Text = "✖  PHÁT HIỆN LỖI (FAIL)";



                lblStatus.ForeColor = UITheme.Danger;



                pnlStatusBanner.BackgroundColor = UITheme.DangerBg;



                pnlStatusBanner.BorderColor = UITheme.Danger;



            }



        }



        private void ClearResult(string message)



        {



            _lastResultOk = null;



            _lastStatusMsg = message;



            lblStatus.Text = message;



            lblStatus.ForeColor = UITheme.TextSecondary;



            pnlStatusBanner.BackgroundColor = UITheme.BgCardAlt;



            pnlStatusBanner.BorderColor = UITheme.BorderCard;



            lblDefect.Text = "-";



            lblDefect.ForeColor = UITheme.TextSecondary;



            lblConfidence.Text = "-";



            lblConfidence.ForeColor = UITheme.Info;



            lblTime.Text = "-";



            lblTime.ForeColor = UITheme.Warning;



            if (picResult.Image != null)



            {



                Image old = picResult.Image;



                picResult.Image = null;



                old.Dispose();



            }



        }



        private void UpdateStatsUI()



        {



            lblStatTotalVal.Text = _totalCount.ToString();



            lblStatOkVal.Text = _okCount.ToString();



            lblStatNgVal.Text = _ngCount.ToString();



            double yieldRate = _totalCount > 0 ? ((double)_okCount / _totalCount) * 100.0 : 0.0;



            lblStatYieldVal.Text = $"{yieldRate:F1} %";



        }



        private void btnResetStats_Click(object sender, EventArgs e)



        {



            if (MessageBox.Show("Bạn có chắc chắn muốn đặt lại toàn bộ thống kê nhận diện hôm nay về 0?", "Xác nhận đặt lại", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)



            {



                BearingLogManager.ClearToday();



                _totalCount = 0;



                _okCount = 0;



                _ngCount = 0;



                UpdateStatsUI();



                dgvInspectionLog.Rows.Clear();



                ClearResult("SẴN SÀNG");



            }



        }



        private void AppendLog(string message)



        {



            System.Diagnostics.Debug.WriteLine($"[HOME_LOG] {message}");



        }



        private void InitInspectionGrid()



        {



            dgvInspectionLog.Columns.Clear();



            dgvInspectionLog.AutoGenerateColumns = false;



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colId",



                HeaderText = "STT",



                Width = 45,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }



            });



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colTime",



                HeaderText = "Thời gian",



                Width = 75,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }



            });



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colLabel",



                HeaderText = "Loại vòng bi",



                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft }



            });



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colResult",



                HeaderText = "Kết quả",



                Width = 75,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }



            });



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colConf",



                HeaderText = "Độ tin cậy",



                Width = 75,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }



            });



            dgvInspectionLog.Columns.Add(new DataGridViewTextBoxColumn



            {



                Name = "colMs",



                HeaderText = "Xử lý",



                Width = 65,



                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }



            });



            dgvInspectionLog.CellFormatting += DgvInspectionLog_CellFormatting;



            ApplyGridTheme();



            // Nạp các bản ghi hôm nay vào DataGridView (tối đa 50 bản ghi gần nhất của hôm nay)



            var todayList = BearingLogManager.GetToday();



            var toShow = todayList.Count > 50 ? todayList.Skip(todayList.Count - 50).ToList() : todayList;



            foreach (var item in toShow)



            {



                AddEntryToHomeGrid(item);



            }



        }



        private void ApplyGridTheme()



        {



            if (dgvInspectionLog == null) return;



            dgvInspectionLog.BackgroundColor = UITheme.BgCardAlt;



            dgvInspectionLog.GridColor = UITheme.BorderCard;



            dgvInspectionLog.BorderStyle = BorderStyle.None;



            dgvInspectionLog.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;



            dgvInspectionLog.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;



            dgvInspectionLog.RowHeadersVisible = false;



            dgvInspectionLog.EnableHeadersVisualStyles = false;



            dgvInspectionLog.ColumnHeadersDefaultCellStyle.BackColor = UITheme.IsDarkMode ? Color.FromArgb(30, 41, 59) : Color.FromArgb(241, 245, 249);



            dgvInspectionLog.ColumnHeadersDefaultCellStyle.ForeColor = UITheme.IsDarkMode ? Color.FromArgb(226, 232, 240) : Color.FromArgb(15, 23, 42);



            dgvInspectionLog.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);



            dgvInspectionLog.ColumnHeadersHeight = 26;



            dgvInspectionLog.DefaultCellStyle.BackColor = UITheme.BgCardAlt;



            dgvInspectionLog.DefaultCellStyle.ForeColor = UITheme.TextPrimary;



            dgvInspectionLog.DefaultCellStyle.SelectionBackColor = UITheme.IsDarkMode ? Color.FromArgb(41, 53, 76) : Color.FromArgb(224, 231, 255);



            dgvInspectionLog.DefaultCellStyle.SelectionForeColor = UITheme.TextPrimary;



            dgvInspectionLog.DefaultCellStyle.Font = new Font("Segoe UI", 8F);



            dgvInspectionLog.RowTemplate.Height = 24;



            try



            {



                typeof(DataGridView).InvokeMember("DoubleBuffered",



                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,



                    null, dgvInspectionLog, new object[] { true });



            }



            catch { }



        }



        private void DgvInspectionLog_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)



        {



            if (e.RowIndex < 0 || e.Value == null) return;



            if (dgvInspectionLog.Columns[e.ColumnIndex].Name == "colResult")



            {



                string val = e.Value.ToString() ?? "";



                if (val.Contains("PASS"))



                {



                    e.CellStyle.ForeColor = UITheme.Success;



                    e.CellStyle.Font = new Font(dgvInspectionLog.Font, FontStyle.Bold);



                }



                else if (val.Contains("FAIL"))



                {



                    e.CellStyle.ForeColor = UITheme.Danger;



                    e.CellStyle.Font = new Font(dgvInspectionLog.Font, FontStyle.Bold);



                }



            }



        }



        private void AddEntryToHomeGrid(BearingLogEntry entry)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => AddEntryToHomeGrid(entry)));



                return;



            }



            string resultStr = entry.IsOk ? "✔ PASS" : "✖ FAIL";



            string timeStr = entry.Timestamp.ToString("HH:mm:ss");



            string confStr = $"{entry.Confidence:P1}";



            string msStr = $"{entry.ProcessTimeMs:F0} ms";



            dgvInspectionLog.Rows.Insert(0, entry.Id, timeStr, entry.DisplayName, resultStr, confStr, msStr);



            if (dgvInspectionLog.Rows.Count > 50)



            {



                dgvInspectionLog.Rows.RemoveAt(dgvInspectionLog.Rows.Count - 1);



            }



        }



        private async void OnSerialDetectTrigger(int sensorId)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => OnSerialDetectTrigger(sensorId)));



                return;



            }



            if (!_isSystemRunning) return;



            AppendLog($"⚡ [TRIGGER CB{sensorId}] Cảm biến phát hiện vòng bi! Chờ 250ms ổn định quán tính...");



            if (_cam == null || !_cam.IsRunning)



            {



                AppendLog("Lỗi: Camera chưa bật, không thể nhận diện!");



                _serial?.SendInspectResult(true, DatProtocol.DEFECT_OK);



                return;



            }



            if (_isDetecting) return;



            _isDetecting = true;



            try



            {



                // Chờ 250ms để phôi hoàn toàn đứng yên sau khi dừng băng tải, tránh mờ nhòe ảnh
                await Task.Delay(250);

                using (DetectionResult result = await _cam.DetectAsync())



                {



                    if (result == null || result.Frame == null || result.Results == null || result.Results.Count == 0)



                    {



                        AppendLog("Không tìm thấy vòng bi trong khung hình. Tiếp tục chạy băng tải.");



                        _serial?.SendInspectResult(true, DatProtocol.DEFECT_OK);



                        return;



                    }



                    BearingResult best = result.Results[0];



                    Image old = picResult.Image;



                    picResult.Image = (Image)result.Frame.Clone();



                    old?.Dispose();



                    _totalCount++;



                    if (best.IsOk)



                        _okCount++;



                    else



                        _ngCount++;



                    UpdateStatsUI();



                    SetStatus(best.IsOk);



                    var info = GetClassInfo(best.Label);



                    lblDefect.Text = info.DisplayName;



                    lblDefect.ForeColor = info.TextColor;



                    lblConfidence.Text = best.Confidence.ToString("P1");



                    lblTime.Text = $"{result.TotalMs:F0} ms";



                    var logEntry = BearingLogManager.Add(best.Label, info.DisplayName, best.IsOk, best.Confidence, result.TotalMs);



                    AddEntryToHomeGrid(logEntry);



                    byte defectCode = DatProtocol.DEFECT_OK;



                    if (!best.IsOk)



                    {



                        if (best.Label.Contains("rusted"))



                            defectCode = DatProtocol.DEFECT_RUSTED;



                        else if (best.Label.Contains("shield"))



                            defectCode = DatProtocol.DEFECT_DAMAGED_SHIELD;



                        else



                            defectCode = DatProtocol.DEFECT_SURFACE_DAMAGE;



                    }



                    _serial?.SendInspectResult(best.IsOk, defectCode);



                    AppendLog($"Phản hồi kết quả AI về Robot: {(best.IsOk ? "PASS" : "FAIL")} (Mã phân loại: {defectCode})");



                }



            }



            catch (Exception ex)



            {



                AppendLog($"Lỗi nhận diện AI: {ex.Message}");



                _serial?.SendInspectResult(true, DatProtocol.DEFECT_OK);



            }



            finally



            {



                _isDetecting = false;



            }



        }



    }



}



