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
        private readonly List<int> _cameraIndices = new List<int>();

        public FormSetting(CameraManager cam, SerialManager serial)
        {
            InitializeComponent();

            splitContainer1.SplitterDistance =
                splitContainer1.ClientSize.Width / 2;

            _cam = cam;
            _serial = serial;

            _cam.FrameReady += Camera_FrameReady;
        }

        // =========================================================
        // CAMERA
        // =========================================================

        public void LoadWebcam()
        {
            
            cbWebcam.Items.Clear();
            _cameraIndices.Clear();

            Cursor = Cursors.WaitCursor;
            try
            {
                _cameraIndices.AddRange(CameraManager.GetAvailableCameras());
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            // Camera đang chạy bị CameraManager chiếm nên dò không thấy
            // -> tự thêm lại để người dùng thấy đúng camera hiện tại.
            if (_cam.IsRunning &&
                !_cameraIndices.Contains(_cam.CurrentCameraIndex))
            {
                _cameraIndices.Add(_cam.CurrentCameraIndex);
                _cameraIndices.Sort();
            }

            if (_cameraIndices.Count == 0)
                return;

            foreach (int idx in _cameraIndices)
                cbWebcam.Items.Add("Camera " + idx);

            int selected = _cam.IsRunning
                ? _cameraIndices.IndexOf(_cam.CurrentCameraIndex)
                : -1;

            cbWebcam.SelectedIndex = selected >= 0 ? selected : 0;
        }

        // Event do CameraManager raise trên UI thread
        // -> gán thẳng vào PictureBox, không cần BeginInvoke.
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
            if (cbWebcam.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn Camera.",
                    "Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            int camIndex = _cameraIndices[cbWebcam.SelectedIndex];

            if (!_cam.Start(camIndex))
            {
                MessageBox.Show(
                    "Không thể kết nối Camera.",
                    "Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            // StopAsync đợi frame đang inference xong rồi mới release camera.
            await _cam.StopAsync();

            if (picWebcam.Image != null)
            {
                Image old = picWebcam.Image;
                picWebcam.Image = null;
                old.Dispose();
            }
        }

        // =========================================================
        // SERIAL (giữ nguyên)
        // =========================================================

        private void LoadCOM()
        {
            cbCOM.Items.Clear();

            string[] ports = _serial.GetPorts();

            if (ports.Length == 0)
                return;

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
            // Camera
            if(!_cam.IsRunning)
            {
                LoadWebcam();
            }
            else
            {

            }

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
                cbBaud.SelectedIndex < 0 ||
                cbDataSize.SelectedIndex < 0 ||
                cbParityBit.SelectedIndex < 0 ||
                cbStopBit.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Vui lòng chọn đầy đủ thông số Serial.",
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
                    "Kiểm tra lại COM hoặc thông số Serial.",
                    "Serial",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            MessageBox.Show(
                "Kết nối Serial thành công.",
                "Serial",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnDisconnectSer_Click(object sender, EventArgs e)
        {
            if (_serial != null && _serial.IsRunning)
                _serial.Disconnect();
        }

        private void FormSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            _cam.FrameReady -= Camera_FrameReady;

            if (picWebcam.Image != null)
            {
                Image old = picWebcam.Image;
                picWebcam.Image = null;
                old.Dispose();
            }
        }

        private async void btnConnectIP_Click(object sender, EventArgs e)
        {
            try
            {
                if (!await _cam.StartIpAsync(txtIP.Text.Trim()))
                {
                    MessageBox.Show("Không thể kết nối camera IP.", "Camera",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}