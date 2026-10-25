"""TODO: triển khai chức năng của module này."""
"""Kiem tra dataset YOLO xuat tu Roboflow.
Chay tu thu muc goc: python tools/inspect_data.py
Tuy chon: python tools/inspect_data.py --dataset "D:/dataset"
"""
import argparse
import sys
from pathlib import Path

IMAGE_EXTS = {".jpg", ".jpeg", ".png", ".bmp", ".webp", ".tif", ".tiff"}
SPLITS = ("train", "valid", "val", "test")


def read_yaml(path):
    try:
        import yaml
    except ImportError:
        raise RuntimeError("Thieu PyYAML. Chay: python -m pip install PyYAML")
    with path.open("r", encoding="utf-8-sig") as f:
        data = yaml.safe_load(f) or {}
    if not isinstance(data, dict):
        raise ValueError("data.yaml khong co cau truc YAML mapping hop le.")
    return data


def get_names(data):
    names = data.get("names", [])
    if isinstance(names, list):
        return [str(x) for x in names]
    if isinstance(names, dict):
        try:
            return [str(names[k]) for k in sorted(names, key=lambda x: int(x))]
        except (ValueError, TypeError):
            return [str(v) for v in names.values()]
    return []


def check_label(path, class_count):
    errors, valid = [], 0
    try:
        lines = path.read_text(encoding="utf-8-sig").splitlines()
    except OSError as exc:
        return 0, [f"Khong doc duoc file: {exc}"]

    for line_no, raw in enumerate(lines, 1):
        line = raw.strip()
        if not line:
            continue
        parts = line.split()
        if len(parts) != 5:
            errors.append(f"{path.name}:{line_no}: can 5 gia tri YOLO, nhan {len(parts)}")
            continue
        try:
            cls_float = float(parts[0])
            coords = [float(v) for v in parts[1:]]
        except ValueError:
            errors.append(f"{path.name}:{line_no}: co gia tri khong phai so")
            continue
        if not cls_float.is_integer() or cls_float < 0:
            errors.append(f"{path.name}:{line_no}: class_id phai la so nguyen >= 0")
            continue
        if class_count and int(cls_float) >= class_count:
            errors.append(f"{path.name}:{line_no}: class_id vuot so lop trong data.yaml")
        if not all(0 <= x <= 1 for x in coords):
            errors.append(f"{path.name}:{line_no}: toa do/kich thuoc phai trong [0, 1]")
            continue
        x, y, w, h = coords
        if w <= 0 or h <= 0:
            errors.append(f"{path.name}:{line_no}: width va height phai > 0")
            continue
        valid += 1
    return valid, errors


def main():
    parser = argparse.ArgumentParser(description="Kiem tra dataset YOLO.")
    parser.add_argument("--dataset", default="data/yolo_dataset",
                        help="Thu muc dataset (mac dinh data/yolo_dataset)")
    parser.add_argument("--yaml", default=None, help="Duong dan data.yaml neu can chi dinh")
    args = parser.parse_args()

    dataset = Path(args.dataset).expanduser().resolve()
    print("=" * 65)
    print("KIEM TRA DATASET YOLO - PHAN LOAI VONG BI")
    print(f"Dataset: {dataset}")
    print("=" * 65)

    if not dataset.is_dir():
        print("[LOI] Khong tim thay thu muc dataset.")
        return 2

    yaml_path = Path(args.yaml).expanduser().resolve() if args.yaml else dataset / "data.yaml"
    if not yaml_path.is_file() and not args.yaml:
        candidates = list(dataset.glob("**/data.yaml"))
        if len(candidates) == 1:
            yaml_path = candidates[0]

    names = []
    if yaml_path.is_file():
        print(f"[YAML] {yaml_path}")
        try:
            data = read_yaml(yaml_path)
            names = get_names(data)
            print(f"[YAML] nc: {data.get('nc', '(khong khai bao)')}")
            print(f"[YAML] names: {names if names else '(thieu names)'}")
            if data.get("nc") is not None and names and data["nc"] != len(names):
                print("[LOI] nc khong khop so luong names.")
            if names == ["bearing"]:
                print("[OK] Dataset co mot lop bearing.")
            elif names:
                print("[CANH BAO] Names khong phai duy nhat lop 'bearing'; hay xac nhan truoc khi train.")
        except (RuntimeError, ValueError, OSError) as exc:
            print(f"[LOI] Khong doc duoc data.yaml: {exc}")
            return 2
    else:
        print("[CANH BAO] Khong tim thay data.yaml. Hay kiem tra ban export Roboflow.")

    total_images = total_boxes = total_missing = total_errors = 0
    found_split = False

    for split in SPLITS:
        split_dir = dataset / split
        images_dir, labels_dir = split_dir / "images", split_dir / "labels"
        if not split_dir.is_dir() or not (images_dir.is_dir() or labels_dir.is_dir()):
            continue
        found_split = True
        images = sorted(p for p in images_dir.rglob("*")
                        if p.is_file() and p.suffix.lower() in IMAGE_EXTS) if images_dir.is_dir() else []
        labels = sorted(labels_dir.rglob("*.txt")) if labels_dir.is_dir() else []
        label_set = set(labels)
        boxes = missing = 0
        errors = []

        for img in images:
            rel = img.relative_to(images_dir)
            label = (labels_dir / rel).with_suffix(".txt")
            if label not in label_set or not label.is_file():
                missing += 1
                continue
            count, problems = check_label(label, len(names) if names else None)
            boxes += count
            errors.extend((label, problem) for problem in problems)

        print(f"\n[{split.upper()}]")
        print(f"  Anh: {len(images)}")
        print(f"  File nhan TXT: {len(labels)}")
        print(f"  Bounding box hop le: {boxes}")
        print(f"  Anh thieu file nhan: {missing}")
        print(f"  Loi dinh dang nhan: {len(errors)}")

        if not images:
            print("  [CANH BAO] Khong tim thay anh trong images.")
        if missing:
            print("  [CANH BAO] Anh thieu file nhan. Anh negative nen co file .txt trong tuong ung, de trong.")
            for img in [p for p in images if not ((labels_dir / p.relative_to(images_dir)).with_suffix(".txt")).is_file()][:5]:
                print(f"    - {img.relative_to(dataset)}")
        for label, problem in errors[:10]:
            try:
                label_display = label.relative_to(dataset)
            except ValueError:
                label_display = label
            print(f"  [LOI NHAN] {label_display}: {problem}")
        if len(errors) > 10:
            print(f"  ... con {len(errors) - 10} loi nhan khac")

        total_images += len(images)
        total_boxes += boxes
        total_missing += missing
        total_errors += len(errors)

    if not found_split:
        print("\n[LOI] Khong tim thay split/images va split/labels.")
        return 2

    print("\n" + "=" * 65)
    print("TONG KET")
    print(f"Tong anh: {total_images}")
    print(f"Tong bounding box hop le: {total_boxes}")
    print(f"Tong anh thieu file nhan: {total_missing}")
    print(f"Tong loi nhan: {total_errors}")
    if total_images == 0:
        print("[CHUA SAN SANG] Khong co anh de kiem tra.")
        return 2
    if total_missing or total_errors:
        print("[CAN KIEM TRA] Xu ly cac van de duoc bao cao truoc khi train.")
        return 1
    print("[KIEM TRA CO BAN DAT] Khong thay anh thieu file nhan/loi dinh dang.")
    print("Van can xem truc quan bounding box de xac nhan nhan dung.")
    return 0


if __name__ == "__main__":
    sys.exit(main())
