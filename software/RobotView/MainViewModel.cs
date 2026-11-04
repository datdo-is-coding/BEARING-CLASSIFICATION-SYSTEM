using HelixToolkit.Geometry;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;

using SharpDX;
using System.ComponentModel; // INotifyPropertyChanged
using System.Runtime.CompilerServices;
using System.Numerics;
using HelixToolkit.SharpDX.Model.Scene;
using System;


namespace RobotView
{
    public class MainViewModel : INotifyPropertyChanged, IDisposable
    {
        public EffectsManager EffectsManager { get; }

        public Camera camera { get; }

        

        public LineGeometry3D LineMesh { get; }
        

        public RobotModel Robot { get; }

        
        
        
        public MainViewModel()
        {
            if (DesignerProperties.GetIsInDesignMode(
                new System.Windows.DependencyObject()))
            {
                return;
            }
            EffectsManager = new DefaultEffectsManager();
            camera = new PerspectiveCamera
            {
                Position = new System.Windows.Media.Media3D.Point3D(300, 400, 360),
                LookDirection = new System.Windows.Media.Media3D.Vector3D(-5, -5, -5),




                UpDirection = new System.Windows.Media.Media3D.Vector3D(0, 0, 1),

                FieldOfView = 60,
                NearPlaneDistance = 0.1,
                FarPlaneDistance = 10000
            };

            var LineBuilder = new LineBuilder();

            float size = 10000;
            float step = 500;

            for (float x = -size; x <= size; x += step)
            {
                LineBuilder.AddLine(
                    new Vector3(x, size, 0),
                    new Vector3(x, -size, 0));
            }

            for (float y = -size; y <= size; y += step)
            {
                LineBuilder.AddLine(
                    new Vector3(size, y, 0),
                    new Vector3(-size, y, 0));
            }
            


            LineMesh = LineBuilder.ToLineGeometry3D();




            Robot = new RobotModel();
    
            Robot.AddMesh(
                "D:\\C-SHARP\\RobotView\\models\\base_asm.stl",
                1.0f,
                new Vector3(MathF.PI / 2, 0, MathF.PI / 2),
                new Vector3(-75, 0, 0));

            Robot.AddMesh(
                "D:\\C-SHARP\\RobotView\\models\\link1.stl",
                1.0f,
                new Vector3(MathF.PI / 2, 0, MathF.PI / 2),
                new Vector3(-45, 30, 45));

            Robot.AddMesh(
                "D:\\C-SHARP\\RobotView\\models\\link2.stl",
                1.0f,
                new Vector3(MathF.PI / 2, 0, MathF.PI / 2),
                new Vector3(-176, -24.6f, 55));

            Robot.AddMesh(
                "D:\\C-SHARP\\RobotView\\models\\link3.stl",
                1.0f,
                new Vector3(MathF.PI / 2, 0, MathF.PI / 2),
                new Vector3(-176, -25, 80 + 175 - 25));

            Robot.AddMesh(
                "D:\\C-SHARP\\Robot4DOF\\models\\tool.stl",
                1.0f,
                new Vector3(-MathF.PI / 2, 0, MathF.PI),
                new Vector3(-16, 82.3f, 80 + 175.8f + 175f + 34.5f));



            



        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            
            // RobotModel
            Robot?.Dispose();

            // EffectsManager
            EffectsManager?.Dispose();
        }

        protected void OnPropertyChanged([CallerMemberName] string info = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        protected bool Set<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
        {
            if (object.Equals(backingField, value))
            {
                return false;
            }

            backingField = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
