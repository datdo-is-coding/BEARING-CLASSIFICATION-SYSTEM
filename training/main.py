
from pathlib import Path
import json
import time

import cv2
import numpy as np
import torch
import torch.nn as nn
from PIL import Image
from torchvision import models, transforms
from ultralytics import YOLO


# ==========================================
# 1. XAC DINH DUONG DAN DU AN
# ==========================================
PROJECT_DIR = Path(__file__).resolve().parent

YOLO_EXPERIMENT_DIR = PROJECT_DIR / "experiments" / "yolo"
RESNET_WEIGHTS = (
    PROJECT_DIR / "weights" / "resnet34" / "best_resnet34.pth"
)
CLASS_MAPPING_FILE = PROJECT_DIR / "models" / "class_mapping.json"

DEFAULT_CLASSES = [
    "damaged_shield",
    "normal",
    "rusted",
    "surface_damage",
]

# ==========================================
# 2. CAU HINH
# ==========================================
CAMERA_INDICES = [0, 1]

YOLO_CONF = 0.35
YOLO_IOU = 0.45
YOLO_IMGSZ = 640

RESNET_INPUT_SIZE = 224

MIN_CROP_SIZE = 16

DEVICE = torch.device(
    "cuda" if torch.cuda.is_available() else "cpu"
)

# Chuan hoa anh theo ImageNet, dung voi ResNet da huan luyen
RESNET_TRANSFORM = transforms.Compose([
    transforms.Resize((RESNET_INPUT_SIZE, RESNET_INPUT_SIZE)),
    transforms.ToTensor(),
    transforms.Normalize(
        mean=[0.485, 0.456, 0.406],
        std=[0.229, 0.224, 0.225],
    ),
])


# ==========================================
# 3. TIM YOLO BEST.PT
# ==========================================
def find_yolo_weights():
    candidates = list(
        YOLO_EXPERIMENT_DIR.glob("*/weights/best.pt")
    )

    if not candidates:
        raise FileNotFoundError(
            "Khong tim thay YOLO best.pt trong:\n"
            f"{YOLO_EXPERIMENT_DIR}\n"
            "Hay huan luyen YOLO truoc."
        )

    # Lay checkpoint duoc cap nhat gan nhat
    return max(candidates, key=lambda path: path.stat().st_mtime)


# ==========================================
# 4. DOC TEN CAC LOP RESNET
# ==========================================
def load_class_names():
    if not CLASS_MAPPING_FILE.is_file():
        print(
            "[CANH BAO] Khong tim thay class_mapping.json. "
            "Su dung thu tu lop mac dinh."
        )
        return DEFAULT_CLASSES

    with open(CLASS_MAPPING_FILE, "r", encoding="utf-8") as file:
        mapping = json.load(file)

    if not isinstance(mapping, dict):
        raise ValueError(
            "class_mapping.json phai la dictionary."
        )

    if "classification" not in mapping:
        raise ValueError(
            "Khong tim thay muc 'classification' trong class_mapping.json."
        )

    classification_mapping = mapping["classification"]

    if not isinstance(classification_mapping, dict):
        raise ValueError(
            "Muc 'classification' phai la dictionary."
        )

    try:
        sorted_items = sorted(
            classification_mapping.items(),
            key=lambda item: int(item[0]),
        )
    except (ValueError, TypeError):
        raise ValueError(
            "Cac key trong classification phai la chi so lop dang so."
        )

    indices = [int(index) for index, _ in sorted_items]

    if indices != list(range(len(sorted_items))):
        raise ValueError(
            "Chi so lop classification phai lien tuc tu 0."
        )

    names = [name for _, name in sorted_items]

    if len(names) != 4:
        raise ValueError(
            f"Can 4 lop ResNet, nhung mapping co {len(names)} lop."
        )

    print(f"[OK] Classification classes: {names}")
    return names


# ==========================================
# 5. KHOI TAO RESNET-34
# ==========================================
def load_resnet():
    if not RESNET_WEIGHTS.is_file():
        raise FileNotFoundError(
            f"Khong tim thay ResNet checkpoint:\n{RESNET_WEIGHTS}"
        )

    # Khoi tao dung kien truc ResNet-34 voi 4 lop
    model = models.resnet34(weights=None)
    model.fc = nn.Sequential(
        nn.Dropout(p=0.3),
        nn.Linear(model.fc.in_features, 4),
    )

    checkpoint = torch.load(
        RESNET_WEIGHTS,
        map_location="cpu",
        weights_only=True,
    )

    if isinstance(checkpoint, dict):
        if "model_state_dict" in checkpoint:
            state_dict = checkpoint["model_state_dict"]
        elif "state_dict" in checkpoint:
            state_dict = checkpoint["state_dict"]
        elif "model" in checkpoint and isinstance(
            checkpoint["model"], dict
        ):
            state_dict = checkpoint["model"]
        else:
            state_dict = checkpoint
    else:
        raise TypeError(
            "Checkpoint ResNet khong co dinh dang state_dict hop le."
        )

    # Bo tien to module. neu checkpoint duoc luu tu DataParallel
    state_dict = {
        key.removeprefix("module."): value
        for key, value in state_dict.items()
    }

    model.load_state_dict(state_dict, strict=True)
    model.to(DEVICE)
    model.eval()

    print(f"[OK] ResNet-34: {RESNET_WEIGHTS}")
    return model


