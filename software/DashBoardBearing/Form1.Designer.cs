namespace DashBoardBearing
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelSideBar = new Panel();
            btnSetting = new FontAwesome.Sharp.IconButton();
            btnLog = new FontAwesome.Sharp.IconButton();
            btnRobot = new FontAwesome.Sharp.IconButton();
            btnVision = new FontAwesome.Sharp.IconButton();
            btnHome = new FontAwesome.Sharp.IconButton();
            picLogo = new PictureBox();
            panelLogo = new Panel();
            panelMain = new Panel();
            statusStrip = new StatusStrip();
            panelSideBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            panelMain.SuspendLayout();
            SuspendLayout();
            // 
            // panelSideBar
            // 
            panelSideBar.BackColor = Color.White;
            panelSideBar.Controls.Add(btnSetting);
            panelSideBar.Controls.Add(btnLog);
            panelSideBar.Controls.Add(btnRobot);
            panelSideBar.Controls.Add(btnVision);
            panelSideBar.Controls.Add(btnHome);
            panelSideBar.Controls.Add(picLogo);
            panelSideBar.Controls.Add(panelLogo);
            panelSideBar.Dock = DockStyle.Left;
            panelSideBar.Location = new Point(0, 0);
            panelSideBar.Name = "panelSideBar";
            panelSideBar.Size = new Size(217, 699);
            panelSideBar.TabIndex = 0;
            // 
            // btnSetting
            // 
            btnSetting.Dock = DockStyle.Top;
            btnSetting.FlatAppearance.BorderSize = 0;
            btnSetting.FlatStyle = FlatStyle.Flat;
            btnSetting.Font = new Font("Century Gothic", 12F);
            btnSetting.IconChar = FontAwesome.Sharp.IconChar.HomeLg;
            btnSetting.IconColor = Color.Gray;
            btnSetting.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSetting.IconSize = 30;
            btnSetting.ImageAlign = ContentAlignment.MiddleLeft;
            btnSetting.Location = new Point(0, 393);
            btnSetting.Name = "btnSetting";
            btnSetting.Padding = new Padding(8, 4, 8, 8);
            btnSetting.Size = new Size(217, 60);
            btnSetting.TabIndex = 6;
            btnSetting.Text = "SETTING";
            btnSetting.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnSetting.UseVisualStyleBackColor = true;
            btnSetting.Click += btnSetting_Click;
            // 
            // btnLog
            // 
            btnLog.Dock = DockStyle.Top;
            btnLog.FlatAppearance.BorderSize = 0;
            btnLog.FlatStyle = FlatStyle.Flat;
            btnLog.Font = new Font("Century Gothic", 12F);
            btnLog.IconChar = FontAwesome.Sharp.IconChar.HomeLg;
            btnLog.IconColor = Color.Gray;
            btnLog.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnLog.IconSize = 30;
            btnLog.ImageAlign = ContentAlignment.MiddleLeft;
            btnLog.Location = new Point(0, 333);
            btnLog.Name = "btnLog";
            btnLog.Padding = new Padding(8, 4, 8, 8);
            btnLog.Size = new Size(217, 60);
            btnLog.TabIndex = 5;
            btnLog.Text = "LOG";
            btnLog.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLog.UseVisualStyleBackColor = true;
            btnLog.Click += btnLog_Click;
            // 
            // btnRobot
            // 
            btnRobot.Dock = DockStyle.Top;
            btnRobot.FlatAppearance.BorderSize = 0;
            btnRobot.FlatStyle = FlatStyle.Flat;
            btnRobot.Font = new Font("Century Gothic", 12F);
            btnRobot.IconChar = FontAwesome.Sharp.IconChar.A;
            btnRobot.IconColor = Color.Gray;
            btnRobot.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnRobot.IconSize = 30;
            btnRobot.ImageAlign = ContentAlignment.MiddleLeft;
            btnRobot.Location = new Point(0, 273);
            btnRobot.Name = "btnRobot";
            btnRobot.Padding = new Padding(8, 4, 8, 8);
            btnRobot.Size = new Size(217, 60);
            btnRobot.TabIndex = 4;
            btnRobot.Text = "ROBOT";
            btnRobot.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRobot.UseVisualStyleBackColor = true;
            btnRobot.Click += btnRobot_Click;
            // 
            // btnVision
            // 
            btnVision.Dock = DockStyle.Top;
            btnVision.FlatAppearance.BorderSize = 0;
            btnVision.FlatStyle = FlatStyle.Flat;
            btnVision.Font = new Font("Century Gothic", 12F);
            btnVision.IconChar = FontAwesome.Sharp.IconChar.CameraAlt;
            btnVision.IconColor = Color.Gray;
            btnVision.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnVision.IconSize = 30;
            btnVision.ImageAlign = ContentAlignment.MiddleLeft;
            btnVision.Location = new Point(0, 213);
            btnVision.Name = "btnVision";
            btnVision.Padding = new Padding(8, 4, 8, 8);
            btnVision.Size = new Size(217, 60);
            btnVision.TabIndex = 3;
            btnVision.Text = "VISION";
            btnVision.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnVision.UseVisualStyleBackColor = true;
            btnVision.Click += btnVision_Click;
            // 
            // btnHome
            // 
            btnHome.Dock = DockStyle.Top;
            btnHome.FlatAppearance.BorderSize = 0;
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Font = new Font("Century Gothic", 12F);
            btnHome.IconChar = FontAwesome.Sharp.IconChar.HomeLg;
            btnHome.IconColor = Color.Gray;
            btnHome.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnHome.IconSize = 30;
            btnHome.ImageAlign = ContentAlignment.MiddleLeft;
            btnHome.Location = new Point(0, 153);
            btnHome.Name = "btnHome";
            btnHome.Padding = new Padding(8, 4, 8, 8);
            btnHome.Size = new Size(217, 60);
            btnHome.TabIndex = 2;
            btnHome.Text = "HOME";
            btnHome.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnHome.UseVisualStyleBackColor = true;
            btnHome.Click += btnHome_Click;
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.logo;
            picLogo.Location = new Point(31, 3);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(150, 150);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 0;
            picLogo.TabStop = false;
            // 
            // panelLogo
            // 
            panelLogo.Dock = DockStyle.Top;
            panelLogo.Location = new Point(0, 0);
            panelLogo.Name = "panelLogo";
            panelLogo.Size = new Size(217, 153);
            panelLogo.TabIndex = 1;
            // 
            // panelMain
            // 
            panelMain.BackColor = Color.White;
            panelMain.Controls.Add(statusStrip);
            panelMain.Dock = DockStyle.Fill;
            panelMain.Location = new Point(217, 0);
            panelMain.Name = "panelMain";
            panelMain.Size = new Size(915, 699);
            panelMain.TabIndex = 1;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Location = new Point(0, 677);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(915, 22);
            statusStrip.TabIndex = 0;
            statusStrip.Text = "statusStrip1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1132, 699);
            Controls.Add(panelMain);
            Controls.Add(panelSideBar);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            panelSideBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            panelMain.ResumeLayout(false);
            panelMain.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panelSideBar;
        private PictureBox picLogo;
        private FontAwesome.Sharp.IconButton btnHome;
        private Panel panelLogo;
        private FontAwesome.Sharp.IconButton btnLog;
        private FontAwesome.Sharp.IconButton btnRobot;
        private FontAwesome.Sharp.IconButton btnVision;
        private Panel panelMain;
        private FontAwesome.Sharp.IconButton btnSetting;
        private StatusStrip statusStrip;
    }
}
