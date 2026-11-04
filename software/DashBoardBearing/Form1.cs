using FontAwesome.Sharp;
using RobotView;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DashBoardBearing
{
    public partial class Form1 : Form
    {
        private Form? currentForm;
        private IconButton? currentBtn;
        private Panel leftBorderBtn;

        public CameraManager cameraManager;
        public SerialManager serialManager;
        private Robot4DOF? robotManger;

        private bool isCameraOnline = false;
        private bool isRobotOnline = false;

        public Form1()
        {
            InitializeComponent();

            leftBorderBtn = new Panel
            {
                Size = new Size(5, 55),
                BackColor = UITheme.Primary,
                Visible = false
            };
            panelSideBar.Controls.Add(leftBorderBtn);

            // Resilient ONNX model path detection
            string yoloPath = FindModelPath("yolo_bearing.onnx");
            string resnetPath = FindModelPath("resnet34_bearing.onnx");

            cameraManager = new CameraManager(yoloPath, resnetPath);
            cameraManager.CameraStatus += CameraStatusChanged;

            serialManager = new SerialManager();
            serialManager.SerialStatus += SerialPortStatusChanged;
            serialManager.serialPort = new System.IO.Ports.SerialPort();
            serialManager.serialPort.DataReceived += DataReceived;

            FormHome.SystemRunStatusChanged += UpdateSystemRunStatusBadge;
            panelContent.Resize += PanelContent_Resize;
            Resize += Form1_Resize;
            UITheme.ThemeChanged += ApplyTheme;
            ApplyTheme();
            UpdateClock();
        }

        private static string FindModelPath(string fileName)
        {
            string[] searchPaths = new[]
            {
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model", fileName),
                Path.Combine(Application.StartupPath, "Model", fileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Model", fileName),
                Path.Combine("D:\\Bearing-Classification\\BEARING-CLASSIFICATION-SYSTEM\\software\\DashBoardBearing\\Model", fileName),
                Path.Combine("D:\\C-SHARP\\DashBoardBearing\\Model", fileName)
            };

            foreach (string p in searchPaths)
            {
                try
                {
                    if (File.Exists(p))
                        return Path.GetFullPath(p);
                }
                catch { }
            }

            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model", fileName);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Auto open Home Dashboard on startup
            btnHome.PerformClick();
        }

        private void btnThemeToggle_Click(object sender, EventArgs e)
        {
            UITheme.ToggleTheme();
        }

        private void ApplyTheme()
        {
            BackColor = UITheme.BgDark;
            panelMain.BackColor = UITheme.BgDark;
            panelContent.BackColor = UITheme.BgDark;
            panelHeader.BackColor = UITheme.BgCard;
            flowHeaderRight.BackColor = UITheme.BgCard;
            panelSideBar.BackColor = UITheme.BgSidebar;
            panelSideBottom.BackColor = UITheme.BgCardAlt;

            lblPageTitle.ForeColor = UITheme.TextPrimary;
            lblBrandTitle.ForeColor = UITheme.TextPrimary;
            lblBrandSubtitle.ForeColor = UITheme.PrimaryLight;
            lblVersion.ForeColor = UITheme.TextPrimary;
            lblTech.ForeColor = UITheme.TextMuted;
            lblClock.ForeColor = UITheme.TextSecondary;

            // Theme toggle button style
            if (UITheme.IsDarkMode)
            {
                btnThemeToggle.IconChar = IconChar.Sun;
                btnThemeToggle.Text = "  Sáng";
                btnThemeToggle.ForeColor = Color.FromArgb(253, 224, 71);
                btnThemeToggle.IconColor = Color.FromArgb(253, 224, 71);
                btnThemeToggle.BackColor = UITheme.BgCardAlt;
            }
            else
            {
                btnThemeToggle.IconChar = IconChar.Moon;
                btnThemeToggle.Text = "  Tối";
                btnThemeToggle.ForeColor = UITheme.TextPrimary;
                btnThemeToggle.IconColor = UITheme.TextPrimary;
                btnThemeToggle.BackColor = UITheme.BgCardAlt;
            }

            // Navigation buttons
            IconButton[] navButtons = { btnHome, btnVision, btnRobot, btnLog, btnSetting };
            foreach (var btn in navButtons)
            {
                if (btn == currentBtn)
                {
                    btn.BackColor = UITheme.BgCardAlt;
                    btn.ForeColor = UITheme.PrimaryLight;
                    btn.IconColor = UITheme.PrimaryLight;
                }
                else
                {
                    btn.BackColor = UITheme.BgSidebar;
                    btn.ForeColor = UITheme.TextSecondary;
                    btn.IconColor = UITheme.TextSecondary;
                }
            }

            leftBorderBtn.BackColor = UITheme.Primary;

            // Refresh status badge backgrounds
            UpdateCameraBadgeUI();
            UpdateRobotBadgeUI();
            UpdateSystemRunStatusBadge(FormHome.IsSystemRunning);
        }

        private void timerClock_Tick(object sender, EventArgs e)
        {
            UpdateClock();
        }

        private void UpdateClock()
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm:ss  |  dd/MM/yyyy");
        }

        private void DataReceived(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {
            }
        }

        private void OpenForm(Form form, string title)
        {
            if (currentForm != null)
            {
                panelContent.Controls.Remove(currentForm);
                currentForm.Close();
                currentForm.Dispose();
                currentForm = null;
            }

            lblPageTitle.Text = title;

            currentForm = form;
            currentForm.TopLevel = false;
            currentForm.FormBorderStyle = FormBorderStyle.None;
            currentForm.Bounds = panelContent.ClientRectangle;
            currentForm.Dock = DockStyle.Fill;
            panelContent.Controls.Add(currentForm);
            panelContent.Tag = currentForm;
            currentForm.BringToFront();
            currentForm.Show();
            ResizeCurrentForm();
        }

        private void PanelContent_Resize(object? sender, EventArgs e)
        {
            ResizeCurrentForm();
        }

        private void Form1_Resize(object? sender, EventArgs e)
        {
            ResizeCurrentForm();
        }

        private void ResizeCurrentForm()
        {
            if (currentForm != null && !currentForm.IsDisposed)
            {
                currentForm.Bounds = panelContent.ClientRectangle;
            }
        }

        private void ActivateButton(object senderBtn)
        {
            if (senderBtn == null) return;

            DisableButton();

            currentBtn = (IconButton)senderBtn;
            currentBtn.BackColor = UITheme.BgCardAlt;
            currentBtn.ForeColor = UITheme.PrimaryLight;
            currentBtn.IconColor = UITheme.PrimaryLight;

            leftBorderBtn.BackColor = UITheme.Primary;
            leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
            leftBorderBtn.Visible = true;
            leftBorderBtn.BringToFront();
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = UITheme.BgSidebar;
                currentBtn.ForeColor = UITheme.TextSecondary;
                currentBtn.IconColor = UITheme.TextSecondary;
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenForm(new FormHome(cameraManager, serialManager), "TỔNG QUAN HỆ THỐNG");
        }

        private void btnVision_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenForm(new FormVision(cameraManager), "THỊ GIÁC MÁY TÍNH");
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenForm(new FormRobot(null, serialManager), "ĐIỀU KHIỂN ROBOT 3D");
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenForm(new FormLog(), "NHẬT KÝ HỆ THỐNG");
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenForm(new FormSetting(cameraManager, serialManager), "CÀI ĐẶT CẤU HÌNH");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UITheme.ThemeChanged -= ApplyTheme;
            FormHome.SystemRunStatusChanged -= UpdateSystemRunStatusBadge;

            if (currentForm != null)
            {
                panelContent.Controls.Remove(currentForm);
                currentForm.Close();
                currentForm.Dispose();
                currentForm = null;
            }

            if (robotManger != null)
            {
                robotManger?.Dispose();
                robotManger = null;
            }
        }

        private void UpdateSystemRunStatusBadge(bool isRunning)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateSystemRunStatusBadge(isRunning)));
                return;
            }

            if (isRunning)
            {
                lblSystemBadge.Text = "● HỆ THỐNG: HOẠT ĐỘNG";
                lblSystemBadge.ForeColor = UITheme.Success;
                pnlStatusSystem.BackColor = UITheme.SuccessBg;
            }
            else
            {
                lblSystemBadge.Text = "● HỆ THỐNG: ĐANG DỪNG";
                lblSystemBadge.ForeColor = UITheme.Danger;
                pnlStatusSystem.BackColor = UITheme.DangerBg;
            }
        }

        private void CameraStatusChanged(bool connected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => CameraStatusChanged(connected)));
                return;
            }

            isCameraOnline = connected;
            UpdateCameraBadgeUI();
        }

        private void UpdateCameraBadgeUI()
        {
            if (isCameraOnline)
            {
                lblCameraBadge.Text = "● CAMERA: ONLINE";
                lblCameraBadge.ForeColor = UITheme.Success;
                pnlStatusCamera.BackColor = UITheme.SuccessBg;
            }
            else
            {
                lblCameraBadge.Text = "● CAMERA: OFFLINE";
                lblCameraBadge.ForeColor = UITheme.Danger;
                pnlStatusCamera.BackColor = UITheme.DangerBg;
            }
        }

        private void SerialPortStatusChanged(bool connected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => SerialPortStatusChanged(connected)));
                return;
            }

            isRobotOnline = connected;
            UpdateRobotBadgeUI();
        }

        private void UpdateRobotBadgeUI()
        {
            if (isRobotOnline)
            {
                lblRobotBadge.Text = "● ROBOT: CONNECTED";
                lblRobotBadge.ForeColor = UITheme.Success;
                pnlStatusRobot.BackColor = UITheme.SuccessBg;
            }
            else
            {
                lblRobotBadge.Text = "● ROBOT: OFFLINE";
                lblRobotBadge.ForeColor = UITheme.Danger;
                pnlStatusRobot.BackColor = UITheme.DangerBg;
            }
        }
    }
}
