using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DashBoardBearing
{
    /// <summary>
    /// Bản ghi lịch sử nhận diện của 1 vòng bi
    /// </summary>
    public class BearingLogEntry
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Label { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsOk { get; set; }
        public float Confidence { get; set; }
        public double ProcessTimeMs { get; set; }
        public string Note { get; set; } = string.Empty;
    }

    /// <summary>
    /// Bộ quản lý và lưu trữ bền vững (persistent storage) lịch sử nhận diện vòng bi.
    /// Thread-safe, hỗ trợ truy vấn theo khoảng thời gian, xuất CSV và thông báo thời gian thực.
    /// </summary>
    public static class BearingLogManager
    {
        private static readonly object _syncLock = new object();
        private static readonly List<BearingLogEntry> _cache = new List<BearingLogEntry>();
        private static int _nextId = 1;
        private static bool _isLoaded = false;

        public static event Action<BearingLogEntry>? EntryAdded;
        public static event Action? LogCleared;

        private static string GetStoragePath()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "DashBoardBearing",
                "Logs"
            );
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "bearing_history.json");
        }

        private static void EnsureLoaded()
        {
            if (_isLoaded) return;
            lock (_syncLock)
            {
                if (_isLoaded) return;
                _isLoaded = true;
                _cache.Clear();

                try
                {
                    string path = GetStoragePath();
                    if (File.Exists(path))
                    {
                        string json = File.ReadAllText(path, Encoding.UTF8);
                        var list = JsonSerializer.Deserialize<List<BearingLogEntry>>(json);
                        if (list != null && list.Count > 0)
                        {
                            _cache.AddRange(list);
                            _nextId = _cache.Max(e => e.Id) + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[BearingLogManager] Lỗi đọc dữ liệu: {ex.Message}");
                }
            }
        }

        private static void SaveInternal()
        {
            try
            {
                string path = GetStoragePath();
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_cache, options);
                File.WriteAllText(path, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BearingLogManager] Lỗi lưu dữ liệu: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm kết quả nhận diện vòng bi mới vào cơ sở dữ liệu và phát sự kiện
        /// </summary>
        public static BearingLogEntry Add(string label, string displayName, bool isOk, float confidence, double processTimeMs, string note = "")
        {
            EnsureLoaded();
            BearingLogEntry entry;
            lock (_syncLock)
            {
                entry = new BearingLogEntry
                {
                    Id = _nextId++,
                    Timestamp = DateTime.Now,
                    Label = label,
                    DisplayName = displayName,
                    IsOk = isOk,
                    Confidence = confidence,
                    ProcessTimeMs = processTimeMs,
                    Note = string.IsNullOrWhiteSpace(note) ? (isOk ? "Đạt chuẩn" : "Phát hiện lỗi") : note
                };

                _cache.Add(entry);
                SaveInternal();
            }

            EntryAdded?.Invoke(entry);
            return entry;
        }

        /// <summary>
        /// Lấy toàn bộ lịch sử (sắp xếp mới nhất lên đầu)
        /// </summary>
        public static List<BearingLogEntry> GetAll()
        {
            EnsureLoaded();
            lock (_syncLock)
            {
                return _cache.OrderByDescending(e => e.Timestamp).ToList();
            }
        }

        /// <summary>
        /// Lấy N bản ghi nhận diện gần đây nhất
        /// </summary>
        public static List<BearingLogEntry> GetRecent(int count = 20)
        {
            EnsureLoaded();
            lock (_syncLock)
            {
                return _cache.OrderByDescending(e => e.Timestamp).Take(count).ToList();
            }
        }

        /// <summary>
        /// Lấy danh sách nhận diện trong ngày hôm nay (từ 00:00:00 đến 23:59:59), sắp xếp tăng dần theo thời gian
        /// </summary>
        public static List<BearingLogEntry> GetToday()
        {
            EnsureLoaded();
            DateTime todayStart = DateTime.Today;
            DateTime todayEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            lock (_syncLock)
            {
                return _cache
                    .Where(e => e.Timestamp >= todayStart && e.Timestamp <= todayEnd)
                    .OrderBy(e => e.Timestamp)
                    .ToList();
            }
        }

        /// <summary>
        /// Lấy số liệu thống kê nhận diện hôm nay: Tổng số, Đạt (OK), Lỗi (NG), và Tỷ lệ Đạt (Yield)
        /// </summary>
        public static (int Total, int Ok, int Ng, double YieldRate) GetTodayStats()
        {
            EnsureLoaded();
            DateTime todayStart = DateTime.Today;
            DateTime todayEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            lock (_syncLock)
            {
                var todayList = _cache.Where(e => e.Timestamp >= todayStart && e.Timestamp <= todayEnd).ToList();
                int total = todayList.Count;
                int ok = todayList.Count(e => e.IsOk);
                int ng = total - ok;
                double yieldRate = total > 0 ? ((double)ok / total) * 100.0 : 0.0;
                return (total, ok, ng, yieldRate);
            }
        }

        /// <summary>
        /// Xóa sạch các bản ghi nhận diện trong ngày hôm nay
        /// </summary>
        public static void ClearToday()
        {
            EnsureLoaded();
            DateTime todayStart = DateTime.Today;
            DateTime todayEnd = DateTime.Today.AddDays(1).AddTicks(-1);
            lock (_syncLock)
            {
                _cache.RemoveAll(e => e.Timestamp >= todayStart && e.Timestamp <= todayEnd);
                SaveInternal();
            }

            LogCleared?.Invoke();
        }

        /// <summary>
        /// Tra cứu theo khoảng thời gian từ ngày bắt đầu đến ngày kết thúc
        /// </summary>
        public static List<BearingLogEntry> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            EnsureLoaded();
            DateTime from = fromDate;
            DateTime to = toDate;

            // Nếu người dùng chọn ngày không có giờ, mở rộng toDate hết ngày đó
            if (to.TimeOfDay == TimeSpan.Zero)
            {
                to = to.Date.AddDays(1).AddTicks(-1);
            }

            lock (_syncLock)
            {
                return _cache
                    .Where(e => e.Timestamp >= from && e.Timestamp <= to)
                    .OrderByDescending(e => e.Timestamp)
                    .ToList();
            }
        }

        /// <summary>
        /// Xóa sạch toàn bộ lịch sử
        /// </summary>
        public static void ClearAll()
        {
            lock (_syncLock)
            {
                _cache.Clear();
                _nextId = 1;
                try
                {
                    string path = GetStoragePath();
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
                catch { }
            }

            LogCleared?.Invoke();
        }

        /// <summary>
        /// Xuất dữ liệu ra file CSV tương thích tốt với Microsoft Excel
        /// </summary>
        public static bool ExportToCsv(string filePath, IEnumerable<BearingLogEntry> entries)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine("STT,Thời gian,Loại vòng bi,Mã nhãn,Kết quả,Độ tin cậy (%),Thời gian xử lý (ms),Ghi chú");
                foreach (var e in entries)
                {
                    string resultStr = e.IsOk ? "PASS" : "FAIL";
                    string timeStr = e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss");
                    string confStr = (e.Confidence * 100.0f).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
                    string procStr = e.ProcessTimeMs.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);

                    sb.AppendLine($"\"{e.Id}\",\"{timeStr}\",\"{e.DisplayName}\",\"{e.Label}\",\"{resultStr}\",\"{confStr}\",\"{procStr}\",\"{e.Note}\"");
                }

                // Xuất file UTF8 kèm BOM để Excel tiếng Việt đọc không bị lỗi font
                File.WriteAllText(filePath, sb.ToString(), new UTF8Encoding(true));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

