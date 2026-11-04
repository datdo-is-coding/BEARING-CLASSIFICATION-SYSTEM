using HelixToolkit.SharpDX.Assimp;
using HelixToolkit.SharpDX.Model.Scene;
using System.Numerics;
using System.IO;
using HelixToolkit.SharpDX.Model;
using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Windows;

namespace RobotView
{
    public class RobotModel : IDisposable
    {
        public SceneNode? BaseNode { get; private set; }
        public SceneNode? Link1Node { get; private set; }
        public SceneNode? Link2Node { get; private set; }
        public SceneNode? Link3Node { get; private set; }
        public SceneNode? Link4Node { get; private set; }

        // =========================================================
        // ROBOT PARAMETERS
        // =========================================================

        float L1 = 80.0f;
        float L2 = 175.0f;
        float L3 = 175.0f;
        float L4 = 35.0f;

        // =========================================================
        // IK ANGLES - FLOAT
        // =========================================================

        public float ik_theta1;
        public float ik_theta2;
        public float ik_theta3;
        public float ik_theta4;

        // =========================================================
        // SERVO ANGLES - FLOAT
        // =========================================================

        public float servoAngle1;
        public float servoAngle2;
        public float servoAngle3;
        public float servoAngle4;

        // =========================================================
        // JOINT 1
        // =========================================================

        // Pivot J1 trong WORLD
        private readonly Vector3 joint1Position =
            new Vector3(0, 75, 0);

        // Trục quay J1
        private readonly Vector3 joint1Axis =
            Vector3.UnitZ;

        // Transform ban đầu Link1
        private Matrix4x4 link1InitialMatrix =
            Matrix4x4.Identity;

        // =========================================================
        // JOINT 2
        // =========================================================

        // Tâm J2
        private readonly Vector3 joint2Position =
            new Vector3(0, 65, 80);

        private readonly Vector3 joint2Axis =
            Vector3.UnitX;

        // Transform ban đầu Link2
        private Matrix4x4 link2InitialMatrix =
            Matrix4x4.Identity;

        // =========================================================
        // JOINT 3
        // =========================================================

        private readonly Vector3 joint3Position =
            new Vector3(0, 63.1f, 80 + 175.8f);

        private readonly Vector3 joint3Axis =
            Vector3.UnitX;

        // Transform ban đầu Link3
        private Matrix4x4 link3InitialMatrix =
            Matrix4x4.Identity;

        // =========================================================
        // JOINT 4
        // =========================================================

        private readonly Vector3 joint4Position =
            new Vector3(0, 67.6f, 80 + 175.8f + 175f);

        private readonly Vector3 joint4Axis =
            Vector3.UnitX;

        // Transform ban đầu Link4
        private Matrix4x4 link4InitialMatrix =
            Matrix4x4.Identity;


        // =========================================================
        // LOAD STL
        // =========================================================

        public SceneNode AddMesh(
            string path,
            float scale,
            Vector3 rotation,
            Vector3 translation)
        {
            using var importer = new Importer();

            var scene = importer.Load(path);

            if (scene == null)
            {
                throw new Exception(
                    $"Không load được STL:\n{path}");
            }

            SceneNode root = scene.Root;

            // -----------------------------------------------------
            // Transform ban đầu
            // -----------------------------------------------------

            Matrix4x4 matrix =
                Matrix4x4.CreateScale(scale)
                *
                Matrix4x4.CreateRotationX(rotation.X)
                *
                Matrix4x4.CreateRotationY(rotation.Y)
                *
                Matrix4x4.CreateRotationZ(rotation.Z)
                *
                Matrix4x4.CreateTranslation(translation);

            root.ModelMatrix = matrix;

            // -----------------------------------------------------
            // Xác định mesh
            // -----------------------------------------------------

            string fileName =
                Path.GetFileName(path).ToLower();

            if (fileName.Contains("base"))
            {
                BaseNode = root;
            }
            else if (fileName.Contains("link1"))
            {
                Link1Node = root;
                link1InitialMatrix = matrix;
            }
            else if (fileName.Contains("link2"))
            {
                Link2Node = root;
                link2InitialMatrix = matrix;
            }
            else if (fileName.Contains("link3"))
            {
                Link3Node = root;
                link3InitialMatrix = matrix;
            }
            else if (fileName.Contains("tool"))
            {
                Link4Node = root;
                link4InitialMatrix = matrix;
            }

            // -----------------------------------------------------
            // Material
            // -----------------------------------------------------

            foreach (var node in root.Traverse())
            {
                if (node is MeshNode meshNode)
                {
                    if (fileName.Contains("tool"))
                    {
                        meshNode.Material =
                            PhongMaterials.Chrome;
                    }
                    else
                    {
                        meshNode.Material =
                            PhongMaterials.BlackPlastic;
                    }
                }
            }

            return root;
        }


