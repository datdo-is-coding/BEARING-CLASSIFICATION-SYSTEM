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
            splitContainer1 = new SplitContainer();
            roundedPanel1 = new RoundedPanel();
            splitContainer2 = new SplitContainer();
            picWebcam = new PictureBox();
            textBox2 = new TextBox();
            btnDetect = new Button();
            label4 = new Label();
            textBox1 = new TextBox();
            label5 = new Label();
            roundedPanel2 = new RoundedPanel();
            splitContainer3 = new SplitContainer();
            lblDefect = new Label();
            label7 = new Label();
            lblConfidence = new Label();
            label6 = new Label();
            lblTime = new Label();
            label2 = new Label();
            lblStatus = new Label();
            label1 = new Label();
            picResult = new PictureBox();
            label3 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            roundedPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picWebcam).BeginInit();
            roundedPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picResult).BeginInit();
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
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(roundedPanel2);
            splitContainer1.Size = new Size(897, 652);
            splitContainer1.SplitterDistance = 518;
            splitContainer1.TabIndex = 13;
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BackgroundColor = Color.White;
            roundedPanel1.BorderColor = Color.PaleVioletRed;
            roundedPanel1.Controls.Add(splitContainer2);
            roundedPanel1.Controls.Add(textBox1);
            roundedPanel1.Controls.Add(label5);
            roundedPanel1.Dock = DockStyle.Fill;
            roundedPanel1.Location = new Point(0, 0);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(518, 652);
            roundedPanel1.TabIndex = 0;
            roundedPanel1.Paint += roundedPanel1_Paint;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 31);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(picWebcam);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(textBox2);
            splitContainer2.Panel2.Controls.Add(btnDetect);
            splitContainer2.Panel2.Controls.Add(label4);
            splitContainer2.Size = new Size(518, 621);
            splitContainer2.SplitterDistance = 325;
            splitContainer2.TabIndex = 10;
            // 
            // picWebcam
            // 
            picWebcam.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picWebcam.BackColor = Color.Silver;
            picWebcam.Location = new Point(39, 14);
            picWebcam.Name = "picWebcam";
            picWebcam.Size = new Size(433, 290);
            picWebcam.SizeMode = PictureBoxSizeMode.Zoom;
            picWebcam.TabIndex = 9;
            picWebcam.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Location = new Point(39, 52);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(433, 198);
            textBox2.TabIndex = 12;
            // 
            // btnDetect
            // 
            btnDetect.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDetect.Location = new Point(412, 256);
            btnDetect.Name = "btnDetect";
            btnDetect.Size = new Size(103, 24);
            btnDetect.TabIndex = 11;
            btnDetect.Text = "DETECT";
            btnDetect.UseVisualStyleBackColor = true;
            btnDetect.Click += btnDetect_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Top;
            label4.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(58, 31);
            label4.TabIndex = 10;
            label4.Text = "LOG";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(27, 445);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(76, 0);
            textBox1.TabIndex = 9;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Top;
            label5.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(0, 0);
            label5.Name = "label5";
            label5.Size = new Size(109, 31);
            label5.TabIndex = 1;
            label5.Text = "CAMERA";
            // 
            // roundedPanel2
            // 
            roundedPanel2.BackColor = Color.White;
            roundedPanel2.BackgroundColor = Color.White;
            roundedPanel2.BorderColor = Color.PaleVioletRed;
            roundedPanel2.Controls.Add(splitContainer3);
            roundedPanel2.Controls.Add(label3);
            roundedPanel2.Dock = DockStyle.Fill;
            roundedPanel2.Location = new Point(0, 0);
            roundedPanel2.Name = "roundedPanel2";
            roundedPanel2.Size = new Size(375, 652);
            roundedPanel2.TabIndex = 14;
            // 
            // splitContainer3
            // 
            splitContainer3.Dock = DockStyle.Fill;
            splitContainer3.Location = new Point(0, 31);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(lblDefect);
            splitContainer3.Panel1.Controls.Add(label7);
            splitContainer3.Panel1.Controls.Add(lblConfidence);
            splitContainer3.Panel1.Controls.Add(label6);
            splitContainer3.Panel1.Controls.Add(lblTime);
            splitContainer3.Panel1.Controls.Add(label2);
            splitContainer3.Panel1.Controls.Add(lblStatus);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(label1);
            splitContainer3.Panel2.Controls.Add(picResult);
            splitContainer3.Size = new Size(375, 621);
            splitContainer3.SplitterDistance = 325;
            splitContainer3.TabIndex = 13;
            // 
            // lblDefect
            // 
            lblDefect.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblDefect.AutoSize = true;
            lblDefect.BackColor = SystemColors.Control;
            lblDefect.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDefect.ForeColor = Color.Red;
            lblDefect.ImageAlign = ContentAlignment.TopCenter;
            lblDefect.Location = new Point(130, 206);
            lblDefect.Name = "lblDefect";
            lblDefect.Size = new Size(0, 23);
            lblDefect.TabIndex = 23;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(255, 255, 128);
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Black;
            label7.Location = new Point(29, 206);
            label7.Name = "label7";
            label7.Size = new Size(87, 23);
            label7.TabIndex = 22;
            label7.Text = "Trạng thái";
            // 
            // lblConfidence
            // 
            lblConfidence.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblConfidence.AutoSize = true;
            lblConfidence.BackColor = SystemColors.Control;
            lblConfidence.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblConfidence.ForeColor = Color.Red;
            lblConfidence.ImageAlign = ContentAlignment.TopCenter;
            lblConfidence.Location = new Point(131, 243);
            lblConfidence.Name = "lblConfidence";
            lblConfidence.Size = new Size(0, 23);
            lblConfidence.TabIndex = 21;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.BackColor = Color.FromArgb(128, 255, 128);
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Black;
            label6.Location = new Point(29, 242);
            label6.Name = "label6";
            label6.Size = new Size(87, 23);
            label6.TabIndex = 20;
            label6.Text = "Độ tin cậy";
            // 
            // lblTime
            // 
            lblTime.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblTime.AutoSize = true;
            lblTime.BackColor = SystemColors.Control;
            lblTime.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTime.ForeColor = Color.Red;
            lblTime.ImageAlign = ContentAlignment.TopCenter;
            lblTime.Location = new Point(214, 278);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(0, 23);
            lblTime.TabIndex = 17;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.BackColor = Color.Yellow;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(29, 278);
            label2.Name = "label2";
            label2.Size = new Size(166, 23);
            label2.TabIndex = 19;
            label2.Text = "THỜI GIAN NỘI SUY";
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStatus.AutoSize = true;
            lblStatus.BackColor = Color.White;
            lblStatus.Font = new Font("Segoe UI", 64.2000046F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblStatus.Location = new Point(69, 25);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 145);
            lblStatus.TabIndex = 18;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(166, 31);
            label1.TabIndex = 18;
            label1.Text = "ẢNH VÒNG BI";
            // 
            // picResult
            // 
            picResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            picResult.BackColor = Color.Silver;
            picResult.Location = new Point(83, 34);
            picResult.Name = "picResult";
            picResult.Size = new Size(224, 225);
            picResult.SizeMode = PictureBoxSizeMode.Zoom;
            picResult.TabIndex = 17;
            picResult.TabStop = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Top;
            label3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(116, 31);
            label3.TabIndex = 12;
            label3.Text = "KẾT QUẢ ";
            // 
            // FormHome
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(897, 652);
            Controls.Add(splitContainer1);
            Name = "FormHome";
            Text = "FormHome";
            FormClosing += FormHome_FormClosing;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            roundedPanel1.ResumeLayout(false);
            roundedPanel1.PerformLayout();
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picWebcam).EndInit();
            roundedPanel2.ResumeLayout(false);
            roundedPanel2.PerformLayout();
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picResult).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private RoundedPanel roundedPanel1;
        private Label label5;
        private RoundedPanel roundedPanel2;
        private Label label3;
        private Label label1;
        private PictureBox picResult;
        private TextBox textBox1;
        private SplitContainer splitContainer2;
        private PictureBox picWebcam;
        private Button btnDetect;
        private Label label4;
        private SplitContainer splitContainer3;
        private Label lblDefect;
        private Label label7;
        private Label lblConfidence;
        private Label label6;
        private Label lblTime;
        private Label label2;
        private Label lblStatus;
        private TextBox textBox2;
    }
}