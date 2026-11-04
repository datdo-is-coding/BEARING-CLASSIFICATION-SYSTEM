using RobotView;



using System;



using System.Drawing;



using System.Threading.Tasks;



using System.Windows.Forms;



using System.Windows.Forms.Integration;



namespace DashBoardBearing



{



    public partial class FormRobot : Form



    {



        private Robot4DOF? _robotView;



        private ElementHost? _robotHost;



        private readonly SerialManager? _serialManager;



        private readonly RobotTeachConfig _teachConfig;



        // Flags



        private bool _isUpdatingUi = false;



        private bool _isUpdatingXYZ = false;



        private System.Windows.Forms.Timer? _debounceSerialTimer;



        // Smooth animation to target angles



        private System.Windows.Forms.Timer? _animTimer;



        private float _animStartJ1, _animStartJ2, _animStartJ3;



        private float _animTargetJ1, _animTargetJ2, _animTargetJ3;



        private int _animStep = 0;



        private const int TotalAnimSteps = 15;



        // Giới hạn chiều cao an toàn chống va chạm cơ khí với mặt đất



        public const float MinSafeHeight = 10.0f; // mm



        private float _lastSafeJ1 = 0.0f;



        private float _lastSafeJ2 = 100.1f;



        private float _lastSafeJ3 = 120.3f;



        private DateTime _lastSafetyWarnTime = DateTime.MinValue;



        /// <summary>



        /// Khởi tạo toàn bộ sự kiện của Form và các Controls ở đây.



        /// Tách riêng khỏi InitializeComponent() trong Designer để Visual Studio Form Designer



        /// không bao giờ bị lỗi parsing CodeDom hoặc không mở được giao diện.



        /// </summary>



        private void InitEventHandlers()



        {



            // Form events



            Load += FormRobot_Load;



            Resize += FormRobot_Resize;



            FormClosing += FormRobot_FormClosing;



            pnlRightScrollContent.Resize += (s, e) => AdjustRightPanelLayout();



            // Header & Global Actions



            btnHomeAll.Click += btnHomeAll_Click;



            btnSendJoints.Click += btnSendJoints_Click;



            btnMoveXYZ.Click += btnMoveXYZ_Click;



            // Joint Quick Preset Buttons



            btnJ1_0.Click += btnJ1_0_Click;



            btnJ1_90.Click += btnJ1_90_Click;



            btnJ1_180.Click += btnJ1_180_Click;



            btnJ2_0.Click += btnJ2_0_Click;



            btnJ2_90.Click += btnJ2_90_Click;



            btnJ2_180.Click += btnJ2_180_Click;



            btnJ3_0.Click += btnJ3_0_Click;



            btnJ3_90.Click += btnJ3_90_Click;



            btnJ3_180.Click += btnJ3_180_Click;



            // Sliders & NumericUpDown



            tbJ1.Scroll += tbJoint_Scroll;



            numJ1.ValueChanged += numJoint_ValueChanged;



            tbJ2.Scroll += tbJoint_Scroll;



            numJ2.ValueChanged += numJoint_ValueChanged;



            tbJ3.Scroll += tbJoint_Scroll;



            numJ3.ValueChanged += numJoint_ValueChanged;



            // Teach Point Buttons



            btnTeachStart.Click += btnTeachStart_Click;



            btnMoveStart.Click += btnMoveStart_Click;



            btnTeachPick.Click += btnTeachPick_Click;



            btnMovePick.Click += btnMovePick_Click;



            btnTeachRusted.Click += btnTeachRusted_Click;



            btnMoveRusted.Click += btnMoveRusted_Click;



            btnTeachShield.Click += btnTeachShield_Click;



            btnMoveShield.Click += btnMoveShield_Click;



            btnTeachSurface.Click += btnTeachSurface_Click;



            btnMoveSurface.Click += btnMoveSurface_Click;



            // Console / Log Commands



            btnClearLog.Click += btnClearLog_Click;



            btnSendCmd.Click += btnSendCmd_Click;



            txtLogCmd.KeyDown += txtLogCmd_KeyDown;



        }



        public FormRobot(Robot4DOF? robotView = null, SerialManager? serialManager = null)



        {



            InitializeComponent();



            InitEventHandlers();



            _robotView = robotView ?? new Robot4DOF();



            _serialManager = serialManager;



            _teachConfig = RobotTeachConfig.Load();



            InitDebounceTimer();



            InitRobotView();



            



            UITheme.ThemeChanged += ApplyTheme;



        }



        private void FormRobot_Load(object sender, EventArgs e)



        {



            ApplyTheme();



            AdjustSplitter();



            AdjustRightPanelLayout();



            UpdateTeachUI();



            if (_serialManager != null && _serialManager.IsRunning)



            {



                _serialManager.RequestTeachPoints();



                AppendLog("[TEACH SYNC] Robot đang kết nối: Đang đọc 5 điểm Teach từ Flash ESP32...");



            }



            numCoordZ.Minimum = (decimal)MinSafeHeight;



            _lastSafeJ1 = 0.0f;



            _lastSafeJ2 = 100.1f;



            _lastSafeJ3 = 120.3f;



            // Vị trí mặc định chuẩn theo dự án Robot4DOF gốc (Home: X=164, Y=0, Z=104)



            _isUpdatingUi = true;



            numJ1.Value = 0.0M;



            numJ2.Value = 100.1M;



            numJ3.Value = 120.3M;



            tbJ1.Value = 0;



            tbJ2.Value = 1001;



            tbJ3.Value = 1203;



            _isUpdatingUi = false;



            float initJ4 = RobotKinematics.CalculatePerpendicularJ4(100.1f, 120.3f);



            var (initX, initY, initZ) = RobotKinematics.Forward(0.0f, 100.1f, 120.3f, initJ4);



            _isUpdatingXYZ = true;



            try



            {



                numCoordX.Value = Math.Clamp((decimal)Math.Round(initX, 1), numCoordX.Minimum, numCoordX.Maximum);



                numCoordY.Value = Math.Clamp((decimal)Math.Round(initY, 1), numCoordY.Minimum, numCoordY.Maximum);



                numCoordZ.Value = Math.Clamp((decimal)Math.Round(initZ, 1), numCoordZ.Minimum, numCoordZ.Maximum);



            }



            finally



            {



                _isUpdatingXYZ = false;



            }



            SyncJointsTo3D(0.0f, 100.1f, 120.3f, initJ4, initX, initY, initZ);



            pnlRightScrollContent.AutoScrollPosition = new Point(0, 0);



            // Hook Serial Manager if available



            if (_serialManager != null)
            {
                _serialManager.SerialStatus += OnSerialStatusChanged;
                _serialManager.DataReceivedString += OnSerialDataReceived;
                _serialManager.DataSentString += OnSerialDataSent;
                _serialManager.TelemetryReceived += OnTelemetryReceived;
                _serialManager.TeachPointsReceived += OnTeachPointsReceived;
                _serialManager.AckReceived += OnSerialAckReceived;
                UpdateSerialStatusBadge(_serialManager.IsRunning);
            }



            else



            {



                UpdateSerialStatusBadge(false);



            }



            AppendLog("Khởi tạo hệ thống Robot thành công theo chuẩn Robot4DOF. Sẵn sàng điều khiển.");



        }



