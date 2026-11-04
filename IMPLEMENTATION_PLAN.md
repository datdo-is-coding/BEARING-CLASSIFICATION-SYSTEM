# KẾ HOẠCH TRIỂN KHAI TÍCH HỢP BÁO CÁO TỔNG KẾT NCKH
## ĐỀ TÀI: NGHIÊN CỨU THIẾT KẾ HỆ THỐNG KIỂM TRA VÀ PHÂN LOẠI VÒNG BI TỰ ĐỘNG SỬ DỤNG THỊ GIÁC MÁY TÍNH VÀ IOT (MÃ SỐ: ĐTNH.54/2026)

---

## 🎯 1. MỤC TIÊU VÀ PHẠM VI
1. **Tích hợp toàn diện tài liệu mới `BaoCaoRobot4Truc.docx` vào Báo cáo chính `54. Nguyễn Văn Tân  23810410208.BCTK-ĐTNH.2026.docx`**:
   - Trích xuất và chèn toàn bộ **5 hình ảnh kỹ thuật chất lượng cao** (Sơ đồ hệ trục D-H cải tiến, Sơ đồ hình học khớp 4, Bản dựng 3D SolidWorks, Schematic mạch điều khiển ESP32, Layout PCB).
   - Tích hợp đầy đủ lý thuyết toán học: **Bảng thông số Denavit-Hartenberg (D-H) cải tiến**, ma trận biến đổi thuần nhất $T_E^0$, hệ phương trình **động học thuận** $(p_x, p_y, p_z)$, giải thuật **động học ngược giải tích** ($\theta_1, \theta_2, \theta_3$) và nghiệm hình học cho khớp $\theta_4$ đảm bảo vuông góc mặt đất.
   - Tích hợp chi tiết thiết kế phần cứng: Thiết kế in 3D nhựa cơ khí, động cơ servo MG996R và SG90, 4 khối mạch PCB (Khối nguồn hạ áp kép XL4015 + XL1509/AMS1117, Khối cách ly quang PC817, Khối Gate driver + MOSFET, Khối vi điều khiển ESP32-WROOM-32U).
2. **Cập nhật kiến trúc hệ thống mới theo quyết định của chủ nhiệm đề tài**:
   - Thống nhất mô hình phân tầng: **App C# WinForms (Edge Control Station)** giao tiếp trực tiếp phần cứng qua Serial UART $\leftrightarrow$ **Web Dashboard IoT (Flask Server)** qua mạng LAN/TCP-IP để giám sát từ xa đa thiết bị và điều khiển trung tâm.
3. **Hoàn thiện các vị trí còn trống và thiếu trong Báo cáo chính**:
   - Điền đầy đủ nội dung và hình ảnh vào các tiêu đề trống trong **Phụ lục A** (`Robot bậc 4:`, `Chi tiết bản vẽ các khớp:`, `Sơ đồ mạch PCB:`).
   - Rà soát tính nhất quán của các bảng biểu, danh mục hình ảnh và công thức toán học.

---

## 🗺️ 2. BẢNG ÁNH XẠ NỘI DUNG VÀ VỊ TRÍ CHỈNH SỬA CHI TIẾT

