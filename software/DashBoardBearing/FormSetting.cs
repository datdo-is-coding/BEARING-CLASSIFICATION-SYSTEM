using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;

namespace DashBoardBearing
{
    public partial class FormSetting : Form
    {
        private readonly CameraManager _cam;
        private readonly SerialManager _serial;

        // Map: vị trí trong cbWebcam -> index camera thật của OpenCV
        private readonly List<CameraDeviceInfo> _cameraDevices = new List<CameraDeviceInfo>();
        private readonly List<int> _cameraIndices = new List<int>();

        public FormSetting(CameraManager cam, SerialManager serial)
        {
            InitializeComponent();

            _cam = cam;
            _serial = serial;

            if (_cam != null)
            {
                _cam.FrameReady += Camera_FrameReady;
            }

            UITheme.ThemeChanged += ApplyTheme;
        }

        private void ApplyTheme()
        {
            BackColor = UITheme.BgDark;

            cardCamera.BackgroundColor = UITheme.BgCard;
            cardCamera.BorderColor = UITheme.BorderCard;

            cardSerial.BackgroundColor = UITheme.BgCard;
            cardSerial.BorderColor = UITheme.BorderCard;

            lblCamHeaderTitle.ForeColor = UITheme.TextPrimary;
            lblSerialHeaderTitle.ForeColor = UITheme.TextPrimary;

            lblWebcamTitle.ForeColor = UITheme.TextSecondary;
            lblIpTitle.ForeColor = UITheme.TextSecondary;
            lblPreviewTitle.ForeColor = UITheme.TextSecondary;

            lblComTitle.ForeColor = UITheme.TextSecondary;
            lblBaud.ForeColor = UITheme.TextSecondary;
            lblDataSize.ForeColor = UITheme.TextSecondary;
            lblParity.ForeColor = UITheme.TextSecondary;
            lblStopBit.ForeColor = UITheme.TextSecondary;

            ComboBox[] combos = { cbWebcam, cbCOM, cbBaud, cbDataSize, cbParityBit, cbStopBit };
            foreach (var cb in combos)
            {
                cb.BackColor = UITheme.BgInput;
                cb.ForeColor = UITheme.TextPrimary;
            }

            txtIP.BackColor = UITheme.BgInput;
            txtIP.ForeColor = UITheme.TextPrimary;

            picWebcam.BackColor = UITheme.BgViewport;

            btnRefreshCam.BackColor = UITheme.BgCardAlt;
            btnRefreshCam.ForeColor = UITheme.TextSecondary;
            btnRefreshCam.IconColor = UITheme.TextSecondary;
            btnRefreshCam.FlatAppearance.BorderColor = UITheme.BorderCard;

            btnRefreshPorts.BackColor = UITheme.BgCardAlt;
            btnRefreshPorts.ForeColor = UITheme.TextSecondary;
            btnRefreshPorts.IconColor = UITheme.TextSecondary;
            btnRefreshPorts.FlatAppearance.BorderColor = UITheme.BorderCard;
        }

        // =========================================================
        // CAMERA
        // =========================================================

