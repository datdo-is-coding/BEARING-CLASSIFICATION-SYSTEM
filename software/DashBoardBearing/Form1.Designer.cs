namespace DashBoardBearing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelSideBar = new Panel();
            panelSideBottom = new Panel();
            lblVersion = new Label();
            lblTech = new Label();
            btnSetting = new FontAwesome.Sharp.IconButton();
            btnLog = new FontAwesome.Sharp.IconButton();
            btnRobot = new FontAwesome.Sharp.IconButton();
            btnVision = new FontAwesome.Sharp.IconButton();
            btnHome = new FontAwesome.Sharp.IconButton();
            panelLogo = new Panel();
            lblBrandSubtitle = new Label();
            lblBrandTitle = new Label();
            picLogo = new PictureBox();
            panelMain = new Panel();
            panelContent = new Panel();
            panelHeader = new Panel();
            flowHeaderRight = new FlowLayoutPanel();
            btnThemeToggle = new FontAwesome.Sharp.IconButton();
            lblClock = new Label();
            pnlStatusSystem = new Panel();
            lblSystemBadge = new Label();
            pnlStatusRobot = new Panel();
            lblRobotBadge = new Label();
            pnlStatusCamera = new Panel();
            lblCameraBadge = new Label();
            lblPageTitle = new Label();
            timerClock = new System.Windows.Forms.Timer(components);
            panelSideBar.SuspendLayout();
            panelSideBottom.SuspendLayout();
            panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panelMain.SuspendLayout();
            panelHeader.SuspendLayout();
            flowHeaderRight.SuspendLayout();
            pnlStatusSystem.SuspendLayout();
            pnlStatusRobot.SuspendLayout();
            pnlStatusCamera.SuspendLayout();
            SuspendLayout();
            // 
            // panelSideBar
            // 
            panelSideBar.BackColor = Color.White;
            panelSideBar.Controls.Add(panelSideBottom);
            panelSideBar.Controls.Add(btnSetting);
            panelSideBar.Controls.Add(btnLog);
            panelSideBar.Controls.Add(btnRobot);
            panelSideBar.Controls.Add(btnVision);
            panelSideBar.Controls.Add(btnHome);
            panelSideBar.Controls.Add(panelLogo);
            panelSideBar.Dock = DockStyle.Left;
            panelSideBar.Location = new Point(0, 0);
            panelSideBar.Name = "panelSideBar";
            panelSideBar.Size = new Size(240, 780);
            panelSideBar.TabIndex = 0;
            // 
            // panelSideBottom
            // 
            panelSideBottom.BackColor = Color.FromArgb(248, 250, 252);
            panelSideBottom.Controls.Add(lblVersion);
            panelSideBottom.Controls.Add(lblTech);
            panelSideBottom.Dock = DockStyle.Bottom;
            panelSideBottom.Location = new Point(0, 715);
            panelSideBottom.Name = "panelSideBottom";
            panelSideBottom.Padding = new Padding(12);
            panelSideBottom.Size = new Size(240, 65);
            panelSideBottom.TabIndex = 7;
            // 
            // lblVersion
            // 
            lblVersion.Dock = DockStyle.Top;
            lblVersion.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVersion.ForeColor = Color.FromArgb(15, 23, 42);
            lblVersion.Location = new Point(12, 12);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(216, 20);
            lblVersion.TabIndex = 0;
            lblVersion.Text = "BEARING AI • v2.0";
            lblVersion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTech
            // 
            lblTech.Dock = DockStyle.Bottom;
            lblTech.Font = new Font("Segoe UI", 8F);
            lblTech.ForeColor = Color.FromArgb(100, 116, 139);
            lblTech.Location = new Point(12, 35);
            lblTech.Name = "lblTech";
            lblTech.Size = new Size(216, 18);
            lblTech.TabIndex = 1;
            lblTech.Text = "YOLO + ResNet + 4DOF Robot";
            lblTech.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSetting
            // 
            btnSetting.Dock = DockStyle.Top;
            btnSetting.FlatAppearance.BorderSize = 0;
            btnSetting.FlatStyle = FlatStyle.Flat;
            btnSetting.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSetting.ForeColor = Color.FromArgb(71, 85, 105);
            btnSetting.IconChar = FontAwesome.Sharp.IconChar.Sliders;
            btnSetting.IconColor = Color.FromArgb(71, 85, 105);
            btnSetting.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSetting.IconSize = 22;
            btnSetting.ImageAlign = ContentAlignment.MiddleLeft;
            btnSetting.Location = new Point(0, 395);
            btnSetting.Name = "btnSetting";
            btnSetting.Padding = new Padding(22, 0, 10, 0);
            btnSetting.Size = new Size(240, 55);
            btnSetting.TabIndex = 6;
            btnSetting.Text = "   CÀI ĐẶT";
            btnSetting.TextAlign = ContentAlignment.MiddleLeft;
            btnSetting.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnLog
            // 
            btnLog.Dock = DockStyle.Top;
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLog.ForeColor = Color.FromArgb(71, 85, 105);
            btnLog.IconChar = FontAwesome.Sharp.IconChar.ClockRotateLeft;
            btnLog.IconColor = Color.FromArgb(71, 85, 105);
            btnLog.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnLog.IconSize = 22;
            btnLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnLog.Location = new Point(0, 340);
            btnLog.Name = "btnLog";
            btnLog.Padding = new Padding(22, 0, 10, 0);
            btnLog.Size = new Size(240, 55);
            btnLog.TabIndex = 5;
            btnLog.Text = "   NHẬT KÝ";
            btnLog.TextAlign = ContentAlignment.MiddleLeft;
            btnLog.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // btnRobot
            // 
            btnRobot.Dock = DockStyle.Top;
            btnRobot.FlatAppearance.BorderSize = 0;
            btnRobot.FlatStyle = FlatStyle.Flat;
            btnRobot.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRobot.ForeColor = Color.FromArgb(71, 85, 105);
            btnRobot.IconChar = FontAwesome.Sharp.IconChar.Robot;
            btnRobot.IconColor = Color.FromArgb(71, 85, 105);
            btnRobot.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRobot.IconSize = 22;
            btnRobot.ImageAlign = ContentAlignment.MiddleLeft;
            btnRobot.Location = new Point(0, 285);
            btnRobot.Name = "btnRobot";
            btnRobot.Padding = new Padding(22, 0, 10, 0);
            btnRobot.Size = new Size(240, 55);
            btnRobot.TabIndex = 4;
            btnRobot.Text = "   ROBOT 3D";
            btnRobot.TextAlign = ContentAlignment.MiddleLeft;
            btnRobot.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRobot.UseVisualStyleBackColor = true;
            btnRobot.Click += btnRobot_Click;
            // 
            // btnVision
            // 
            btnVision.Dock = DockStyle.Top;
            btnVision.FlatAppearance.BorderSize = 0;
            btnVision.FlatStyle = FlatStyle.Flat;
            btnVision.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnVision.ForeColor = Color.FromArgb(71, 85, 105);
            btnVision.IconChar = FontAwesome.Sharp.IconChar.Camera;
            btnVision.IconColor = Color.FromArgb(71, 85, 105);
            btnVision.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnVision.IconSize = 22;
            btnVision.ImageAlign = ContentAlignment.MiddleLeft;
            btnVision.Location = new Point(0, 230);
            btnVision.Name = "btnVision";
            btnVision.Padding = new Padding(22, 0, 10, 0);
            btnVision.Size = new Size(240, 55);
            btnVision.TabIndex = 3;
            btnVision.Text = "   VISION";
            btnVision.TextAlign = ContentAlignment.MiddleLeft;
            btnVision.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnVision.UseVisualStyleBackColor = true;
            btnVision.Click += btnVision_Click;
            // 
            // btnHome
            // 
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnHome.ForeColor = Color.FromArgb(71, 85, 105);
            btnHome.IconChar = FontAwesome.Sharp.IconChar.GaugeHigh;
            btnHome.IconColor = Color.FromArgb(71, 85, 105);
            btnHome.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHome.IconSize = 22;
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 175);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(22, 0, 10, 0);
            btnHome.Size = new Size(240, 55);
            btnHome.TabIndex = 2;
            btnHome.Text = "   DASHBOARD";
            btnHome.TextAlign = ContentAlignment.MiddleLeft;
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            // 
            // panelLogo
            // 
            panelLogo.Controls.Add(lblBrandSubtitle);
            panelLogo.Controls.Add(lblBrandTitle);
            panelLogo.Controls.Add(picLogo);
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(240, 175);
            panelLogo.TabIndex = 1;
            // 
            // lblBrandSubtitle
            // 
            lblBrandSubtitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblBrandSubtitle.ForeColor = Color.FromArgb(37, 99, 235);
            lblBrandSubtitle.Location = new Point(12, 145);
            lblBrandSubtitle.Name = "lblBrandSubtitle";
            lblBrandSubtitle.Size = new Size(216, 20);
            lblBrandSubtitle.TabIndex = 2;
            lblBrandSubtitle.Text = "AI INSPECTION & SORTING";
            lblBrandSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblBrandTitle
            // 
            lblBrandTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBrandTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblBrandTitle.Location = new Point(12, 118);
            lblBrandTitle.Name = "lblBrandTitle";
            lblBrandTitle.Size = new Size(216, 24);
            lblBrandTitle.TabIndex = 1;
            lblBrandTitle.Text = "BEARING SYSTEM";
            lblBrandTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.logo;
            picLogo.Location = new Point(65, 16);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(110, 95);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.FromArgb(241, 245, 249);
            panelMain.Controls.Add(panelContent);
            panelMain.Controls.Add(panelHeader);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(240, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(1040, 780);
            panelMain.TabIndex = 1;
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.FromArgb(241, 245, 249);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(0, 60);
            panelContent.Name = "panelContent";
            panelContent.Size = new Size(1040, 720);
            panelContent.TabIndex = 2;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.White;
            panelHeader.Controls.Add(lblPageTitle);
            panelHeader.Controls.Add(flowHeaderRight);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Padding = new Padding(20, 0, 16, 0);
            panelHeader.Size = new Size(1040, 60);
            panelHeader.TabIndex = 1;
            // 
            // flowHeaderRight
            // 
            flowHeaderRight.AutoSize = true;
            flowHeaderRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowHeaderRight.BackColor = Color.Transparent;
            flowHeaderRight.Controls.Add(lblClock);
            flowHeaderRight.Controls.Add(btnThemeToggle);
            flowHeaderRight.Controls.Add(pnlStatusSystem);
            flowHeaderRight.Controls.Add(pnlStatusRobot);
            flowHeaderRight.Controls.Add(pnlStatusCamera);
            flowHeaderRight.Dock = DockStyle.Right;
            flowHeaderRight.FlowDirection = FlowDirection.RightToLeft;
            flowHeaderRight.Location = new Point(164, 0);
            flowHeaderRight.Name = "flowHeaderRight";
            flowHeaderRight.Padding = new Padding(0, 14, 0, 14);
            flowHeaderRight.Size = new Size(860, 60);
            flowHeaderRight.TabIndex = 1;
            flowHeaderRight.WrapContents = false;
            // 
            // lblClock
            // 
            lblClock.Font = new Font("Consolas", 9.5F, FontStyle.Bold);
            lblClock.ForeColor = Color.FromArgb(71, 85, 105);
            lblClock.Location = new Point(671, 14);
            lblClock.Margin = new Padding(4, 0, 4, 0);
            lblClock.Name = "lblClock";
            lblClock.Size = new Size(85, 32);
            lblClock.TabIndex = 4;
            lblClock.Text = "00:00:00";
            lblClock.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnThemeToggle
            // 
            btnThemeToggle.BackColor = Color.FromArgb(241, 245, 249);
            btnThemeToggle.Cursor = Cursors.Hand;
            btnThemeToggle.FlatAppearance.BorderSize = 0;
            btnThemeToggle.FlatStyle = FlatStyle.Flat;
            btnThemeToggle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnThemeToggle.ForeColor = Color.FromArgb(15, 23, 42);
            btnThemeToggle.IconChar = FontAwesome.Sharp.IconChar.Moon;
            btnThemeToggle.IconColor = Color.FromArgb(15, 23, 42);
            btnThemeToggle.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnThemeToggle.IconSize = 16;
            btnThemeToggle.ImageAlign = ContentAlignment.MiddleLeft;
            btnThemeToggle.Location = new Point(576, 14);
            btnThemeToggle.Margin = new Padding(6, 0, 6, 0);
            btnThemeToggle.Name = "btnThemeToggle";
            btnThemeToggle.Size = new Size(70, 32);
            btnThemeToggle.TabIndex = 3;
            btnThemeToggle.Text = "  Tối";
            btnThemeToggle.TextAlign = ContentAlignment.MiddleCenter;
            btnThemeToggle.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnThemeToggle.UseVisualStyleBackColor = false;
            btnThemeToggle.Click += btnThemeToggle_Click;
            // 
            // pnlStatusSystem
            // 
            pnlStatusSystem.BackColor = Color.FromArgb(254, 226, 226);
            pnlStatusSystem.Controls.Add(lblSystemBadge);
            pnlStatusSystem.Location = new Point(346, 14);
            pnlStatusSystem.Margin = new Padding(6, 0, 6, 0);
            pnlStatusSystem.Name = "pnlStatusSystem";
            pnlStatusSystem.Size = new Size(180, 32);
            pnlStatusSystem.TabIndex = 2;
            // 
            // lblSystemBadge
            // 
            lblSystemBadge.Dock = DockStyle.Fill;
            lblSystemBadge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblSystemBadge.ForeColor = Color.FromArgb(239, 68, 68);
            lblSystemBadge.Location = new Point(0, 0);
            lblSystemBadge.Name = "lblSystemBadge";
            lblSystemBadge.Size = new Size(180, 32);
            lblSystemBadge.TabIndex = 0;
            lblSystemBadge.Text = "● HỆ THỐNG: ĐANG DỪNG";
            lblSystemBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlStatusRobot
            // 
            pnlStatusRobot.BackColor = Color.FromArgb(254, 226, 226);
            pnlStatusRobot.Controls.Add(lblRobotBadge);
            pnlStatusRobot.Location = new Point(175, 14);
            pnlStatusRobot.Margin = new Padding(6, 0, 6, 0);
            pnlStatusRobot.Name = "pnlStatusRobot";
            pnlStatusRobot.Size = new Size(140, 32);
            pnlStatusRobot.TabIndex = 1;
            // 
            // lblRobotBadge
            // 
            lblRobotBadge.Dock = DockStyle.Fill;
            lblRobotBadge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblRobotBadge.ForeColor = Color.FromArgb(239, 68, 68);
            lblRobotBadge.Location = new Point(0, 0);
            lblRobotBadge.Name = "lblRobotBadge";
            lblRobotBadge.Size = new Size(140, 32);
            lblRobotBadge.TabIndex = 0;
            lblRobotBadge.Text = "● ROBOT: OFFLINE";
            lblRobotBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlStatusCamera
            // 
            pnlStatusCamera.BackColor = Color.FromArgb(254, 226, 226);
            pnlStatusCamera.Controls.Add(lblCameraBadge);
            pnlStatusCamera.Location = new Point(9, 14);
            pnlStatusCamera.Margin = new Padding(6, 0, 6, 0);
            pnlStatusCamera.Name = "pnlStatusCamera";
            pnlStatusCamera.Size = new Size(140, 32);
            pnlStatusCamera.TabIndex = 0;
            // 
            // lblCameraBadge
            // 
            lblCameraBadge.Dock = DockStyle.Fill;
            lblCameraBadge.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblCameraBadge.ForeColor = Color.FromArgb(239, 68, 68);
            lblCameraBadge.Location = new Point(0, 0);
            lblCameraBadge.Name = "lblCameraBadge";
            lblCameraBadge.Size = new Size(140, 32);
            lblCameraBadge.TabIndex = 0;
            lblCameraBadge.Text = "● CAMERA: OFFLINE";
            lblCameraBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPageTitle
            // 
            lblPageTitle.Dock = DockStyle.Fill;
            lblPageTitle.AutoEllipsis = true;
            lblPageTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblPageTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblPageTitle.Location = new Point(20, 0);
            lblPageTitle.Name = "lblPageTitle";
            lblPageTitle.Size = new Size(144, 60);
            lblPageTitle.TabIndex = 0;
            lblPageTitle.Text = "TỔNG QUAN HỆ THỐNG";
            lblPageTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // timerClock
            // 
            timerClock.Enabled = true;
            timerClock.Interval = 1000;
            timerClock.Tick += timerClock_Tick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1280, 780);
            Controls.Add(panelMain);
            Controls.Add(panelSideBar);
            MinimumSize = new Size(1180, 720);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hệ Thống Phân Loại Vòng Bi Tự Động - Bearing Classification AI";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            panelSideBar.ResumeLayout(false);
            panelSideBottom.ResumeLayout(false);
            panelLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panelMain.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            flowHeaderRight.ResumeLayout(false);
            pnlStatusSystem.ResumeLayout(false);
            pnlStatusRobot.ResumeLayout(false);
            pnlStatusCamera.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSideBar;
        private PictureBox picLogo;
        private FontAwesome.Sharp.IconButton btnHome;
        private Panel panelLogo;
        private FontAwesome.Sharp.IconButton btnLog;
        private FontAwesome.Sharp.IconButton btnRobot;
        private FontAwesome.Sharp.IconButton btnVision;
        private Panel panelMain;
        private FontAwesome.Sharp.IconButton btnSetting;
        private Panel panelHeader;
        private FlowLayoutPanel flowHeaderRight;
        private Panel panelContent;
        private Label lblPageTitle;
        private Panel pnlStatusCamera;
        private Label lblCameraBadge;
        private Panel pnlStatusRobot;
        private Label lblRobotBadge;
        private Panel pnlStatusSystem;
        private Label lblSystemBadge;
        private Label lblClock;
        private System.Windows.Forms.Timer timerClock;
        private Label lblBrandTitle;
        private Label lblBrandSubtitle;
        private Panel panelSideBottom;
        private Label lblVersion;
        private Label lblTech;
        private FontAwesome.Sharp.IconButton btnThemeToggle;
    }
}
