using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DashBoardBearing
{
    public partial class FormLog : Form
    {
        private List<BearingLogEntry> _currentEntries = new List<BearingLogEntry>();

        public FormLog()
        {
            InitializeComponent();
            InitEventHandlers();
        }

        private void InitEventHandlers()
        {
            btnFilter.Click += BtnFilter_Click;
            btnToday.Click += BtnToday_Click;
            btnAll.Click += BtnAll_Click;
            btnExportCsv.Click += BtnExportCsv_Click;
            btnClear.Click += BtnClear_Click;

            FormClosing += FormLog_FormClosing;
        }

        private void FormLog_Load(object sender, EventArgs e)
        {
            ApplyTheme();
            InitDataGridView();

            // Mặc định khoảng thời gian: Hôm nay từ 00:00:00 đến 23:59:59
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);

            // Nạp dữ liệu hôm nay
            LoadData(BearingLogManager.GetByDateRange(dtpFrom.Value, dtpTo.Value));

            // Lắng nghe sự kiện nếu có vòng bi mới được nhận diện ở tab Home
            BearingLogManager.EntryAdded += OnEntryAdded;
            BearingLogManager.LogCleared += OnLogCleared;

            UITheme.ThemeChanged += ApplyTheme;
        }

        private void FormLog_FormClosing(object? sender, FormClosingEventArgs e)
        {
            UITheme.ThemeChanged -= ApplyTheme;
            BearingLogManager.EntryAdded -= OnEntryAdded;
            BearingLogManager.LogCleared -= OnLogCleared;
        }

        private void InitDataGridView()
        {
            dgvBearingHistory.AutoGenerateColumns = false;

            if (colId != null) colId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (colTime != null) colTime.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (colLabel != null) colLabel.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            if (colResult != null) colResult.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (colConf != null) colConf.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (colProcess != null) colProcess.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (colNote != null) colNote.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            dgvBearingHistory.CellFormatting -= DgvBearingHistory_CellFormatting;
            dgvBearingHistory.CellFormatting += DgvBearingHistory_CellFormatting;

            try
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                    null, dgvBearingHistory, new object[] { true });
            }
            catch { }
        }

        private void DgvBearingHistory_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            if (dgvBearingHistory.Columns[e.ColumnIndex].Name == "colResult")
            {
                string val = e.Value.ToString() ?? "";
                if (val.Contains("PASS") || val.Contains("ĐẠT"))
                {
                    e.CellStyle.ForeColor = UITheme.Success;
                    e.CellStyle.Font = new Font(dgvBearingHistory.Font, FontStyle.Bold);
                }
                else if (val.Contains("FAIL") || val.Contains("LỖI"))
                {
                    e.CellStyle.ForeColor = UITheme.Danger;
                    e.CellStyle.Font = new Font(dgvBearingHistory.Font, FontStyle.Bold);
                }
            }
        }

        private void LoadData(List<BearingLogEntry> entries)
        {
            _currentEntries = entries ?? new List<BearingLogEntry>();

            dgvBearingHistory.Rows.Clear();
            foreach (var item in _currentEntries)
            {
                string resultStr = item.IsOk ? "✔ PASS" : "✖ FAIL";
                string timeStr = item.Timestamp.ToString("dd/MM/yyyy HH:mm:ss");
                string confStr = $"{item.Confidence:P1}";
                string msStr = $"{item.ProcessTimeMs:F0} ms";

                dgvBearingHistory.Rows.Add(
                    item.Id,
                    timeStr,
                    item.DisplayName,
                    resultStr,
                    confStr,
                    msStr,
                    item.Note
                );
            }

            UpdateSummaryBadge();
        }

        private void UpdateSummaryBadge()
        {
            int total = _currentEntries.Count;
            int pass = _currentEntries.Count(e => e.IsOk);
            int fail = total - pass;
            double passPercent = total > 0 ? ((double)pass / total) * 100.0 : 0.0;

            lblSummaryBadge.Text = $"Tổng: {total:N0}  |  Đạt: {pass:N0} ({passPercent:F1}%)  |  Lỗi: {fail:N0}";

            if (total == 0)
            {
                lblSummaryBadge.BackColor = UITheme.BgCardAlt;
                lblSummaryBadge.ForeColor = UITheme.TextSecondary;
            }
            else if (fail == 0)
            {
                lblSummaryBadge.BackColor = UITheme.SuccessBg;
                lblSummaryBadge.ForeColor = UITheme.Success;
            }
            else
            {
                lblSummaryBadge.BackColor = UITheme.IsDarkMode ? Color.FromArgb(41, 53, 76) : Color.FromArgb(239, 246, 255);
                lblSummaryBadge.ForeColor = UITheme.PrimaryLight;
            }
        }

        private void OnEntryAdded(BearingLogEntry entry)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => OnEntryAdded(entry)));
                return;
            }

            // Nếu vòng bi mới nằm trong khoảng thời gian đang lọc, thêm vào bảng
            if (entry.Timestamp >= dtpFrom.Value && entry.Timestamp <= dtpTo.Value)
            {
                _currentEntries.Insert(0, entry);

                string resultStr = entry.IsOk ? "✔ PASS" : "✖ FAIL";
                string timeStr = entry.Timestamp.ToString("dd/MM/yyyy HH:mm:ss");
                string confStr = $"{entry.Confidence:P1}";
                string msStr = $"{entry.ProcessTimeMs:F0} ms";

                dgvBearingHistory.Rows.Insert(0,
                    entry.Id,
                    timeStr,
                    entry.DisplayName,
                    resultStr,
                    confStr,
                    msStr,
                    entry.Note
                );

                UpdateSummaryBadge();
            }
        }

        private void OnLogCleared()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnLogCleared));
                return;
            }

            LoadData(new List<BearingLogEntry>());
        }

        private void BtnFilter_Click(object? sender, EventArgs e)
        {
            DateTime from = dtpFrom.Value;
            DateTime to = dtpTo.Value;

            if (from > to)
            {
                MessageBox.Show("Thời gian 'Từ ngày' phải nhỏ hơn hoặc bằng 'Đến ngày'.", "Lỗi lọc dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadData(BearingLogManager.GetByDateRange(from, to));
        }

        private void BtnToday_Click(object? sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);
            LoadData(BearingLogManager.GetByDateRange(dtpFrom.Value, dtpTo.Value));
        }

        private void BtnAll_Click(object? sender, EventArgs e)
        {
            var all = BearingLogManager.GetAll();
            if (all.Count > 0)
            {
                dtpFrom.Value = all.Min(x => x.Timestamp);
                dtpTo.Value = all.Max(x => x.Timestamp).AddSeconds(1);
            }
            LoadData(all);
        }

        private void BtnExportCsv_Click(object? sender, EventArgs e)
        {
            if (_currentEntries.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trong danh sách hiện tại để xuất CSV.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp CSV (Excel UTF-8)|*.csv",
                FileName = $"LichSuNhanDienVongBi_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Xuất dữ liệu lịch sử nhận diện vòng bi"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (BearingLogManager.ExportToCsv(sfd.FileName, _currentEntries))
                {
                    MessageBox.Show($"Đã xuất thành công {_currentEntries.Count} bản ghi ra tệp:\n{sfd.FileName}", "Xuất CSV thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra trong quá trình ghi tệp CSV.", "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnClear_Click(object? sender, EventArgs e)
        {
            var res = MessageBox.Show(
                "Bạn có chắc chắn muốn xóa toàn bộ lịch sử nhận diện vòng bi không?\n\nLưu ý: Dữ liệu đã xóa sẽ không thể phục hồi.",
                "Xác nhận xóa lịch sử",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2
            );

            if (res == DialogResult.Yes)
            {
                BearingLogManager.ClearAll();
                MessageBox.Show("Đã xóa sạch toàn bộ lịch sử nhận diện vòng bi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ApplyTheme()
        {
            BackColor = UITheme.BgDark;
            cardLog.BackColor = UITheme.BgCard;
            cardLog.BackgroundColor = UITheme.BgCard;
            cardLog.BorderColor = UITheme.BorderCard;

            pnlLogHeader.BackColor = Color.Transparent;
            lblLogTitle.BackColor = Color.Transparent;
            lblLogTitle.ForeColor = UITheme.TextPrimary;

            lblLogSubtitle.BackColor = Color.Transparent;
            lblLogSubtitle.ForeColor = UITheme.TextSecondary;

            iconLogHeader.BackColor = Color.Transparent;
            iconLogHeader.ForeColor = UITheme.PrimaryLight;

            pnlToolbar.BackColor = Color.Transparent;
            lblFrom.ForeColor = UITheme.TextSecondary;
            lblTo.ForeColor = UITheme.TextSecondary;

            btnToday.BackColor = UITheme.BgCardAlt;
            btnToday.ForeColor = UITheme.TextSecondary;
            btnToday.IconColor = UITheme.TextSecondary;
            btnToday.FlatAppearance.BorderColor = UITheme.BorderCard;

            btnAll.BackColor = UITheme.BgCardAlt;
            btnAll.ForeColor = UITheme.TextSecondary;
            btnAll.IconColor = UITheme.TextSecondary;
            btnAll.FlatAppearance.BorderColor = UITheme.BorderCard;

            // DataGridView Theme
            dgvBearingHistory.BackgroundColor = UITheme.BgCardAlt;
            dgvBearingHistory.GridColor = UITheme.BorderCard;
            dgvBearingHistory.BorderStyle = BorderStyle.None;
            dgvBearingHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBearingHistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvBearingHistory.RowHeadersVisible = false;
            dgvBearingHistory.EnableHeadersVisualStyles = false;

            dgvBearingHistory.ColumnHeadersDefaultCellStyle.BackColor = UITheme.IsDarkMode ? Color.FromArgb(30, 41, 59) : Color.FromArgb(241, 245, 249);
            dgvBearingHistory.ColumnHeadersDefaultCellStyle.ForeColor = UITheme.IsDarkMode ? Color.FromArgb(226, 232, 240) : Color.FromArgb(15, 23, 42);
            dgvBearingHistory.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvBearingHistory.ColumnHeadersHeight = 42;

            dgvBearingHistory.DefaultCellStyle.BackColor = UITheme.BgCardAlt;
            dgvBearingHistory.DefaultCellStyle.ForeColor = UITheme.TextPrimary;
            dgvBearingHistory.DefaultCellStyle.SelectionBackColor = UITheme.IsDarkMode ? Color.FromArgb(41, 53, 76) : Color.FromArgb(224, 231, 255);
            dgvBearingHistory.DefaultCellStyle.SelectionForeColor = UITheme.TextPrimary;
            dgvBearingHistory.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvBearingHistory.RowTemplate.Height = 38;
            dgvBearingHistory.ScrollBars = ScrollBars.Both;

            UpdateSummaryBadge();
        }
    }
}