        private void InitDebounceTimer()



        {



            _debounceSerialTimer = new System.Windows.Forms.Timer();



            _debounceSerialTimer.Interval = 80; // 80ms debounce for smooth dragging without serial packet flooding



            _debounceSerialTimer.Tick += (s, e) =>



            {



                _debounceSerialTimer.Stop();



                SendCurrentJointsToSerial();



            };



        }



        private void InitRobotView()



        {



            try



            {



                if (_robotView != null)



                {



                    _robotHost = new ElementHost



                    {



                        Dock = DockStyle.Fill,



                        Child = _robotView



                    };



                    panelRobot.Controls.Add(_robotHost);



                }



            }



            catch (Exception ex)



            {



                AppendLog($"[ERR_3D] Không thể khởi tạo 3D Viewport: {ex.Message}");



            }



        }



        private void FormRobot_Resize(object sender, EventArgs e)



        {



            AdjustSplitter();



            AdjustRightPanelLayout();



        }



        private void AdjustSplitter()



        {



            try



            {



                if (splitContainerRobot != null && splitContainerRobot.Width > 500)



                {



                    // Allocate 48% to 3D Viewport and 52% to Control Panel (min 420px for controls)



                    int target = (int)(splitContainerRobot.Width * 0.48);



                    int minLeft = Math.Max(350, splitContainerRobot.Panel1MinSize);



                    int maxLeft = splitContainerRobot.Width - Math.Max(420, splitContainerRobot.Panel2MinSize);



                    if (target >= minLeft && target <= maxLeft)



                    {



                        splitContainerRobot.SplitterDistance = target;



                    }



                    else if (target > maxLeft && maxLeft >= minLeft)



                    {



                        splitContainerRobot.SplitterDistance = maxLeft;



                    }



                }



            }



            catch { }



        }



        /// <summary>



        /// Dãn đều khoảng cách và kích thước các panel trong tab Teach điểm để tận dụng tối đa chiều cao



        /// loại bỏ khoảng trống trắng ở dưới khi cửa sổ phóng to hoặc màn hình lớn.



        /// </summary>



        private void AdjustRightPanelLayout()



        {



            try



            {



                if (pnlRightScrollContent == null || groupTeachPoints == null || groupJointSliders == null || groupCoordinatesXYZ == null) return;



                int availableHeight = pnlRightScrollContent.ClientSize.Height;



                int fixedTopHeight = groupJointSliders.Height + groupCoordinatesXYZ.Height;



                int remainingForTeach = availableHeight - fixedTopHeight;



                RoundedPanel[] panels = { pnlTeachStart, pnlTeachPick, pnlTeachRusted, pnlTeachShield, pnlTeachSurface };



                Label[] coordLabels = { lblCoordsStart, lblCoordsPick, lblCoordsRusted, lblCoordsShield, lblCoordsSurface };



                if (remainingForTeach > 450)



                {



                    // Khi có khoảng trống ở dưới, dãn đều chiều cao các thẻ và khoảng cách giữa các thẻ



                    int extra = remainingForTeach - 448;



                    int itemExtra = Math.Min(extra / 7, 14); // tăng tối đa 14px cho mỗi card (74 -> 88px)



                    int itemHeight = 74 + itemExtra;



                    int remainingAfterItems = extra - (itemExtra * panels.Length);



                    int gap = Math.Clamp(10 + (remainingAfterItems / (panels.Length + 1)), 10, 22);



                    int startY = 28;



                    for (int i = 0; i < panels.Length; i++)



                    {



                        panels[i].Top = startY + i * (itemHeight + gap);



                        panels[i].Height = itemHeight;



                        if (i < coordLabels.Length && coordLabels[i] != null)



                        {



                            coordLabels[i].Top = itemHeight - 36;



                            coordLabels[i].Height = 30;



                        }



                    }



                    int totalTeachH = startY + (panels.Length * itemHeight) + ((panels.Length - 1) * gap) + 12;



                    groupTeachPoints.Height = Math.Max(totalTeachH, remainingForTeach);



                }



                else



                {



                    // Kích thước chuẩn



                    int itemHeight = 74;



                    int gap = 10;



                    int startY = 28;



                    for (int i = 0; i < panels.Length; i++)



                    {



                        panels[i].Top = startY + i * (itemHeight + gap);



                        panels[i].Height = itemHeight;



                        if (i < coordLabels.Length && coordLabels[i] != null)



                        {



                            coordLabels[i].Top = 36;



                            coordLabels[i].Height = 28;



                        }



                    }



                    groupTeachPoints.Height = 448;



                }



                // Tự động dãn khoảng cách các ô tọa độ X, Y, Z theo chiều ngang để thoáng và dễ nhìn



                int totalW = groupCoordinatesXYZ.ClientSize.Width;



                if (totalW > 280)



                {



                    btnMoveXYZ.Left = totalW - btnMoveXYZ.Width - 6;



                    int leftSpace = btnMoveXYZ.Left - 10;



                    int itemW = Math.Clamp((leftSpace - 66) / 3, 72, 98);



                    int remainingW = leftSpace - (itemW * 3 + 66);



                    int gapXYZ = Math.Max(remainingW / 2, 10);



                    int curX = 4;



                    lblCoordX.Left = curX;



                    numCoordX.Left = curX + 24;



                    numCoordX.Width = itemW;



                    curX += 24 + itemW + gapXYZ;



                    lblCoordY.Left = curX;



                    numCoordY.Left = curX + 24;



                    numCoordY.Width = itemW;



                    curX += 24 + itemW + gapXYZ;



                    lblCoordZ.Left = curX;



                    numCoordZ.Left = curX + 24;



                    numCoordZ.Width = itemW;



                }



            }



            catch (Exception ex) { }



        }



