"""TODO: triển khai chức năng của module này."""

from pathlib import Path
import csv
import cv2


# ==================================================
# 1. CAU HINH DUONG DAN
# ==================================================
PROJECT_DIR = Path(__file__).resolve().parents[1]

DATASET_DIR = PROJECT_DIR / "data" / "yolo_dataset"
OUTPUT_DIR = PROJECT_DIR / "data" / "bearing_crops"

# Them 5% khoang dem quanh bounding box
PADDING_RATIO = 0.05

IMAGE_EXTENSIONS = {
    ".jpg", ".jpeg", ".png", ".bmp",
    ".webp", ".tif", ".tiff"
}


# ==================================================
# 2. TIM THU MUC ANH VA NHAN
# ==================================================
def find_split_dirs(split_name):
    """Tim images va labels, chap nhan valid hoac val."""

    aliases = {
        "train": ["train"],
        "valid": ["valid", "val"],
        "test": ["test"],
    }

    for folder_name in aliases[split_name]:
        images_dir = DATASET_DIR / folder_name / "images"
        labels_dir = DATASET_DIR / folder_name / "labels"

        if images_dir.is_dir() and labels_dir.is_dir():
            return folder_name, images_dir, labels_dir

    return None, None, None


# ==================================================
# 3. CAT VONG BI TU TOA DO YOLO
# ==================================================
def crop_bearing(image, values):
    """
    values = [class_id, x_center, y_center, width, height]
    Toa do YOLO duoc chuan hoa tu 0 den 1.
    """

    _, xc, yc, box_w, box_h = values

    image_h, image_w = image.shape[:2]

    # Chuyen toa do chuan hoa sang pixel
    center_x = xc * image_w
    center_y = yc * image_h
    width = box_w * image_w
    height = box_h * image_h

    # Them padding
    pad_x = width * PADDING_RATIO
    pad_y = height * PADDING_RATIO

    x1 = max(0, int(round(center_x - width / 2 - pad_x)))
    y1 = max(0, int(round(center_y - height / 2 - pad_y)))
    x2 = min(image_w, int(round(center_x + width / 2 + pad_x)))
    y2 = min(image_h, int(round(center_y + height / 2 + pad_y)))

    if x2 <= x1 or y2 <= y1:
        return None, (x1, y1, x2, y2)

    crop = image[y1:y2, x1:x2]

    return crop, (x1, y1, x2, y2)


