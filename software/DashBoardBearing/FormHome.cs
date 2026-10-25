using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using RobotView;
namespace DashBoardBearing
{
    public partial class FormHome : Form
    {
        CameraManager _cam;

        // Tên class của model -> tên hiển thị
        private static readonly Dictionary<string, string> ClassDisplayNames =
            new Dictionary<string, string>
            {
                { "normal",         "Đạt" },
                { "rusted",         "Gỉ sét" },
                { "damaged_shield", "Hư nắp che" },
                { "surface_damage", "Trầy bề mặt" }
            };

        public FormHome(CameraManager cameraManager, Robot4DOF? robotView = null)
        {
            InitializeComponent();
            _cam = cameraManager;
            if (_cam != null && _cam.IsRunning)
            {
                _cam.FrameReady += Camera_FrameReady;
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

        private async void btnDetect_Click(object sender, EventArgs e)
        {
            btnDetect.Enabled = false;

            try
            {
                using (DetectionResult result = await _cam.DetectAsync())
                {
                    if (result == null) return;

                    // Không thấy vòng bi -> xóa kết quả cũ, tránh hiện nhầm.
                    if (result.Frame == null)
                    {
                        ClearResult("Không phát hiện vòng bi");
                        return;
                    }

                    // Vòng bi gần tâm nhất: cùng vòng bi với ảnh crop (result.Frame).
                    BearingResult best = result.Results[0];

                    // Clone vì result sẽ bị Dispose khi ra khỏi using
                    Image old = picResult.Image;
                    picResult.Image = (Image)result.Frame.Clone();
                    old?.Dispose();

                    // OK / NG của chính vòng bi đang hiển thị
                    SetStatus(best.IsOk);

                    // Class lỗi + độ tin cậy
                    lblDefect.Text = GetDisplayName(best.Label);
                    lblConfidence.Text = best.Confidence.ToString("P1");

                    lblTime.Text = $"{result.TotalMs:F0} ms";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                btnDetect.Enabled = true;
            }
        }

        private static string GetDisplayName(string label)
        {
            return ClassDisplayNames.TryGetValue(label, out string name)
                ? name
                : label;   // class lạ -> hiện nguyên tên
        }

        private void SetStatus(bool isOk)
        {
            lblStatus.Text = isOk ? "OK" : "NG";
            lblStatus.BackColor = isOk ? Color.Green : Color.Red;
            lblStatus.ForeColor = Color.White;
        }

        private void ClearResult(string message)
        {
            lblStatus.Text = message;
            lblStatus.BackColor = Color.Gray;
            lblStatus.ForeColor = Color.White;

            lblDefect.Text = "-";
            lblConfidence.Text = "-";
            lblTime.Text = "-";

            if (picResult.Image != null)
            {
                Image old = picResult.Image;
                picResult.Image = null;
                old.Dispose();
            }
        }

        private void picResult_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void roundedPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}