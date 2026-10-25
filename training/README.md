# Hệ thống Kiểm tra và Phân loại Vòng bi Tự động (NCKH)

Dự án nghiên cứu khoa học phát triển hệ thống thị giác máy tính 2 tầng (Two-stage Vision Pipeline) kết hợp cánh tay robot 4-DOF điều khiển qua ESP32 để tự động phát hiện, định vị và phân loại chất lượng vòng bi (Đạt chuẩn **OK** hoặc Phế phẩm **NG**).

---

## 1. Kiến trúc Hệ thống Two-Stage

Quy trình nhận diện và phân loại được chia thành 2 giai đoạn chuyên biệt:

```
[ Camera Stream (640x480) ]
            │
            ▼
┌─────────────────────────────────────────────────────────────┐
│ Giai đoạn 1: YOLOv8n Object Detection                       │
│ - Xác định vị trí (Bounding Box) và tâm vòng bi (bearing)    │
│ - Ngưỡng tin cậy: Conf = 0.35, IOU = 0.45, Imgsz = 640      │
└─────────────────────────────────────────────────────────────┘
            │
            ▼ (Tọa độ ROI + Padding 5%)
┌─────────────────────────────────────────────────────────────┐
│ Tiền xử lý (Cropping & Normalization)                        │
│ - Cắt vùng vòng bi (ROI) từ toàn khung hình                 │
│ - Resize về 224x224, chuẩn hóa ImageNet (Mean, Std)         │
└─────────────────────────────────────────────────────────────┘
            │
            ▼
┌─────────────────────────────────────────────────────────────┐
│ Giai đoạn 2: ResNet-34 Defect Classification                │
│ - Phân tích bề mặt và phân loại vào 4 lớp khuyết tật        │
│ - Input: Tensor 3x224x224 -> Softmax Output Probabilities   │
└─────────────────────────────────────────────────────────────┘
            │
            ▼
┌─────────────────────────────────────────────────────────────┐
│ Quyết định Phân loại Chất lượng & Điều khiển Robot          │
│ • normal           ──> [ OK ]  (Vòng bi đạt chuẩn)          │
│ • damaged_shield   ──> [ NG ]  (Lỗi nắp chắn bụi)           │
│ • rusted           ──> [ NG ]  (Lỗi gỉ sét)                 │
│ • surface_damage   ──> [ NG ]  (Lỗi trầy xước/rỗ bề mặt)    │
└─────────────────────────────────────────────────────────────┘
            │
            ▼ (UART / Serial COM Port)
[ ESP32 Điều khiển Cánh tay Robot 4-DOF gắp phân loại vào khay ]
```

---

## 2. Kết quả Thực nghiệm & Hiệu năng Mô hình

### 2.1. Model 1: YOLOv8n (Định vị vòng bi)
- **Tập dữ liệu**: Gán nhãn YOLO định dạng Roboflow, 1 lớp duy nhất `bearing` (class ID: 0).
- **Môi trường huấn luyện**: 92 epochs trên GPU NVIDIA RTX 2050, batch size 4, kích thước ảnh 640x640.
- **Trọng số tốt nhất**: `experiments/yolo/yolo_v8n_rtx2050/weights/best.pt` (6.5 MB).
- **Chỉ số đánh giá (Validation Set)**:
  - **Precision**: 98.90%
  - **Recall**: 99.15%
  - **mAP@50**: **99.46%**
  - **mAP@50-95**: **92.75%**

### 2.2. Model 2: ResNet-34 (Phân loại chi tiết khuyết tật)
- **Tập dữ liệu**: 4,800 ảnh train (đã cân bằng 1,200 ảnh/lớp qua Data Augmentation), 245 ảnh validation, 235 ảnh test độc lập.
- **Chiến lược huấn luyện**: Two-stage fine-tuning (Head 2 epochs, Backbone Layer 4 + Head 18 epochs, AdamW + Label Smoothing 0.05 + Dropout 0.3).
- **Trọng số tốt nhất**: `weights/resnet34/best_resnet34.pth` (85.3 MB).
- **Độ chính xác tổng thể (Test Set - 235 mẫu)**: **97.45%** (Macro F1: 97.18%).