        private void ApplyTheme()



        {



            BackColor = UITheme.BgDark;



            panelLeftCard.BackgroundColor = UITheme.BgCard;



            panelLeftCard.BorderColor = UITheme.BorderCard;



            panelRightCard.BackgroundColor = UITheme.BgCard;



            panelRightCard.BorderColor = UITheme.BorderCard;



            lblRobotTitle.ForeColor = UITheme.TextPrimary;



            lblInstruction.ForeColor = UITheme.TextSecondary;



            lblRightTitle.ForeColor = UITheme.TextPrimary;



            lblJointsHeader.ForeColor = UITheme.PrimaryLight;



            lblTeachHeader.ForeColor = UITheme.PrimaryLight;



            lblLogTitle.ForeColor = UITheme.TextPrimary;



            // Labels inside joints



            lblJ1Title.ForeColor = UITheme.TextPrimary;



            lblJ2Title.ForeColor = UITheme.TextPrimary;



            lblJ3Title.ForeColor = UITheme.TextPrimary;



            chkAutoSendSerial.ForeColor = UITheme.TextSecondary;



            // Quick angle buttons (0°, 90°, 180°)



            Button[] quickButtons = {



                btnJ1_0, btnJ1_90, btnJ1_180,



                btnJ2_0, btnJ2_90, btnJ2_180,



                btnJ3_0, btnJ3_90, btnJ3_180



            };



            foreach (var b in quickButtons)



            {



                b.BackColor = UITheme.BgCardAlt;



                b.ForeColor = UITheme.TextSecondary;



            }



            // Numeric input colors



            NumericUpDown[] nums = { numJ1, numJ2, numJ3, numCoordX, numCoordY, numCoordZ };



            foreach (var n in nums)



            {



                n.BackColor = UITheme.BgCardAlt;



                n.ForeColor = UITheme.TextPrimary;



            }



            lblXYZHeader.ForeColor = UITheme.PrimaryLight;



            lblCoordX.ForeColor = Color.FromArgb(239, 68, 68); // Đỏ - Trục X



            lblCoordY.ForeColor = Color.FromArgb(16, 185, 129); // Xanh lá - Trục Y



            lblCoordZ.ForeColor = Color.FromArgb(59, 130, 246); // Xanh dương - Trục Z



            // Teach Point Panels



            RoundedPanel[] teachPanels = { pnlTeachStart, pnlTeachPick, pnlTeachRusted, pnlTeachShield, pnlTeachSurface };



            foreach (var p in teachPanels)



            {



                p.BackgroundColor = UITheme.BgCardAlt;



                p.BorderColor = UITheme.BorderCard;



            }



            lblCoordsStart.ForeColor = UITheme.TextSecondary;



            lblCoordsPick.ForeColor = UITheme.TextSecondary;



            lblCoordsRusted.ForeColor = UITheme.TextSecondary;



            lblCoordsShield.ForeColor = UITheme.TextSecondary;



            lblCoordsSurface.ForeColor = UITheme.TextSecondary;



            // Action buttons



            btnHomeAll.BackColor = UITheme.BgCardAlt;



            btnHomeAll.ForeColor = UITheme.PrimaryLight;



            btnHomeAll.IconColor = UITheme.PrimaryLight;



            btnClearLog.BackColor = UITheme.BgCardAlt;



            btnClearLog.ForeColor = UITheme.TextSecondary;



            btnClearLog.IconColor = UITheme.TextSecondary;



            // Log Console



            pnlSerialLogCard.BackColor = UITheme.BgCardAlt;



            pnlLogCmd.BackColor = UITheme.BgCard;



            txtLogCmd.BackColor = UITheme.BgCardAlt;



            txtLogCmd.ForeColor = UITheme.TextPrimary;



            txtSerialLog.BackColor = UITheme.IsDarkMode ? Color.FromArgb(10, 15, 26) : Color.FromArgb(15, 23, 42);



            txtSerialLog.ForeColor = Color.FromArgb(226, 232, 240);



        }



        // =========================================================================



        // SLIDER & NUMERIC VALUE SYNC (RESOLUTION 0.1°)



        // =========================================================================



        private void HandleSafetyCollisionTrigger(float attemptedZ)



        {



            numCoordZ.BackColor = Color.FromArgb(254, 226, 226);



            lblCoordZ.ForeColor = Color.FromArgb(220, 38, 38);



            if ((DateTime.Now - _lastSafetyWarnTime).TotalMilliseconds > 1000)



            {



                _lastSafetyWarnTime = DateTime.Now;



                AppendLog($"⚠️ [BẢO VỆ AN TOÀN] Chiều cao h = {attemptedZ:F1} mm < {MinSafeHeight:F1} mm! Đã chặn trượt để tránh va chạm cơ khí với mặt đất.");



            }



        }