# ==================================================
# 4. CHUONG TRINH CHINH
# ==================================================
def main():

    if not DATASET_DIR.is_dir():
        raise FileNotFoundError(
            f"Khong tim thay dataset: {DATASET_DIR}"
        )

    OUTPUT_DIR.mkdir(parents=True, exist_ok=True)

    manifest_path = OUTPUT_DIR / "crop_manifest.csv"

    manifest_rows = []
    total_images = 0
    total_crops = 0
    total_errors = 0

    # Xu ly tung split rieng biet
    for split in ["train", "valid", "test"]:

        actual_split, images_dir, labels_dir = find_split_dirs(split)

        if images_dir is None:
            print(
                f"[CANH BAO] Khong tim thay thu muc "
                f"images/labels cho {split}"
            )
            continue

        # Anh crop chua duoc gan nhan tinh trang
        output_split_dir = OUTPUT_DIR / split / "unlabeled"
        output_split_dir.mkdir(parents=True, exist_ok=True)

        image_paths = sorted(
            p for p in images_dir.iterdir()
            if p.is_file()
            and p.suffix.lower() in IMAGE_EXTENSIONS
        )

        split_count = 0

        print(f"\n[{split.upper()}] So anh: {len(image_paths)}")

        for image_path in image_paths:

            total_images += 1

            label_path = labels_dir / f"{image_path.stem}.txt"

            if not label_path.exists():
                print(f"[CANH BAO] Thieu nhan: {label_path}")
                total_errors += 1
                continue

            image = cv2.imread(str(image_path))

            if image is None:
                print(f"[CANH BAO] Khong doc duoc: {image_path}")
                total_errors += 1
                continue

            lines = label_path.read_text(
                encoding="utf-8"
            ).splitlines()

            box_index = 0

            for line_number, line in enumerate(lines, start=1):

                line = line.strip()

                # File nhan rong: anh khong co vong bi
                if not line:
                    continue

                parts = line.split()

                if len(parts) != 5:
                    print(
                        f"[CANH BAO] Sai dinh dang: "
                        f"{label_path}, dong {line_number}"
                    )
                    total_errors += 1
                    continue

                try:
                    values = [float(value) for value in parts]
                except ValueError:
                    print(
                        f"[CANH BAO] Toa do khong hop le: "
                        f"{label_path}, dong {line_number}"
                    )
                    total_errors += 1
                    continue

                class_id = values[0]
                xc, yc, bw, bh = values[1:]

                # Kiem tra nhan YOLO
                if (
                    not class_id.is_integer()
                    or class_id < 0
                    or not all(
                        0 <= value <= 1
                        for value in [xc, yc, bw, bh]
                    )
                    or bw <= 0
                    or bh <= 0
                ):
                    print(
                        f"[CANH BAO] Nhan ngoai pham vi: "
                        f"{label_path}, dong {line_number}"
                    )
                    total_errors += 1
                    continue

                crop, box = crop_bearing(image, values)

                if crop is None or crop.size == 0:
                    print(
                        f"[CANH BAO] Crop rong: "
                        f"{image_path.name}, box {line_number}"
                    )
                    total_errors += 1
                    continue

                box_index += 1

                # Ten crop gan voi ten anh nguon
                crop_name = (
                    f"{image_path.stem}_box{box_index:03d}.jpg"
                )
                crop_path = output_split_dir / crop_name

                success = cv2.imwrite(str(crop_path), crop)

                if not success:
                    print(f"[CANH BAO] Khong ghi duoc: {crop_path}")
                    total_errors += 1
                    continue

                x1, y1, x2, y2 = box

                manifest_rows.append({
                    "split": split,
                    "source_split_folder": actual_split,
                    "source_image": str(
                        image_path.relative_to(PROJECT_DIR)
                    ),
                    "source_label": str(
                        label_path.relative_to(PROJECT_DIR)
                    ),
                    "crop_path": str(
                        crop_path.relative_to(PROJECT_DIR)
                    ),
                    "crop_filename": crop_name,
                    "class_id_detection": int(class_id),
                    "box_index": box_index,
                    "x1_px": x1,
                    "y1_px": y1,
                    "x2_px": x2,
                    "y2_px": y2,
                    "crop_width": crop.shape[1],
                    "crop_height": crop.shape[0],
                    "defect_label": "",
                })

                split_count += 1
                total_crops += 1

        print(f"[OK] Da tao {split_count} anh crop")
        print(f"     Luu tai: {output_split_dir}")

    # ==================================================
    # 5. LUU FILE CSV DE TRUY XUAT NGUON
    # ==================================================
    columns = [
        "split",
        "source_split_folder",
        "source_image",
        "source_label",
        "crop_path",
        "crop_filename",
        "class_id_detection",
        "box_index",
        "x1_px",
        "y1_px",
        "x2_px",
        "y2_px",
        "crop_width",
        "crop_height",
        "defect_label",
    ]

    with open(
        manifest_path,
        "w",
        newline="",
        encoding="utf-8-sig"
    ) as file:

        writer = csv.DictWriter(file, fieldnames=columns)
        writer.writeheader()
        writer.writerows(manifest_rows)

    # ==================================================
    # 6. TONG KET
    # ==================================================
    print("\n" + "=" * 60)
    print("HOAN TAT CAT ANH VONG BI")
    print("=" * 60)

    print(f"Tong anh nguon : {total_images}")
    print(f"Tong anh crop  : {total_crops}")
    print(f"So loi         : {total_errors}")
    print(f"Thu muc output : {OUTPUT_DIR}")
    print(f"File manifest  : {manifest_path}")

    print("\nLuu y:")
    print("- Anh crop chua co nhan tinh trang.")
    print("- Khong tu dong gan nhan loi.")
    print("- Giu nguyen split train/valid/test.")
    print("- Kiem tra crop truoc khi gan nhan ResNet.")


if __name__ == "__main__":
    main()