#### Chi tiết từng lớp trên tập Test:
| Nhãn khuyết tật | Phân nhóm | Precision | Recall | F1-Score | Số mẫu test | Đánh giá |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| **`damaged_shield`** | **NG** | 97.7% | **100.0%** | 98.8% | 43 | Bắt chính xác 43/43 mẫu hỏng nắp chắn |
| **`normal`** | **OK** | 97.7% | **91.3%** | 94.4% | 46 | 42/46 mẫu đúng (4 mẫu nhận diện nhầm sang lỗi) |
| **`rusted`** | **NG** | 97.3% | **99.1%** | 98.2% | 109 | 108/109 mẫu đúng (1 mẫu nhầm sang normal) |
| **`surface_damage`** | **NG** | 97.3% | **97.3%** | 97.3% | 37 | 36/37 mẫu đúng (1 mẫu nhầm sang hỏng nắp) |

---

## 3. Cấu trúc Dự án & Tài nguyên

```
NCKH_Phan_loai_vong_bi/
│
├── configs/
│   └── config.yaml               # Cấu hình ngưỡng conf, iou, đường dẫn mô hình
├── models/
│   └── class_mapping.json        # Ánh xạ nhãn: detection & classification
├── weights/
│   ├── yolov8n.pt                # YOLOv8n gốc
│   ├── yolo26n.pt                # Checkpoint phụ
│   └── resnet34/
│       ├── best_resnet34.pth     # Trọng số ResNet-34 tối ưu (Val Loss thấp nhất)
│       ├── best_resnet34_accuracy.pth
│       ├── config.json           # Thông số siêu tham số huấn luyện
│       ├── training_history.csv  # Lịch sử train/val qua các epoch
│       └── evaluation_test/      # Báo cáo ma trận nhầm lẫn & metrics tập test
│
├── experiments/
│   └── yolo/
│       ├── yolo_v8n_rtx2050/     # Toàn bộ biểu đồ F1, PR, confusion matrix của YOLO
│       │   └── weights/best.pt   # File trọng số YOLO tối ưu
│       └── yolo_v8n_validation/ # Kết quả kiểm chứng độc lập
│
├── tools/                        # Bộ công cụ tiền xử lý và gán nhãn
│   ├── annotation_guide.md       # Quy chuẩn gán nhãn dataset
│   ├── crop_from_yolo_labels.py  # Cắt ảnh vòng bi từ nhãn YOLO
│   ├── label_bearing_crops.py    # Giao diện gán nhãn 4 lớp cho ảnh crop
│   ├── prepare_resnet_dataset.py # Cân bằng dữ liệu và chia split chống rò rỉ
│   ├── inspect_data.py           # Kiểm tra lỗi cú pháp file nhãn YOLO
│   └── visualize_yolo_labels.py  # Trực quan hóa bounding box trên ảnh gốc
│
├── training/                     # Mã nguồn huấn luyện
│   ├── yolo/
│   │   ├── train_yolo.py         # Huấn luyện YOLOv8
│   │   └── val_yolo.py           # Đánh giá YOLOv8
│   └── resnet/
│       ├── dataset.py            # Custom Dataset & DataLoaders PyTorch
│       ├── train_resnet.py       # Fine-tuning 2 giai đoạn ResNet-34
│       ├── evaluate_resnet.py    # Đánh giá ma trận nhầm lẫn trên tập test
│       └── plot_training_history.py
│
├── main.py                       # Pipeline chạy trực tiếp Camera (YOLO + ResNet)
├── requirements.txt              # Thư viện Python phụ thuộc
└── README.md                     # Tài liệu hướng dẫn này
```

---

## 4. Hướng dẫn Chạy Thử nghiệm Pipeline Python

### Cài đặt môi trường
```bash
pip install -r requirements.txt
# Cài PyTorch hỗ trợ GPU CUDA:
pip install torch torchvision --index-url https://download.pytorch.org/whl/cu121
```

### Chạy hệ thống nhận diện Real-time với Camera
```bash
python main.py
```
- Phím `Q` hoặc `ESC`: Thoát chương trình.

---

## 5. Hướng dẫn Tích hợp và Đưa lên C# WinForms

Để triển khai giao diện điều khiển chuyên nghiệp trên máy tính công nghiệp phục vụ đồ án NCKH, giải pháp tối ưu nhất là chuyển đổi 2 mô hình sang chuẩn **ONNX** và chạy trực tiếp trong ứng dụng **C# Windows Forms** (.NET).

### 5.1. Kiến trúc Tích hợp C# WinForms

