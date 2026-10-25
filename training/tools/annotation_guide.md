# Hướng dẫn gán nhãn

## YOLO detection
- Nếu detector dùng một lớp, tên lớp là `bearing` và class ID là `0`.
- Vẽ bounding box riêng cho từng vòng bi.
- Ảnh không có vòng bi có thể có file nhãn YOLO trống.
- Không gán nhãn vật thể khác nếu detector chỉ có lớp `bearing`.
- Giữ cố định train/valid/test và kiểm tra nhãn trước khi huấn luyện.

## Phân loại tình trạng (nếu cần ResNet)
Các lớp mẫu: `normal`, `rusted`, `surface_damage`, `damaged_shield`.
Cần kiểm tra nhãn của từng crop; không tự suy ra tình trạng chỉ từ bounding box.
