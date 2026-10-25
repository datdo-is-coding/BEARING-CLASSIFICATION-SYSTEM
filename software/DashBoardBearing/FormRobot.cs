using RobotView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace DashBoardBearing
{
    public partial class FormRobot : Form
    {
        private Robot4DOF _robotView;
        private ElementHost _robotHost;
        public FormRobot(Robot4DOF? robotView = null)
        {
            InitializeComponent();

            _robotView = robotView ?? new Robot4DOF();

            InitRobotView();
        }

        private void InitRobotView()
        {
            _robotHost = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = _robotView
            };

            panelRobot.Controls.Add(_robotHost);
        }

        private void FormRobot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_robotHost != null)
            {
                panelRobot.Controls.Remove(_robotHost);
                _robotHost.Child = null;
                _robotHost.Dispose();
                _robotHost = null;
            }
        }
    }
}