        private void tbJoint_Scroll(object? sender, EventArgs e)
        {
            if (_isUpdatingUi) return;

            float candidateJ1 = (float)numJ1.Value;
            float candidateJ2 = (float)numJ2.Value;
            float candidateJ3 = (float)numJ3.Value;

            if (sender == tbJ1)
            {
                candidateJ1 = (float)(tbJ1.Value / 10.0);
            }
            else if (sender == tbJ2)
            {
                candidateJ2 = (float)(tbJ2.Value / 10.0);
            }
            else if (sender == tbJ3)
            {
                candidateJ3 = (float)(tbJ3.Value / 10.0);
            }

            float candidateJ4 = RobotKinematics.CalculatePerpendicularJ4(candidateJ2, candidateJ3);
            var (candX, candY, candZ) = RobotKinematics.Forward(candidateJ1, candidateJ2, candidateJ3, candidateJ4);
            if (candZ < MinSafeHeight)
            {
                _isUpdatingUi = true;
                try
                {
                    tbJ1.Value = Math.Clamp((int)Math.Round(_lastSafeJ1 * 10), -900, 900);
                    tbJ2.Value = Math.Clamp((int)Math.Round(_lastSafeJ2 * 10), 0, 1800);
                    tbJ3.Value = Math.Clamp((int)Math.Round(_lastSafeJ3 * 10), 0, 1500);
                    numJ1.Value = (decimal)_lastSafeJ1;
                    numJ2.Value = (decimal)_lastSafeJ2;
                    numJ3.Value = (decimal)_lastSafeJ3;
                }
                finally
                {
                    _isUpdatingUi = false;
                }
                HandleSafetyCollisionTrigger(candZ);
                return;
            }

            _lastSafeJ1 = candidateJ1;
            _lastSafeJ2 = candidateJ2;
            _lastSafeJ3 = candidateJ3;

            _isUpdatingUi = true;
            try
            {
                if (sender == tbJ1) numJ1.Value = (decimal)candidateJ1;
                else if (sender == tbJ2) numJ2.Value = (decimal)candidateJ2;
                else if (sender == tbJ3) numJ3.Value = (decimal)candidateJ3;
            }
            finally
            {
                _isUpdatingUi = false;
            }

            OnJointAngleChanged(fromUser: true);
        }

        private void numJoint_ValueChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingUi) return;

            float candidateJ1 = (float)numJ1.Value;
            float candidateJ2 = (float)numJ2.Value;
            float candidateJ3 = (float)numJ3.Value;

            float candidateJ4 = RobotKinematics.CalculatePerpendicularJ4(candidateJ2, candidateJ3);
            var (candX, candY, candZ) = RobotKinematics.Forward(candidateJ1, candidateJ2, candidateJ3, candidateJ4);
            if (candZ < MinSafeHeight)
            {
                _isUpdatingUi = true;
                try
                {
                    numJ1.Value = (decimal)_lastSafeJ1;
                    numJ2.Value = (decimal)_lastSafeJ2;
                    numJ3.Value = (decimal)_lastSafeJ3;
                    tbJ1.Value = Math.Clamp((int)Math.Round(_lastSafeJ1 * 10), -900, 900);
                    tbJ2.Value = Math.Clamp((int)Math.Round(_lastSafeJ2 * 10), 0, 1800);
                    tbJ3.Value = Math.Clamp((int)Math.Round(_lastSafeJ3 * 10), 0, 1500);
                }
                finally
                {
                    _isUpdatingUi = false;
                }
                HandleSafetyCollisionTrigger(candZ);
                return;
            }

            _lastSafeJ1 = candidateJ1;
            _lastSafeJ2 = candidateJ2;
            _lastSafeJ3 = candidateJ3;

            _isUpdatingUi = true;
            try
            {
                if (sender == numJ1) tbJ1.Value = Math.Clamp((int)Math.Round((double)candidateJ1 * 10), -900, 900);
                else if (sender == numJ2) tbJ2.Value = Math.Clamp((int)Math.Round((double)candidateJ2 * 10), 0, 1800);
                else if (sender == numJ3) tbJ3.Value = Math.Clamp((int)Math.Round((double)candidateJ3 * 10), 0, 1500);
            }
            finally
            {
                _isUpdatingUi = false;
            }

