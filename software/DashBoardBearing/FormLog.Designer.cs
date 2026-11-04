namespace DashBoardBearing
{
    partial class FormLog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cardLog = new RoundedPanel();
            dgvBearingHistory = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colTime = new DataGridViewTextBoxColumn();
            colLabel = new DataGridViewTextBoxColumn();
            colResult = new DataGridViewTextBoxColumn();
            colConf = new DataGridViewTextBoxColumn();
            colProcess = new DataGridViewTextBoxColumn();
            colNote = new DataGridViewTextBoxColumn();
            pnlToolbar = new Panel();
            lblSummaryBadge = new Label();
            btnClear = new FontAwesome.Sharp.IconButton();
            btnExportCsv = new FontAwesome.Sharp.IconButton();
            btnAll = new FontAwesome.Sharp.IconButton();
            btnToday = new FontAwesome.Sharp.IconButton();
            btnFilter = new FontAwesome.Sharp.IconButton();
            dtpTo = new DateTimePicker();
            lblTo = new Label();
            dtpFrom = new DateTimePicker();
            lblFrom = new Label();
            pnlLogHeader = new Panel();
            lblLogSubtitle = new Label();
            lblLogTitle = new Label();
            iconLogHeader = new FontAwesome.Sharp.IconPictureBox();
            cardLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBearingHistory).BeginInit();
            pnlToolbar.SuspendLayout();
            pnlLogHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconLogHeader).BeginInit();
            SuspendLayout();
            // 
            // cardLog
            // 
            cardLog.BackColor = Color.White;
            cardLog.BackgroundColor = Color.White;
            cardLog.BorderColor = Color.FromArgb(226, 232, 240);
            cardLog.BorderRadius = 12;
            cardLog.BorderSize = 1;
            cardLog.Controls.Add(dgvBearingHistory);
            cardLog.Controls.Add(pnlToolbar);
            cardLog.Controls.Add(pnlLogHeader);
            cardLog.Dock = DockStyle.Fill;
            cardLog.Location = new Point(14, 14);
            cardLog.Name = "cardLog";
            cardLog.Padding = new Padding(15);
            cardLog.Size = new Size(1012, 676);
            cardLog.TabIndex = 0;
            // 
            // dgvBearingHistory
            // 
            dgvBearingHistory.AllowUserToAddRows = false;
            dgvBearingHistory.AllowUserToDeleteRows = false;
            dgvBearingHistory.AllowUserToResizeRows = false;
            dgvBearingHistory.BackgroundColor = Color.FromArgb(248, 250, 252);
            dgvBearingHistory.BorderStyle = BorderStyle.None;
            dgvBearingHistory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBearingHistory.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBearingHistory.ColumnHeadersHeight = 42;
            dgvBearingHistory.RowTemplate.Height = 38;
            dgvBearingHistory.ScrollBars = ScrollBars.Both;
            dgvBearingHistory.Columns.AddRange(new DataGridViewColumn[] { colId, colTime, colLabel, colResult, colConf, colProcess, colNote });
            dgvBearingHistory.Dock = DockStyle.Fill;
            dgvBearingHistory.Location = new Point(15, 165);
            dgvBearingHistory.MultiSelect = false;
            dgvBearingHistory.Name = "dgvBearingHistory";
            dgvBearingHistory.ReadOnly = true;
            dgvBearingHistory.RowHeadersVisible = false;
            dgvBearingHistory.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBearingHistory.Size = new Size(982, 496);
            dgvBearingHistory.TabIndex = 2;
            // 
            // colId
            // 
            colId.HeaderText = "STT";
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Width = 80;
            // 
            // colTime
            // 
            colTime.HeaderText = "Thời gian nhận diện";
            colTime.Name = "colTime";
            colTime.ReadOnly = true;
            colTime.Width = 200;
            // 
            // colLabel
            // 
            colLabel.HeaderText = "Loại vòng bi";
            colLabel.Name = "colLabel";
            colLabel.ReadOnly = true;
            colLabel.Width = 240;
            // 
            // colResult
            // 
            colResult.HeaderText = "Đánh giá";
            colResult.Name = "colResult";
            colResult.ReadOnly = true;
            colResult.Width = 130;
            // 
            // colConf
            // 
            colConf.HeaderText = "Độ tin cậy";
            colConf.Name = "colConf";
            colConf.ReadOnly = true;
            colConf.Width = 125;
            // 
            // colProcess
            // 
            colProcess.HeaderText = "Thời gian xử lý";
            colProcess.Name = "colProcess";
            colProcess.ReadOnly = true;
            colProcess.Width = 140;
            // 
            // colNote
            // 
            colNote.MinimumWidth = 250;
            colNote.Width = 300;
            colNote.HeaderText = "Ghi chú phân loại";
            colNote.Name = "colNote";
            colNote.ReadOnly = true;
            // 
            // pnlToolbar
            // 
            pnlToolbar.BackColor = Color.Transparent;
            pnlToolbar.Controls.Add(lblSummaryBadge);
            pnlToolbar.Controls.Add(btnClear);
            pnlToolbar.Controls.Add(btnExportCsv);
            pnlToolbar.Controls.Add(btnAll);
            pnlToolbar.Controls.Add(btnToday);
            pnlToolbar.Controls.Add(btnFilter);
            pnlToolbar.Controls.Add(dtpTo);
            pnlToolbar.Controls.Add(lblTo);
            pnlToolbar.Controls.Add(dtpFrom);
            pnlToolbar.Controls.Add(lblFrom);
            pnlToolbar.Dock = DockStyle.Top;
            pnlToolbar.Location = new Point(15, 65);
            pnlToolbar.Name = "pnlToolbar";
            pnlToolbar.Size = new Size(982, 98);
            pnlToolbar.TabIndex = 1;
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFrom.ForeColor = Color.FromArgb(100, 116, 139);
            lblFrom.Location = new Point(4, 11);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(68, 20);
            lblFrom.TabIndex = 0;
            lblFrom.Text = "Từ ngày:";
            // 
            // dtpFrom
            // 
            dtpFrom.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpFrom.Font = new Font("Segoe UI", 9F);
            dtpFrom.Format = DateTimePickerFormat.Custom;
            dtpFrom.Location = new Point(76, 7);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(185, 27);
            dtpFrom.TabIndex = 1;
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTo.ForeColor = Color.FromArgb(100, 116, 139);
            lblTo.Location = new Point(272, 11);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(79, 20);
            lblTo.TabIndex = 2;
            lblTo.Text = "Đến ngày:";
            // 
            // dtpTo
            // 
            dtpTo.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpTo.Font = new Font("Segoe UI", 9F);
            dtpTo.Format = DateTimePickerFormat.Custom;
            dtpTo.Location = new Point(355, 7);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(185, 27);
            dtpTo.TabIndex = 3;
            // 
            // btnFilter
            // 
            btnFilter.BackColor = Color.FromArgb(37, 99, 235);
            btnFilter.Cursor = Cursors.Hand;
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.FlatStyle = FlatStyle.Flat;
            btnFilter.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnFilter.ForeColor = Color.White;
            btnFilter.IconChar = FontAwesome.Sharp.IconChar.Search;
            btnFilter.IconColor = Color.White;
            btnFilter.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnFilter.IconSize = 14;
            btnFilter.Location = new Point(552, 5);
            btnFilter.Name = "btnFilter";
            btnFilter.Size = new Size(105, 32);
            btnFilter.TabIndex = 4;
            btnFilter.Text = " Tra cứu";
            btnFilter.TextAlign = ContentAlignment.MiddleRight;
            btnFilter.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnFilter.UseVisualStyleBackColor = false;
            // 
            // btnToday
            // 
            btnToday.BackColor = Color.FromArgb(241, 245, 249);
            btnToday.Cursor = Cursors.Hand;
            btnToday.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Segoe UI", 8.5F);
            btnToday.ForeColor = Color.FromArgb(71, 85, 105);
            btnToday.IconChar = FontAwesome.Sharp.IconChar.CalendarDay;
            btnToday.IconColor = Color.FromArgb(71, 85, 105);
            btnToday.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnToday.IconSize = 14;
            btnToday.Location = new Point(665, 5);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(100, 32);
            btnToday.TabIndex = 5;
            btnToday.Text = " Hôm nay";
            btnToday.TextAlign = ContentAlignment.MiddleRight;
            btnToday.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnToday.UseVisualStyleBackColor = false;
            // 
            // btnAll
            // 
            btnAll.BackColor = Color.FromArgb(241, 245, 249);
            btnAll.Cursor = Cursors.Hand;
            btnAll.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnAll.FlatStyle = FlatStyle.Flat;
            btnAll.Font = new Font("Segoe UI", 8.5F);
            btnAll.ForeColor = Color.FromArgb(71, 85, 105);
            btnAll.IconChar = FontAwesome.Sharp.IconChar.ListUl;
            btnAll.IconColor = Color.FromArgb(71, 85, 105);
            btnAll.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnAll.IconSize = 14;
            btnAll.Location = new Point(773, 5);
            btnAll.Name = "btnAll";
            btnAll.Size = new Size(95, 32);
            btnAll.TabIndex = 6;
            btnAll.Text = " Tất cả";
            btnAll.TextAlign = ContentAlignment.MiddleRight;
            btnAll.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAll.UseVisualStyleBackColor = false;
            // 
            // lblSummaryBadge
            // 
            lblSummaryBadge.BackColor = Color.FromArgb(241, 245, 249);
            lblSummaryBadge.Font = new Font("Consolas", 9F, FontStyle.Bold);
            lblSummaryBadge.ForeColor = Color.FromArgb(71, 85, 105);
            lblSummaryBadge.Location = new Point(4, 52);
            lblSummaryBadge.Name = "lblSummaryBadge";
            lblSummaryBadge.Padding = new Padding(8, 0, 8, 0);
            lblSummaryBadge.Size = new Size(460, 36);
            lblSummaryBadge.TabIndex = 7;
            lblSummaryBadge.Text = "Tổng: 0 | Đạt: 0 (0.0%) | Lỗi: 0";
            lblSummaryBadge.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnExportCsv
            // 
            btnExportCsv.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExportCsv.BackColor = Color.FromArgb(16, 185, 129);
            btnExportCsv.Cursor = Cursors.Hand;
            btnExportCsv.FlatAppearance.BorderSize = 0;
            btnExportCsv.FlatStyle = FlatStyle.Flat;
            btnExportCsv.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnExportCsv.ForeColor = Color.White;
            btnExportCsv.IconChar = FontAwesome.Sharp.IconChar.FileExcel;
            btnExportCsv.IconColor = Color.White;
            btnExportCsv.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnExportCsv.IconSize = 15;
            btnExportCsv.Location = new Point(742, 52);
            btnExportCsv.Name = "btnExportCsv";
            btnExportCsv.Size = new Size(118, 36);
            btnExportCsv.TabIndex = 8;
            btnExportCsv.Text = " Xuất CSV";
            btnExportCsv.TextAlign = ContentAlignment.MiddleRight;
            btnExportCsv.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnExportCsv.UseVisualStyleBackColor = false;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.BackColor = Color.FromArgb(239, 68, 68);
            btnClear.Cursor = Cursors.Hand;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnClear.ForeColor = Color.White;
            btnClear.IconChar = FontAwesome.Sharp.IconChar.TrashCan;
            btnClear.IconColor = Color.White;
            btnClear.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnClear.IconSize = 15;
            btnClear.Location = new Point(868, 52);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(114, 36);
            btnClear.TabIndex = 9;
            btnClear.Text = " Xóa lịch sử";
            btnClear.TextAlign = ContentAlignment.MiddleRight;
            btnClear.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnClear.UseVisualStyleBackColor = false;
            // 
            // pnlLogHeader
            // 
            pnlLogHeader.BackColor = Color.Transparent;
            pnlLogHeader.Controls.Add(lblLogSubtitle);
            pnlLogHeader.Controls.Add(lblLogTitle);
            pnlLogHeader.Controls.Add(iconLogHeader);
            pnlLogHeader.Dock = DockStyle.Top;
            pnlLogHeader.Location = new Point(15, 15);
            pnlLogHeader.Name = "pnlLogHeader";
            pnlLogHeader.Size = new Size(982, 50);
            pnlLogHeader.TabIndex = 0;
            // 
            // lblLogSubtitle
            // 
            lblLogSubtitle.AutoSize = true;
            lblLogSubtitle.BackColor = Color.Transparent;
            lblLogSubtitle.Font = new Font("Segoe UI", 8.5F);
            lblLogSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
            lblLogSubtitle.Location = new Point(46, 28);
            lblLogSubtitle.Name = "lblLogSubtitle";
            lblLogSubtitle.Size = new Size(410, 20);
            lblLogSubtitle.TabIndex = 2;
            lblLogSubtitle.Text = "Tra cứu, lọc dữ liệu theo thời gian thực và quản lý lịch sử nhận diện";
            // 
            // lblLogTitle
            // 
            lblLogTitle.AutoSize = true;
            lblLogTitle.BackColor = Color.Transparent;
            lblLogTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblLogTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblLogTitle.Location = new Point(44, 3);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(292, 28);
            lblLogTitle.TabIndex = 1;
            lblLogTitle.Text = "LỊCH SỬ NHẬN DIỆN VÒNG BI";
            // 
            // iconLogHeader
            // 
            iconLogHeader.BackColor = Color.Transparent;
            iconLogHeader.ForeColor = Color.FromArgb(37, 99, 235);
            iconLogHeader.IconChar = FontAwesome.Sharp.IconChar.ClockRotateLeft;
            iconLogHeader.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconLogHeader.IconSize = 30;
            iconLogHeader.Location = new Point(3, 8);
            iconLogHeader.Name = "iconLogHeader";
            iconLogHeader.Size = new Size(30, 30);
            iconLogHeader.SizeMode = PictureBoxSizeMode.CenterImage;
            iconLogHeader.TabIndex = 0;
            iconLogHeader.TabStop = false;
            // 
            // FormLog
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1040, 704);
            Controls.Add(cardLog);
            Name = "FormLog";
            Padding = new Padding(14);
            Text = "FormLog";
            Load += FormLog_Load;
            cardLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBearingHistory).EndInit();
            pnlToolbar.ResumeLayout(false);
            pnlToolbar.PerformLayout();
            pnlLogHeader.ResumeLayout(false);
            pnlLogHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconLogHeader).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private RoundedPanel cardLog;
        private Panel pnlLogHeader;
        private Label lblLogSubtitle;
        private Label lblLogTitle;
        private FontAwesome.Sharp.IconPictureBox iconLogHeader;
        private Panel pnlToolbar;
        private Label lblFrom;
        private DateTimePicker dtpFrom;
        private Label lblTo;
        private DateTimePicker dtpTo;
        private FontAwesome.Sharp.IconButton btnFilter;
        private FontAwesome.Sharp.IconButton btnToday;
        private FontAwesome.Sharp.IconButton btnAll;
        private Label lblSummaryBadge;
        private FontAwesome.Sharp.IconButton btnExportCsv;
        private FontAwesome.Sharp.IconButton btnClear;
        private DataGridView dgvBearingHistory;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colTime;
        private DataGridViewTextBoxColumn colLabel;
        private DataGridViewTextBoxColumn colResult;
        private DataGridViewTextBoxColumn colConf;
        private DataGridViewTextBoxColumn colProcess;
        private DataGridViewTextBoxColumn colNote;
    }
}