        // =========================================================
        // ROTATE VECTOR
        // =========================================================

        private Vector3 RotateVector(
            Vector3 vector,
            Vector3 axis,
            float angleRad)
        {
            axis = Vector3.Normalize(axis);

            Quaternion q =
                Quaternion.CreateFromAxisAngle(
                    axis,
                    angleRad);

            return Vector3.Transform(
                vector,
                q);
        }


        // =========================================================
        // TẠO ROTATION QUANH PIVOT
        // =========================================================

        private Matrix4x4 CreatePivotRotation(
            Vector3 position,
            Vector3 axis,
            float angleRad)
        {
            axis = Vector3.Normalize(axis);

            Matrix4x4 rotation =
                Matrix4x4.CreateFromAxisAngle(
                    axis,
                    angleRad);

            return
                Matrix4x4.CreateTranslation(-position)
                *
                rotation
                *
                Matrix4x4.CreateTranslation(position);
        }


        // =========================================================
        // SET JOINT ANGLES
        // FLOAT
        // =========================================================

        public void SetJointAngles(
            float joint1AngleDegree,
            float joint2AngleDegree,
            float joint3AngleDegree,
            float joint4AngleDegree)
        {
            // -----------------------------------------------------
            // Degree -> Radian
            // -----------------------------------------------------

            float theta1 =
                joint1AngleDegree *
                MathF.PI / 180.0f;

            float theta2 =
                joint2AngleDegree *
                MathF.PI / 180.0f;

            float theta3 =
                joint3AngleDegree *
                MathF.PI / 180.0f;

            float theta4 =
                joint4AngleDegree *
                MathF.PI / 180.0f;


            // =====================================================
            // J1
            // =====================================================

            Vector3 axis1 =
                Vector3.Normalize(joint1Axis);

            Matrix4x4 j1Rotation =
                Matrix4x4.CreateTranslation(
                    -joint1Position)
                *
                Matrix4x4.CreateFromAxisAngle(
                    axis1,
                    theta1)
                *
                Matrix4x4.CreateTranslation(
                    joint1Position);

            if (Link1Node != null)
            {
                Link1Node.ModelMatrix =
                    link1InitialMatrix
                    *
                    j1Rotation;
            }


            // =====================================================
            // J2
            // =====================================================

            Vector3 p2Relative =
                joint2Position -
                joint1Position;

            Vector3 p2Current =
                joint1Position +
                RotateVector(
                    p2Relative,
                    axis1,
                    theta1);

            Vector3 axis2Current =
                RotateVector(
                    joint2Axis,
                    axis1,
                    theta1);

            axis2Current =
                Vector3.Normalize(axis2Current);

            Matrix4x4 j2Rotation =
                CreatePivotRotation(
                    p2Current,
                    axis2Current,
                    theta2);

            if (Link2Node != null)
            {
                Link2Node.ModelMatrix =
                    link2InitialMatrix
                    *
                    j1Rotation
                    *
                    j2Rotation;
            }


            // =====================================================
            // J3
            // =====================================================

            Vector3 p3Relative =
                joint3Position -
                joint2Position;

            Vector3 p3RelativeAfterJ1 =
                RotateVector(
                    p3Relative,
                    axis1,
                    theta1);

            Vector3 p3Current =
                p2Current +
                RotateVector(
                    p3RelativeAfterJ1,
                    axis2Current,
                    theta2);

            Vector3 axis3Current =
                RotateVector(
                    joint3Axis,
                    axis1,
                    theta1);

            axis3Current =
                Vector3.Normalize(axis3Current);

            Matrix4x4 j3Rotation =
                CreatePivotRotation(
                    p3Current,
                    axis3Current,
                    theta3);

            if (Link3Node != null)
            {
                Link3Node.ModelMatrix =
                    link3InitialMatrix
                    *
                    j1Rotation
                    *
                    j2Rotation
                    *
                    j3Rotation;
            }


            // =====================================================
            // J4
            // =====================================================

            Vector3 p4Relative =
                joint4Position -
                joint3Position;

            // J1
            Vector3 p4RelativeAfterJ1 =
                RotateVector(
                    p4Relative,
                    axis1,
                    theta1);

            // J2
            Vector3 p4RelativeAfterJ2 =
                RotateVector(
                    p4RelativeAfterJ1,
                    axis2Current,
                    theta2);

            // J3
            Vector3 p4Current =
                p3Current +
                RotateVector(
                    p4RelativeAfterJ2,
                    axis3Current,
                    theta3);

            // Trục J4 bị ảnh hưởng bởi J1
            // và các chuyển động trước đó
            Vector3 axis4Current =
                RotateVector(
                    joint4Axis,
                    axis1,
                    theta1);

            axis4Current =
                Vector3.Normalize(axis4Current);

            // Rotation J4 quanh pivot J4
            Matrix4x4 j4Rotation =
                CreatePivotRotation(
                    p4Current,
                    axis4Current,
                    theta4);

            if (Link4Node != null)
            {
                Link4Node.ModelMatrix =
                    link4InitialMatrix
                    *
                    j1Rotation
                    *
                    j2Rotation
                    *
                    j3Rotation
                    *
                    j4Rotation;
            }
        }


