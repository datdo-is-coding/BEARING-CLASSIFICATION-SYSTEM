using System;
using System.IO;
using System.Text.Json;

namespace DashBoardBearing
{
    public static class RobotKinematics
    {
        public const float L1 = 80.0f;
        public const float L2 = 175.0f;
        public const float L3 = 175.0f;
        public const float L4 = 35.0f;

        /// <summary>
        /// Tính góc J4 để đầu công cụ (Link 4 / Tool) luôn vuông góc với mặt đất:
        /// Tổng góc pitch của robot: (J2 - 90°) + J3 + J4 = 180° (hướng thẳng đứng vuông góc mặt bàn)
        /// => J4 = 270° - J2 - J3
        /// </summary>
        public static float CalculatePerpendicularJ4(float j2, float j3)
        {
            return 270.0f - j2 - j3;
        }

        /// <summary>
        /// Forward Kinematics chuẩn y hệt dự án Robot4DOF:
        /// Từ các góc Slider (J1, J2, J3, J4) -> Tọa độ Cartesian (X, Y, Z in mm)
        /// J1: -90° đến +90° (xoay đế)
        /// J2: 0° đến 180° (cánh tay dưới, 90° là đứng)
        /// J3: 0° đến 150° (cánh tay trên)
        /// J4: Luôn tự động tính vuông góc mặt đất nếu không truyền
        /// </summary>
        public static (float X, float Y, float Z) Forward(float j1, float j2, float j3, float? j4 = null)
        {
            float actualJ4 = j4 ?? CalculatePerpendicularJ4(j2, j3);
            float th1_rad = j1 * MathF.PI / 180.0f;
            float th2_rad = (j2 - 180.0f) * MathF.PI / 180.0f;
            float th3_rad = j3 * MathF.PI / 180.0f;

            float r = L2 * MathF.Cos(th2_rad) + L3 * MathF.Cos(th2_rad + th3_rad);
            float Z_arm = L2 * MathF.Sin(th2_rad) + L3 * MathF.Sin(th2_rad + th3_rad);

            float x = r * MathF.Cos(th1_rad);
            float y = r * MathF.Sin(th1_rad);
            float z = L1 - L4 - Z_arm;

            return (x, y, z);
        }

        /// <summary>
        /// Inverse Kinematics chuẩn y hệt thuật toán trong Robot4DOF (RobotModel.IK):
        /// Từ tọa độ (X, Y, Z in mm) -> Tính ra góc khớp Slider (J1, J2, J3, J4)
        /// X: Hướng vươn tới phía trước (+)
        /// Y: Hướng lệch ngang trái/phải (+/-)
        /// Z: Chiều cao tính từ mặt bàn (+)
        /// </summary>
        public static bool Inverse(float x, float y, float z, out float j1, out float j2, out float j3, out float j4)
        {
            float z_offset = z + L4;
            j1 = MathF.Atan2(y, x) * 180.0f / MathF.PI;

            float r = MathF.Sqrt(x * x + y * y);
            float Z_int = L1 - z_offset;

            float c3 = (r * r + Z_int * Z_int - L2 * L2 - L3 * L3) / (2.0f * L2 * L3);
            if (c3 < -1.0f || c3 > 1.0f)
            {
                j1 = j2 = j3 = j4 = 0;
                return false; // Ngoài tầm với vật lý của robot
            }

            float s3 = MathF.Sqrt(Math.Max(0.0f, 1.0f - c3 * c3));
            float ik_theta3 = MathF.Atan2(s3, c3) * 180.0f / MathF.PI;

            float A = L2 + L3 * c3;
            float B = L3 * s3;
            float denom = r * r + Z_int * Z_int;
            if (denom < 1e-4f)
            {
                j1 = j2 = j3 = j4 = 0;
                return false;
            }

            float c2 = (r * A + Z_int * B) / denom;
            float s2 = (Z_int * A - r * B) / denom;
            float ik_theta2 = MathF.Atan2(s2, c2) * 180.0f / MathF.PI;

            // Quy đổi sang thang Slider chuẩn của Robot4DOF:
            j2 = ik_theta2 + 180.0f;
            j3 = ik_theta3;
            // J4 tự động giữ đầu công cụ luôn vuông góc với mặt đất:
            j4 = CalculatePerpendicularJ4(j2, j3);

            return true;
        }
    }

