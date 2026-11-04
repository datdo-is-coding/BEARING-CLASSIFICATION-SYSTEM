# DANH SÁCH NỘI DUNG CẦN ĐIỀN VÀ HOÀN THIỆN
## BÁO CÁO TỔNG KẾT ĐỀ TÀI NCKH SINH VIÊN (ĐTNH.54/2026)
> **Tên đề tài:** Nghiên cứu, thiết kế hệ thống kiểm tra và phân loại vòng bi tự động sử dụng thị giác máy tính và IoT  
> **Chủ nhiệm đề tài:** Nguyễn Văn Tân (MSV: 23810410208 - Lớp: D18CNKTDK2)  
> **Thành viên tham gia:** Hắc Ngọc Lê Thành (23810410257), Nguyễn Thanh Phong (23810440091)  
> **Người hướng dẫn:** TS. Đoàn Diễm Vương  
> **Đơn vị:** Khoa Tự động hoá, Trí tuệ nhân tạo và Hệ thống thông minh – Trường Đại học Điện lực  
> **Tài liệu gốc:** `54. Nguyễn Văn Tân  23810410208.BCTK-ĐTNH.2026.docx`

---

## MỤC LỤC THEO DÕI
1. [Hồ sơ hành chính & Tính nhất quán của đề tài](#1-hồ-sơ-hành-chính--tính-nhất-quán-của-đề-tài)
2. [Phân hệ Cơ khí - Động học - Băng tải (Chương II & Phụ lục A)](#2-phân-hệ-cơ-khí---động-học---băng-tải)
3. [Phân hệ Phần cứng Điện tử & ESP32 (Chương II & Phụ lục C)](#3-phân-hệ-phần-cứng-điện-tử--esp32)
4. [Phân hệ AI & Thị giác máy tính (Chương II, Chương III & Phụ lục D)](#4-phân-hệ-ai--thị-giác-máy-tính)
5. [Phân hệ Phần mềm PC & Web Dashboard (Chương II & Phụ lục B)](#5-phân-hệ-phần-mềm-pc--web-dashboard)
6. [Số liệu Thực nghiệm & Đánh giá Vận hành (Chương III & Phụ lục E)](#6-số-liệu-thực-nghiệm--đánh-giá-vận-hành)
7. [Chuẩn hóa Định dạng, Hình ảnh, Bảng biểu & Tài liệu tham khảo](#7-chuẩn-hóa-định-dạng-hình-ảnh-bảng-biểu--tài-liệu-tham-khảo)

---

## 1. HỒ SƠ HÀNH CHÍNH & TÍNH NHẤT QUÁN CỦA ĐỀ TÀI

- [ ] **Bản giải trình tiếp thu ý kiến Hội đồng / GVHD:**
  - Báo cáo đã ghi nhận ý kiến đóng góp từ Hội đồng đánh giá và GVHD TS. Đoàn Diễm Vương. Cần rà soát lại các mục chỉnh sửa theo đúng biên bản góp ý của hội đồng xét duyệt thuyết minh/nghiệm thu.
- [ ] **Thống nhất giải pháp phần cứng (PC + ESP32 thay cho PLC):**
  - Trong thuyết minh ban đầu từng đề cập đến PLC, nhưng hệ thống thực tế chuyển sang mô hình **PC xử lý trung tâm + vi điều khiển ESP32 điều khiển cơ cấu**.
  - Cần rà soát toàn bộ báo cáo để đảm bảo **không còn chỗ nào sót lại từ khóa "PLC"**, mô tả nhất quán kiến trúc PC + ESP32.
- [ ] **Chữ ký & xác nhận:**
  - Hoàn thiện trang chữ ký của Chủ nhiệm đề tài, các thành viên, Người hướng dẫn và xác nhận của Khoa.

---

## 2. PHÂN HỆ CƠ KHÍ - ĐỘNG HỌC - BĂNG TẢI

### 2.1. Đo đạc và thông số thực tế của Băng tải (Mục 2.4.1)
*Báo cáo ghi rõ: "Báo cáo chưa có số đo ma sát, công suất động cơ hoặc hệ số dự trữ tải, do đó các phép tính dưới đây được trình bày dưới dạng quy trình thiết kế để nhóm điền thông số đo đạc của phần cứng thực tế".*
- [ ] **Thông số động cơ kéo băng tải:** Điền công suất danh định $P$ (W), điện áp 24V, tốc độ quay, tỷ số truyền hộp giảm tốc.
- [ ] **Đo lường vận hành thực tế:**
  - Tốc độ dài thực tế của dây băng $v$ (m/s hoặc cm/s).
  - Tải trọng tối đa thử nghiệm trên mặt băng (kg hoặc số lượng vòng bi cùng lúc).
  - Kết quả kiểm tra độ lệch ngang của dây băng, độ đồng tâm của rulo và độ trượt khi có tải.

### 2.2. Động học và vùng làm việc của Robot 4 bậc tự do (Mục 2.4.3 & Phụ lục A)
- [ ] **Kích thước hình học thực tế các khâu:**
  - Điền giá trị thực tế sau khi gia công/in 3D vào hệ phương trình động học: $L_1$ (chiều cao đế), $L_2$ (khâu 1), $L_3$ (khâu 2), $L_4$ (khâu gắn đầu hút nam châm).
- [ ] **Giới hạn góc quay và vùng làm việc (Workspace):**
  - Bảng giới hạn góc quay an toàn của từng khớp: $\theta_1, \theta_2, \theta_3, \theta_4$ (độ rộng xung PWM tương ứng từ 500µs – 2500µs).
  - Xác nhận vùng làm việc thực tế có bao phủ được vị trí phôi dừng trên băng tải và toàn bộ các khay chứa sản phẩm phân loại hay không.
- [ ] **Khảo sát lực giữ của Nam châm điện:**
  - Nam châm điện 24V/12V: Đo lực hút giữ thực tế với mẫu vòng bi có bề mặt cong/hẹp, có bám dầu mỡ.
  - Kiểm tra thời gian nhả vòng bi (thời gian xả điện tích từ trường) để đảm bảo không bị rơi vãi hoặc kẹt phôi khi ngắt điện.

### 2.3. Bảng tọa độ các khay phân loại
- [ ] Lập bảng đối chiếu tọa độ $(x, y, z)$ và góc các khớp tương ứng cho các vị trí:
  1. Vị trí gắp phôi tại cảm biến quang trên băng tải.
  2. Khay 1: Đạt chuẩn (**OK**).
  3. Khay 2: Lỗi rỉ sét (**NG_RI_SET**).
  4. Khay 3: Lỗi mất nắp/vỡ nắp (**NG_MAT_NAP** / damaged_shield).
  5. Khay 4: Lỗi khuyết tật bề mặt/thiếu bi (**NG_HU_HAI_BE_MAT** / surface_damage).

### 2.4. Bản vẽ kỹ thuật trong Phụ lục A
*Hiện tại các dòng sau đang để trống tiêu đề:*
- [ ] **Robot bậc 4:** Chèn hình render tổng thể mô hình 3D (SolidWorks) có ghi kích thước và hệ trục tọa độ khớp.
- [ ] **Chi tiết bản vẽ các khớp:** Chèn bản vẽ chế tạo 2D chi tiết các khâu, bệ đỡ, gá lắp động cơ servo.
- [ ] **Sơ đồ mạch PCB:** Chèn bản vẽ Schematic và Layout 2D/3D của bo mạch điều khiển ESP32.

---

## 3. PHÂN HỆ PHẦN CỨNG ĐIỆN TỬ & ESP32

### 3.1. Sơ đồ nguyên lý và bảng chân GPIO ESP32 (Mục 2.7.1)
- [ ] **Lập bảng đấu nối chân GPIO thực tế:**
  | Tên cổng / Thiết bị | Chân GPIO trên ESP32 | Loại tín hiệu | Mạch đệm / Cách ly |
  | :--- | :--- | :--- | :--- |
  | Cảm biến quang NPN 24V | GPIO ... | Input | Opto PC817 (kéo trở về 3.3V) |
  | Servo Khớp 1 (Đế - MG996R) | GPIO ... | Output PWM | Tín hiệu 3.3V / Nguồn XL4015 |
  | Servo Khớp 2 (MG996R) | GPIO ... | Output PWM | Tín hiệu 3.3V / Nguồn XL4015 |
  | Servo Khớp 3 (MG996R) | GPIO ... | Output PWM | Tín hiệu 3.3V / Nguồn XL4015 |
  | Servo Khớp 4 (SG90) | GPIO ... | Output PWM | Tín hiệu 3.3V / Nguồn XL4015 |
  | Đóng ngắt Băng tải 24V | GPIO ... | Output | MOSFET + Opto/Gate Driver |
  | Đóng ngắt Nam châm điện | GPIO ... | Output | MOSFET + Diode dập xung ngược |

- [ ] **Kiểm tra nguồn cấp và sụt áp:**
  - Điền kết quả đo dòng thực tế khi toàn bộ 4 servo quay và cuộn hút nam châm kích hoạt.
  - Xác nhận nguồn hạ áp XL4015 (chỉnh về 5V–6V cấp cho servo) không bị sụt áp làm reset ESP32.

### 3.2. Firmware ESP32 chính thức (Mục 2.7.3 & Phụ lục C)
*Báo cáo ghi rõ: "Chưa có mã firmware chính thức kèm hồ sơ nguồn... Cần đính kèm mã hoàn chỉnh vào Phụ lục C".*
- [ ] **Đính kèm mã nguồn Arduino / ESP-IDF hoàn chỉnh vào Phụ lục C:**
  - [ ] Bộ giải thuật động học ngược (Inverse Kinematics).
  - [ ] Máy trạng thái điều khiển robot: `STATE_IDLE` -> `STATE_WAIT_SENSOR` -> `STATE_STOP_CONVEYOR` -> `STATE_PICK` -> `STATE_MOVE_TRAY` -> `STATE_RELEASE` -> `STATE_HOME`.
  - [ ] Cơ chế giao tiếp PC: Nhận lệnh phân loại qua Serial/UART (hoặc HTTP/WiFi), trả về chuỗi phản hồi (ACK/Status) kèm mã chu kỳ `cycle_id` để PC xác nhận robot đã gắp thả xong.
  - [ ] Xử lý ngắt khẩn cấp (Emergency Stop) khi có sự cố cơ khí.

---

## 4. PHÂN HỆ AI & THỊ GIÁC MÁY TÍNH

### 4.1. Chuẩn hóa số liệu Bộ dữ liệu (Dataset Manifest) (Mục 2.5.3 & 3.2.1)
*Báo cáo phát hiện chênh lệch: Bộ phát hiện có 1.200 ảnh sinh 2.747 crop, nhưng bảng thống kê nhãn phân loại một thời điểm chỉ có 2.544 crop (chênh lệch 203 crop).*
- [ ] **Chốt số liệu Dataset chính thức:**
  - **Tập YOLOv8 (Phát hiện):** Xác nhận số ảnh và bounding box trên từng tập:
    - Train: 960 ảnh (2.211 box)
    - Validation: 120 ảnh (272 box)
    - Test: 120 ảnh (264 box)
    - Tổng: 1.200 ảnh (2.747 box)
  - **Tập ResNet-34 (Phân loại):** Giải trình rõ con số chính xác của bộ crop đưa vào huấn luyện mô hình phân loại (đã lọc bao nhiêu crop rác/mờ, phân chia train/val/test theo ảnh gốc như thế nào để tránh Data Leakage).
  - Điền số lượng mẫu chi tiết của 4 lớp vào bảng phân loại chính thức:
    - Normal (Đạt chuẩn)
    - Damaged Shield (Mất nắp / Hư nắp)
    - Rusted (Rỉ sét)
    - Surface Damage (Khuyết tật bề mặt / Nứt / Trầy xước)

### 4.2. Thống nhất ánh xạ nhãn (Class Mapping) (Phụ lục D)
- [ ] **Đảm bảo đồng nhất giữa mã code và hiển thị:**
  - Phụ lục D đang quy định:
    ```json
    {
      "detection": {"0": "bearing"},
      "classification": {
        "0": "damaged_shield",
        "1": "normal",
        "2": "rusted",
        "3": "surface_damage"
      }
    }
    ```
  - Cần đối chiếu chính xác thứ tự index này với file `class_mapping.json` và code huấn luyện để tránh việc dự đoán nhầm lớp do lệch index.

### 4.3. Hồ sơ Huấn luyện & Checkpoint mô hình (Mục 2.5.4 & 3.2.2)
- [ ] **Lưu trữ thông tin Checkpoint:**
  - Đường dẫn/tên file trọng số: `yolov8n_bearing_best.pt` và `resnet34_bearing_best.pth`.
  - Bảng siêu tham số huấn luyện: Kích thước ảnh đầu vào (640x640 cho YOLO, 224x224 cho ResNet), Optimizer (AdamW), Learning Rate ($10^{-4}$), Batch size, Epochs (20 epoch).
- [ ] **Phân tích 6 mẫu nhận diện sai của ResNet-34 trên tập test:**
  *Tập test gồm 235 ảnh, đúng 229 ảnh (Accuracy 97.45%), sai 6 ảnh:*
  - 3 ảnh *normal* bị nhận nhầm thành *rusted*.
  - 1 ảnh *normal* bị nhận nhầm thành *surface_damage*.
  - 1 ảnh *rusted* bị nhận nhầm thành *normal*.
  - 1 ảnh *surface_damage* bị nhận nhầm thành *damaged_shield*.
  - **Cần làm:** Chèn hình ảnh cắt của 6 mẫu này vào Mục 3.2.3 và nêu nguyên nhân kỹ thuật (do góc chiếu sáng gây lóa kim loại, bóng tối tạo cảm giác rỉ sét, hoặc vết trầy xước quá nhỏ gần biên).

---

## 5. PHÂN HỆ PHẦN MỀM PC & WEB DASHBOARD

### 5.1. Mã nguồn tích hợp Camera và Suy luận AI (Phụ lục B)
- [ ] **Hoàn thiện script `main.py` trên PC:**
  - Đọc luồng video từ Camera điện thoại (IP Camera / DroidCam / RTSP / Snapshot HTTP).
  - Tích hợp pipeline: Bắt khung hình -> YOLOv8n crop ROI -> ResNet-34 phân loại -> Gửi kết quả JSON sang Web Dashboard (`POST /api/detection`).
  - Gửi lệnh điều khiển chu trình gắp sang ESP32 qua cổng COM Serial.
  - Đính kèm mã nguồn sạch đã tối ưu vào Phụ lục B.

### 5.2. Đồng bộ hóa giữa Web Dashboard và Cơ cấu cơ khí (Mục 2.6.4 & 3.4)
- [ ] **Bổ sung cơ chế xác nhận phản hồi (ACK):**
  - Hiện tại Dashboard cập nhật ngay khi AI nhận diện. Cần bổ sung trường lưu trữ trạng thái: *Đã nhận diện* vs *Đã gắp vào khay thành công*.
  - Bảng thống kê số lượng trên Web chỉ tăng khi ESP32 trả về tín hiệu đã gắp thả xong.
- [ ] **Chụp ảnh thực tế giao diện:**
  - Chèn ảnh chụp màn hình Web Dashboard khi đang kết nối trực tiếp với hệ thống (hiển thị trạng thái Online của AI, Camera, ESP32, biểu đồ tròn thống kê 4 lớp, bảng nhật ký phân loại thời gian thực).

---

## 6. SỐ LIỆU THỰC NGHIỆM & ĐÁNH GIÁ VẬN HÀNH

### 6.1. Bảng số liệu thử nghiệm vận hành chi tiết (Phụ lục E)
*Báo cáo hiện chỉ đưa ra các con số tổng hợp: gắp thả ~95%, phân loại đúng >90%, FPS >15, chu kỳ 3–7s. Báo cáo ghi chú: "Bảng lượt thử chi tiết và cấu hình PC cần được lưu kèm hồ sơ nghiệm thu để tăng khả năng tái lập".*
- [ ] **Lập Bảng nhật ký thực nghiệm (tối thiểu 50 – 100 lần chạy thật) vào Phụ lục E:**
  | Lần thử | Loại mẫu thử | Nhãn thực tế | Kết quả AI nhận diện | Độ tin cậy (Confidence) | Kết quả gắp cơ khí | Thời gian chu trình (s) | Ghi chú / Nguyên nhân lỗi |
  | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :--- |
  | 1 | Vòng bi 6204 | Normal | Normal | 98.2% | Thành công | 4.2s | Hoạt động chuẩn xác |
  | 2 | Vòng bi 6204 | Rusted | Rusted | 96.5% | Thành công | 4.5s | Đã thả vào khay 2 |
  | ... | ... | ... | ... | ... | ... | ... | ... |
  | 15 | Vòng bi 6204 | Damaged | Normal | 62.1% | Thất bại | 6.8s | Lóa đèn, trượt giác hút |

- [ ] **Cấu hình máy tính thử nghiệm (Testbed Specifications):**
  - Điền cấu hình PC thực tế đã dùng đo FPS: CPU (Intel/AMD), GPU (NVIDIA RTX... hay CPU inference), Dung lượng RAM, Hệ điều hành, Phiên bản Python/PyTorch/CUDA.

### 6.2. Ảnh chụp mô hình hệ thống sau khi lắp ráp hoàn thiện (Mục 3.1)
- [ ] Thay thế các hình CAD bằng **ảnh chụp thực tế chất lượng cao**:
  - Ảnh chụp toàn cảnh mô hình hệ thống hoàn chỉnh (Băng tải + Cảm biến quang + Điện thoại/Camera + Tay máy 4-DOF + Hộp điều khiển/PCB + Các khay chứa thành phẩm).
  - Ảnh chụp cận cảnh cụm tay máy bốn bậc tự do và đầu gắp nam châm điện.
  - Ảnh chụp cận cảnh bo mạch điều khiển ESP32 và nguồn cấp.

---

## 7. CHUẨN HÓA ĐỊNH DẠNG, HÌNH ẢNH, BẢNG BIỂU & TÀI LIỆU THAM KHẢO

- [ ] **Cập nhật lại số trang tự động:**
  - Cập nhật trường Table of Contents (TOC), Danh mục hình ảnh và Danh mục bảng biểu để số trang khớp tuyệt đối với văn bản sau khi chèn thêm hình ảnh, bảng biểu và phụ lục.
- [ ] **Kiểm tra chú thích Hình và Bảng:**
  - Đảm bảo toàn bộ các hình (từ Hình 2.1 đến Hình 3.5) và các bảng (từ Bảng 2.1 đến Bảng 3.3) đều có đầy đủ chú thích rõ ràng, đúng quy chuẩn báo cáo khoa học.
- [ ] **Rà soát danh mục Tài liệu tham khảo:**
  - Đối chiếu 24 tài liệu tham khảo ở cuối sách với các vị trí trích dẫn dạng `[1]`, `[2]`,... trong nội dung các chương.
