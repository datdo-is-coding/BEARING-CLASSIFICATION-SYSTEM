namespace DashBoardBearing
{
    partial class FormVision
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
            splitContainerVision = new SplitContainer();
            panelLeftCard = new RoundedPanel();
            pnlVisionActions = new Panel();
            btnVisionDetect = new FontAwesome.Sharp.IconButton();
            pnlVisionFeedContainer = new Panel();
            picVisionFeed = new PictureBox();
            pnlVisionHeader = new Panel();
            lblLiveBadge = new Label();
            lblVisionTitle = new Label();
            iconVision = new FontAwesome.Sharp.IconPictureBox();
            panelRightCard = new RoundedPanel();
            pnlVisionCrop = new RoundedPanel();
            picVisionCrop = new PictureBox();
            lblVisionCropTitle = new Label();
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
            ((System.ComponentModel.ISupportInitialize)splitContainerVision).BeginInit();
            splitContainerVision.Panel1.SuspendLayout();
            splitContainerVision.Panel2.SuspendLayout();
            splitContainerVision.SuspendLayout();
            panelLeftCard.SuspendLayout();
            pnlVisionActions.SuspendLayout();
            pnlVisionFeedContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picVisionFeed).BeginInit();
            pnlVisionHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconVision).BeginInit();
            panelRightCard.SuspendLayout();
            pnlVisionCrop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picVisionCrop).BeginInit();
            pnlMetricsGroup.SuspendLayout();
            pnlTimeMetric.SuspendLayout();
            pnlConfMetric.SuspendLayout();
            pnlDefectMetric.SuspendLayout();
            pnlStatusBanner.SuspendLayout();
            pnlResultHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconResult).BeginInit();
            SuspendLayout();
            // 
            // splitContainerVision
            // 
            splitContainerVision.Dock = DockStyle.Fill;
            splitContainerVision.Location = new Point(14, 14);
            splitContainerVision.Name = "splitContainerVision";
            // 
            // splitContainerVision.Panel1
            // 
            splitContainerVision.Panel1.Controls.Add(panelLeftCard);
            splitContainerVision.Panel1.Padding = new Padding(0, 0, 7, 0);
            // 
            // splitContainerVision.Panel2
            // 
            splitContainerVision.Panel2.Controls.Add(panelRightCard);
            splitContainerVision.Panel2.Padding = new Padding(7, 0, 0, 0);
            splitContainerVision.Size = new Size(1012, 676);
            splitContainerVision.SplitterDistance = 570;
            splitContainerVision.TabIndex = 0;
            // 
            // panelLeftCard
            // 
            panelLeftCard.BackColor = Color.White;
            panelLeftCard.BackgroundColor = Color.White;
            panelLeftCard.BorderColor = Color.FromArgb(226, 232, 240);
            panelLeftCard.BorderRadius = 12;
            panelLeftCard.BorderSize = 1;
            panelLeftCard.Controls.Add(pnlVisionActions);
            panelLeftCard.Controls.Add(pnlVisionFeedContainer);
            panelLeftCard.Controls.Add(pnlVisionHeader);
            panelLeftCard.Dock = DockStyle.Fill;
            panelLeftCard.Location = new Point(0, 0);
            panelLeftCard.Name = "panelLeftCard";
            panelLeftCard.Padding = new Padding(15);
            panelLeftCard.Size = new Size(563, 676);
            panelLeftCard.TabIndex = 0;
            // 
            // pnlVisionActions
            // 
            pnlVisionActions.Controls.Add(btnVisionDetect);
            pnlVisionActions.Dock = DockStyle.Bottom;
            pnlVisionActions.Location = new Point(15, 601);
            pnlVisionActions.Name = "pnlVisionActions";
            pnlVisionActions.Padding = new Padding(0, 10, 0, 0);
            pnlVisionActions.Size = new Size(533, 60);
            pnlVisionActions.TabIndex = 2;
            // 
            // btnVisionDetect
            // 
            btnVisionDetect.BackColor = Color.FromArgb(37, 99, 235);
            btnVisionDetect.Dock = DockStyle.Fill;
            btnVisionDetect.FlatAppearance.BorderSize = 0;
            btnVisionDetect.FlatStyle = FlatStyle.Flat;
            btnVisionDetect.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnVisionDetect.ForeColor = Color.White;
            btnVisionDetect.IconChar = FontAwesome.Sharp.IconChar.Crosshairs;
            btnVisionDetect.IconColor = Color.White;
            btnVisionDetect.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnVisionDetect.IconSize = 24;
            btnVisionDetect.Location = new Point(0, 10);
            btnVisionDetect.Name = "btnVisionDetect";
            btnVisionDetect.Size = new Size(533, 50);
            btnVisionDetect.TabIndex = 0;
            btnVisionDetect.Text = "  NHẬN DIỆN VÒNG BI (MANUAL DETECT)";
            btnVisionDetect.TextAlign = ContentAlignment.MiddleCenter;
            btnVisionDetect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnVisionDetect.UseVisualStyleBackColor = false;
            btnVisionDetect.Click += btnVisionDetect_Click;
            // 
            // pnlVisionFeedContainer
            // 
            pnlVisionFeedContainer.BackColor = Color.FromArgb(15, 23, 42);
            pnlVisionFeedContainer.Controls.Add(picVisionFeed);
            pnlVisionFeedContainer.Dock = DockStyle.Fill;
            pnlVisionFeedContainer.Location = new Point(15, 55);
            pnlVisionFeedContainer.Name = "pnlVisionFeedContainer";
            pnlVisionFeedContainer.Padding = new Padding(3);
            pnlVisionFeedContainer.Size = new Size(533, 606);
            pnlVisionFeedContainer.TabIndex = 1;
            // 
            // picVisionFeed
            // 
            picVisionFeed.BackColor = Color.FromArgb(15, 23, 42);
            picVisionFeed.Dock = DockStyle.Fill;
            picVisionFeed.Location = new Point(3, 3);
            picVisionFeed.Name = "picVisionFeed";
            picVisionFeed.Size = new Size(527, 600);
            picVisionFeed.SizeMode = PictureBoxSizeMode.Zoom;
            picVisionFeed.TabIndex = 0;
            picVisionFeed.TabStop = false;
            // 
            // pnlVisionHeader
            // 
            pnlVisionHeader.Controls.Add(lblLiveBadge);
            pnlVisionHeader.Controls.Add(lblVisionTitle);
            pnlVisionHeader.Controls.Add(iconVision);
            pnlVisionHeader.Dock = DockStyle.Top;
            pnlVisionHeader.Location = new Point(15, 15);
            pnlVisionHeader.Name = "pnlVisionHeader";
            pnlVisionHeader.Size = new Size(533, 40);
            pnlVisionHeader.TabIndex = 0;
            // 
            // lblLiveBadge
            // 
            lblLiveBadge.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblLiveBadge.BackColor = Color.FromArgb(220, 252, 231);
            lblLiveBadge.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblLiveBadge.ForeColor = Color.FromArgb(16, 185, 129);
            lblLiveBadge.Location = new Point(433, 8);
            lblLiveBadge.Name = "lblLiveBadge";
            lblLiveBadge.Size = new Size(100, 24);
            lblLiveBadge.TabIndex = 2;
            lblLiveBadge.Text = "● LIVE STREAM";
            lblLiveBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblVisionTitle
            // 
            lblVisionTitle.AutoSize = true;
            lblVisionTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblVisionTitle.ForeColor = Color.FromArgb(15, 23, 42);
            lblVisionTitle.Location = new Point(35, 8);
            lblVisionTitle.Name = "lblVisionTitle";
            lblVisionTitle.Size = new Size(207, 25);
            lblVisionTitle.TabIndex = 1;
            lblVisionTitle.Text = "HÌNH ẢNH THỊ GIÁC";
            // 
            // iconVision
            // 
            iconVision.BackColor = Color.Transparent;
            iconVision.ForeColor = Color.FromArgb(37, 99, 235);
            iconVision.IconChar = FontAwesome.Sharp.IconChar.Eye;
            iconVision.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconVision.IconSize = 24;
            iconVision.Location = new Point(3, 8);
            iconVision.Name = "iconVision";
            iconVision.Size = new Size(24, 24);
            iconVision.SizeMode = PictureBoxSizeMode.CenterImage;
            iconVision.TabIndex = 0;
            iconVision.TabStop = false;
            // 
            // panelRightCard
            // 
            panelRightCard.BackColor = Color.White;
            panelRightCard.BackgroundColor = Color.White;
            panelRightCard.BorderColor = Color.FromArgb(226, 232, 240);
            panelRightCard.BorderRadius = 12;
            panelRightCard.BorderSize = 1;
            panelRightCard.Controls.Add(pnlVisionCrop);
            panelRightCard.Controls.Add(pnlMetricsGroup);
            panelRightCard.Controls.Add(pnlStatusBanner);
            panelRightCard.Controls.Add(pnlResultHeader);
            panelRightCard.Dock = DockStyle.Fill;
            panelRightCard.Location = new Point(7, 0);
            panelRightCard.Name = "panelRightCard";
            panelRightCard.Padding = new Padding(15);
            panelRightCard.Size = new Size(431, 676);
            panelRightCard.TabIndex = 0;
            // 
            // pnlVisionCrop
            // 
            pnlVisionCrop.BackColor = Color.FromArgb(248, 250, 252);
            pnlVisionCrop.BackgroundColor = Color.FromArgb(248, 250, 252);
            pnlVisionCrop.BorderColor = Color.FromArgb(226, 232, 240);
            pnlVisionCrop.BorderRadius = 8;
            pnlVisionCrop.BorderSize = 1;
            pnlVisionCrop.Controls.Add(picVisionCrop);
            pnlVisionCrop.Controls.Add(lblVisionCropTitle);
            pnlVisionCrop.Dock = DockStyle.Fill;
            pnlVisionCrop.Location = new Point(15, 290);
            pnlVisionCrop.Name = "pnlVisionCrop";
            pnlVisionCrop.Padding = new Padding(10);
            pnlVisionCrop.Size = new Size(401, 371);
            pnlVisionCrop.TabIndex = 3;
            // 
            // picVisionCrop
            // 
            picVisionCrop.BackColor = Color.FromArgb(15, 23, 42);
            picVisionCrop.Dock = DockStyle.Fill;
            picVisionCrop.Location = new Point(10, 35);
            picVisionCrop.Name = "picVisionCrop";
            picVisionCrop.Size = new Size(381, 326);
            picVisionCrop.SizeMode = PictureBoxSizeMode.Zoom;
            picVisionCrop.TabIndex = 1;
            picVisionCrop.TabStop = false;
            // 
            // lblVisionCropTitle
            // 
            lblVisionCropTitle.Dock = DockStyle.Top;
            lblVisionCropTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblVisionCropTitle.ForeColor = Color.FromArgb(71, 85, 105);
            lblVisionCropTitle.Location = new Point(10, 10);
            lblVisionCropTitle.Name = "lblVisionCropTitle";
            lblVisionCropTitle.Size = new Size(381, 25);
            lblVisionCropTitle.TabIndex = 0;
            lblVisionCropTitle.Text = "ẢNH CROP VÙNG VÒNG BI (DETECTED REGION)";
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
            pnlStatusBanner.BorderRadius = 10;
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
            lblResultTitle.Size = new Size(207, 25);
            lblResultTitle.TabIndex = 1;
            lblResultTitle.Text = "KẾT QUẢ PHÂN LOẠI";
            // 
            // iconResult
            // 
            iconResult.BackColor = Color.Transparent;
            iconResult.ForeColor = Color.FromArgb(37, 99, 235);
            iconResult.IconChar = FontAwesome.Sharp.IconChar.SquarePollVertical;
            iconResult.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconResult.IconSize = 24;
            iconResult.Location = new Point(3, 8);
            iconResult.Name = "iconResult";
            iconResult.Size = new Size(24, 24);
            iconResult.SizeMode = PictureBoxSizeMode.CenterImage;
            iconResult.TabIndex = 0;
            iconResult.TabStop = false;
            // 
            // FormVision
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(241, 245, 249);
            ClientSize = new Size(1040, 704);
            Controls.Add(splitContainerVision);
            Name = "FormVision";
            Padding = new Padding(14);
            Text = "FormVision";
            FormClosing += FormVision_FormClosing;
            Load += FormVision_Load;
            Resize += FormVision_Resize;
            splitContainerVision.Panel1.ResumeLayout(false);
            splitContainerVision.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerVision).EndInit();
            splitContainerVision.ResumeLayout(false);
            panelLeftCard.ResumeLayout(false);
            pnlVisionActions.ResumeLayout(false);
            pnlVisionFeedContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picVisionFeed).EndInit();
            pnlVisionHeader.ResumeLayout(false);
            pnlVisionHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconVision).EndInit();
            panelRightCard.ResumeLayout(false);
            pnlVisionCrop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picVisionCrop).EndInit();
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

        private SplitContainer splitContainerVision;
        private RoundedPanel panelLeftCard;
        private Panel pnlVisionHeader;
        private Label lblLiveBadge;
        private Label lblVisionTitle;
        private FontAwesome.Sharp.IconPictureBox iconVision;
        private Panel pnlVisionFeedContainer;
        private PictureBox picVisionFeed;
        private Panel pnlVisionActions;
        private FontAwesome.Sharp.IconButton btnVisionDetect;
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
        private RoundedPanel pnlVisionCrop;
        private Label lblVisionCropTitle;
        private PictureBox picVisionCrop;
    }
}