        public void LoadWebcam()
        {
            cbWebcam.Items.Clear();
            _cameraIndices.Clear();
            _cameraDevices.Clear();

            Cursor = Cursors.WaitCursor;
            try
            {
                var devices = CameraManager.GetAvailableCameraDevices();
                _cameraDevices.AddRange(devices);
                foreach (var d in devices)
                {
                    _cameraIndices.Add(d.Index);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi quét camera: {ex.Message}", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (_cameraDevices.Count == 0)
            {
                cbWebcam.Items.Add("Không tìm thấy thiết bị");
                cbWebcam.SelectedIndex = 0;
                return;
            }

            foreach (var dev in _cameraDevices)
            {
                cbWebcam.Items.Add($"{dev.Name} (Index {dev.Index})");
            }

            // Tự động chọn camera đang hoạt động (nếu có)
            int selected = -1;
            if (_cam != null && _cam.IsRunning)
            {
                for (int i = 0; i < _cameraDevices.Count; i++)
                {
                    if (_cameraDevices[i].Index == _cam.CurrentCameraIndex)
                    {
                        selected = i;
                        break;
                    }
                }
            }

            cbWebcam.SelectedIndex = selected >= 0 ? selected : 0;
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

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (cbWebcam.SelectedIndex < 0 || _cameraIndices.Count == 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn Camera từ danh sách.",
                    "Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int camIndex = _cameraIndices[cbWebcam.SelectedIndex];

            if (!_cam.Start(camIndex))
            {
                MessageBox.Show(
                    "Không thể kết nối Camera được chọn.",
                    "Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(
                    "Mở Camera thành công!",
                    "Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (_cam != null)
            {
                await _cam.StopAsync();

                if (picWebcam.Image != null)
                {
                    Image old = picWebcam.Image;
                    picWebcam.Image = null;
                    old.Dispose();
                }
            }
        }

        private void btnRefreshCam_Click(object sender, EventArgs e)
        {
            LoadWebcam();
        }

        private async void btnConnectIP_Click(object sender, EventArgs e)
        {
            string url = txtIP.Text.Trim();
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ RTSP / HTTP của camera.", "Camera IP", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!await _cam.StartIpAsync(url))
                {
                    MessageBox.Show("Không thể kết nối camera IP.", "Camera",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Kết nối Camera IP thành công!", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Camera IP", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // SERIAL
        // =========================================================

        private void LoadCOM()
        {
            cbCOM.Items.Clear();

            string[] ports = _serial.GetPorts();

            if (ports.Length == 0)
            {
                cbCOM.Items.Add("Không có cổng COM");
                cbCOM.SelectedIndex = 0;
                return;
            }

            cbCOM.Items.AddRange(ports);

            if (_serial.IsRunning)
            {
                int index = cbCOM.Items.IndexOf(_serial.CurrentPortName);

                if (index >= 0)
                    cbCOM.SelectedIndex = index;
            }
            else
            {
                cbCOM.SelectedIndex = 0;
            }
        }

        private void btnRefreshPorts_Click(object sender, EventArgs e)
        {
            LoadCOM();
        }

        private void LoadBaud()
        {
            cbBaud.Items.Clear();

            cbBaud.Items.Add("9600");
            cbBaud.Items.Add("19200");
            cbBaud.Items.Add("38400");
            cbBaud.Items.Add("57600");
            cbBaud.Items.Add("115200");

            if (_serial.IsRunning)
            {
                int index = cbBaud.Items.IndexOf(
                    _serial.CurrentBaudRate.ToString());

                if (index >= 0)
                    cbBaud.SelectedIndex = index;
            }
            else
            {
                cbBaud.SelectedIndex = cbBaud.Items.IndexOf("115200");
            }
        }

        private void LoadDataSize()
        {
            cbDataSize.Items.Clear();

            cbDataSize.Items.Add("5");
            cbDataSize.Items.Add("6");
            cbDataSize.Items.Add("7");
            cbDataSize.Items.Add("8");

            if (_serial.IsRunning)
            {
                int index = cbDataSize.Items.IndexOf(
                    _serial.CurrentDataBits.ToString());

                if (index >= 0)
                    cbDataSize.SelectedIndex = index;
            }
            else
            {
                cbDataSize.SelectedIndex = cbDataSize.Items.IndexOf("8");
            }
        }

        private void LoadParity()
        {
            cbParityBit.Items.Clear();

            cbParityBit.Items.Add("None");
            cbParityBit.Items.Add("Odd");
            cbParityBit.Items.Add("Even");
            cbParityBit.Items.Add("Mark");
            cbParityBit.Items.Add("Space");

            if (_serial.IsRunning)
            {
                int index = cbParityBit.Items.IndexOf(
                    _serial.CurrentParity.ToString());

                if (index >= 0)
                    cbParityBit.SelectedIndex = index;
            }
            else
            {
                cbParityBit.SelectedIndex = cbParityBit.Items.IndexOf("None");
            }
        }

        private void LoadStopBit()
        {
            cbStopBit.Items.Clear();

            cbStopBit.Items.Add("1");
            cbStopBit.Items.Add("1.5");
            cbStopBit.Items.Add("2");

            if (_serial.IsRunning)
            {
                string stopBit = "";

                switch (_serial.CurrentStopBits)
                {
                    case StopBits.One:
                        stopBit = "1";
                        break;

                    case StopBits.OnePointFive:
                        stopBit = "1.5";
                        break;

                    case StopBits.Two:
                        stopBit = "2";
                        break;
                }

                int index = cbStopBit.Items.IndexOf(stopBit);

                if (index >= 0)
                    cbStopBit.SelectedIndex = index;
            }
            else
            {
                cbStopBit.SelectedIndex = cbStopBit.Items.IndexOf("1");
            }
        }

        private void FormSetting_Load(object sender, EventArgs e)
        {
            ApplyTheme();

            // Camera
            LoadWebcam();

            // Serial
            LoadCOM();
            LoadBaud();
            LoadDataSize();
            LoadParity();
            LoadStopBit();
        }

        private void btnConnectSerial_Click(object sender, EventArgs e)
        {
            if (cbCOM.SelectedIndex < 0 ||
                cbCOM.SelectedItem.ToString().Contains("Không") ||
                cbBaud.SelectedIndex < 0 ||
                cbDataSize.SelectedIndex < 0 ||
                cbParityBit.SelectedIndex < 0 ||
                cbStopBit.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn đầy đủ thông số Serial hợp lệ.",
                    "Serial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string portName = cbCOM.SelectedItem.ToString();
            int baudRate = int.Parse(cbBaud.SelectedItem.ToString());
            int dataBits = int.Parse(cbDataSize.SelectedItem.ToString());

            Parity parity;

            switch (cbParityBit.SelectedItem.ToString())
            {
                case "None": parity = Parity.None; break;
                case "Odd": parity = Parity.Odd; break;
                case "Even": parity = Parity.Even; break;
                case "Mark": parity = Parity.Mark; break;
                case "Space": parity = Parity.Space; break;
                default:
                    MessageBox.Show("Parity không hợp lệ.");
                    return;
            }

            StopBits stopBits;

            switch (cbStopBit.SelectedItem.ToString())
            {
                case "1": stopBits = StopBits.One; break;
                case "1.5": stopBits = StopBits.OnePointFive; break;
                case "2": stopBits = StopBits.Two; break;
                default:
                    MessageBox.Show("Stop bit không hợp lệ.");
                    return;
            }

            bool result = _serial.Connect(
                portName, baudRate, dataBits, parity, stopBits);

            if (!result)
            {
                MessageBox.Show(
                    "Không thể kết nối Serial.\n\n" +
                    "Kiểm tra lại COM hoặc quyền truy cập cổng.",
                    "Serial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show(
                $"Kết nối Serial thành công qua cổng {portName} ({baudRate} bps)!",
                "Serial",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnDisconnectSer_Click(object sender, EventArgs e)
        {
            if (_serial != null && _serial.IsRunning)
            {
                _serial.Disconnect();
                MessageBox.Show("Đã ngắt kết nối Serial.", "Serial", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FormSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            UITheme.ThemeChanged -= ApplyTheme;

            if (_cam != null)
            {
                _cam.FrameReady -= Camera_FrameReady;
            }

            if (picWebcam.Image != null)
            {
                Image old = picWebcam.Image;
                picWebcam.Image = null;
                old.Dispose();
            }
        }
    }
}