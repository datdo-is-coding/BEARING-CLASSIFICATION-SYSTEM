namespace DashBoardBearing
{
    partial class FormHome
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
            panelTopStats = new Panel();
            tableTopCards = new TableLayoutPanel();
            cardTotal = new RoundedPanel();
            lblStatTotalVal = new Label();
            lblStatTotalTitle = new Label();
            iconTotal = new FontAwesome.Sharp.IconPictureBox();
            cardOk = new RoundedPanel();
            lblStatOkVal = new Label();
            lblStatOkTitle = new Label();
            iconOk = new FontAwesome.Sharp.IconPictureBox();
            cardNg = new RoundedPanel();
            lblStatNgVal = new Label();
            lblStatNgTitle = new Label();
            iconNg = new FontAwesome.Sharp.IconPictureBox();
            cardYield = new RoundedPanel();
            lblStatYieldVal = new Label();
            lblStatYieldTitle = new Label();
            iconYield = new FontAwesome.Sharp.IconPictureBox();
            splitContainerMain = new SplitContainer();
            panelLeftCard = new RoundedPanel();
            pnlCameraHeader = new Panel();
            lblLiveBadge = new Label();
            lblCameraTitle = new Label();
            iconCamera = new FontAwesome.Sharp.IconPictureBox();
            pnlWebcamContainer = new Panel();
            picWebcam = new PictureBox();
            pnlCameraActions = new Panel();
            btnStart = new FontAwesome.Sharp.IconButton();
            btnStop = new FontAwesome.Sharp.IconButton();
            btnResetStats = new FontAwesome.Sharp.IconButton();
            pnlLogContainer = new Panel();
            dgvInspectionLog = new DataGridView();
            lblLogTitle = new Label();
            panelRightCard = new RoundedPanel();
            pnlResultCrop = new RoundedPanel();
            picResult = new PictureBox();
            lblResultCropTitle = new Label();
            pnlMetricsGroup = new TableLayoutPanel();
            pnlTimeMetric = new RoundedPanel();
            lblTime = new Label();
            labelTimeTitle = new Label();
            pnlConfMetric = new RoundedPanel();
            lblConfidence = new Label();
            labelConfTitle = new Label();
            pnlDefectMetric = new RoundedPanel();
            lblDefect = new Label();
            labelDefectTitle = new Label();
            pnlStatusBanner = new RoundedPanel();
            lblStatus = new Label();
            pnlResultHeader = new Panel();
            lblResultTitle = new Label();
            iconResult = new FontAwesome.Sharp.IconPictureBox();
            panelTopStats.SuspendLayout();
            tableTopCards.SuspendLayout();
            cardTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconTotal).BeginInit();
            cardOk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconOk).BeginInit();
            cardNg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconNg).BeginInit();
            cardYield.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconYield).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            panelLeftCard.SuspendLayout();
            pnlCameraHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconCamera).BeginInit();
            pnlWebcamContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picWebcam).BeginInit();
            pnlCameraActions.SuspendLayout();
            pnlLogContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInspectionLog).BeginInit();
            panelRightCard.SuspendLayout();
            pnlResultCrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picResult).BeginInit();
            pnlMetricsGroup.SuspendLayout();
            pnlTimeMetric.SuspendLayout();
            pnlConfMetric.SuspendLayout();
            pnlDefectMetric.SuspendLayout();
            pnlStatusBanner.SuspendLayout();
            pnlResultHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconResult).BeginInit();
            SuspendLayout();
            // 
            // panelTopStats
            // 
            panelTopStats.Controls.Add(tableTopCards);
            panelTopStats.Dock = DockStyle.Top;
            panelTopStats.Location = new Point(14, 14);
            panelTopStats.Name = "panelTopStats";
            panelTopStats.Size = new Size(1012, 85);
            panelTopStats.TabIndex = 0;
            // 
            // tableTopCards
            // 
            tableTopCards.ColumnCount = 4;
            tableTopCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTopCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTopCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTopCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableTopCards.Controls.Add(cardTotal, 0, 0);
            tableTopCards.Controls.Add(cardOk, 1, 0);
            tableTopCards.Controls.Add(cardNg, 2, 0);
            tableTopCards.Controls.Add(cardYield, 3, 0);
            tableTopCards.Dock = DockStyle.Fill;
            tableTopCards.Location = new Point(0, 0);
            tableTopCards.Name = "tableTopCards";
            tableTopCards.RowCount = 1;
            tableTopCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableTopCards.Size = new Size(1012, 85);
            tableTopCards.TabIndex = 0;
            // 
            // cardTotal
            // 
            cardTotal.BackColor = Color.White;
            cardTotal.BackgroundColor = Color.White;
            cardTotal.BorderColor = Color.FromArgb(226, 232, 240);
            cardTotal.BorderSize = 1;
            cardTotal.Controls.Add(lblStatTotalVal);
            cardTotal.Controls.Add(lblStatTotalTitle);
            cardTotal.Controls.Add(iconTotal);
            cardTotal.Dock = DockStyle.Fill;
            cardTotal.Location = new Point(0, 0);
            cardTotal.Margin = new Padding(0, 0, 6, 0);
            cardTotal.Name = "cardTotal";
            cardTotal.Padding = new Padding(12);
            cardTotal.Size = new Size(247, 85);
            cardTotal.TabIndex = 0;
            // 
            // lblStatTotalVal
            // 
            lblStatTotalVal.AutoSize = true;
            lblStatTotalVal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStatTotalVal.ForeColor = Color.FromArgb(15, 23, 42);
            lblStatTotalVal.Location = new Point(12, 33);
            lblStatTotalVal.Name = "lblStatTotalVal";
            lblStatTotalVal.Size = new Size(35, 41);
            lblStatTotalVal.TabIndex = 2;
            lblStatTotalVal.Text = "0";
            // 
            // lblStatTotalTitle
            // 
            lblStatTotalTitle.AutoSize = true;
            lblStatTotalTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatTotalTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblStatTotalTitle.Location = new Point(12, 10);
            lblStatTotalTitle.Name = "lblStatTotalTitle";
            lblStatTotalTitle.Size = new Size(140, 20);
            lblStatTotalTitle.TabIndex = 1;
            lblStatTotalTitle.Text = "TỔNG LƯỢT QUÉT";
            // 
            // iconTotal
            // 
            iconTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconTotal.BackColor = Color.Transparent;
            iconTotal.ForeColor = Color.FromArgb(37, 99, 235);
            iconTotal.IconChar = FontAwesome.Sharp.IconChar.LayerGroup;
            iconTotal.IconColor = Color.FromArgb(37, 99, 235);
            iconTotal.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconTotal.IconSize = 36;
            iconTotal.Location = new Point(194, 24);
            iconTotal.Name = "iconTotal";
            iconTotal.Size = new Size(36, 36);
            iconTotal.SizeMode = PictureBoxSizeMode.CenterImage;
            iconTotal.TabIndex = 0;
            iconTotal.TabStop = false;
            // 
            // cardOk
            // 
            cardOk.BackColor = Color.White;
            cardOk.BackgroundColor = Color.White;
            cardOk.BorderColor = Color.FromArgb(226, 232, 240);
            cardOk.BorderSize = 1;
            cardOk.Controls.Add(lblStatOkVal);
            cardOk.Controls.Add(lblStatOkTitle);
            cardOk.Controls.Add(iconOk);
            cardOk.Dock = DockStyle.Fill;
            cardOk.Location = new Point(259, 0);
            cardOk.Margin = new Padding(6, 0, 6, 0);
            cardOk.Name = "cardOk";
            cardOk.Padding = new Padding(12);
            cardOk.Size = new Size(241, 85);
            cardOk.TabIndex = 1;
            // 
            // lblStatOkVal
            // 
            lblStatOkVal.AutoSize = true;
            lblStatOkVal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStatOkVal.ForeColor = Color.FromArgb(16, 185, 129);
            lblStatOkVal.Location = new Point(12, 33);
            lblStatOkVal.Name = "lblStatOkVal";
            lblStatOkVal.Size = new Size(35, 41);
            lblStatOkVal.TabIndex = 2;
            lblStatOkVal.Text = "0";
            // 
            // lblStatOkTitle
            // 
            lblStatOkTitle.AutoSize = true;
            lblStatOkTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatOkTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblStatOkTitle.Location = new Point(12, 10);
            lblStatOkTitle.Name = "lblStatOkTitle";
            lblStatOkTitle.Size = new Size(135, 20);
            lblStatOkTitle.TabIndex = 1;
            lblStatOkTitle.Text = "ĐẠT CHUẨN (OK)";
            // 
            // iconOk
            // 
            iconOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconOk.BackColor = Color.Transparent;
            iconOk.ForeColor = Color.FromArgb(16, 185, 129);
            iconOk.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            iconOk.IconColor = Color.FromArgb(16, 185, 129);
            iconOk.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconOk.IconSize = 36;
            iconOk.Location = new Point(189, 24);
            iconOk.Name = "iconOk";
            iconOk.Size = new Size(36, 36);
            iconOk.SizeMode = PictureBoxSizeMode.CenterImage;
            iconOk.TabIndex = 0;
            iconOk.TabStop = false;
            // 
            // cardNg
            // 
            cardNg.BackColor = Color.White;
            cardNg.BackgroundColor = Color.White;
            cardNg.BorderColor = Color.FromArgb(226, 232, 240);
            cardNg.BorderSize = 1;
            cardNg.Controls.Add(lblStatNgVal);
            cardNg.Controls.Add(lblStatNgTitle);
            cardNg.Controls.Add(iconNg);
            cardNg.Dock = DockStyle.Fill;
            cardNg.Location = new Point(512, 0);
            cardNg.Margin = new Padding(6, 0, 6, 0);
            cardNg.Name = "cardNg";
            cardNg.Padding = new Padding(12);
            cardNg.Size = new Size(241, 85);
            cardNg.TabIndex = 2;
            // 
            // lblStatNgVal
            // 
            lblStatNgVal.AutoSize = true;
            lblStatNgVal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStatNgVal.ForeColor = Color.FromArgb(239, 68, 68);
            lblStatNgVal.Location = new Point(12, 33);
            lblStatNgVal.Name = "lblStatNgVal";
            lblStatNgVal.Size = new Size(35, 41);
            lblStatNgVal.TabIndex = 2;
            lblStatNgVal.Text = "0";
            // 
            // lblStatNgTitle
            // 
            lblStatNgTitle.AutoSize = true;
            lblStatNgTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatNgTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblStatNgTitle.Location = new Point(12, 10);
            lblStatNgTitle.Name = "lblStatNgTitle";
            lblStatNgTitle.Size = new Size(155, 20);
            lblStatNgTitle.TabIndex = 1;
            lblStatNgTitle.Text = "PHÁT HIỆN LỖI (NG)";
            // 
            // iconNg
            // 
            iconNg.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconNg.BackColor = Color.Transparent;
            iconNg.ForeColor = Color.FromArgb(239, 68, 68);
            iconNg.IconChar = FontAwesome.Sharp.IconChar.Warning;
            iconNg.IconColor = Color.FromArgb(239, 68, 68);
            iconNg.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconNg.IconSize = 36;
            iconNg.Location = new Point(189, 24);
            iconNg.Name = "iconNg";
            iconNg.Size = new Size(36, 36);
            iconNg.SizeMode = PictureBoxSizeMode.CenterImage;
            iconNg.TabIndex = 0;
            iconNg.TabStop = false;
            // 
            // cardYield
            // 
            cardYield.BackColor = Color.White;
            cardYield.BackgroundColor = Color.White;
            cardYield.BorderColor = Color.FromArgb(226, 232, 240);
            cardYield.BorderSize = 1;
            cardYield.Controls.Add(lblStatYieldVal);
            cardYield.Controls.Add(lblStatYieldTitle);
            cardYield.Controls.Add(iconYield);
            cardYield.Dock = DockStyle.Fill;
            cardYield.Location = new Point(765, 0);
            cardYield.Margin = new Padding(6, 0, 0, 0);
            cardYield.Name = "cardYield";
            cardYield.Padding = new Padding(12);
            cardYield.Size = new Size(247, 85);
            cardYield.TabIndex = 3;
            // 
            // lblStatYieldVal
            // 
            lblStatYieldVal.AutoSize = true;
            lblStatYieldVal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblStatYieldVal.ForeColor = Color.FromArgb(2, 132, 199);
            lblStatYieldVal.Location = new Point(12, 33);
            lblStatYieldVal.Name = "lblStatYieldVal";
            lblStatYieldVal.Size = new Size(94, 41);
            lblStatYieldVal.TabIndex = 2;
            lblStatYieldVal.Text = "0.0 %";
            // 
            // lblStatYieldTitle
            // 
            lblStatYieldTitle.AutoSize = true;
            lblStatYieldTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblStatYieldTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblStatYieldTitle.Location = new Point(12, 10);
            lblStatYieldTitle.Name = "lblStatYieldTitle";
            lblStatYieldTitle.Size = new Size(111, 20);
            lblStatYieldTitle.TabIndex = 1;
            lblStatYieldTitle.Text = "TỶ LỆ ĐẠT (%)";
            // 
            // iconYield
            // 
            iconYield.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            iconYield.BackColor = Color.Transparent;
            iconYield.ForeColor = Color.FromArgb(2, 132, 199);
            iconYield.IconChar = FontAwesome.Sharp.IconChar.PieChart;
            iconYield.IconColor = Color.FromArgb(2, 132, 199);
            iconYield.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconYield.IconSize = 36;
            iconYield.Location = new Point(195, 24);
            iconYield.Name = "iconYield";
            iconYield.Size = new Size(36, 36);
            iconYield.SizeMode = PictureBoxSizeMode.CenterImage;
            iconYield.TabIndex = 0;
            iconYield.TabStop = false;
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(14, 99);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(panelLeftCard);
            splitContainerMain.Panel1.Padding = new Padding(0, 0, 7, 0);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(panelRightCard);
            splitContainerMain.Panel2.Padding = new Padding(7, 0, 0, 0);
            splitContainerMain.Size = new Size(1012, 591);
            splitContainerMain.SplitterDistance = 570;
            splitContainerMain.TabIndex = 1;
            // 
            // panelLeftCard
            // 
            panelLeftCard.BackColor = Color.White;
            panelLeftCard.BackgroundColor = Color.White;
            panelLeftCard.BorderColor = Color.FromArgb(226, 232, 240);
            panelLeftCard.BorderRadius = 12;
            panelLeftCard.BorderSize = 1;
            panelLeftCard.Controls.Add(pnlWebcamContainer);
            panelLeftCard.Controls.Add(pnlCameraActions);
            panelLeftCard.Controls.Add(pnlLogContainer);
            panelLeftCard.Controls.Add(pnlCameraHeader);
            panelLeftCard.Dock = DockStyle.Fill;
            panelLeftCard.Location = new Point(0, 0);
            panelLeftCard.Name = "panelLeftCard";
            panelLeftCard.Padding = new Padding(12);
            panelLeftCard.Size = new Size(563, 591);
            panelLeftCard.TabIndex = 0;
            // 
            // pnlCameraHeader
            // 
            pnlCameraHeader.Controls.Add(lblLiveBadge);
            pnlCameraHeader.Controls.Add(lblCameraTitle);
            pnlCameraHeader.Controls.Add(iconCamera);
            pnlCameraHeader.Dock = DockStyle.Top;
            pnlCameraHeader.Location = new Point(12, 12);
            pnlCameraHeader.Margin = new Padding(0, 0, 0, 4);
            pnlCameraHeader.Name = "pnlCameraHeader";
            pnlCameraHeader.Size = new Size(539, 38);
            pnlCameraHeader.TabIndex = 0;
            // 
            // lblLiveBadge
            // 
            lblLiveBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLiveBadge.BackColor = Color.FromArgb(220, 252, 231);
            lblLiveBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblLiveBadge.ForeColor = Color.FromArgb(16, 185, 129);
            lblLiveBadge.Location = new Point(433, 7);
            lblLiveBadge.Name = "lblLiveBadge";
            lblLiveBadge.Size = new Size(100, 24);
            lblLiveBadge.TabIndex = 2;
            lblLiveBadge.Text = "● LIVE STREAM";
            lblLiveBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblCameraTitle
            // 
            lblCameraTitle.AutoSize = true;
            lblCameraTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblCameraTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblCameraTitle.Location = new Point(35, 7);
            lblCameraTitle.Name = "lblCameraTitle";
            lblCameraTitle.Size = new Size(189, 25);
            lblCameraTitle.TabIndex = 1;
            lblCameraTitle.Text = "CAMERA TRỰC TIẾP";
            // 
            // iconCamera
            // 
            iconCamera.BackColor = Color.Transparent;
            iconCamera.ForeColor = Color.FromArgb(37, 99, 235);
            iconCamera.IconChar = FontAwesome.Sharp.IconChar.VideoCamera;
            iconCamera.IconColor = Color.FromArgb(37, 99, 235);
            iconCamera.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconCamera.IconSize = 24;
            iconCamera.Location = new Point(3, 7);
            iconCamera.Name = "iconCamera";
            iconCamera.Size = new Size(24, 24);
            iconCamera.SizeMode = PictureBoxSizeMode.CenterImage;
            iconCamera.TabIndex = 0;
            iconCamera.TabStop = false;
            // 
            // pnlWebcamContainer
            // 
            pnlWebcamContainer.BackColor = Color.FromArgb(15, 23, 42);
            pnlWebcamContainer.Controls.Add(picWebcam);
            pnlWebcamContainer.Dock = DockStyle.Fill;
            pnlWebcamContainer.Location = new Point(12, 50);
            pnlWebcamContainer.Margin = new Padding(0, 0, 0, 6);
            pnlWebcamContainer.Name = "pnlWebcamContainer";
            pnlWebcamContainer.Padding = new Padding(3);
            pnlWebcamContainer.Size = new Size(539, 329);
            pnlWebcamContainer.TabIndex = 1;
            // 
            // picWebcam
            // 
            picWebcam.BackColor = Color.FromArgb(15, 23, 42);
            picWebcam.Dock = DockStyle.Fill;
            picWebcam.Location = new Point(3, 3);
            picWebcam.Name = "picWebcam";
            picWebcam.Size = new Size(533, 323);
            picWebcam.SizeMode = PictureBoxSizeMode.Zoom;
            picWebcam.TabIndex = 0;
            picWebcam.TabStop = false;
            // 
            // pnlCameraActions
            // 
            pnlCameraActions.Controls.Add(btnStart);
            pnlCameraActions.Controls.Add(btnStop);
            pnlCameraActions.Controls.Add(btnResetStats);
            pnlCameraActions.Dock = DockStyle.Bottom;
            pnlCameraActions.Location = new Point(12, 381);
            pnlCameraActions.Margin = new Padding(0, 0, 0, 6);
            pnlCameraActions.Name = "pnlCameraActions";
            pnlCameraActions.Padding = new Padding(0, 4, 0, 4);
            pnlCameraActions.Size = new Size(539, 44);
            pnlCameraActions.TabIndex = 2;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(16, 185, 129);
            btnStart.Cursor = Cursors.Hand;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.IconChar = FontAwesome.Sharp.IconChar.Play;
            btnStart.IconColor = Color.White;
            btnStart.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnStart.IconSize = 16;
            btnStart.Location = new Point(0, 4);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(130, 36);
            btnStart.TabIndex = 0;
            btnStart.Text = " BẮT ĐẦU";
            btnStart.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // btnStop
            // 
            btnStop.BackColor = Color.FromArgb(239, 68, 68);
            btnStop.Cursor = Cursors.Hand;
            btnStop.Enabled = false;
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.FlatStyle = FlatStyle.Flat;
            btnStop.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnStop.ForeColor = Color.White;
            btnStop.IconChar = FontAwesome.Sharp.IconChar.Stop;
            btnStop.IconColor = Color.White;
            btnStop.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnStop.IconSize = 16;
            btnStop.Location = new Point(140, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(115, 36);
            btnStop.TabIndex = 1;
            btnStop.Text = " DỪNG";
            btnStop.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnStop.UseVisualStyleBackColor = false;
            btnStop.Click += btnStop_Click;
            // 
            // btnResetStats
            // 
            btnResetStats.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnResetStats.BackColor = Color.FromArgb(248, 250, 252);
            btnResetStats.Cursor = Cursors.Hand;
            btnResetStats.FlatAppearance.BorderColor = Color.FromArgb(226, 232, 240);
            btnResetStats.FlatStyle = FlatStyle.Flat;
            btnResetStats.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnResetStats.ForeColor = Color.FromArgb(71, 85, 105);
            btnResetStats.IconChar = FontAwesome.Sharp.IconChar.SyncAlt;
            btnResetStats.IconColor = Color.FromArgb(71, 85, 105);
            btnResetStats.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnResetStats.IconSize = 15;
            btnResetStats.Location = new Point(419, 4);
            btnResetStats.Name = "btnResetStats";
            btnResetStats.Size = new Size(120, 36);
            btnResetStats.TabIndex = 2;
            btnResetStats.Text = " ĐẶT LẠI";
            btnResetStats.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnResetStats.UseVisualStyleBackColor = false;
            btnResetStats.Click += btnResetStats_Click;
            // 
            // pnlLogContainer
            // 
            pnlLogContainer.Controls.Add(dgvInspectionLog);
            pnlLogContainer.Controls.Add(lblLogTitle);
            pnlLogContainer.Dock = DockStyle.Bottom;
            pnlLogContainer.Location = new Point(12, 407);
            pnlLogContainer.Margin = new Padding(0);
            pnlLogContainer.Name = "pnlLogContainer";
            pnlLogContainer.Padding = new Padding(0, 4, 0, 0);
            pnlLogContainer.Size = new Size(539, 172);
            pnlLogContainer.TabIndex = 3;
            // 
            // dgvInspectionLog
            // 
            dgvInspectionLog.AllowUserToAddRows = false;
            dgvInspectionLog.AllowUserToDeleteRows = false;
            dgvInspectionLog.AllowUserToResizeRows = false;
            dgvInspectionLog.BackgroundColor = Color.FromArgb(248, 250, 252);
            dgvInspectionLog.BorderStyle = BorderStyle.None;
            dgvInspectionLog.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvInspectionLog.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvInspectionLog.Dock = DockStyle.Fill;
            dgvInspectionLog.Location = new Point(0, 26);
            dgvInspectionLog.MultiSelect = false;
            dgvInspectionLog.Name = "dgvInspectionLog";
            dgvInspectionLog.ReadOnly = true;
            dgvInspectionLog.RowHeadersVisible = false;
            dgvInspectionLog.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInspectionLog.Size = new Size(539, 146);
            dgvInspectionLog.TabIndex = 1;
            // 
            // lblLogTitle
            // 
            lblLogTitle.Dock = DockStyle.Top;
            lblLogTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            lblLogTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblLogTitle.Location = new Point(0, 4);
            lblLogTitle.Name = "lblLogTitle";
            lblLogTitle.Size = new Size(539, 22);
            lblLogTitle.TabIndex = 0;
            lblLogTitle.Text = "LỊCH SỬ NHẬN DIỆN VÒNG BI GẦN ĐÂY";
            // 
            // panelRightCard
            // 
            panelRightCard.BackColor = Color.White;
            panelRightCard.BackgroundColor = Color.White;
            panelRightCard.BorderColor = Color.FromArgb(226, 232, 240);
            panelRightCard.BorderRadius = 12;
            panelRightCard.BorderSize = 1;
            panelRightCard.Controls.Add(pnlResultCrop);
            panelRightCard.Controls.Add(pnlMetricsGroup);
            panelRightCard.Controls.Add(pnlStatusBanner);
            panelRightCard.Controls.Add(pnlResultHeader);
            panelRightCard.Dock = DockStyle.Fill;
            panelRightCard.Location = new Point(7, 0);
            panelRightCard.Name = "panelRightCard";
            panelRightCard.Padding = new Padding(15);
            panelRightCard.Size = new Size(431, 591);
            panelRightCard.TabIndex = 0;
            // 
            // pnlResultCrop
            // 
            pnlResultCrop.BackColor = Color.FromArgb(248, 250, 252);
            pnlResultCrop.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlResultCrop.BorderColor = Color.FromArgb(226, 232, 240);
            pnlResultCrop.BorderRadius = 8;
            pnlResultCrop.BorderSize = 1;
            pnlResultCrop.Controls.Add(picResult);
            pnlResultCrop.Controls.Add(lblResultCropTitle);
            pnlResultCrop.Dock = DockStyle.Fill;
            pnlResultCrop.Location = new Point(15, 290);
            pnlResultCrop.Name = "pnlResultCrop";
            pnlResultCrop.Padding = new Padding(10);
            pnlResultCrop.Size = new Size(401, 286);
            pnlResultCrop.TabIndex = 3;
            // 
            // picResult
            // 
            picResult.BackColor = Color.FromArgb(15, 23, 42);
            picResult.Dock = DockStyle.Fill;
            picResult.Location = new Point(10, 35);
            picResult.Name = "picResult";
            picResult.Size = new Size(381, 241);
            picResult.SizeMode = PictureBoxSizeMode.Zoom;
            picResult.TabIndex = 1;
            picResult.TabStop = false;
            // 
            // lblResultCropTitle
            // 
            lblResultCropTitle.Dock = DockStyle.Top;
            lblResultCropTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblResultCropTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblResultCropTitle.Location = new Point(10, 10);
            lblResultCropTitle.Name = "lblResultCropTitle";
            lblResultCropTitle.Size = new Size(381, 25);
            lblResultCropTitle.TabIndex = 0;
            lblResultCropTitle.Text = "VÙNG VÒNG BI PHÂN TÍCH (CROP)";
            // 
            // pnlMetricsGroup
            // 
            pnlMetricsGroup.ColumnCount = 3;
            pnlMetricsGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            pnlMetricsGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            pnlMetricsGroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            pnlMetricsGroup.Controls.Add(pnlTimeMetric, 2, 0);
            pnlMetricsGroup.Controls.Add(pnlConfMetric, 1, 0);
            pnlMetricsGroup.Controls.Add(pnlDefectMetric, 0, 0);
            pnlMetricsGroup.Dock = DockStyle.Top;
            pnlMetricsGroup.Location = new Point(15, 195);
            pnlMetricsGroup.Name = "pnlMetricsGroup";
            pnlMetricsGroup.RowCount = 1;
            pnlMetricsGroup.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlMetricsGroup.Size = new Size(401, 95);
            pnlMetricsGroup.TabIndex = 2;
            // 
            // pnlTimeMetric
            // 
            pnlTimeMetric.BackColor = Color.FromArgb(248, 250, 252);
            pnlTimeMetric.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlTimeMetric.BorderColor = Color.FromArgb(226, 232, 240);
            pnlTimeMetric.BorderRadius = 8;
            pnlTimeMetric.BorderSize = 1;
            pnlTimeMetric.Controls.Add(lblTime);
            pnlTimeMetric.Controls.Add(labelTimeTitle);
            pnlTimeMetric.Dock = DockStyle.Fill;
            pnlTimeMetric.Location = new Point(283, 3);
            pnlTimeMetric.Name = "pnlTimeMetric";
            pnlTimeMetric.Padding = new Padding(8);
            pnlTimeMetric.Size = new Size(115, 89);
            pnlTimeMetric.TabIndex = 2;
            // 
            // lblTime
            // 
            lblTime.Dock = DockStyle.Fill;
            lblTime.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTime.ForeColor = Color.FromArgb(217, 119, 6);
            lblTime.Location = new Point(8, 30);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(99, 51);
            lblTime.TabIndex = 1;
            lblTime.Text = "-";
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelTimeTitle
            // 
            labelTimeTitle.Dock = DockStyle.Top;
            labelTimeTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            labelTimeTitle.ForeColor = Color.FromArgb(71, 85, 105);
            labelTimeTitle.Location = new Point(8, 8);
            labelTimeTitle.Name = "labelTimeTitle";
            labelTimeTitle.Size = new Size(99, 22);
            labelTimeTitle.TabIndex = 0;
            labelTimeTitle.Text = "THỜI GIAN";
            labelTimeTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlConfMetric
            // 
            pnlConfMetric.BackColor = Color.FromArgb(248, 250, 252);
            pnlConfMetric.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlConfMetric.BorderColor = Color.FromArgb(226, 232, 240);
            pnlConfMetric.BorderRadius = 8;
            pnlConfMetric.BorderSize = 1;
            pnlConfMetric.Controls.Add(lblConfidence);
            pnlConfMetric.Controls.Add(labelConfTitle);
            pnlConfMetric.Dock = DockStyle.Fill;
            pnlConfMetric.Location = new Point(163, 3);
            pnlConfMetric.Name = "pnlConfMetric";
            pnlConfMetric.Padding = new Padding(8);
            pnlConfMetric.Size = new Size(114, 89);
            pnlConfMetric.TabIndex = 1;
            // 
            // lblConfidence
            // 
            lblConfidence.Dock = DockStyle.Fill;
            lblConfidence.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblConfidence.ForeColor = Color.FromArgb(2, 132, 199);
            lblConfidence.Location = new Point(8, 30);
            lblConfidence.Name = "lblConfidence";
            lblConfidence.Size = new Size(98, 51);
            lblConfidence.TabIndex = 1;
            lblConfidence.Text = "-";
            lblConfidence.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelConfTitle
            // 
            labelConfTitle.Dock = DockStyle.Top;
            labelConfTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            labelConfTitle.ForeColor = Color.FromArgb(71, 85, 105);
            labelConfTitle.Location = new Point(8, 8);
            labelConfTitle.Name = "labelConfTitle";
            labelConfTitle.Size = new Size(98, 22);
            labelConfTitle.TabIndex = 0;
            labelConfTitle.Text = "ĐỘ TIN CẬY";
            labelConfTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlDefectMetric
            // 
            pnlDefectMetric.BackColor = Color.FromArgb(248, 250, 252);
            pnlDefectMetric.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlDefectMetric.BorderColor = Color.FromArgb(226, 232, 240);
            pnlDefectMetric.BorderRadius = 8;
            pnlDefectMetric.BorderSize = 1;
            pnlDefectMetric.Controls.Add(lblDefect);
            pnlDefectMetric.Controls.Add(labelDefectTitle);
            pnlDefectMetric.Dock = DockStyle.Fill;
            pnlDefectMetric.Location = new Point(3, 3);
            pnlDefectMetric.Name = "pnlDefectMetric";
            pnlDefectMetric.Padding = new Padding(8);
            pnlDefectMetric.Size = new Size(154, 89);
            pnlDefectMetric.TabIndex = 0;
            // 
            // lblDefect
            // 
            lblDefect.Dock = DockStyle.Fill;
            lblDefect.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblDefect.ForeColor = Color.FromArgb(15, 23, 42);
            lblDefect.Location = new Point(8, 30);
            lblDefect.Name = "lblDefect";
            lblDefect.Size = new Size(138, 51);
            lblDefect.TabIndex = 1;
            lblDefect.Text = "-";
            lblDefect.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelDefectTitle
            // 
            labelDefectTitle.Dock = DockStyle.Top;
            labelDefectTitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Bold);
            labelDefectTitle.ForeColor = Color.FromArgb(71, 85, 105);
            labelDefectTitle.Location = new Point(8, 8);
            labelDefectTitle.Name = "labelDefectTitle";
            labelDefectTitle.Size = new Size(138, 22);
            labelDefectTitle.TabIndex = 0;
            labelDefectTitle.Text = "PHÂN LOẠI LỖI";
            labelDefectTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlStatusBanner
            // 
            pnlStatusBanner.BackColor = Color.FromArgb(248, 250, 252);
            pnlStatusBanner.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlStatusBanner.BorderColor = Color.FromArgb(226, 232, 240);
            pnlStatusBanner.BorderSize = 1;
            pnlStatusBanner.Controls.Add(lblStatus);
            pnlStatusBanner.Dock = DockStyle.Top;
            pnlStatusBanner.Location = new Point(15, 55);
            pnlStatusBanner.Name = "pnlStatusBanner";
            pnlStatusBanner.Padding = new Padding(12);
            pnlStatusBanner.Size = new Size(401, 140);
            pnlStatusBanner.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(71, 85, 105);
            lblStatus.Location = new Point(12, 12);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(377, 116);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "SẴN SÀNG";
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pnlResultHeader
            // 
            pnlResultHeader.Controls.Add(lblResultTitle);
            pnlResultHeader.Controls.Add(iconResult);
            pnlResultHeader.Dock = DockStyle.Top;
            pnlResultHeader.Location = new Point(15, 15);
            pnlResultHeader.Name = "pnlResultHeader";
            pnlResultHeader.Size = new Size(401, 40);
            pnlResultHeader.TabIndex = 0;
            // 
            // lblResultTitle
            // 
            lblResultTitle.AutoSize = true;
            lblResultTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblResultTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblResultTitle.Location = new Point(35, 8);
            lblResultTitle.Name = "lblResultTitle";
            lblResultTitle.Size = new Size(199, 25);
            lblResultTitle.TabIndex = 1;
            lblResultTitle.Text = "KẾT QUẢ PHÂN LOẠI";
            // 
            // iconResult
            // 
            iconResult.BackColor = Color.Transparent;
            iconResult.ForeColor = Color.FromArgb(37, 99, 235);
            iconResult.IconChar = FontAwesome.Sharp.IconChar.Poll;
            iconResult.IconColor = Color.FromArgb(37, 99, 235);
            iconResult.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconResult.IconSize = 24;
            iconResult.Location = new Point(3, 8);
            iconResult.Name = "iconResult";
            iconResult.Size = new Size(24, 24);
            iconResult.SizeMode = PictureBoxSizeMode.CenterImage;
            iconResult.TabIndex = 0;
            iconResult.TabStop = false;
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1040, 704);
            Controls.Add(splitContainerMain);
            Controls.Add(panelTopStats);
            Name = "FormHome";
            Padding = new Padding(14);
            Text = "FormHome";
            FormClosing += FormHome_FormClosing;
            Load += FormHome_Load;
            Resize += FormHome_Resize;
            panelTopStats.ResumeLayout(false);
            tableTopCards.ResumeLayout(false);
            cardTotal.ResumeLayout(false);
            cardTotal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconTotal).EndInit();
            cardOk.ResumeLayout(false);
            cardOk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconOk).EndInit();
            cardNg.ResumeLayout(false);
            cardNg.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconNg).EndInit();
            cardYield.ResumeLayout(false);
            cardYield.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconYield).EndInit();
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            panelLeftCard.ResumeLayout(false);
            pnlCameraHeader.ResumeLayout(false);
            pnlCameraHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconCamera).EndInit();
            pnlWebcamContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picWebcam).EndInit();
            pnlCameraActions.ResumeLayout(false);
            pnlLogContainer.ResumeLayout(false);
            pnlLogContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInspectionLog).EndInit();
            panelRightCard.ResumeLayout(false);
            pnlResultCrop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picResult).EndInit();
            pnlMetricsGroup.ResumeLayout(false);
            pnlTimeMetric.ResumeLayout(false);
            pnlConfMetric.ResumeLayout(false);
            pnlDefectMetric.ResumeLayout(false);
            pnlStatusBanner.ResumeLayout(false);
            pnlResultHeader.ResumeLayout(false);
            pnlResultHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconResult).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelTopStats;
        private TableLayoutPanel tableTopCards;
        private RoundedPanel cardTotal;
        private Label lblStatTotalVal;
        private Label lblStatTotalTitle;
        private FontAwesome.Sharp.IconPictureBox iconTotal;
        private RoundedPanel cardOk;
        private Label lblStatOkVal;
        private Label lblStatOkTitle;
        private FontAwesome.Sharp.IconPictureBox iconOk;
        private RoundedPanel cardNg;
        private Label lblStatNgVal;
        private Label lblStatNgTitle;
        private FontAwesome.Sharp.IconPictureBox iconNg;
        private RoundedPanel cardYield;
        private Label lblStatYieldVal;
        private Label lblStatYieldTitle;
        private FontAwesome.Sharp.IconPictureBox iconYield;
        private SplitContainer splitContainerMain;
        private RoundedPanel panelLeftCard;
        private Panel pnlCameraHeader;
        private Label lblCameraTitle;
        private FontAwesome.Sharp.IconPictureBox iconCamera;
        private Label lblLiveBadge;
        private Panel pnlCameraActions;
        private FontAwesome.Sharp.IconButton btnStart;
        private FontAwesome.Sharp.IconButton btnStop;
        private FontAwesome.Sharp.IconButton btnResetStats;
        private Panel pnlWebcamContainer;
        private PictureBox picWebcam;
        private Panel pnlLogContainer;
        private Label lblLogTitle;
        private DataGridView dgvInspectionLog;
        private RoundedPanel panelRightCard;
        private Panel pnlResultHeader;
        private Label lblResultTitle;
        private FontAwesome.Sharp.IconPictureBox iconResult;
        private RoundedPanel pnlStatusBanner;
        private Label lblStatus;
        private TableLayoutPanel pnlMetricsGroup;
        private RoundedPanel pnlDefectMetric;
        private Label labelDefectTitle;
        private Label lblDefect;
        private RoundedPanel pnlConfMetric;
        private Label labelConfTitle;
        private Label lblConfidence;
        private RoundedPanel pnlTimeMetric;
        private Label labelTimeTitle;
        private Label lblTime;
        private RoundedPanel pnlResultCrop;
        private Label lblResultCropTitle;
        private PictureBox picResult;
    }
}