# ==========================================
# 6. PHAN LOAI ANH BANG RESNET
# ==========================================
@torch.inference_mode()
def classify_bearing(crop, model, class_names):
    if crop is None or crop.size == 0:
        return None, 0.0

    # OpenCV dung BGR, ResNet duoc huan luyen voi RGB
    rgb = cv2.cvtColor(crop, cv2.COLOR_BGR2RGB)
    image = Image.fromarray(rgb)

    tensor = RESNET_TRANSFORM(image).unsqueeze(0).to(DEVICE)

    logits = model(tensor)
    probabilities = torch.softmax(logits, dim=1)[0]

    confidence, predicted_index = torch.max(probabilities, dim=0)

    index = int(predicted_index.item())

    if index >= len(class_names):
        return None, 0.0

    return class_names[index], float(confidence.item())


# ==========================================
# 7. MO CAMERA MAY TINH
# ==========================================
def open_camera():
    for camera_index in CAMERA_INDICES:
        camera = cv2.VideoCapture(camera_index, cv2.CAP_DSHOW)

        if not camera.isOpened():
            camera.release()
            continue

        # Yeu cau kich thuoc khung hinh
        camera.set(cv2.CAP_PROP_FRAME_WIDTH, 640)
        camera.set(cv2.CAP_PROP_FRAME_HEIGHT, 480)
        camera.set(cv2.CAP_PROP_BUFFERSIZE, 1)

        # Doc thu mot frame
        success, frame = camera.read()

        if success and frame is not None and frame.size > 0:
            print(f"[OK] Camera index: {camera_index}")
            print(
                f"[OK] Do phan giai thuc te: "
                f"{frame.shape[1]}x{frame.shape[0]}"
            )
            return camera

        camera.release()

    raise RuntimeError(
        "Khong mo duoc camera. Hay kiem tra quyen truy cap camera "
        "va dong cac ung dung dang su dung camera."
    )


# ==========================================
# 8. HAM CHINH
# ==========================================
def main():
    print("=" * 60)
    print("NHAN DIEN VONG BI - YOLOv8n + RESNET-34")
    print("=" * 60)
    print(f"Device: {DEVICE}")

    if DEVICE.type == "cuda":
        print(f"GPU: {torch.cuda.get_device_name(0)}")

    # Tai YOLO
    yolo_weights = find_yolo_weights()
    detector = YOLO(str(yolo_weights))

    print(f"[OK] YOLO: {yolo_weights}")

    # Tai ResNet va ten lop
    class_names = load_class_names()
    classifier = load_resnet()

    print(f"[OK] Cac lop: {class_names}")

    # Mo camera
    camera = open_camera()

    previous_time = time.perf_counter()
    fps = 0.0

    try:
        while True:
            success, frame = camera.read()

            if not success or frame is None:
                print("[CANH BAO] Khong doc duoc frame tu camera.")
                break

            # Phat hien vong bi bang YOLO
            results = detector.predict(
                source=frame,
                conf=YOLO_CONF,
                iou=YOLO_IOU,
                imgsz=YOLO_IMGSZ,
                device=0 if DEVICE.type == "cuda" else "cpu",
                verbose=False,
            )

            result = results[0]

            if result.boxes is not None:
                for box in result.boxes:
                    x1, y1, x2, y2 = (
                        box.xyxy[0].detach().cpu().numpy().astype(int)
                    )

                    detection_conf = float(
                        box.conf[0].detach().cpu().item()
                    )

                    # Gioi han toa do trong khung hinh
                    height, width = frame.shape[:2]
                    x1 = max(0, min(x1, width - 1))
                    x2 = max(0, min(x2, width))
                    y1 = max(0, min(y1, height - 1))
                    y2 = max(0, min(y2, height))

                    if (
                        x2 - x1 < MIN_CROP_SIZE
                        or y2 - y1 < MIN_CROP_SIZE
                    ):
                        continue

                    # Cat vung vong bi de dua vao ResNet
                    crop = frame[y1:y2, x1:x2]

                    class_name, class_conf = classify_bearing(
                        crop,
                        classifier,
                        class_names,
                    )

                    if class_name is None:
                        continue

                    label = (
                        f"{class_name} "
                        f"{class_conf * 100:.1f}%"
                    )

                    # Ve khung phat hien
                    cv2.rectangle(
                        frame,
                        (x1, y1),
                        (x2, y2),
                        (0, 255, 0),
                        2,
                    )

                    # Hien thi ket qua ResNet va confidence YOLO
                    cv2.putText(
                        frame,
                        label,
                        (x1, max(25, y1 - 10)),
                        cv2.FONT_HERSHEY_SIMPLEX,
                        0.6,
                        (0, 255, 0),
                        2,
                        cv2.LINE_AA,
                    )

                    cv2.putText(
                        frame,
                        f"YOLO: {detection_conf * 100:.1f}%",
                        (x1, min(height - 8, y2 + 20)),
                        cv2.FONT_HERSHEY_SIMPLEX,
                        0.5,
                        (255, 200, 0),
                        1,
                        cv2.LINE_AA,
                    )

            # Tinh FPS hien thi
            current_time = time.perf_counter()
            elapsed = current_time - previous_time
            previous_time = current_time

            if elapsed > 0:
                instant_fps = 1.0 / elapsed
                fps = (
                    instant_fps
                    if fps == 0
                    else 0.8 * fps + 0.2 * instant_fps
                )

            cv2.putText(
                frame,
                f"FPS: {fps:.1f}",
                (10, 30),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.8,
                (0, 255, 255),
                2,
                cv2.LINE_AA,
            )

            cv2.imshow("Bearing Recognition - YOLO + ResNet", frame)

            # Nhan Q hoac ESC de thoat
            key = cv2.waitKey(1) & 0xFF
            if key == ord("q") or key == 27:
                break

    finally:
        camera.release()
        cv2.destroyAllWindows()
        print("[INFO] Da dong camera va ket thuc chuong trinh.")


if __name__ == "__main__":
    main()