    public class JointPose
    {
        public float J1 { get; set; } = 0.0f;
        public float J2 { get; set; } = 100.1f;
        public float J3 { get; set; } = 120.3f;
        public float J4 { get; set; } = 0.0f;

        public float X { get; set; } = 164.0f;
        public float Y { get; set; } = 0.0f;
        public float Z { get; set; } = 104.0f;

        public JointPose()
        {
            UpdateXYZFromJoints();
        }

        public JointPose(float j1, float j2, float j3, float? j4 = null)
        {
            J1 = j1;
            J2 = j2;
            J3 = j3;
            J4 = j4 ?? RobotKinematics.CalculatePerpendicularJ4(j2, j3);
            UpdateXYZFromJoints();
        }

        public static JointPose FromXYZ(float x, float y, float z, float? j4 = null)
        {
            var pose = new JointPose
            {
                X = (float)Math.Round(x, 1),
                Y = (float)Math.Round(y, 1),
                Z = (float)Math.Round(z, 1)
            };
            if (RobotKinematics.Inverse(x, y, z, out float j1, out float j2, out float j3, out float calcJ4))
            {
                pose.J1 = (float)Math.Round(j1, 1);
                pose.J2 = (float)Math.Round(j2, 1);
                pose.J3 = (float)Math.Round(j3, 1);
                pose.J4 = (float)Math.Round(j4 ?? calcJ4, 1);
            }
            else
            {
                pose.J4 = (float)Math.Round(j4 ?? RobotKinematics.CalculatePerpendicularJ4(pose.J2, pose.J3), 1);
            }
            return pose;
        }

        public void UpdateXYZFromJoints()
        {
            var (x, y, z) = RobotKinematics.Forward(J1, J2, J3, J4);
            X = (float)Math.Round(x, 1);
            Y = (float)Math.Round(y, 1);
            Z = (float)Math.Round(z, 1);
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture,
                "XYZ: {0:F1}, {1:F1}, {2:F1} | J: {3:F1}°, {4:F1}°, {5:F1}°",
                X, Y, Z, J1, J2, J3);
        }
    }

    public class RobotTeachConfig
    {
        // 5 vị trí mặc định chuẩn theo không gian làm việc thực tế (J4 tự động vuông góc với mặt đất)
        public JointPose StartPoint { get; set; } = new JointPose(0.0f, 100.1f, 120.3f);       // Home: X=164, Y=0, Z=104
        public JointPose PickPoint { get; set; } = new JointPose(0.0f, 115.0f, 105.0f);        // Gắp phôi
        public JointPose DropRusted { get; set; } = new JointPose(-40.0f, 110.0f, 110.0f);      // Thả phôi gỉ
        public JointPose DropDamagedShield { get; set; } = new JointPose(40.0f, 110.0f, 110.0f); // Thả phôi nắp che
        public JointPose DropSurfaceDamage { get; set; } = new JointPose(70.0f, 110.0f, 110.0f); // Thả trầy xước

        private static string GetConfigPath()
        {
            try
            {
                string localApp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DashBoardBearing");
                Directory.CreateDirectory(localApp);
                return Path.Combine(localApp, "robot_teach_points.json");
            }
            catch
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "robot_teach_points.json");
            }
        }

        public static RobotTeachConfig Load()
        {
            try
            {
                string path = GetConfigPath();
                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    var cfg = JsonSerializer.Deserialize<RobotTeachConfig>(json);
                    if (cfg != null)
                    {
                        cfg.StartPoint?.UpdateXYZFromJoints();
                        cfg.PickPoint?.UpdateXYZFromJoints();
                        cfg.DropRusted?.UpdateXYZFromJoints();
                        cfg.DropDamagedShield?.UpdateXYZFromJoints();
                        cfg.DropSurfaceDamage?.UpdateXYZFromJoints();
                        return cfg;
                    }
                }
            }
            catch { }
            return new RobotTeachConfig();
        }

        public void Save()
        {
            try
            {
                StartPoint?.UpdateXYZFromJoints();
                PickPoint?.UpdateXYZFromJoints();
                DropRusted?.UpdateXYZFromJoints();
                DropDamagedShield?.UpdateXYZFromJoints();
                DropSurfaceDamage?.UpdateXYZFromJoints();

                string path = GetConfigPath();
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(path, json);
            }
            catch { }
        }
    }
}
