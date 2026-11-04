using System;
using System.IO;
using System.Text;

namespace DashBoardBearing
{
    public sealed class SerialLogger
    {
        private readonly object logLock = new object();

        public string LogDirectory { get; }

        public SerialLogger()
        {
            LogDirectory = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "DashBoardBearing",
                "Logs"
            );

            Directory.CreateDirectory(LogDirectory);
        }

        private string GetLogFilePath()
        {
            return Path.Combine(
                LogDirectory,
                $"serial_{DateTime.Now:yyyy-MM-dd}.log"
            );
        }

        public void Write(string category, string message)
        {
            string line =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] " +
                $"[{category}] {message}";

            lock (logLock)
            {
                File.AppendAllText(
                    GetLogFilePath(),
                    line + Environment.NewLine,
                    Encoding.UTF8
                );
            }
        }

        public string LoadToday()
        {
            lock (logLock)
            {
                string path = GetLogFilePath();

                return File.Exists(path)
                    ? File.ReadAllText(path, Encoding.UTF8)
                    : "Chưa có log trong ngày hôm nay.";
            }
        }

        public string LoadLatest()
        {
            lock (logLock)
            {
                if (!Directory.Exists(LogDirectory))
                    return "Thư mục log chưa tồn tại.";

                var files = new DirectoryInfo(LogDirectory)
                    .GetFiles("serial_*.log");

                if (files.Length == 0)
                    return "Chưa có log.";

                Array.Sort(files, (a, b) =>
                    b.LastWriteTime.CompareTo(a.LastWriteTime));

                return File.ReadAllText(files[0].FullName, Encoding.UTF8);
            }
        }
    }
}