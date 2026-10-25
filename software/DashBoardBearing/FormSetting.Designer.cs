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
            roundedPanel1 = new RoundedPanel();
            btnConnectIP = new Button();
            txtIP = new TextBox();
            label5 = new Label();
            picWebcam = new PictureBox();
            btnDisconnect = new Button();
            btnConnect = new Button();
            label4 = new Label();
            label3 = new Label();
            cbWebcam = new ComboBox();
            label1 = new Label();
            roundedPanel2 = new RoundedPanel();
            btnDisconnectSer = new Button();
            btnConnectSerial = new Button();
            label10 = new Label();
            cbStopBit = new ComboBox();
            label8 = new Label();
            cbParityBit = new ComboBox();
            label9 = new Label();
            cbDataSize = new ComboBox();
            label6 = new Label();
            cbBaud = new ComboBox();
            label7 = new Label();
            cbCOM = new ComboBox();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picWebcam).BeginInit();
            roundedPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(roundedPanel1);
            splitContainer1.Panel1.Padding = new Padding(15, 15, 5, 40);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(roundedPanel2);
            splitContainer1.Panel2.Padding = new Padding(5, 15, 15, 40);
            splitContainer1.Size = new Size(897, 652);
            splitContainer1.SplitterDistance = 455;
            splitContainer1.TabIndex = 0;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BackgroundColor = Color.White;
            roundedPanel1.BorderColor = Color.PaleVioletRed;
            roundedPanel1.Controls.Add(btnConnectIP);
            roundedPanel1.Controls.Add(txtIP);
            roundedPanel1.Controls.Add(label5);
            roundedPanel1.Controls.Add(picWebcam);
            roundedPanel1.Controls.Add(btnDisconnect);
            roundedPanel1.Controls.Add(btnConnect);
            roundedPanel1.Controls.Add(label4);
            roundedPanel1.Controls.Add(label3);
            roundedPanel1.Controls.Add(cbWebcam);
            roundedPanel1.Controls.Add(label1);
            roundedPanel1.Dock = DockStyle.Fill;
            roundedPanel1.Location = new Point(15, 15);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(435, 597);
            roundedPanel1.TabIndex = 0;
            // 
            // btnConnectIP
            // 
            btnConnectIP.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConnectIP.FlatStyle = FlatStyle.Flat;
            btnConnectIP.Location = new Point(322, 168);
            btnConnectIP.Name = "btnConnectIP";
            btnConnectIP.Size = new Size(105, 29);
            btnConnectIP.TabIndex = 11;
            btnConnectIP.Text = "Kết nối";
            btnConnectIP.UseVisualStyleBackColor = true;
            btnConnectIP.Click += btnConnectIP_Click;
            // 
            // txtIP
            // 
            txtIP.Location = new Point(46, 168);
            txtIP.Name = "txtIP";
            txtIP.Size = new Size(262, 27);
            txtIP.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(24, 300);
            label5.Name = "label5";
            label5.Size = new Size(132, 23);
            label5.TabIndex = 9;
            label5.Text = "Camera Preview";
            // 
            // picWebcam
            // 
            picWebcam.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picWebcam.BackColor = Color.DimGray;
            picWebcam.Location = new Point(29, 337);
            picWebcam.Name = "picWebcam";
            picWebcam.Size = new Size(392, 243);
            picWebcam.SizeMode = PictureBoxSizeMode.Zoom;
            picWebcam.TabIndex = 8;
            picWebcam.TabStop = false;
            // 
            // btnDisconnect
            // 
            btnDisconnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDisconnect.FlatStyle = FlatStyle.Flat;
            btnDisconnect.Location = new Point(322, 251);
            btnDisconnect.Name = "btnDisconnect";
            btnDisconnect.Size = new Size(105, 29);
            btnDisconnect.TabIndex = 7;
            btnDisconnect.Text = "Ngắt kết nối";
            btnDisconnect.UseVisualStyleBackColor = true;
            btnDisconnect.Click += btnDisconnect_Click;
            // 
            // btnConnect
            // 
            btnConnect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConnect.FlatStyle = FlatStyle.Flat;
            btnConnect.Location = new Point(322, 84);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(105, 29);
            btnConnect.TabIndex = 6;
            btnConnect.Text = "Kết nối";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += btnConnect_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(24, 135);
            label4.Name = "label4";
            label4.Size = new Size(82, 23);
            label4.TabIndex = 5;
            label4.Text = "Địa chỉ IP";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(23, 52);
            label3.Name = "label3";
            label3.Size = new Size(100, 23);
            label3.TabIndex = 3;
            label3.Text = "Tên Camera";
            // 
            // cbWebcam
            // 
            cbWebcam.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbWebcam.FormattingEnabled = true;
            cbWebcam.Location = new Point(46, 87);
            cbWebcam.Name = "cbWebcam";
            cbWebcam.Size = new Size(262, 28);
            cbWebcam.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(109, 31);
            label1.TabIndex = 0;
            label1.Text = "CAMERA";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BackgroundColor = Color.White;
            roundedPanel2.BorderColor = Color.PaleVioletRed;
            roundedPanel2.Controls.Add(btnDisconnectSer);
            roundedPanel2.Controls.Add(btnConnectSerial);
            roundedPanel2.Controls.Add(label10);
            roundedPanel2.Controls.Add(cbStopBit);
            roundedPanel2.Controls.Add(label8);
            roundedPanel2.Controls.Add(cbParityBit);
            roundedPanel2.Controls.Add(label9);
            roundedPanel2.Controls.Add(cbDataSize);
            roundedPanel2.Controls.Add(label6);
            roundedPanel2.Controls.Add(cbBaud);
            roundedPanel2.Controls.Add(label7);
            roundedPanel2.Controls.Add(cbCOM);
            roundedPanel2.Controls.Add(label2);
            roundedPanel2.Dock = DockStyle.Fill;
            roundedPanel2.Location = new Point(5, 15);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Size = new Size(418, 597);
            roundedPanel2.TabIndex = 1;
            // 
            // btnDisconnectSer
            // 
            btnDisconnectSer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDisconnectSer.FlatStyle = FlatStyle.Flat;
            btnDisconnectSer.Location = new Point(163, 465);
            btnDisconnectSer.Name = "btnDisconnectSer";
            btnDisconnectSer.Size = new Size(105, 29);
            btnDisconnectSer.TabIndex = 17;
            btnDisconnectSer.Text = "Ngắt kết nối";
            btnDisconnectSer.UseVisualStyleBackColor = true;
            btnDisconnectSer.Click += btnDisconnectSer_Click;
            // 
            // btnConnectSerial
            // 
            btnConnectSerial.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnConnectSerial.FlatStyle = FlatStyle.Flat;
            btnConnectSerial.Location = new Point(274, 465);
            btnConnectSerial.Name = "btnConnectSerial";
            btnConnectSerial.Size = new Size(105, 29);
            btnConnectSerial.TabIndex = 16;
            btnConnectSerial.Text = "Kết nối";
            btnConnectSerial.UseVisualStyleBackColor = true;
            btnConnectSerial.Click += btnConnectSerial_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(41, 380);
            label10.Name = "label10";
            label10.Size = new Size(69, 23);
            label10.TabIndex = 15;
            label10.Text = "Stop Bit";
            // 
            // cbStopBit
            // 
            cbStopBit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbStopBit.FormattingEnabled = true;
            cbStopBit.Location = new Point(63, 412);
            cbStopBit.Name = "cbStopBit";
            cbStopBit.Size = new Size(315, 28);
            cbStopBit.TabIndex = 14;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(42, 298);
            label8.Name = "label8";
            label8.Size = new Size(77, 23);
            label8.TabIndex = 13;
            label8.Text = "Parity Bit";
            // 
            // cbParityBit
            // 
            cbParityBit.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbParityBit.FormattingEnabled = true;
            cbParityBit.Location = new Point(64, 330);
            cbParityBit.Name = "cbParityBit";
            cbParityBit.Size = new Size(315, 28);
            cbParityBit.TabIndex = 12;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(41, 215);
            label9.Name = "label9";
            label9.Size = new Size(81, 23);
            label9.TabIndex = 11;
            label9.Text = "Data Size";
            // 
            // cbDataSize
            // 
            cbDataSize.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbDataSize.FormattingEnabled = true;
            cbDataSize.Location = new Point(64, 250);
            cbDataSize.Name = "cbDataSize";
            cbDataSize.Size = new Size(315, 28);
            cbDataSize.TabIndex = 10;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(38, 135);
            label6.Name = "label6";
            label6.Size = new Size(106, 23);
            label6.TabIndex = 9;
            label6.Text = "Tốc độ Baud";
            // 
            // cbBaud
            // 
            cbBaud.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbBaud.FormattingEnabled = true;
            cbBaud.Location = new Point(60, 167);
            cbBaud.Name = "cbBaud";
            cbBaud.Size = new Size(315, 28);
            cbBaud.TabIndex = 8;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(37, 52);
            label7.Name = "label7";
            label7.Size = new Size(95, 23);
            label7.TabIndex = 7;
            label7.Text = "Cổng COM";
            // 
            // cbCOM
            // 
            cbCOM.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbCOM.FormattingEnabled = true;
            cbCOM.Location = new Point(60, 87);
            cbCOM.Name = "cbCOM";
            cbCOM.Size = new Size(315, 28);
            cbCOM.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Top;
            label2.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(90, 31);
            label2.TabIndex = 1;
            label2.Text = "ROBOT";
            // 
            // FormSetting
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(234, 236, 241);
            ClientSize = new Size(897, 652);
            Controls.Add(splitContainer1);
            Name = "FormSetting";
            Text = "FormSetting";
            FormClosing += FormSetting_FormClosing;
            Load += FormSetting_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picWebcam).EndInit();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private RoundedPanel roundedPanel1;
        private RoundedPanel roundedPanel2;
        private Label label4;
        private Label label3;
        private ComboBox cbWebcam;
        private Label label1;
        private Label label2;
        private Button btnDisconnect;
        private Button btnConnect;
        private Label label5;
        private PictureBox picWebcam;
        private Label label6;
        private ComboBox cbBaud;
        private Label label7;
        private ComboBox cbCOM;
        private Label label8;
        private ComboBox cbParityBit;
        private Label label9;
        private ComboBox cbDataSize;
        private Button btnDisconnectSer;
        private Button btnConnectSerial;
        private Label label10;
        private ComboBox cbStopBit;
        private Button btnConnectIP;
        private TextBox txtIP;
    }
}