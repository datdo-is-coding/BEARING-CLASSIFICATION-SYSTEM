
import shutil
import pandas as pd
from pathlib import Path

# =====================================================
# XUAT ANH TU CSV VAO CAC THU MUC NHAN
# Chua chia train/val/test, chua augment
# =====================================================

# Sua dong nay neu file script khong nam trong training/resnet/
PROJECT_ROOT = Path(__file__).resolve().parents[2]

CSV_PATH = (
    PROJECT_ROOT
    / "data"
    / "bearing_crops"
    / "bearing_crop_labels.csv"
)

SOURCE_ROOT = PROJECT_ROOT / "data" / "bearing_crops"

OUTPUT_ROOT = (
    PROJECT_ROOT
    / "data"
    / "bearing_crops"
    / "dataset_labeled"
)

LABELS = [
    "normal",
    "rusted",
    "surface_damage",
    "damaged_shield",
]


def resolve_image_path(value):
    """Tim anh tu duong dan ghi trong CSV."""
    p = Path(str(value).strip())

    candidates = [
        p,
        PROJECT_ROOT / p,
        SOURCE_ROOT / p,
        PROJECT_ROOT / "data" / p,
        SOURCE_ROOT / p.name,
    ]

    for candidate in candidates:
        if candidate.exists() and candidate.is_file():
            return candidate.resolve()

    return None


def main():
    if not CSV_PATH.exists():
        raise FileNotFoundError(
            f"Khong tim thay CSV:\n{CSV_PATH}\n"
            "Hay kiem tra PROJECT_ROOT va duong dan CSV_PATH."
        )

    df = pd.read_csv(CSV_PATH)

    required = {"crop_path", "defect_label"}
    missing = required - set(df.columns)

    if missing:
        raise ValueError(
            f"CSV thieu cot: {sorted(missing)}. "
            f"Cac cot hien co: {list(df.columns)}"
        )

    # Tao cac thu muc nhan
    for label in LABELS:
        (OUTPUT_ROOT / label).mkdir(
            parents=True, exist_ok=True
        )

    unlabeled_dir = OUTPUT_ROOT / "unlabeled"
    unlabeled_dir.mkdir(parents=True, exist_ok=True)

    report = []
    copied = 0
    missing_images = 0

    for index, row in df.iterrows():
        crop_path = str(row["crop_path"]).strip()
        label = str(row["defect_label"]).strip()

        if not crop_path or crop_path.lower() == "nan":
            report.append({
                "crop_path": crop_path,
                "defect_label": label,
                "status": "EMPTY_PATH",
                "output_path": "",
            })
            continue

        source = resolve_image_path(crop_path)

        if source is None:
            missing_images += 1
            report.append({
                "crop_path": crop_path,
                "defect_label": label,
                "status": "IMAGE_NOT_FOUND",
                "output_path": "",
            })
            print(f"[KHONG TIM THAY] {crop_path}")
            continue

        if label not in LABELS:
            destination_dir = unlabeled_dir
            status = "UNLABELED"
        else:
            destination_dir = OUTPUT_ROOT / label
            status = "COPIED"

        # Them so thu tu de tranh trung ten anh
        destination_name = (
            f"{index + 1:06d}_{source.name}"
        )
        destination = destination_dir / destination_name

        shutil.copy2(source, destination)

        if status == "COPIED":
            copied += 1

        report.append({
            "crop_path": crop_path,
            "defect_label": label,
            "status": status,
            "output_path": str(destination),
        })

    report_path = OUTPUT_ROOT / "export_report.csv"
    pd.DataFrame(report).to_csv(
        report_path,
        index=False,
        encoding="utf-8-sig",
    )

    print("\n========== HOAN TAT ==========")
    print(f"CSV dau vao : {CSV_PATH}")
    print(f"Thu muc anh : {OUTPUT_ROOT}")
    print(f"Anh co nhan da sao chep: {copied}")
    print(f"Anh khong tim thay: {missing_images}")
    print(
        "Anh chua co nhan/nhan khong hop le: "
        f"{sum(r['status'] == 'UNLABELED' for r in report)}"
    )
    print(f"Bao cao: {report_path}")


if __name__ == "__main__":
    main()
