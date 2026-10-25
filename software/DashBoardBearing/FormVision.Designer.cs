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
            roundedPanel1 = new RoundedPanel();
            SuspendLayout();
            // 
            // roundedPanel1
            // 
            roundedPanel1.BackColor = Color.White;
            roundedPanel1.BackgroundColor = Color.White;
            roundedPanel1.BorderColor = Color.PaleVioletRed;
            roundedPanel1.Location = new Point(130, 108);
            roundedPanel1.Name = "roundedPanel1";
            roundedPanel1.Size = new Size(250, 125);
            roundedPanel1.TabIndex = 0;
            // 
            // FormVision
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(897, 652);
            Controls.Add(roundedPanel1);
            Name = "FormVision";
            Text = "FormVision";
            ResumeLayout(false);
        }

        #endregion

        private RoundedPanel roundedPanel1;
    }
}