| STT | Vị trí trong Báo cáo chính 54 Nguyễn Văn Tân | Nội dung / Hình ảnh tích hợp từ `BaoCaoRobot4Truc` & Dự án | Mục đích & Chi tiết thay đổi |
| :---: | :--- | :--- | :--- |
| **1** | **Chương II - Mục 2.3**<br>*(Thiết kế kiến trúc tổng thể)* | Cập nhật sơ đồ khối và phân cấp hệ thống theo mô hình mới: Trạm C# WinForms kết nối Web qua LAN | Phản ánh đúng thực tế: App C# là trung tâm điều khiển phần cứng tại chỗ; Web Dashboard là giao diện giám sát & quản trị từ xa qua LAN/TCP-IP. |
| **2** | **Chương II - Mục 2.4.3.1**<br>*(Mô hình động học tay máy 4 khớp)* | **1. Chèn Hình 2.1 mới (`image1.png`)**: Sơ đồ gắn hệ trục tọa độ Denavit-Hartenberg cải tiến.<br>**2. Bổ sung Bảng D-H cải tiến** cho 4 khớp quay ($i=1..5$).<br>**3. Bổ sung ma trận chuyển đổi**: $T_1^0, T_2^1, T_3^2, T_4^3, T_E^4$ và ma trận tổng $T_E^0$.<br>**4. Bổ sung công thức động học thuận**: Biểu thức $(p_x, p_y, p_z)$ theo các góc $\theta_i$ và chiều dài khâu $L_1, L_2, L_3, L_4$. | Thay thế phần mô tả ký hiệu chung chung bằng hệ phương trình toán học giải tích chuẩn xác, tường minh từ `BaoCaoRobot4Truc`. |
| **3** | **Chương II - Mục 2.4.3.1 (tiếp)**<br>*(Động học ngược và định hướng đầu gắp)* | **1. Bổ sung giải thuật Động học ngược**: Công thức giải tích $\theta_1 = \text{atan2}(p_y, p_x)$, $c_3, s_3 \rightarrow \theta_3$, và $\theta_2 = \text{atan2}(h, r) - \text{atan2}(\dots)$ với điều kiện $|c_3| \le 1$.<br>**2. Chèn Hình 2.2 mới (`image2.png`)**: Sơ đồ hình học khớp 4.<br>**3. Bổ sung nghiệm hình học khớp 4**: Ràng buộc $(90^\circ - \theta_2) + (180^\circ - \theta_3) + (180^\circ - \theta_4) + 90^\circ = 360^\circ \Rightarrow \theta_4 = 180^\circ - \theta_2 - \theta_3$ để đầu nam châm luôn vuông góc với mặt đất, tối đa diện tích tiếp xúc với vòng bi. | Hoàn thiện lời giải động học ngược mà code firmware đang áp dụng, giải thích rõ cơ sở tính toán góc khớp cho ESP32. |
| **4** | **Chương II - Mục 2.4.3.2 & 2.4.4**<br>*(Chọn động cơ, cơ khí & CAD)* | **1. Bổ sung thuyết minh lựa chọn động cơ**: Phân tích kỹ thuật lý do chọn Servo MG996R (chịu tải lớn, bánh răng kim loại cho 3 khớp gốc) và SG90 (gọn nhẹ cho cổ tay).<br>**2. Bổ sung phương pháp chế tạo**: Gia công cơ khí nhựa in 3D FDM.<br>**3. Chèn Hình 2.3 mới (`image3.png`)**: Bản vẽ mô hình 3D SolidWorks của cánh tay robot 4 trục. | Làm rõ căn cứ thiết kế cơ khí và minh chứng mô hình 3D thực tế của nhóm. |
| **5** | **Chương II - Mục 2.7.1**<br>*(Thiết kế phần cứng và mạch PCB)* | **1. Bổ sung phân tích 4 khối chức năng chi tiết của mạch PCB**:<br>• *Khối nguồn kép*: XL4015 (5V/5A cho servo chống sụt áp) + XL1509/AMS1117 (3.3V cho logic ESP32).<br>• *Khối tín hiệu vào*: Opto PC817 cách ly chuyển 24V (cảm biến quang NPN) về 3.3V.<br>• *Khối công suất ra*: Gate driver + MOSFET mở bão hòa đóng cắt băng tải & nam châm.<br>• *Khối xử lý*: ESP32-WROOM-32U 240MHz WiFi/BLE.<br>**2. Chèn Hình 2.10 mới (`image4.png`)**: Sơ đồ nguyên lý Schematic.<br>**3. Chèn Hình 2.11 mới (`image5.png`)**: Bản vẽ Layout PCB 2 lớp. | Bổ sung đầy đủ luận cứ kỹ thuật chọn linh kiện và hình ảnh thiết kế mạch PCB thực tế của đề tài. |
| **6** | **Chương II - Mục 2.6**<br>*(Phần mềm PC & Web Dashboard)* | Cập nhật cấu trúc phần mềm kép: Phân định rõ nhiệm vụ của ứng dụng **DashBoardBearing (C# WinForms)** và ứng dụng **RobotBearingDashboard (Flask Web IoT)**, giao thức REST API kết nối LAN (`/api/detection`, `/api/heartbeat`, `/api/status`, `/api/control`). | Đồng bộ báo cáo với cấu trúc phần mềm thực tế trong mã nguồn dự án. |
| **7** | **Phụ lục A**<br>*(Bản vẽ và thiết kế cơ khí)* | Điền nội dung vào các tiêu đề hiện đang để trống:<br>• **Robot bậc 4:** Chèn hình render tổng thể 3D SolidWorks (`image3.png`) kèm kích thước các khâu $L_1, L_2, L_3, L_4$.<br>• **Chi tiết bản vẽ các khớp:** Chèn sơ đồ động học D-H và hình học khớp 4 (`image1.png`, `image2.png`).<br>• **Sơ đồ mạch PCB:** Chèn bản vẽ Schematic (`image4.png`) và Layout PCB (`image5.png`). | Xóa bỏ hoàn toàn tình trạng các mục phụ lục bị trống không có nội dung/hình ảnh. |
| **8** | **Phụ lục C**<br>*(Mã nguồn ESP32)* | Đối chiếu mã nguồn giải thuật động học trong `kinematics.hpp` / `kinematics.cpp` khớp 100% với hệ phương trình động học D-H đã trình bày ở Mục 2.4.3.1. | Đảm bảo sự nhất quán tuyệt đối giữa lý thuyết tính toán và code nhúng thực tế. |

---

## 📷 3. DANH SÁCH HÌNH ẢNH TRÍCH XUẤT TỪ `BaoCaoRobot4Truc.docx`

| Tên file trích xuất | Kích thước / Định dạng | Nội dung hình ảnh | Vị trí chèn trong Báo cáo chính |
| :--- | :---: | :--- | :--- |
| `robot_dh_axes.png` *(image1.png)* | PNG (Độ nét cao) | Sơ đồ gán hệ trục tọa độ Denavit-Hartenberg cải tiến trên robot 4-DOF | Mục 2.4.3.1 & Phụ lục A |
| `robot_joint4_geometry.png` *(image2.png)* | PNG (Độ nét cao) | Sơ đồ phân tích hình học khớp 4 vuông góc mặt đất | Mục 2.4.3.1 & Phụ lục A |
| `robot_solidworks_3d.png` *(image3.png)* | PNG (Độ nét cao) | Mô hình bản vẽ 3D SolidWorks hoàn chỉnh cánh tay robot 4 trục | Mục 2.4.4 & Phụ lục A |
| `robot_pcb_schematic.png` *(image4.png)* | PNG (Độ nét cao) | Sơ đồ nguyên lý Schematic mạch điều khiển ESP32 | Mục 2.7.1 & Phụ lục A |
| `robot_pcb_layout.png` *(image5.png)* | PNG (Độ nét cao) | Bản vẽ thiết kế Layout mạch in PCB | Mục 2.7.1 & Phụ lục A |

---

## ⚙️ 4. QUY TRÌNH THỰC HIỆN KỸ THUẬT

1. **Bước 1: Tạo bản sao lưu an toàn (Backup)**:
   - Tạo file sao lưu: `54. Nguyễn Văn Tân  23810410208.BCTK-ĐTNH.2026.BAK.docx` trước khi thực hiện bất kỳ can thiệp nào.
2. **Bước 2: Trích xuất tài nguyên hình ảnh**:
   - Dùng Python mở tệp nén `BaoCaoRobot4Truc.docx`, trích xuất 5 file ảnh trong `word/media/` và lưu tạm.
3. **Bước 3: Nhúng hình ảnh và cập nhật Relationship XML**:
   - Thêm 5 file ảnh vào thư mục `word/media/` của file báo cáo chính.
   - Thêm các thẻ `<Relationship>` tương ứng trong `word/_rels/document.xml.rels`.
4. **Bước 4: Cập nhật nội dung văn bản & công thức toán học (`word/document.xml`)**:
   - Chèn các đoạn văn, bảng D-H cải tiến và các công thức toán học động học thuận/nghịch vào Mục 2.4.3.1.
   - Chèn phần phân tích nguồn kép XL4015 + XL1509/AMS1117, cách ly quang PC817, MOSFET Gate driver vào Mục 2.7.1.
   - Cập nhật kiến trúc phân tầng C# WinForms (Edge) $\leftrightarrow$ Flask Web Dashboard (LAN) vào Mục 2.3 và 2.6.
   - Điền nội dung và chèn hình ảnh vào các đề mục đang trống trong Phụ lục A.
5. **Bước 5: Kiểm tra và xác thực file sau chỉnh sửa**:
   - Kiểm tra tính toàn vẹn của cấu trúc file `.docx` (đóng gói zip hợp lệ, XML chuẩn cú pháp).
   - Mở và kiểm tra hiển thị trên Microsoft Word để đảm bảo không bị lỗi font, lệch trang hay mất định dạng.

---

## 💬 5. ĐIỂM CẦN BẠN XÁC NHẬN / GÓP Ý TRƯỚC KHI THỰC HIỆN

1. **Chiều dài khâu thực tế**: Trong `BaoCaoRobot4Truc`, các khâu ký hiệu là $L_1, L_2, L_3, L_4$. Trong code nhúng `firmware/include/kinematics.hpp` đang định nghĩa:
   - $L_1 = 80.0\text{ mm}$ (Đế)
   - $L_2 = 105.0\text{ mm}$ (Khâu vai - khuỷu)
   - $L_3 = 98.0\text{ mm}$ (Khâu khuỷu - cổ tay)
   - $L_4 = 65.0\text{ mm}$ (Đầu hút nam châm)  
   👉 *Kế hoạch sẽ đưa trực tiếp bộ số kích thước mm này vào các phương trình tính toán trong báo cáo.*
2. **Hình ảnh trong Phụ lục A**: 3 mục `Robot bậc 4:`, `Chi tiết bản vẽ các khớp:`, `Sơ đồ mạch PCB:` sẽ được chèn trực tiếp các hình ảnh từ SolidWorks, sơ đồ D-H và Schematic/Layout PCB.
3. **Kiến trúc mạng**: Báo cáo sẽ mô tả rõ ràng App C# là Edge App kết nối ESP32 qua COM, Web là IoT Dashboard kết nối LAN.

---
*Sau khi bạn duyệt kế hoạch này, tôi sẽ tiến hành tạo bản backup và thực hiện chỉnh sửa trực tiếp vào file báo cáo Word.*
