from pathlib import Path
import random
import cv2

# Thư mục gốc của dự án
PROJECT_DIR = Path(__file__).resolve().parents[1]
DATASET_DIR = PROJECT_DIR / "data" / "yolo_dataset"

SPLIT = "train"  # Có thể đổi thành "valid" hoặc "test"
NUM_IMAGES = 20

IMAGE_DIR = DATASET_DIR / SPLIT / "images"
LABEL_DIR = DATASET_DIR / SPLIT / "labels"

image_exts = {".jpg", ".jpeg", ".png", ".bmp", ".webp"}
images = [
    p for p in IMAGE_DIR.iterdir()
    if p.suffix.lower() in image_exts
]

random.seed(42)
selected = random.sample(images, min(NUM_IMAGES, len(images)))

print(f"So anh kiem tra: {len(selected)}")

for image_path in selected:
    label_path = LABEL_DIR / f"{image_path.stem}.txt"

    image = cv2.imread(str(image_path))
    if image is None:
        print(f"[LOI] Khong doc duoc anh: {image_path}")
        continue

    h, w = image.shape[:2]

    if label_path.exists():
        with open(label_path, "r", encoding="utf-8") as f:
            for line in f:
                values = line.strip().split()
                if len(values) != 5:
                    continue

                cls, xc, yc, bw, bh = map(float, values)

                # Chuyen toa do YOLO ve pixel
                x1 = int((xc - bw / 2) * w)
                y1 = int((yc - bh / 2) * h)
                x2 = int((xc + bw / 2) * w)
                y2 = int((yc + bh / 2) * h)

                x1 = max(0, min(w - 1, x1))
                y1 = max(0, min(h - 1, y1))
                x2 = max(0, min(w - 1, x2))
                y2 = max(0, min(h - 1, y2))

                cv2.rectangle(
                    image, (x1, y1), (x2, y2),
                    (0, 255, 0), 2
                )
                cv2.putText(
                    image, "bearing",
                    (x1, max(y1 - 8, 20)),
                    cv2.FONT_HERSHEY_SIMPLEX,
                    0.6, (0, 255, 0), 2
                )

    cv2.imshow("YOLO Label Inspection", image)
    print(f"Dang xem: {image_path.name}")
    key = cv2.waitKey(0)

    # ESC: thoat; phim bat ky: xem anh tiep theo
    if key == 27:
        break

cv2.destroyAllWindows()