namespace DashBoardBearing
{
    partial class FormSetting
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
            splitContainer1 = new SplitContainer();
            cardCamera = new RoundedPanel();
            pnlCamPreview = new Panel();
            picWebcam = new PictureBox();
            lblPreviewTitle = new Label();
            pnlIpGroup = new Panel();
            btnConnectIP = new FontAwesome.Sharp.IconButton();
            txtIP = new TextBox();
            lblIpTitle = new Label();
            pnlCamActions = new Panel();
            btnDisconnect = new FontAwesome.Sharp.IconButton();
            btnConnect = new FontAwesome.Sharp.IconButton();
            pnlWebcamGroup = new Panel();
            btnRefreshCam = new FontAwesome.Sharp.IconButton();
            cbWebcam = new ComboBox();
            lblWebcamTitle = new Label();
            pnlCamHeader = new Panel();
            lblCamHeaderTitle = new Label();
            iconCam = new FontAwesome.Sharp.IconPictureBox();
            cardSerial = new RoundedPanel();
            pnlSerialActions = new Panel();
            btnDisconnectSer = new FontAwesome.Sharp.IconButton();
            btnConnectSerial = new FontAwesome.Sharp.IconButton();
            pnlSerialParams = new TableLayoutPanel();
            pnlStopBit = new Panel();
            cbStopBit = new ComboBox();
            lblStopBit = new Label();
            pnlParity = new Panel();
            cbParityBit = new ComboBox();
            lblParity = new Label();
            pnlDataSize = new Panel();
            cbDataSize = new ComboBox();
            lblDataSize = new Label();
            pnlBaud = new Panel();
            cbBaud = new ComboBox();
            lblBaud = new Label();
            pnlComGroup = new Panel();
            btnRefreshPorts = new FontAwesome.Sharp.IconButton();
            cbCOM = new ComboBox();
            lblComTitle = new Label();
            pnlSerialHeader = new Panel();
            lblSerialHeaderTitle = new Label();
            iconSerial = new FontAwesome.Sharp.IconPictureBox();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            cardCamera.SuspendLayout();
            pnlCamPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picWebcam).BeginInit();
            pnlIpGroup.SuspendLayout();
            pnlCamActions.SuspendLayout();
            pnlWebcamGroup.SuspendLayout();
            pnlCamHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconCam).BeginInit();
            cardSerial.SuspendLayout();
            pnlSerialActions.SuspendLayout();
            pnlSerialParams.SuspendLayout();
            pnlStopBit.SuspendLayout();
            pnlParity.SuspendLayout();
            pnlDataSize.SuspendLayout();
            pnlBaud.SuspendLayout();
            pnlComGroup.SuspendLayout();
            pnlSerialHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSerial).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(14, 14);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(cardCamera);
            splitContainer1.Panel1.Padding = new Padding(0, 0, 7, 0);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(cardSerial);
            splitContainer1.Panel2.Padding = new Padding(7, 0, 0, 0);
            splitContainer1.Size = new Size(1012, 676);
            splitContainer1.SplitterDistance = 506;
            splitContainer1.TabIndex = 0;
            // 
            // cardCamera
            // 
            cardCamera.BackColor = Color.FromArgb(24, 34, 53);
            cardCamera.BackgroundColor = Color.FromArgb(24, 34, 53);
            cardCamera.BorderColor = Color.FromArgb(41, 53, 76);
            cardCamera.BorderRadius = 12;
            cardCamera.BorderSize = 1;
            cardCamera.Controls.Add(pnlCamPreview);
            cardCamera.Controls.Add(pnlIpGroup);
            cardCamera.Controls.Add(pnlCamActions);
            cardCamera.Controls.Add(pnlWebcamGroup);
            cardCamera.Controls.Add(pnlCamHeader);
            cardCamera.Dock = DockStyle.Fill;
            cardCamera.Location = new Point(0, 0);
            cardCamera.Name = "cardCamera";
            cardCamera.Padding = new Padding(15);
            cardCamera.Size = new Size(499, 676);
            cardCamera.TabIndex = 0;
            // 
            // pnlCamPreview
            // 
            pnlCamPreview.Controls.Add(picWebcam);
            pnlCamPreview.Controls.Add(lblPreviewTitle);
            pnlCamPreview.Dock = DockStyle.Fill;
            pnlCamPreview.Location = new Point(15, 290);
            pnlCamPreview.Name = "pnlCamPreview";
            pnlCamPreview.Padding = new Padding(0, 10, 0, 0);
            pnlCamPreview.Size = new Size(469, 371);
            pnlCamPreview.TabIndex = 4;
            // 
            // picWebcam
            // 
            picWebcam.BackColor = Color.FromArgb(10, 15, 26);
            picWebcam.Dock = DockStyle.Fill;
            picWebcam.Location = new Point(0, 36);
            picWebcam.Name = "picWebcam";
            picWebcam.Size = new Size(469, 335);
            picWebcam.SizeMode = PictureBoxSizeMode.Zoom;
            picWebcam.TabIndex = 1;
            picWebcam.TabStop = false;
            // 
            // lblPreviewTitle
            // 
            lblPreviewTitle.Dock = DockStyle.Top;
            lblPreviewTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPreviewTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblPreviewTitle.Location = new Point(0, 10);
            lblPreviewTitle.Name = "lblPreviewTitle";
            lblPreviewTitle.Size = new Size(469, 26);
            lblPreviewTitle.TabIndex = 0;
            lblPreviewTitle.Text = "HÌNH ẢNH XEM TRƯỚC (LIVE PREVIEW)";
            // 
            // pnlIpGroup
            // 
            pnlIpGroup.Controls.Add(btnConnectIP);
            pnlIpGroup.Controls.Add(txtIP);
            pnlIpGroup.Controls.Add(lblIpTitle);
            pnlIpGroup.Dock = DockStyle.Top;
            pnlIpGroup.Location = new Point(15, 195);
            pnlIpGroup.Name = "pnlIpGroup";
            pnlIpGroup.Size = new Size(469, 95);
            pnlIpGroup.TabIndex = 3;
            // 
            // btnConnectIP
            // 
            btnConnectIP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConnectIP.BackColor = Color.FromArgb(37, 99, 235);
            btnConnectIP.FlatAppearance.BorderSize = 0;
            btnConnectIP.FlatStyle = FlatStyle.Flat;
            btnConnectIP.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConnectIP.ForeColor = Color.White;
            btnConnectIP.IconChar = FontAwesome.Sharp.IconChar.NetworkWired;
            btnConnectIP.IconColor = Color.White;
            btnConnectIP.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnConnectIP.IconSize = 16;
            btnConnectIP.Location = new Point(329, 34);
            btnConnectIP.Name = "btnConnectIP";
            btnConnectIP.Size = new Size(140, 32);
            btnConnectIP.TabIndex = 2;
            btnConnectIP.Text = "  KẾT NỐI IP";
            btnConnectIP.TextAlign = ContentAlignment.MiddleCenter;
            btnConnectIP.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConnectIP.UseVisualStyleBackColor = false;
            btnConnectIP.Click += btnConnectIP_Click;
            // 
            // txtIP
            // 
            txtIP.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtIP.BackColor = Color.FromArgb(15, 23, 42);
            txtIP.BorderStyle = BorderStyle.FixedSingle;
            txtIP.Font = new Font("Segoe UI", 10F);
            txtIP.ForeColor = Color.FromArgb(248, 250, 252);
            txtIP.Location = new Point(0, 35);
            txtIP.Name = "txtIP";
            txtIP.PlaceholderText = "rtsp://username:password@ip_address/stream";
            txtIP.Size = new Size(319, 30);
            txtIP.TabIndex = 1;
            // 
            // lblIpTitle
            // 
            lblIpTitle.AutoSize = true;
            lblIpTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblIpTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblIpTitle.Location = new Point(0, 10);
            lblIpTitle.Name = "lblIpTitle";
            lblIpTitle.Size = new Size(189, 20);
            lblIpTitle.TabIndex = 0;
            lblIpTitle.Text = "HOẶC ĐỊA CHỈ IP / RTSP";
            // 
            // pnlCamActions
            // 
            pnlCamActions.Controls.Add(btnDisconnect);
            pnlCamActions.Controls.Add(btnConnect);
            pnlCamActions.Dock = DockStyle.Top;
            pnlCamActions.Location = new Point(15, 145);
            pnlCamActions.Name = "pnlCamActions";
            pnlCamActions.Size = new Size(469, 50);
            pnlCamActions.TabIndex = 2;
            // 
            // btnDisconnect
            // 
            btnDisconnect.BackColor = Color.FromArgb(239, 68, 68);
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDisconnect.ForeColor = Color.White;
            btnDisconnect.IconChar = FontAwesome.Sharp.IconChar.PowerOff;
            btnDisconnect.IconColor = Color.White;
            btnDisconnect.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnDisconnect.IconSize = 16;
            btnDisconnect.Location = new Point(170, 6);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(160, 36);
            btnDisconnect.TabIndex = 1;
            btnDisconnect.Text = "  NGẮT KẾT NỐI";
            btnDisconnect.TextAlign = ContentAlignment.MiddleCenter;
            btnDisconnect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDisconnect.UseVisualStyleBackColor = false;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.BackColor = Color.FromArgb(16, 185, 129);
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConnect.ForeColor = Color.White;
            btnConnect.IconChar = FontAwesome.Sharp.IconChar.Play;
            btnConnect.IconColor = Color.White;
            btnConnect.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnConnect.IconSize = 16;
            btnConnect.Location = new Point(0, 6);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(160, 36);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "  MỞ CAMERA";
            btnConnect.TextAlign = ContentAlignment.MiddleCenter;
            btnConnect.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConnect.UseVisualStyleBackColor = false;
            btnConnect.Click += btnConnect_Click;
            // 
            // pnlWebcamGroup
            // 
            pnlWebcamGroup.Controls.Add(btnRefreshCam);
            pnlWebcamGroup.Controls.Add(cbWebcam);
            pnlWebcamGroup.Controls.Add(lblWebcamTitle);
            pnlWebcamGroup.Dock = DockStyle.Top;
            pnlWebcamGroup.Location = new Point(15, 65);
            pnlWebcamGroup.Name = "pnlWebcamGroup";
            pnlWebcamGroup.Size = new Size(469, 80);
            pnlWebcamGroup.TabIndex = 1;
            // 
            // btnRefreshCam
            // 
            btnRefreshCam.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshCam.BackColor = Color.FromArgb(30, 41, 59);
            btnRefreshCam.FlatAppearance.BorderColor = Color.FromArgb(41, 53, 76);
            btnRefreshCam.FlatStyle = FlatStyle.Flat;
            btnRefreshCam.Font = new Font("Segoe UI", 9F);
            btnRefreshCam.ForeColor = Color.FromArgb(148, 163, 184);
            btnRefreshCam.IconChar = FontAwesome.Sharp.IconChar.ArrowsRotate;
            btnRefreshCam.IconColor = Color.FromArgb(148, 163, 184);
            btnRefreshCam.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefreshCam.IconSize = 16;
            btnRefreshCam.Location = new Point(429, 32);
            btnRefreshCam.Name = "btnRefreshCam";
            btnRefreshCam.Size = new Size(40, 32);
            btnRefreshCam.TabIndex = 2;
            btnRefreshCam.UseVisualStyleBackColor = false;
            btnRefreshCam.Click += btnRefreshCam_Click;
            // 
            // cbWebcam
            // 
            cbWebcam.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbWebcam.BackColor = Color.FromArgb(15, 23, 42);
            cbWebcam.DropDownStyle = ComboBoxStyle.DropDownList;
            cbWebcam.FlatStyle = FlatStyle.Flat;
            cbWebcam.Font = new Font("Segoe UI", 10F);
            cbWebcam.ForeColor = Color.FromArgb(248, 250, 252);
            cbWebcam.FormattingEnabled = true;
            cbWebcam.Location = new Point(0, 33);
            cbWebcam.Name = "cbWebcam";
            cbWebcam.Size = new Size(420, 31);
            cbWebcam.TabIndex = 1;
            // 
            // lblWebcamTitle
            // 
            lblWebcamTitle.AutoSize = true;
            lblWebcamTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblWebcamTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblWebcamTitle.Location = new Point(0, 8);
            lblWebcamTitle.Name = "lblWebcamTitle";
            lblWebcamTitle.Size = new Size(173, 20);
            lblWebcamTitle.TabIndex = 0;
            lblWebcamTitle.Text = "CHỌN THIẾT BỊ CAMERA";
            // 
            // pnlCamHeader
            // 
            pnlCamHeader.Controls.Add(lblCamHeaderTitle);
            pnlCamHeader.Controls.Add(iconCam);
            pnlCamHeader.Dock = DockStyle.Top;
            pnlCamHeader.Location = new Point(15, 15);
            pnlCamHeader.Name = "pnlCamHeader";
            pnlCamHeader.Size = new Size(469, 50);
            pnlCamHeader.TabIndex = 0;
            // 
            // lblCamHeaderTitle
            // 
            lblCamHeaderTitle.AutoSize = true;
            lblCamHeaderTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblCamHeaderTitle.ForeColor = Color.FromArgb(248, 250, 252);
            lblCamHeaderTitle.Location = new Point(40, 10);
            lblCamHeaderTitle.Name = "lblCamHeaderTitle";
            lblCamHeaderTitle.Size = new Size(187, 28);
            lblCamHeaderTitle.TabIndex = 1;
            lblCamHeaderTitle.Text = "CẤU HÌNH CAMERA";
            // 
            // iconCam
            // 
            iconCam.BackColor = Color.Transparent;
            iconCam.ForeColor = Color.FromArgb(59, 130, 246);
            iconCam.IconChar = FontAwesome.Sharp.IconChar.Camera;
            iconCam.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconCam.IconSize = 28;
            iconCam.Location = new Point(0, 10);
            iconCam.Name = "iconCam";
            iconCam.Size = new Size(28, 28);
            iconCam.SizeMode = PictureBoxSizeMode.CenterImage;
            iconCam.TabIndex = 0;
            iconCam.TabStop = false;
            // 
            // cardSerial
            // 
            cardSerial.BackColor = Color.FromArgb(24, 34, 53);
            cardSerial.BackgroundColor = Color.FromArgb(24, 34, 53);
            cardSerial.BorderColor = Color.FromArgb(41, 53, 76);
            cardSerial.BorderRadius = 12;
            cardSerial.BorderSize = 1;
            cardSerial.Controls.Add(pnlSerialActions);
            cardSerial.Controls.Add(pnlSerialParams);
            cardSerial.Controls.Add(pnlComGroup);
            cardSerial.Controls.Add(pnlSerialHeader);
            cardSerial.Dock = DockStyle.Fill;
            cardSerial.Location = new Point(7, 0);
            cardSerial.Name = "cardSerial";
            cardSerial.Padding = new Padding(15);
            cardSerial.Size = new Size(495, 676);
            cardSerial.TabIndex = 1;
            // 
            // pnlSerialActions
            // 
            pnlSerialActions.Controls.Add(btnDisconnectSer);
            pnlSerialActions.Controls.Add(btnConnectSerial);
            pnlSerialActions.Dock = DockStyle.Top;
            pnlSerialActions.Location = new Point(15, 335);
            pnlSerialActions.Name = "pnlSerialActions";
            pnlSerialActions.Padding = new Padding(0, 15, 0, 0);
            pnlSerialActions.Size = new Size(465, 65);
            pnlSerialActions.TabIndex = 3;
            // 
            // btnDisconnectSer
            // 
            btnDisconnectSer.BackColor = Color.FromArgb(239, 68, 68);
            btnDisconnectSer.FlatAppearance.BorderSize = 0;
            btnDisconnectSer.FlatStyle = FlatStyle.Flat;
            btnDisconnectSer.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDisconnectSer.ForeColor = Color.White;
            btnDisconnectSer.IconChar = FontAwesome.Sharp.IconChar.LinkSlash;
            btnDisconnectSer.IconColor = Color.White;
            btnDisconnectSer.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnDisconnectSer.IconSize = 16;
            btnDisconnectSer.Location = new Point(180, 15);
            btnDisconnectSer.Name = "btnDisconnectSer";
            btnDisconnectSer.Size = new Size(165, 38);
            btnDisconnectSer.TabIndex = 1;
            btnDisconnectSer.Text = "  NGẮT SERIAL";
            btnDisconnectSer.TextAlign = ContentAlignment.MiddleCenter;
            btnDisconnectSer.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnDisconnectSer.UseVisualStyleBackColor = false;
            btnDisconnectSer.Click += btnDisconnectSer_Click;
            // 
            // btnConnectSerial
            // 
            btnConnectSerial.BackColor = Color.FromArgb(16, 185, 129);
            btnConnectSerial.FlatAppearance.BorderSize = 0;
            btnConnectSerial.FlatStyle = FlatStyle.Flat;
            btnConnectSerial.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnConnectSerial.ForeColor = Color.White;
            btnConnectSerial.IconChar = FontAwesome.Sharp.IconChar.Link;
            btnConnectSerial.IconColor = Color.White;
            btnConnectSerial.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnConnectSerial.IconSize = 16;
            btnConnectSerial.Location = new Point(0, 15);
            btnConnectSerial.Name = "btnConnectSerial";
            btnConnectSerial.Size = new Size(165, 38);
            btnConnectSerial.TabIndex = 0;
            btnConnectSerial.Text = "  KẾT NỐI SERIAL";
            btnConnectSerial.TextAlign = ContentAlignment.MiddleCenter;
            btnConnectSerial.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnConnectSerial.UseVisualStyleBackColor = false;
            btnConnectSerial.Click += btnConnectSerial_Click;
            // 
            // pnlSerialParams
            // 
            pnlSerialParams.ColumnCount = 2;
            pnlSerialParams.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlSerialParams.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            pnlSerialParams.Controls.Add(pnlStopBit, 1, 1);
            pnlSerialParams.Controls.Add(pnlParity, 0, 1);
            pnlSerialParams.Controls.Add(pnlDataSize, 1, 0);
            pnlSerialParams.Controls.Add(pnlBaud, 0, 0);
            pnlSerialParams.Dock = DockStyle.Top;
            pnlSerialParams.Location = new Point(15, 155);
            pnlSerialParams.Name = "pnlSerialParams";
            pnlSerialParams.RowCount = 2;
            pnlSerialParams.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            pnlSerialParams.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            pnlSerialParams.Size = new Size(465, 180);
            pnlSerialParams.TabIndex = 2;
            // 
            // pnlStopBit
            // 
            pnlStopBit.Controls.Add(cbStopBit);
            pnlStopBit.Controls.Add(lblStopBit);
            pnlStopBit.Dock = DockStyle.Fill;
            pnlStopBit.Location = new Point(235, 93);
            pnlStopBit.Name = "pnlStopBit";
            pnlStopBit.Padding = new Padding(6, 6, 0, 6);
            pnlStopBit.Size = new Size(227, 84);
            pnlStopBit.TabIndex = 3;
            // 
            // cbStopBit
            // 
            cbStopBit.BackColor = Color.FromArgb(15, 23, 42);
            cbStopBit.Dock = DockStyle.Top;
            cbStopBit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbStopBit.FlatStyle = FlatStyle.Flat;
            cbStopBit.Font = new Font("Segoe UI", 10F);
            cbStopBit.ForeColor = Color.FromArgb(248, 250, 252);
            cbStopBit.FormattingEnabled = true;
            cbStopBit.Location = new Point(6, 32);
            cbStopBit.Name = "cbStopBit";
            cbStopBit.Size = new Size(221, 31);
            cbStopBit.TabIndex = 1;
            // 
            // lblStopBit
            // 
            lblStopBit.Dock = DockStyle.Top;
            lblStopBit.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblStopBit.ForeColor = Color.FromArgb(148, 163, 184);
            lblStopBit.Location = new Point(6, 6);
            lblStopBit.Name = "lblStopBit";
            lblStopBit.Size = new Size(221, 26);
            lblStopBit.TabIndex = 0;
            lblStopBit.Text = "STOP BITS";
            // 
            // pnlParity
            // 
            pnlParity.Controls.Add(cbParityBit);
            pnlParity.Controls.Add(lblParity);
            pnlParity.Dock = DockStyle.Fill;
            pnlParity.Location = new Point(3, 93);
            pnlParity.Name = "pnlParity";
            pnlParity.Padding = new Padding(0, 6, 6, 6);
            pnlParity.Size = new Size(226, 84);
            pnlParity.TabIndex = 2;
            // 
            // cbParityBit
            // 
            cbParityBit.BackColor = Color.FromArgb(15, 23, 42);
            cbParityBit.Dock = DockStyle.Top;
            cbParityBit.DropDownStyle = ComboBoxStyle.DropDownList;
            cbParityBit.FlatStyle = FlatStyle.Flat;
            cbParityBit.Font = new Font("Segoe UI", 10F);
            cbParityBit.ForeColor = Color.FromArgb(248, 250, 252);
            cbParityBit.FormattingEnabled = true;
            cbParityBit.Location = new Point(0, 32);
            cbParityBit.Name = "cbParityBit";
            cbParityBit.Size = new Size(220, 31);
            cbParityBit.TabIndex = 1;
            // 
            // lblParity
            // 
            lblParity.Dock = DockStyle.Top;
            lblParity.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblParity.ForeColor = Color.FromArgb(148, 163, 184);
            lblParity.Location = new Point(0, 6);
            lblParity.Name = "lblParity";
            lblParity.Size = new Size(220, 26);
            lblParity.TabIndex = 0;
            lblParity.Text = "PARITY";
            // 
            // pnlDataSize
            // 
            pnlDataSize.Controls.Add(cbDataSize);
            pnlDataSize.Controls.Add(lblDataSize);
            pnlDataSize.Dock = DockStyle.Fill;
            pnlDataSize.Location = new Point(235, 3);
            pnlDataSize.Name = "pnlDataSize";
            pnlDataSize.Padding = new Padding(6, 6, 0, 6);
            pnlDataSize.Size = new Size(227, 84);
            pnlDataSize.TabIndex = 1;
            // 
            // cbDataSize
            // 
            cbDataSize.BackColor = Color.FromArgb(15, 23, 42);
            cbDataSize.Dock = DockStyle.Top;
            cbDataSize.DropDownStyle = ComboBoxStyle.DropDownList;
            cbDataSize.FlatStyle = FlatStyle.Flat;
            cbDataSize.Font = new Font("Segoe UI", 10F);
            cbDataSize.ForeColor = Color.FromArgb(248, 250, 252);
            cbDataSize.FormattingEnabled = true;
            cbDataSize.Location = new Point(6, 32);
            cbDataSize.Name = "cbDataSize";
            cbDataSize.Size = new Size(221, 31);
            cbDataSize.TabIndex = 1;
            // 
            // lblDataSize
            // 
            lblDataSize.Dock = DockStyle.Top;
            lblDataSize.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblDataSize.ForeColor = Color.FromArgb(148, 163, 184);
            lblDataSize.Location = new Point(6, 6);
            lblDataSize.Name = "lblDataSize";
            lblDataSize.Size = new Size(221, 26);
            lblDataSize.TabIndex = 0;
            lblDataSize.Text = "DATA BITS";
            // 
            // pnlBaud
            // 
            pnlBaud.Controls.Add(cbBaud);
            pnlBaud.Controls.Add(lblBaud);
            pnlBaud.Dock = DockStyle.Fill;
            pnlBaud.Location = new Point(3, 3);
            pnlBaud.Name = "pnlBaud";
            pnlBaud.Padding = new Padding(0, 6, 6, 6);
            pnlBaud.Size = new Size(226, 84);
            pnlBaud.TabIndex = 0;
            // 
            // cbBaud
            // 
            cbBaud.BackColor = Color.FromArgb(15, 23, 42);
            cbBaud.Dock = DockStyle.Top;
            cbBaud.DropDownStyle = ComboBoxStyle.DropDownList;
            cbBaud.FlatStyle = FlatStyle.Flat;
            cbBaud.Font = new Font("Segoe UI", 10F);
            cbBaud.ForeColor = Color.FromArgb(248, 250, 252);
            cbBaud.FormattingEnabled = true;
            cbBaud.Location = new Point(0, 32);
            cbBaud.Name = "cbBaud";
            cbBaud.Size = new Size(220, 31);
            cbBaud.TabIndex = 1;
            // 
            // lblBaud
            // 
            lblBaud.Dock = DockStyle.Top;
            lblBaud.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblBaud.ForeColor = Color.FromArgb(148, 163, 184);
            lblBaud.Location = new Point(0, 6);
            lblBaud.Name = "lblBaud";
            lblBaud.Size = new Size(220, 26);
            lblBaud.TabIndex = 0;
            lblBaud.Text = "BAUD RATE";
            // 
            // pnlComGroup
            // 
            pnlComGroup.Controls.Add(btnRefreshPorts);
            pnlComGroup.Controls.Add(cbCOM);
            pnlComGroup.Controls.Add(lblComTitle);
            pnlComGroup.Dock = DockStyle.Top;
            pnlComGroup.Location = new Point(15, 65);
            pnlComGroup.Name = "pnlComGroup";
            pnlComGroup.Size = new Size(465, 90);
            pnlComGroup.TabIndex = 1;
            // 
            // btnRefreshPorts
            // 
            btnRefreshPorts.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefreshPorts.BackColor = Color.FromArgb(30, 41, 59);
            btnRefreshPorts.FlatAppearance.BorderColor = Color.FromArgb(41, 53, 76);
            btnRefreshPorts.FlatStyle = FlatStyle.Flat;
            btnRefreshPorts.Font = new Font("Segoe UI", 9F);
            btnRefreshPorts.ForeColor = Color.FromArgb(148, 163, 184);
            btnRefreshPorts.IconChar = FontAwesome.Sharp.IconChar.ArrowsRotate;
            btnRefreshPorts.IconColor = Color.FromArgb(148, 163, 184);
            btnRefreshPorts.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRefreshPorts.IconSize = 16;
            btnRefreshPorts.Location = new Point(425, 36);
            btnRefreshPorts.Name = "btnRefreshPorts";
            btnRefreshPorts.Size = new Size(40, 32);
            btnRefreshPorts.TabIndex = 2;
            btnRefreshPorts.UseVisualStyleBackColor = false;
            btnRefreshPorts.Click += btnRefreshPorts_Click;
            // 
            // cbCOM
            // 
            cbCOM.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbCOM.BackColor = Color.FromArgb(15, 23, 42);
            cbCOM.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCOM.FlatStyle = FlatStyle.Flat;
            cbCOM.Font = new Font("Segoe UI", 10F);
            cbCOM.ForeColor = Color.FromArgb(248, 250, 252);
            cbCOM.FormattingEnabled = true;
            cbCOM.Location = new Point(0, 37);
            cbCOM.Name = "cbCOM";
            cbCOM.Size = new Size(415, 31);
            cbCOM.TabIndex = 1;
            // 
            // lblComTitle
            // 
            lblComTitle.AutoSize = true;
            lblComTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblComTitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblComTitle.Location = new Point(0, 10);
            lblComTitle.Name = "lblComTitle";
            lblComTitle.Size = new Size(137, 20);
            lblComTitle.TabIndex = 0;
            lblComTitle.Text = "CỔNG KẾT NỐI COM";
            // 
            // pnlSerialHeader
            // 
            pnlSerialHeader.Controls.Add(lblSerialHeaderTitle);
            pnlSerialHeader.Controls.Add(iconSerial);
            pnlSerialHeader.Dock = DockStyle.Top;
            pnlSerialHeader.Location = new Point(15, 15);
            pnlSerialHeader.Name = "pnlSerialHeader";
            pnlSerialHeader.Size = new Size(465, 50);
            pnlSerialHeader.TabIndex = 0;
            // 
            // lblSerialHeaderTitle
            // 
            lblSerialHeaderTitle.AutoSize = true;
            lblSerialHeaderTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSerialHeaderTitle.ForeColor = Color.FromArgb(248, 250, 252);
            lblSerialHeaderTitle.Location = new Point(40, 10);
            lblSerialHeaderTitle.Name = "lblSerialHeaderTitle";
            lblSerialHeaderTitle.Size = new Size(254, 28);
            lblSerialHeaderTitle.TabIndex = 1;
            lblSerialHeaderTitle.Text = "CẤU HÌNH SERIAL ROBOT";
            // 
            // iconSerial
            // 
            iconSerial.BackColor = Color.Transparent;
            iconSerial.ForeColor = Color.FromArgb(59, 130, 246);
            iconSerial.IconChar = FontAwesome.Sharp.IconChar.Microchip;
            iconSerial.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconSerial.IconSize = 28;
            iconSerial.Location = new Point(0, 10);
            iconSerial.Name = "iconSerial";
            iconSerial.Size = new Size(28, 28);
            iconSerial.SizeMode = PictureBoxSizeMode.CenterImage;
            iconSerial.TabIndex = 0;
            iconSerial.TabStop = false;
            // 
            // FormSetting
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(1040, 704);
            Controls.Add(splitContainer1);
            Name = "FormSetting";
            Padding = new Padding(14);
            Text = "FormSetting";
            FormClosing += FormSetting_FormClosing;
            Load += FormSetting_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            cardCamera.ResumeLayout(false);
            pnlCamPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picWebcam).EndInit();
            pnlIpGroup.ResumeLayout(false);
            pnlIpGroup.PerformLayout();
            pnlCamActions.ResumeLayout(false);
            pnlWebcamGroup.ResumeLayout(false);
            pnlWebcamGroup.PerformLayout();
            pnlCamHeader.ResumeLayout(false);
            pnlCamHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconCam).EndInit();
            cardSerial.ResumeLayout(false);
            pnlSerialActions.ResumeLayout(false);
            pnlSerialParams.ResumeLayout(false);
            pnlStopBit.ResumeLayout(false);
            pnlParity.ResumeLayout(false);
            pnlDataSize.ResumeLayout(false);
            pnlBaud.ResumeLayout(false);
            pnlComGroup.ResumeLayout(false);
            pnlComGroup.PerformLayout();
            pnlSerialHeader.ResumeLayout(false);
            pnlSerialHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)iconSerial).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private RoundedPanel cardCamera;
        private Panel pnlCamHeader;
        private Label lblCamHeaderTitle;
        private FontAwesome.Sharp.IconPictureBox iconCam;
        private Panel pnlWebcamGroup;
        private Label lblWebcamTitle;
        private ComboBox cbWebcam;
        private FontAwesome.Sharp.IconButton btnRefreshCam;
        private Panel pnlCamActions;
        private FontAwesome.Sharp.IconButton btnConnect;
        private FontAwesome.Sharp.IconButton btnDisconnect;
        private Panel pnlIpGroup;
        private Label lblIpTitle;
        private TextBox txtIP;
        private FontAwesome.Sharp.IconButton btnConnectIP;
        private Panel pnlCamPreview;
        private Label lblPreviewTitle;
        private PictureBox picWebcam;
        private RoundedPanel cardSerial;
        private Panel pnlSerialHeader;
        private Label lblSerialHeaderTitle;
        private FontAwesome.Sharp.IconPictureBox iconSerial;
        private Panel pnlComGroup;
        private Label lblComTitle;
        private ComboBox cbCOM;
        private FontAwesome.Sharp.IconButton btnRefreshPorts;
        private TableLayoutPanel pnlSerialParams;
        private Panel pnlBaud;
        private Label lblBaud;
        private ComboBox cbBaud;
        private Panel pnlDataSize;
        private Label lblDataSize;
        private ComboBox cbDataSize;
        private Panel pnlParity;
        private Label lblParity;
        private ComboBox cbParityBit;
        private Panel pnlStopBit;
        private Label lblStopBit;
        private ComboBox cbStopBit;
        private Panel pnlSerialActions;
        private FontAwesome.Sharp.IconButton btnConnectSerial;
        private FontAwesome.Sharp.IconButton btnDisconnectSer;
    }
}