        // =========================================================
        // SET J1
        // =========================================================

        


       


        

        public void ResetJoints()
        {
            if (Link1Node != null)
            {
                Link1Node.ModelMatrix =
                    link1InitialMatrix;
            }

            if (Link2Node != null)
            {
                Link2Node.ModelMatrix =
                    link2InitialMatrix;
            }

            if (Link3Node != null)
            {
                Link3Node.ModelMatrix =
                    link3InitialMatrix;
            }

            if (Link4Node != null)
            {
                Link4Node.ModelMatrix =
                    link4InitialMatrix;
            }
        }


   

        public float TO_DEGREE(float angle)
        {
            return
                angle *
                180.0f /
                MathF.PI;
        }


        

        public void IK(
            float x,
            float y,
            float z)
        {


            float z_offset =
                z + L4;


            ik_theta1 =
                TO_DEGREE(
                    MathF.Atan2(
                        y,
                        x));



            float r =
                MathF.Sqrt(
                    x * x +
                    y * y);


            float Z =
                L1 -
                z_offset;


            float c3 =
                (r * r +
                 Z * Z -
                 L2 * L2 -
                 L3 * L3)
                /
                (2.0f * L2 * L3);

            float s3 =
                MathF.Sqrt(
                    1.0f -
                    c3 * c3);

            ik_theta3 =
                TO_DEGREE(
                    MathF.Atan2(
                        s3,
                        c3));
            float A =
                L2 +
                L3 * c3;

            float B =
                L3 * s3;

            float denominator =
                r * r +
                Z * Z;

            float c2 =
                (r * A +
                 Z * B)
                /
                denominator;

            float s2 =
                (Z * A -
                 r * B)
                /
                denominator;

            ik_theta2 =
                TO_DEGREE(
                    MathF.Atan2(
                        s2,
                        c2));


            ik_theta4 =
                180.0f -
                ik_theta2 -
                ik_theta3;

            ToServoAngle();
            
        }
        public void ToServoAngle()
        {
            servoAngle1 =
                ik_theta1 +
                180.0f;

            servoAngle2 =
                ik_theta2 +
                90.0f;

            servoAngle3 =
                ik_theta3;

            servoAngle4 =
                ik_theta4 -
                90.0f;
        }


        public void Dispose()
        {
            DisposeNode(BaseNode);
            DisposeNode(Link1Node);
            DisposeNode(Link2Node);
            DisposeNode(Link3Node);
            DisposeNode(Link4Node);

            BaseNode = null;
            Link1Node = null;
            Link2Node = null;
            Link3Node = null;
            Link4Node = null;
        }

        private void DisposeNode(SceneNode? node)
        {
            if (node == null) return;
            foreach (var child in node.Traverse())
            {
                if (child is MeshNode meshNode)
                {
                    meshNode.Material = null;
                    meshNode.Geometry = null;
                }
                child.Dispose();
            }
            node.Dispose();
        }
    }

}