```
┌─────────────────────────────────────────────────────────────┐
│                      Giao diện C# WinForms                  │
│                                                             │
│  [ Video Stream (PictureBox) ]      [ Bảng Thống Kê ]       │
│  - Khung xanh: OK (Bình thường)     - Số lượng OK / NG      │
│  - Khung đỏ:   NG (Lỗi)             - Tỷ lệ phế phẩm (%)    │
│  - Tọa độ tâm (X, Y)                - FPS thực tế           │
│                                                             │
│  [ Cổng kết nối Serial ]            [ Nút Điều Khiển ]      │
│  - COM3 / 115200 bps                - Start / Stop Camera   │
│  - Gửi lệnh phân loại tới ESP32     - Reset Counter         │
└─────────────────────────────────────────────────────────────┘
            │                                   │
      (Inference Engine)                  (SerialPort)
            ▼                                   ▼
┌───────────────────────────┐         ┌───────────────────┐
│ Microsoft.ML.OnnxRuntime  │         │ ESP32 Board       │
│ • yolo_bearing.onnx       │         │ (Robot 4-DOF Arm) │
│ • resnet34_bearing.onnx   │         └───────────────────┘
│ + OpenCvSharp4            │
└───────────────────────────┘
```

### 5.2. Bước 1: Xuất Mô hình sang Định dạng ONNX
Tạo script xuất ONNX hoặc chạy lệnh sau trong Python:

1. **Xuất YOLOv8n sang ONNX**:
```python
from ultralytics import YOLO

model = YOLO("experiments/yolo/yolo_v8n_rtx2050/weights/best.pt")
model.export(format="onnx", imgsz=640, dynamic=False, opset=12)
# File xuất ra: best.onnx -> đổi tên thành yolo_bearing.onnx
```

2. **Xuất ResNet-34 sang ONNX**:
```python
import torch
from torchvision import models
import torch.nn as nn

model = models.resnet34(weights=None)
model.fc = nn.Sequential(
    nn.Dropout(p=0.3),
    nn.Linear(model.fc.in_features, 4)
)
checkpoint = torch.load("weights/resnet34/best_resnet34.pth", map_location="cpu", weights_only=True)
state_dict = {k.removeprefix("module."): v for k, v in checkpoint["model_state_dict"].items()}
model.load_state_dict(state_dict)
model.eval()

dummy_input = torch.randn(1, 3, 224, 224)
torch.onnx.export(
    model,
    dummy_input,
    "weights/resnet34/resnet34_bearing.onnx",
    input_names=["input"],
    output_names=["output"],
    opset_version=12
)
```

### 5.3. Bước 2: Cài đặt Thư viện NuGet trong Project C# WinForms
Trong Visual Studio (Quản lý NuGet Packages):
1. **`Microsoft.ML.OnnxRuntime`** (hoặc `Microsoft.ML.OnnxRuntime.Gpu` nếu máy có GPU CUDA).
2. **`OpenCvSharp4`** & **`OpenCvSharp4.Extensions`** & **`OpenCvSharp4.runtime.win`**: Bắt hình ảnh camera và tiền xử lý ma trận ảnh.

### 5.4. Bước 3: Luồng Xử lý trong C# (Code tóm tắt)
1. **Camera Capture (OpenCvSharp4)**:
   ```csharp
   VideoCapture capture = new VideoCapture(0);
   Mat frame = new Mat();
   capture.Read(frame);
   ```
2. **YOLO Detection Inference**:
   - Chuyển `frame` thành tensor `[1, 3, 640, 640]`, chuẩn hóa chia `255.0f`.
   - Run inference với `InferenceSession("yolo_bearing.onnx")`.
   - Parse kết quả lấy Bounding Box $(X_1, Y_1, X_2, Y_2)$ và tọa độ tâm $(X_c, Y_c)$.
3. **ResNet Classification Inference**:
   - Cắt vùng ROI theo Bounding Box (có cộng 5% margin).
   - Resize về `224x224`, chuẩn hóa: $(Pixel/255.0 - Mean) / Std$.
   - Run inference với `InferenceSession("resnet34_bearing.onnx")`.
   - Tính Softmax tìm lớp xác suất cao nhất:
     - Index 1 (`normal`) $\rightarrow$ **OK**
     - Index 0, 2, 3 (`damaged_shield`, `rusted`, `surface_damage`) $\rightarrow$ **NG**
4. **Hiển thị & Giao tiếp ESP32**:
   - Vẽ khung xanh (OK) hoặc đỏ (NG) lên khung hình và hiển thị lên `PictureBox`.
   - Gửi tín hiệu điều khiển robot qua `SerialPort`:
     ```csharp
     serialPort.WriteLine($"CMD:{(isOk ? "OK" : "NG")},{centerX},{centerY}\n");
     ```
   - Firmware ESP32 trong thư mục [`Robot_4_DOF_ESP32`](../Robot_4_DOF_ESP32) nhận chuỗi lệnh, điều khiển 4 khớp động cơ servo gắp vòng bi vào đúng khay phân loại.
