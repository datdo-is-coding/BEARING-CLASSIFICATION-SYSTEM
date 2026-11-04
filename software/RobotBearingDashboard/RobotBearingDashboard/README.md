# Bearing AI Dashboard — Website giám sát phân loại vòng bi

## 1. Chạy trên Windows (PowerShell trong VS Code)

```powershell
cd "đường_dẫn_đến_thư_mục_RobotBearingDashboard"
py -m venv .venv
.\.venv\Scripts\python.exe -m pip install -r requirements.txt
.\.venv\Scripts\python.exe app.py
```

Mở http://127.0.0.1:5000 trên trình duyệt. Chọn **Mô phỏng** và nhấn **Bắt đầu** để kiểm tra dashboard. Chế độ mô phỏng tự tạo kết quả mỗi 2.5 giây, không dùng camera thật hoặc AI thật.

## 2. Kết nối AI thực tế

1. Chạy web, chọn **Thực tế**, nhấn **Bắt đầu**.
2. Sau khi chương trình AI phân loại một vòng bi, gửi HTTP POST JSON đến `http://127.0.0.1:5000/api/detection`.

```python
import requests
requests.post('http://127.0.0.1:5000/api/detection', json={
    'category': 'NG_RI_SET', 'confidence': 0.967, 'source': 'ai'
}, timeout=5)
```

Mã hợp lệ: `OK`, `NG_RI_SET`, `NG_NUT_VO`, `NG_THIEU_BI`. Độ tin cậy `confidence` là số từ 0 đến 1. File `send_ai_example.py` minh họa. **Chưa có mã AI `main.py`, nên chưa thể tự động nối mô hình nhận diện cụ thể.**

API trạng thái: `GET /api/status`; API điều khiển: `POST /api/control` với `{"action":"start"}` / `stop` / `demo` / `real` / `reset`; heartbeat: `POST /api/heartbeat` với `{"ai_online":true,"camera_online":true,"esp_online":true}`.

## 3. Camera

Đặt biến môi trường `CAMERA_SNAPSHOT_URL` trỏ đến endpoint trả về ảnh JPEG/PNG từ camera hoặc AI. Ví dụ trong PowerShell, trước khi chạy app:

```powershell
$env:CAMERA_SNAPSHOT_URL="http://127.0.0.1:8000/snapshot.jpg"
```

Giao diện sẽ cập nhật ảnh mỗi 2.5 giây khi ở chế độ thực tế và có heartbeat `camera_online=true`. Không hỗ trợ MJPEG/WebRTC mặc định; cần tích hợp riêng nếu hệ thống camera xuất video liên tục.

## 4. ESP32

Đặt `ESP32_URL` tới máy chủ HTTP của ESP32, ví dụ `$env:ESP32_URL="http://192.168.1.50"`. Web sẽ POST JSON tới `<ESP32_URL>/command` với `{"action":"start"}`, `{"action":"stop"}` hoặc `{"action":"sort","category":"OK"}`. **Firmware ESP32 phải tự triển khai endpoint này**; chưa có phần cứng hoặc giao thức chính thức nên hiện chỉ là hợp đồng API đề xuất.

## 5. Bảo mật và giới hạn

- Chỉ phục vụ trên localhost `127.0.0.1`; không mở trực tiếp ra Internet.
- Có thể đặt `BEARING_API_KEY` trong môi trường, khi đó các lệnh POST phải có header `X-API-Key`. **Chế độ mặc định không có xác thực**, chỉ phù hợp thử nghiệm nội bộ.
- SQLite `bearing.db` tự tạo và lưu lịch sử, nút Reset xóa toàn bộ lịch sử.
- Trạng thái thiết bị hiện được cập nhật bởi heartbeat, chưa có cơ chế timeout; không coi nhãn online là bằng chứng kết nối thực nếu chưa có heartbeat từ thiết bị.
- Chạy bằng `python app.py`, không dùng `flask run`, để bật luồng tạo dữ liệu demo.
- Điều khiển động cơ cần bổ sung xác nhận an toàn, liên động, xử lý mất kết nối và dừng khẩn cấp ở firmware/phần cứng trước khi dùng với robot thật.
