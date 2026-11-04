using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DashBoardBearing
{
    public partial class FormVision : Form
    {
        private readonly CameraManager? _cam;

        private bool? _lastResultOk = null;
        private string _lastStatusMsg = "SẴN SÀNG";

        private static readonly Dictionary<string, (string DisplayName, Color TextColor)> ClassInfo =
            new Dictionary<string, (string DisplayName, Color TextColor)>
            {
                { "normal",         ("Bình thường (Đạt)", Color.FromArgb(16, 185, 129)) },
                { "rusted",         ("Gỉ sét bề mặt",     Color.FromArgb(239, 68, 68)) },
                { "damaged_shield", ("Hư nắp che",       Color.FromArgb(245, 158, 11)) },
                { "surface_damage", ("Trầy xước bề mặt", Color.FromArgb(239, 68, 68)) }
            };

        public FormVision(CameraManager cameraManager)
        {
            InitializeComponent();
            _cam = cameraManager;

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

        public FormVision() : this(null!)
        {
        }

        private void FormVision_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            ClearResult("SẴN SÀNG");
            AdjustSplitter();
        }

        private void FormVision_Resize(object sender, EventArgs e)
        {
            AdjustSplitter();
        }

        private void AdjustSplitter()
        {
            try
            {
                if (splitContainerVision != null && splitContainerVision.Width > 500)
                {
                    int target = (int)(splitContainerVision.Width * 0.55);
                    int minLeft = Math.Max(350, splitContainerVision.Panel1MinSize);
                    int maxLeft = splitContainerVision.Width - Math.Max(300, splitContainerVision.Panel2MinSize);
                    if (target >= minLeft && target <= maxLeft)
                    {
                        splitContainerVision.SplitterDistance = target;
                    }
                }
            }
            catch { }
        }

        private void ApplyTheme()
        {
            BackColor = UITheme.BgDark;

            panelLeftCard.BackgroundColor = UITheme.BgCard;
            panelLeftCard.BorderColor = UITheme.BorderCard;
            panelRightCard.BackgroundColor = UITheme.BgCard;
            panelRightCard.BorderColor = UITheme.BorderCard;

            lblVisionTitle.ForeColor = UITheme.TextPrimary;
            lblResultTitle.ForeColor = UITheme.TextPrimary;

            pnlVisionFeedContainer.BackColor = UITheme.BgViewport;

            pnlDefectMetric.BackgroundColor = UITheme.BgCardAlt;
            pnlDefectMetric.BorderColor = UITheme.BorderCard;
            labelDefectTitle.ForeColor = UITheme.TextSecondary;

            pnlConfMetric.BackgroundColor = UITheme.BgCardAlt;
            pnlConfMetric.BorderColor = UITheme.BorderCard;
            labelConfTitle.ForeColor = UITheme.TextSecondary;

            pnlTimeMetric.BackgroundColor = UITheme.BgCardAlt;
            pnlTimeMetric.BorderColor = UITheme.BorderCard;
            labelTimeTitle.ForeColor = UITheme.TextSecondary;

            pnlVisionCrop.BackgroundColor = UITheme.BgCardAlt;
            pnlVisionCrop.BorderColor = UITheme.BorderCard;
            lblVisionCropTitle.ForeColor = UITheme.TextSecondary;

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

        private void Camera_FrameReady(object sender, FrameReadyEventArgs e)
        {
            if (IsDisposed || Disposing || picVisionFeed.IsDisposed)
            {
                e.Frame?.Dispose();
                return;
            }

            Image oldImage = picVisionFeed.Image;
            picVisionFeed.Image = e.Frame;
            oldImage?.Dispose();
        }

        private async void btnVisionDetect_Click(object sender, EventArgs e)
        {
            if (_cam == null || !_cam.IsRunning)
            {
                MessageBox.Show(
                    "Camera chưa được kết nối! Vui lòng vào Cài đặt để mở Camera.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnVisionDetect.Enabled = false;
            btnVisionDetect.Text = "  ĐANG SUY LUẬN AI...";

            try
            {
                using (DetectionResult result = await _cam.DetectAsync())
                {
                    if (result == null)
                    {
                        MessageBox.Show("Không nhận được dữ liệu phản hồi từ camera.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (result.Frame == null || result.Results == null || result.Results.Count == 0)
                    {
                        ClearResult("KHÔNG THẤY VÒNG BI");
                        MessageBox.Show("Không phát hiện thấy vòng bi trong khung hình.", "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    BearingResult best = result.Results[0];

                    Image old = picVisionCrop.Image;
                    picVisionCrop.Image = (Image)result.Frame.Clone();
                    old?.Dispose();

                    SetStatus(best.IsOk);

                    var info = GetClassInfo(best.Label);
                    lblDefect.Text = info.DisplayName;
                    lblDefect.ForeColor = info.TextColor;

                    lblConfidence.Text = best.Confidence.ToString("P1");
                    lblTime.Text = $"{result.TotalMs:F0} ms";

                    // Lưu lịch sử nhận diện thủ công vào hệ thống
                    BearingLogManager.Add(best.Label, info.DisplayName, best.IsOk, best.Confidence, result.TotalMs, "Nhận diện thủ công (Vision)");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhận diện: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnVisionDetect.Enabled = true;
                btnVisionDetect.Text = "  NHẬN DIỆN VÒNG BI (MANUAL DETECT)";
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

            if (picVisionCrop.Image != null)
            {
                Image old = picVisionCrop.Image;
                picVisionCrop.Image = null;
                old.Dispose();
            }
        }

        private void FormVision_FormClosing(object sender, FormClosingEventArgs e)
        {
            UITheme.ThemeChanged -= ApplyTheme;

            if (_cam != null)
            {
                _cam.FrameReady -= Camera_FrameReady;

                if (picVisionFeed.Image != null)
                {
                    Image old = picVisionFeed.Image;
                    picVisionFeed.Image = null;
                    old.Dispose();
                }
            }
        }
    }
}
