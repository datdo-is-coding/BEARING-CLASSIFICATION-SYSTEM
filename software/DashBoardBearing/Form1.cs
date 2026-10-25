using FontAwesome.Sharp;
using Microsoft.VisualBasic.Logging;
using RobotView;
using System.IO;
namespace DashBoardBearing
{
    public partial class Form1 : Form
    {
        private Form? currentForm;
        private IconButton? currentBtn;
        private Panel leftBorderBtn;

        private ToolStripStatusLabel lblRobot;
        private ToolStripStatusLabel lblCamera;


        public CameraManager cameraManager;
        public SerialManager serialManager;
        private Robot4DOF? robotManger;
        public Form1()
        {
            InitializeComponent();
            btnRobot.IconChar = FontAwesome.Sharp.IconChar.Gears;
            btnLog.IconChar = FontAwesome.Sharp.IconChar.History;
         
            btnSetting.IconChar = FontAwesome.Sharp.IconChar.Gear;
            leftBorderBtn = new Panel();
            panelSideBar.Controls.Add(leftBorderBtn);
            

            lblRobot = new ToolStripStatusLabel("● Robot: Disconnected");
            lblRobot.ForeColor = Color.Red;
            lblCamera = new ToolStripStatusLabel("● Camera: Disconnected");
            lblCamera.ForeColor = Color.Red;
            

            statusStrip.Items.Add(lblRobot);
            statusStrip.Items.Add(new ToolStripSeparator());
            statusStrip.Items.Add(lblCamera);
            statusStrip.Items.Add(new ToolStripSeparator());
    

            cameraManager = new CameraManager(
                "D:\\C-SHARP\\DashBoardBearing\\Model\\yolo_bearing.onnx",
                "D:\\C-SHARP\\DashBoardBearing\\Model\\resnet34_bearing.onnx");
            cameraManager.CameraStatus += CameraStatusChanged;
            
            serialManager = new SerialManager();
            serialManager.SerialStatus += SerialPortStatusChanged;
        }

        private void OpenForm(Form form)
        {
            if (currentForm != null)
            {
                panelMain.Controls.Remove(currentForm);
                currentForm.Close();
                currentForm.Dispose();
                currentForm = null;
            }

            currentForm = form;
            currentForm.TopLevel = false;
            currentForm.FormBorderStyle = FormBorderStyle.None;
            currentForm.Dock = DockStyle.Fill;
            panelMain.Controls.Add(currentForm);
            panelMain.Tag = currentForm;
            currentForm.Show();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.Black);

            OpenForm(new FormHome(cameraManager));
        }



        private void btnVision_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.Black);

            OpenForm(new FormVision());
        }


        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();

                // Button
                currentBtn = (IconButton)senderBtn;

                currentBtn.BackColor = Color.White;
                currentBtn.ForeColor = color;

                currentBtn.TextAlign = ContentAlignment.MiddleCenter;

                currentBtn.IconColor = color;

                currentBtn.TextImageRelation =
                    TextImageRelation.TextBeforeImage;

                currentBtn.ImageAlign =
                    ContentAlignment.MiddleRight;

                // Left border button
                leftBorderBtn.BackColor = color;

                leftBorderBtn.Location =
                    new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Size = new Size(20, 60);

                leftBorderBtn.Visible = true;

                leftBorderBtn.BringToFront();
            }
        }

        // =========================
        // DISABLE BUTTON
        // =========================

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.White;

                currentBtn.ForeColor = Color.Black;

                currentBtn.TextAlign =
                    ContentAlignment.MiddleLeft;

                currentBtn.IconColor = Color.Black;

                currentBtn.TextImageRelation =
                    TextImageRelation.ImageBeforeText;

                currentBtn.ImageAlign =
                    ContentAlignment.MiddleLeft;
            }
        }

        private void btnRobot_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.Black);

            OpenForm(new FormRobot());
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.Black);

            OpenForm(new FormLog());
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Color.Black);

            OpenForm(new FormSetting(cameraManager,serialManager));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (currentForm != null)
            {
                panelMain.Controls.Remove(currentForm);
                currentForm.Close();
                currentForm.Dispose();
                currentForm = null;
            }

            if(robotManger != null)
            {
                robotManger?.Dispose();
                robotManger = null;
            }
        }

        private void CameraStatusChanged(bool connected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    CameraStatusChanged(connected);
                }));

                return;
            }

            if (connected)
            {
                lblCamera.Text = "● Camera: Connected";
                lblCamera.ForeColor = Color.Green;
            }
            else
            {
                lblCamera.Text = "● Camera: Disconnected";
                lblCamera.ForeColor = Color.Red;
            }
        }

        private void SerialPortStatusChanged(bool connected)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    SerialPortStatusChanged(connected);
                }));

                return;
            }

            if (connected)
            {
                
                lblRobot.Text = "● Robot: Connected";
                lblRobot.ForeColor = Color.Green;
            }
            else
            {
                lblRobot.Text = "● Robot: Disconnected";
                lblRobot.ForeColor = Color.Red;
            }
        }
    }
}
