using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.ComponentModel;
namespace RobotView
{
    /// <summary>
    /// Interaction logic for Robot4DOF.xaml
    /// </summary>
    public partial class Robot4DOF : System.Windows.Controls.UserControl, IDisposable
    {
        public MainViewModel vm;
        public Robot4DOF()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            vm = (MainViewModel)DataContext;

            RobotScene.AddNode(
                vm.Robot.BaseNode!);

            RobotScene.AddNode(
                vm.Robot.Link1Node!);

            RobotScene.AddNode(
                vm.Robot.Link2Node!);

            RobotScene.AddNode(
                vm.Robot.Link3Node!);

            RobotScene.AddNode(
                vm.Robot.Link4Node!);

        }
        public void Dispose()
        {
            RobotScene?.Clear();
            RobotScene?.Dispose();
            RobotScene = null;

            vm?.Dispose();
            vm = null;

            viewport?.Dispose();
        }
    }
}