            OnJointAngleChanged(fromUser: true);
        }

        private void btnJ1_0_Click(object? sender, EventArgs e) => numJ1.Value = -90.0M;



        private void btnJ1_90_Click(object? sender, EventArgs e) => numJ1.Value = 0.0M;



        private void btnJ1_180_Click(object? sender, EventArgs e) => numJ1.Value = 90.0M;



        private void btnJ2_0_Click(object? sender, EventArgs e) => numJ2.Value = 0.0M;



        private void btnJ2_90_Click(object? sender, EventArgs e) => numJ2.Value = 90.0M;



        private void btnJ2_180_Click(object? sender, EventArgs e) => numJ2.Value = 180.0M;



        private void btnJ3_0_Click(object? sender, EventArgs e) => numJ3.Value = 0.0M;



        private void btnJ3_90_Click(object? sender, EventArgs e) => numJ3.Value = 75.0M;



        private void btnJ3_180_Click(object? sender, EventArgs e) => numJ3.Value = 150.0M;



        private void OnJointAngleChanged(bool fromUser)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            // J4 luôn tự động giải để đầu công cụ vuông góc với mặt đất (J4 = 270° - J2 - J3)



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



            var (x, y, z) = RobotKinematics.Forward(j1, j2, j3, j4);



            if (!_isUpdatingXYZ)



            {



                _isUpdatingXYZ = true;



                try



                {



                    numCoordX.Value = Math.Clamp((decimal)Math.Round(x, 1), numCoordX.Minimum, numCoordX.Maximum);



                    numCoordY.Value = Math.Clamp((decimal)Math.Round(y, 1), numCoordY.Minimum, numCoordY.Maximum);



                    numCoordZ.Value = Math.Clamp((decimal)Math.Round(z, 1), numCoordZ.Minimum, numCoordZ.Maximum);



                }



                finally



                {



                    _isUpdatingXYZ = false;



                }



            }



            // Khôi phục hiển thị ô tọa độ Z bình thường khi an toàn



            numCoordZ.BackColor = UITheme.BgCardAlt;



            lblCoordZ.ForeColor = Color.FromArgb(59, 130, 246); // Xanh dương - Trục Z



            SyncJointsTo3D(j1, j2, j3, j4, x, y, z);



            if (fromUser && chkAutoSendSerial.Checked)



            {



                _debounceSerialTimer?.Stop();



                _debounceSerialTimer?.Start();



            }



        }



        private void SyncJointsTo3D(float j1, float j2, float j3, float? j4 = null, float? x = null, float? y = null, float? z = null)



        {



            float actualJ4 = j4 ?? RobotKinematics.CalculatePerpendicularJ4(j2, j3);



            if (!x.HasValue || !y.HasValue || !z.HasValue)



            {



                var fk = RobotKinematics.Forward(j1, j2, j3, actualJ4);



                x = fk.X;



                y = fk.Y;



                z = fk.Z;



            }



            try



            {



                if (_robotView?.vm?.Robot != null)



                {



                    // Chuyển đổi chuẩn xác từ góc Slider sang góc mô hình 3D (Robot4DOF)



                    _robotView.vm.Robot.SetJointAngles(



                        j1 + 180.0f,



                        j2 - 90.0f,



                        j3,



                        actualJ4);



                }



            }



            catch (Exception ex)



            {



                AppendLog($"[ERR_ROBOT] Lỗi cập nhật góc 3D: {ex.Message}");



            }



        }



        private void btnHomeAll_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.StartPoint, "Vị trí Bắt Đầu (Start/Home)");



        }



        private void btnMoveXYZ_Click(object sender, EventArgs e)



        {



            float targetX = (float)numCoordX.Value;



            float targetY = (float)numCoordY.Value;



            float targetZ = (float)numCoordZ.Value;



            if (targetZ < MinSafeHeight)



            {



                MessageBox.Show($"Chiều cao Z = {targetZ:F1} mm < {MinSafeHeight:F1} mm!\nRobot không được phép di chuyển xuống dưới độ cao an toàn để tránh va chạm cơ khí với mặt đất.", "Cảnh báo an toàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                AppendLog($"[SAFETY] Từ chối di chuyển tới Z = {targetZ:F1} mm < {MinSafeHeight:F1} mm (bảo vệ chống va chạm mặt đất).");



                return;



            }



            if (!RobotKinematics.Inverse(targetX, targetY, targetZ, out float j1, out float j2, out float j3, out float j4))



            {



                MessageBox.Show($"Tọa độ XYZ ({targetX:F1}, {targetY:F1}, {targetZ:F1}) mm vượt quá tầm với tối đa của cánh tay Robot!", "Ngoài tầm với", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                AppendLog($"[ERR_IK] Vị trí XYZ ({targetX:F1}, {targetY:F1}, {targetZ:F1}) mm ngoài tầm với!");



                return;



            }



            // Kiểm tra giới hạn vật lý của các khớp



            if (j1 < -90.0f || j1 > 90.0f || j2 < 0.0f || j2 > 180.0f || j3 < 0.0f || j3 > 150.0f)



            {



                MessageBox.Show($"Tọa độ XYZ ({targetX:F1}, {targetY:F1}, {targetZ:F1}) mm đòi hỏi góc khớp vượt giới hạn:\n• J1: {j1:F1}° (phạm vi: -90° đến +90°)\n• J2: {j2:F1}° (phạm vi: 0° đến 180°)\n• J3: {j3:F1}° (phạm vi: 0° đến 150°)", "Vượt giới hạn khớp", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                AppendLog($"[ERR_LIMIT] Vị trí XYZ yêu cầu góc vượt giới hạn: J1={j1:F1}°, J2={j2:F1}°, J3={j3:F1}°");



                return;



            }



            AnimateToAngles(j1, j2, j3, j4, $"Tọa độ XYZ ({targetX:F1}, {targetY:F1}, {targetZ:F1})");



        }



        private void btnSendJoints_Click(object sender, EventArgs e)



        {



            SendCurrentJointsToSerial();



        }



        private void SendCurrentJointsToSerial()



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



            if (_serialManager != null && _serialManager.IsRunning)



            {



                _serialManager.SendJog(j1, j2, j3, j4);



            }



            else



            {



                AppendLog($"[SIM_TX] JOG {j1:F2} {j2:F2} {j3:F2} {j4:F2} (Serial chưa kết nối)");



            }



        }



        // =========================================================================



        // TEACH POINTS MANAGEMENT (5 VỊ TRÍ THEO TỌA ĐỘ XYZ & GÓC KHỚP)



        // =========================================================================



        private void UpdateTeachUI()



        {



            lblCoordsStart.Text = _teachConfig.StartPoint.ToString();



            lblCoordsPick.Text = _teachConfig.PickPoint.ToString();



            lblCoordsRusted.Text = _teachConfig.DropRusted.ToString();



            lblCoordsShield.Text = _teachConfig.DropDamagedShield.ToString();



            lblCoordsSurface.Text = _teachConfig.DropSurfaceDamage.ToString();



        }



        // 1. Điểm Bắt Đầu (Start Position)



        private void btnTeachStart_Click(object sender, EventArgs e)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



                        _teachConfig.StartPoint = new JointPose(j1, j2, j3, j4);
            _teachConfig.Save();
            UpdateTeachUI();
            AppendLog($"[TEACH] Đã lưu Điểm Bắt Đầu vào PC: {_teachConfig.StartPoint}");
            if (_serialManager != null && _serialManager.IsRunning)
            {
                bool sent = _serialManager.SendSetTeach(1, (short)_teachConfig.StartPoint.X, (short)_teachConfig.StartPoint.Y, (short)_teachConfig.StartPoint.Z);
                if (sent) AppendLog($"[TX_TEACH] ➔ Đang gửi CMD_SET_TEACH Điểm 1 (X={_teachConfig.StartPoint.X:F0}, Y={_teachConfig.StartPoint.Y:F0}, Z={_teachConfig.StartPoint.Z:F0}) xuống ESP32 Flash...");
                else AppendLog("[TX_TEACH ERR] ❌ Không thể gửi gói tin CMD_SET_TEACH qua cổng Serial.");
            }
            else
            {
                AppendLog("[TEACH WARN] ⚠ Cổng Serial chưa kết nối! Tọa độ chỉ được lưu cục bộ trên PC, chưa nạp vào Flash ESP32.");
            }



        }



        private void btnMoveStart_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.StartPoint, "Điểm Bắt Đầu (Start Position)");



        }



        // 2. Điểm Gắp



        private void btnTeachPick_Click(object sender, EventArgs e)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



                        _teachConfig.PickPoint = new JointPose(j1, j2, j3, j4);
            _teachConfig.Save();
            UpdateTeachUI();
            AppendLog($"[TEACH] Đã lưu Điểm Gắp vào PC: {_teachConfig.PickPoint}");
            if (_serialManager != null && _serialManager.IsRunning)
            {
                bool sent = _serialManager.SendSetTeach(2, (short)_teachConfig.PickPoint.X, (short)_teachConfig.PickPoint.Y, (short)_teachConfig.PickPoint.Z);
                if (sent) AppendLog($"[TX_TEACH] ➔ Đang gửi CMD_SET_TEACH Điểm 2 (X={_teachConfig.PickPoint.X:F0}, Y={_teachConfig.PickPoint.Y:F0}, Z={_teachConfig.PickPoint.Z:F0}) xuống ESP32 Flash...");
                else AppendLog("[TX_TEACH ERR] ❌ Không thể gửi gói tin CMD_SET_TEACH qua cổng Serial.");
            }
            else
            {
                AppendLog("[TEACH WARN] ⚠ Cổng Serial chưa kết nối! Tọa độ chỉ được lưu cục bộ trên PC, chưa nạp vào Flash ESP32.");
            }



        }



        private void btnMovePick_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.PickPoint, "Điểm Gắp (Pick Position)");



        }



        // 3. Điểm Thả Gỉ Sét



        private void btnTeachRusted_Click(object sender, EventArgs e)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



                        _teachConfig.DropRusted = new JointPose(j1, j2, j3, j4);
            _teachConfig.Save();
            UpdateTeachUI();
            AppendLog($"[TEACH] Đã lưu Điểm Thả Gỉ Sét vào PC: {_teachConfig.DropRusted}");
            if (_serialManager != null && _serialManager.IsRunning)
            {
                bool sent = _serialManager.SendSetTeach(3, (short)_teachConfig.DropRusted.X, (short)_teachConfig.DropRusted.Y, (short)_teachConfig.DropRusted.Z);
                if (sent) AppendLog($"[TX_TEACH] ➔ Đang gửi CMD_SET_TEACH Điểm 3 (X={_teachConfig.DropRusted.X:F0}, Y={_teachConfig.DropRusted.Y:F0}, Z={_teachConfig.DropRusted.Z:F0}) xuống ESP32 Flash...");
                else AppendLog("[TX_TEACH ERR] ❌ Không thể gửi gói tin CMD_SET_TEACH qua cổng Serial.");
            }
            else
            {
                AppendLog("[TEACH WARN] ⚠ Cổng Serial chưa kết nối! Tọa độ chỉ được lưu cục bộ trên PC, chưa nạp vào Flash ESP32.");
            }



        }



        private void btnMoveRusted_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.DropRusted, "Điểm Thả: Gỉ Sét (Rusted)");



        }



        // 4. Điểm Thả Hư Nắp Che



        private void btnTeachShield_Click(object sender, EventArgs e)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



                        _teachConfig.DropDamagedShield = new JointPose(j1, j2, j3, j4);
            _teachConfig.Save();
            UpdateTeachUI();
            AppendLog($"[TEACH] Đã lưu Điểm Thả Hư Nắp Che vào PC: {_teachConfig.DropDamagedShield}");
            if (_serialManager != null && _serialManager.IsRunning)
            {
                bool sent = _serialManager.SendSetTeach(4, (short)_teachConfig.DropDamagedShield.X, (short)_teachConfig.DropDamagedShield.Y, (short)_teachConfig.DropDamagedShield.Z);
                if (sent) AppendLog($"[TX_TEACH] ➔ Đang gửi CMD_SET_TEACH Điểm 4 (X={_teachConfig.DropDamagedShield.X:F0}, Y={_teachConfig.DropDamagedShield.Y:F0}, Z={_teachConfig.DropDamagedShield.Z:F0}) xuống ESP32 Flash...");
                else AppendLog("[TX_TEACH ERR] ❌ Không thể gửi gói tin CMD_SET_TEACH qua cổng Serial.");
            }
            else
            {
                AppendLog("[TEACH WARN] ⚠ Cổng Serial chưa kết nối! Tọa độ chỉ được lưu cục bộ trên PC, chưa nạp vào Flash ESP32.");
            }



        }



        private void btnMoveShield_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.DropDamagedShield, "Điểm Thả: Hư Nắp Che (Damaged Shield)");



        }



        // 5. Điểm Thả Trầy Xước Bề Mặt



        private void btnTeachSurface_Click(object sender, EventArgs e)



        {



            float j1 = (float)numJ1.Value;



            float j2 = (float)numJ2.Value;



            float j3 = (float)numJ3.Value;



            float j4 = RobotKinematics.CalculatePerpendicularJ4(j2, j3);



                        _teachConfig.DropSurfaceDamage = new JointPose(j1, j2, j3, j4);
            _teachConfig.Save();
            UpdateTeachUI();
            AppendLog($"[TEACH] Đã lưu Điểm Thả Trầy Xước vào PC: {_teachConfig.DropSurfaceDamage}");
            if (_serialManager != null && _serialManager.IsRunning)
            {
                bool sent = _serialManager.SendSetTeach(5, (short)_teachConfig.DropSurfaceDamage.X, (short)_teachConfig.DropSurfaceDamage.Y, (short)_teachConfig.DropSurfaceDamage.Z);
                if (sent) AppendLog($"[TX_TEACH] ➔ Đang gửi CMD_SET_TEACH Điểm 5 (X={_teachConfig.DropSurfaceDamage.X:F0}, Y={_teachConfig.DropSurfaceDamage.Y:F0}, Z={_teachConfig.DropSurfaceDamage.Z:F0}) xuống ESP32 Flash...");
                else AppendLog("[TX_TEACH ERR] ❌ Không thể gửi gói tin CMD_SET_TEACH qua cổng Serial.");
            }
            else
            {
                AppendLog("[TEACH WARN] ⚠ Cổng Serial chưa kết nối! Tọa độ chỉ được lưu cục bộ trên PC, chưa nạp vào Flash ESP32.");
            }



        }



        private void btnMoveSurface_Click(object sender, EventArgs e)



        {



            MoveToTeachPoint(_teachConfig.DropSurfaceDamage, "Điểm Thả: Trầy Xước (Surface Damage)");



        }



        private void MoveToTeachPoint(JointPose pose, string pointName)



        {



            AppendLog($"[MOVE] Bắt đầu di chuyển tới {pointName} [XYZ: {pose.X:F1}, {pose.Y:F1}, {pose.Z:F1}]...");



            AnimateToAngles(pose.J1, pose.J2, pose.J3, pose.J4, pointName);



        }



        // =========================================================================



        // SMOOTH MOVEMENT INTERPOLATION (ANIMATION)



        // =========================================================================



        private void AnimateToAngles(float targetJ1, float targetJ2, float targetJ3, float? targetJ4 = null, string? label = null)



        {



            _animTimer?.Stop();



            _animStartJ1 = (float)numJ1.Value;



            _animStartJ2 = (float)numJ2.Value;



            _animStartJ3 = (float)numJ3.Value;



            _animTargetJ1 = Math.Clamp(targetJ1, -90.0f, 90.0f);



            _animTargetJ2 = Math.Clamp(targetJ2, 0.0f, 180.0f);



            _animTargetJ3 = Math.Clamp(targetJ3, 0.0f, 150.0f);



            float finalTargetJ4 = targetJ4 ?? RobotKinematics.CalculatePerpendicularJ4(_animTargetJ2, _animTargetJ3);



            _animStep = 0;



            _animTimer = new System.Windows.Forms.Timer();



            _animTimer.Interval = 20; // 20ms * 15 steps = 300ms smooth animation



            _animTimer.Tick += (s, e) =>



            {



                _animStep++;



                float progress = (float)_animStep / TotalAnimSteps;



                // Ease-in-out curve



                float t = progress < 0.5f ? 2f * progress * progress : -1f + (4f - 2f * progress) * progress;



                t = Math.Clamp(t, 0.0f, 1.0f);



                float curJ1 = _animStartJ1 + (_animTargetJ1 - _animStartJ1) * t;



                float curJ2 = _animStartJ2 + (_animTargetJ2 - _animStartJ2) * t;



                float curJ3 = _animStartJ3 + (_animTargetJ3 - _animStartJ3) * t;



                float curJ4 = RobotKinematics.CalculatePerpendicularJ4(curJ2, curJ3);



                _isUpdatingUi = true;



                try



                {



                    numJ1.Value = (decimal)Math.Round(curJ1, 1);



                    numJ2.Value = (decimal)Math.Round(curJ2, 1);



                    numJ3.Value = (decimal)Math.Round(curJ3, 1);



                    tbJ1.Value = Math.Clamp((int)Math.Round(curJ1 * 10), -900, 900);



                    tbJ2.Value = Math.Clamp((int)Math.Round(curJ2 * 10), 0, 1800);



                    tbJ3.Value = Math.Clamp((int)Math.Round(curJ3 * 10), 0, 1500);



                }



                finally



                {



                    _isUpdatingUi = false;



                }



                var (curX, curY, curZ) = RobotKinematics.Forward(curJ1, curJ2, curJ3, curJ4);



                if (!_isUpdatingXYZ)



                {



                    _isUpdatingXYZ = true;



                    try



                    {



                        numCoordX.Value = Math.Clamp((decimal)Math.Round(curX, 1), numCoordX.Minimum, numCoordX.Maximum);



                        numCoordY.Value = Math.Clamp((decimal)Math.Round(curY, 1), numCoordY.Minimum, numCoordY.Maximum);



                        numCoordZ.Value = Math.Clamp((decimal)Math.Round(curZ, 1), numCoordZ.Minimum, numCoordZ.Maximum);



                    }



                    finally



                    {



                        _isUpdatingXYZ = false;



                    }



                }



                SyncJointsTo3D(curJ1, curJ2, curJ3, curJ4, curX, curY, curZ);



                if (_animStep >= TotalAnimSteps)



                {



                    _animTimer.Stop();



                    _animTimer.Dispose();



                    _animTimer = null;



                    // Final target sync



                    var (endX, endY, endZ) = RobotKinematics.Forward(_animTargetJ1, _animTargetJ2, _animTargetJ3, finalTargetJ4);



                    SyncJointsTo3D(_animTargetJ1, _animTargetJ2, _animTargetJ3, finalTargetJ4, endX, endY, endZ);



                    // Send command to physical robot via Dat Protocol



                    if (_serialManager != null && _serialManager.IsRunning)



                    {



                        _serialManager.SendJog(_animTargetJ1, _animTargetJ2, _animTargetJ3, finalTargetJ4);



                        AppendLog($"[TX_DAT] JOG {_animTargetJ1:F2} {_animTargetJ2:F2} {_animTargetJ3:F2} {finalTargetJ4:F2}");



                    }



                    if (!string.IsNullOrEmpty(label))



                    {



                        AppendLog($"[MOVE] Hoàn thành di chuyển tới {label}.");



                    }



                }



            };



            _animTimer.Start();



        }



        // =========================================================================



        // SERIAL LOG CONSOLE & CUSTOM COMMANDS



        // =========================================================================



        private void OnSerialStatusChanged(bool connected)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => OnSerialStatusChanged(connected)));



                return;



            }



            UpdateSerialStatusBadge(connected);
            AppendLog(connected ? $"[SERIAL] Cổng kết nối thành công: {_serialManager?.CurrentPortName}" : "[SERIAL] Đã ngắt kết nối cổng COM.");
            if (connected)
            {
                _serialManager?.RequestTeachPoints();
            }



        }



        private void UpdateSerialStatusBadge(bool connected)



        {



            if (connected)



            {



                lblSerialBadge.Text = $"● COM: {_serialManager?.CurrentPortName ?? "ONLINE"}";



                lblSerialBadge.BackColor = Color.FromArgb(220, 252, 231);



                lblSerialBadge.ForeColor = Color.FromArgb(16, 185, 129);



            }



            else



            {



                lblSerialBadge.Text = "● COM: OFFLINE";



                lblSerialBadge.BackColor = Color.FromArgb(254, 226, 226);



                lblSerialBadge.ForeColor = Color.FromArgb(239, 68, 68);



            }



        }



        private void OnSerialDataReceived(string message)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => OnSerialDataReceived(message)));



                return;



            }



            AppendLog($"[RX] {message}");



        }



        private void OnSerialDataSent(string message)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => OnSerialDataSent(message)));



                return;



            }



            AppendLog($"[TX] {message}");



        }



        public void AppendLog(string message)



        {



            if (InvokeRequired)



            {



                BeginInvoke(new Action(() => AppendLog(message)));



                return;



            }



            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");



            txtSerialLog.AppendText($"[{timestamp}] {message}{Environment.NewLine}");



            txtSerialLog.SelectionStart = txtSerialLog.Text.Length;



            txtSerialLog.ScrollToCaret();



        }



        private void btnClearLog_Click(object sender, EventArgs e)



        {



            txtSerialLog.Clear();



            AppendLog("Đã xóa trắng nhật ký màn hình.");



        }



        private void btnSendCmd_Click(object sender, EventArgs e)



        {



            SendCustomCommand();



        }



        private void txtLogCmd_KeyDown(object sender, KeyEventArgs e)



        {



            if (e.KeyCode == Keys.Enter)



            {



                e.SuppressKeyPress = true;



                SendCustomCommand();



            }



        }



        private void SendCustomCommand()



        {



            string cmd = txtLogCmd.Text.Trim();



            if (string.IsNullOrEmpty(cmd)) return;



            if (_serialManager != null && _serialManager.IsRunning)



            {



                _serialManager.Send(cmd);



            }



            else



            {



                AppendLog($"[SIM_TX] {cmd} (Chưa mở cổng COM - Hãy vào Cài đặt để kết nối)");



            }



            txtLogCmd.Clear();



        }



        private void FormRobot_FormClosing(object sender, FormClosingEventArgs e)



        {



            UITheme.ThemeChanged -= ApplyTheme;



            if (_serialManager != null)
            {
                _serialManager.SerialStatus -= OnSerialStatusChanged;
                _serialManager.DataReceivedString -= OnSerialDataReceived;
                _serialManager.DataSentString -= OnSerialDataSent;
                _serialManager.TelemetryReceived -= OnTelemetryReceived;
                _serialManager.TeachPointsReceived -= OnTeachPointsReceived;
                _serialManager.AckReceived -= OnSerialAckReceived;
            }



            _debounceSerialTimer?.Stop();



            _debounceSerialTimer?.Dispose();



            _debounceSerialTimer = null;



            _animTimer?.Stop();



            _animTimer?.Dispose();



            _animTimer = null;



            if (_robotHost != null)



            {



                panelRobot.Controls.Remove(_robotHost);



                _robotHost.Child = null;



                _robotHost.Dispose();



                _robotHost = null;



            }



        }



        private bool _isApplyingTelemetry = false;



        private void OnTelemetryReceived(float j1, float j2, float j3, float j4, float x, float y, float z)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnTelemetryReceived(j1, j2, j3, j4, x, y, z)));
                return;
            }

            if (_isApplyingTelemetry) return;
            _isApplyingTelemetry = true;

            try
            {
                // Đồng bộ góc khớp và tọa độ XYZ lên mô hình Robot 3D
                SyncJointsTo3D(j1, j2, j3, j4, x, y, z);

                // Cập nhật giá trị hiển thị trên numeric & slider nếu người dùng không thao tác chuột
                if (!_isUpdatingUi && !_isUpdatingXYZ)
                {
                    _isUpdatingUi = true;
                    _isUpdatingXYZ = true;
                    try
                    {
                        if (numJ1 != null) numJ1.Value = (decimal)Math.Clamp(Math.Round(j1, 1), -90, 90);
                        if (numJ2 != null) numJ2.Value = (decimal)Math.Clamp(Math.Round(j2, 1), 0, 180);
                        if (numJ3 != null) numJ3.Value = (decimal)Math.Clamp(Math.Round(j3, 1), 0, 150);

                        if (tbJ1 != null) tbJ1.Value = (int)Math.Clamp(Math.Round(j1 * 10.0), -900, 900);
                        if (tbJ2 != null) tbJ2.Value = (int)Math.Clamp(Math.Round(j2 * 10.0), 0, 1800);
                        if (tbJ3 != null) tbJ3.Value = (int)Math.Clamp(Math.Round(j3 * 10.0), 0, 1500);

                        _lastSafeJ1 = j1;
                        _lastSafeJ2 = j2;
                        _lastSafeJ3 = j3;

                        if (numCoordX != null) numCoordX.Value = (decimal)Math.Clamp(Math.Round(x, 1), (double)numCoordX.Minimum, (double)numCoordX.Maximum);
                        if (numCoordY != null) numCoordY.Value = (decimal)Math.Clamp(Math.Round(y, 1), (double)numCoordY.Minimum, (double)numCoordY.Maximum);
                        if (numCoordZ != null) numCoordZ.Value = (decimal)Math.Clamp(Math.Round(z, 1), (double)numCoordZ.Minimum, (double)numCoordZ.Maximum);
                    }
                    finally
                    {
                        _isUpdatingUi = false;
                        _isUpdatingXYZ = false;
                    }
                }
            }
            catch { }
            finally
            {
                _isApplyingTelemetry = false;
            }
        }

        
        private void OnSerialAckReceived(byte cmd, byte status)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnSerialAckReceived(cmd, status)));
                return;
            }

            if (cmd == DatProtocol.CMD_SET_TEACH)
            {
                if (status == DatProtocol.ACK_STATUS_OK)
                {
                    AppendLog("[FLASH SUCCESS] ✔ ESP32 đã lưu thành công tọa độ Teach vào bộ nhớ Flash NVS (Preferences)!");
                }
                else
                {
                    AppendLog($"[FLASH ERROR] ❌ ESP32 báo lỗi khi lưu Flash: Mã lỗi {status}");
                }
            }
        }

        private void OnTeachPointsReceived(short[][] points)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnTeachPointsReceived(points)));
                return;
            }

            if (points == null || points.Length < 5) return;

            bool hasValidData = false;
            for (int i = 0; i < 5; i++)
            {
                if (points[i][0] != 0 || points[i][1] != 0 || points[i][2] != 0)
                {
                    hasValidData = true;
                    break;
                }
            }

            if (!hasValidData) return;

            _teachConfig.StartPoint = JointPose.FromXYZ(points[0][0], points[0][1], points[0][2]);
            _teachConfig.PickPoint = JointPose.FromXYZ(points[1][0], points[1][1], points[1][2]);
            _teachConfig.DropRusted = JointPose.FromXYZ(points[2][0], points[2][1], points[2][2]);
            _teachConfig.DropDamagedShield = JointPose.FromXYZ(points[3][0], points[3][1], points[3][2]);
            _teachConfig.DropSurfaceDamage = JointPose.FromXYZ(points[4][0], points[4][1], points[4][2]);

            _teachConfig.Save();
            UpdateTeachUI();

            AppendLog("[TEACH SYNC] Đã đồng bộ thành công 5 điểm Teach từ Flash của ESP32!");
        }
    }
}
