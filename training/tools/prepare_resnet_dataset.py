
import cv2
import shutil
import hashlib
import pandas as pd
import numpy as np

from pathlib import Path
from collections import defaultdict

# =========================================================
# PREPARE RESNET DATASET
# 1. Nhan lay tu thu muc dataset_labeled da duyet
# 2. Split lay tu crop_manifest.csv
# 3. Chi augment tap train
# 4. Khong ghi de dataset cu
# =========================================================

# File nam tai:
# NCKH_Phan_loai_vong_bi/tools/prepare_resnet_dataset.py
# parents[1] = thu muc goc NCKH_Phan_loai_vong_bi

ROOT = Path(__file__).resolve().parents[1]

DATA_DIR = ROOT / "data" / "bearing_crops"

LABELED_DIR = DATA_DIR / "dataset_labeled"
REPORT_PATH = LABELED_DIR / "export_report.csv"
MANIFEST_PATH = DATA_DIR / "crop_manifest.csv"

OUTPUT_DIR = ROOT / "data" / "resnet_dataset"

CLASSES = [
    "normal",
    "rusted",
    "surface_damage",
    "damaged_shield",
]

SPLITS = ["train", "valid", "test"]
VALID_EXTENSIONS = {".jpg", ".jpeg", ".png", ".bmp", ".webp"}

TARGET_PER_CLASS = 1200
SEED = 42

# Neu bat True, bat buoc kiem tra source_image.
# Neu manifest co source_image va mot anh nguon xuat hien
# trong nhieu split, chuong trinh se dung.
REQUIRE_SOURCE_SPLIT_CHECK = True

rng = np.random.default_rng(SEED)


# =========================================================
# 1. HAM HO TRO
# =========================================================

def normalize_split(series):
    return (
        series.fillna("")
        .astype(str)
        .str.lower()
        .str.strip()
        .replace({
            "val": "valid",
            "validation": "valid",
        })
    )


def normalize_path_string(value):
    """Chuan hoa duong dan de doi chieu CSV."""
    value = str(value).strip().replace("\\", "/")
    return value.lower()


def normalize_source_key(value):
    """Chuan hoa ten/duong dan anh nguon."""
    if pd.isna(value):
        return ""

    value = str(value).strip().replace("\\", "/")
    return value.lower()


def sha256_file(path):
    """Tinh SHA-256 de phat hien file anh giong byte."""
    h = hashlib.sha256()

    with open(path, "rb") as f:
        for block in iter(lambda: f.read(1024 * 1024), b""):
            h.update(block)

    return h.hexdigest()


def augment_image(img):
    """
    Augment nhe cho anh vong bi.
    Chi ap dung cho tap train.
    """
    out = img.copy()
    h, w = out.shape[:2]

    # Lat ngang voi xac suat 50%
    if rng.random() < 0.5:
        out = cv2.flip(out, 1)

    # Xoay nhe -10 den +10 do
    angle = float(rng.uniform(-10.0, 10.0))

    matrix = cv2.getRotationMatrix2D(
        (w / 2.0, h / 2.0),
        angle,
        1.0,
    )

    out = cv2.warpAffine(
        out,
        matrix,
        (w, h),
        flags=cv2.INTER_LINEAR,
        borderMode=cv2.BORDER_REFLECT_101,
    )

    # Dieu chinh sang va tuong phan nhe
    alpha = float(rng.uniform(0.92, 1.08))
    beta = int(rng.integers(-8, 9))

    out = cv2.convertScaleAbs(
        out,
        alpha=alpha,
        beta=beta,
    )

    return out


def check_required_columns(df, required, filename):
    missing = required - set(df.columns)

    if missing:
        raise ValueError(
            f"{filename} thieu cac cot bat buoc: "
            f"{sorted(missing)}"
        )


def print_path_config():
    print("=" * 60)
    print("KIEM TRA DUONG DAN")
    print("=" * 60)
    print("ROOT         :", ROOT)
    print("LABELED_DIR  :", LABELED_DIR)
    print("REPORT_PATH  :", REPORT_PATH)
    print("MANIFEST_PATH:", MANIFEST_PATH)
    print("OUTPUT_DIR   :", OUTPUT_DIR)
    print("=" * 60)


# =========================================================
# 2. DOC VA KIEM TRA DU LIEU
# =========================================================

def main():
    print_path_config()

    # -----------------------------------------------------
    # 2.1. Kiem tra duong dan
    # -----------------------------------------------------

    if not LABELED_DIR.is_dir():
        raise FileNotFoundError(
            "Khong tim thay dataset da duyet:\n"
            f"{LABELED_DIR}\n\n"
            "Hay kiem tra lai vi tri thu muc dataset_labeled."
        )

    if not REPORT_PATH.is_file():
        raise FileNotFoundError(
            "Khong tim thay export_report.csv:\n"
            f"{REPORT_PATH}"
        )

    if not MANIFEST_PATH.is_file():
        raise FileNotFoundError(
            "Khong tim thay crop_manifest.csv:\n"
            f"{MANIFEST_PATH}"
        )

    if TARGET_PER_CLASS <= 0:
        raise ValueError("TARGET_PER_CLASS phai lon hon 0.")

    
# =====================================================
# XOA DATASET CU VA TAO LAI
# CHI XOA THU MUC resnet_dataset
# =====================================================

if OUTPUT_DIR.exists():
    if not OUTPUT_DIR.is_dir():
        raise NotADirectoryError(
            f"Duong dan output ton tai nhung khong phai thu muc:\n"
            f"{OUTPUT_DIR}"
        )

    print("\nCANH BAO: Dataset cu se bi xoa:")
    print(OUTPUT_DIR)

    shutil.rmtree(OUTPUT_DIR)

    print("Da xoa dataset cu.")


    # -----------------------------------------------------
    # 2.2. Doc CSV
    # -----------------------------------------------------

    report = pd.read_csv(REPORT_PATH)
    manifest = pd.read_csv(MANIFEST_PATH)

    report.columns = report.columns.str.strip()
    manifest.columns = manifest.columns.str.strip()

    check_required_columns(
        report,
        {"crop_path", "output_path"},
        "export_report.csv",
    )

    check_required_columns(
        manifest,
        {"crop_path", "split"},
        "crop_manifest.csv",
    )

    report["crop_key"] = report["crop_path"].map(
        normalize_path_string
    )

    report["filename"] = report["output_path"].map(
        lambda x: Path(str(x).replace("\\", "/")).name
    )

    manifest["crop_key"] = manifest["crop_path"].map(
        normalize_path_string
    )

    manifest["split"] = normalize_split(manifest["split"])

    if (report["crop_key"] == "").any():
        raise ValueError(
            "export_report.csv co crop_path rong."
        )

    if (report["filename"] == "").any():
        raise ValueError(
            "export_report.csv co output_path rong."
        )

    # Moi ten file xuat phai duy nhat de truy nguoc an toan
    if report["filename"].duplicated().any():
        duplicated = report.loc[
            report["filename"].duplicated(keep=False),
            "filename",
        ].tolist()[:10]

        raise ValueError(
            "Ten file trong export_report.csv bi trung. "
            f"Vi du: {duplicated}"
        )

    # Moi crop chi duoc gan mot split
    split_map = manifest[
        ["crop_key", "split"]
    ].drop_duplicates()

    split_conflicts = (
        split_map.groupby("crop_key")["split"].nunique()
    )

    if (split_conflicts > 1).any():
        bad = split_conflicts[
            split_conflicts > 1
        ].index.tolist()[:10]

        raise ValueError(
            "Mot crop co nhieu split trong manifest. "
            f"Vi du: {bad}"
        )

    split_map = split_map.drop_duplicates("crop_key")

    filename_to_crop = dict(
        zip(report["filename"], report["crop_key"])
    )

    # -----------------------------------------------------
    # 2.3. Kiem tra thu muc nhan
    # -----------------------------------------------------

    for cls in CLASSES:
        class_dir = LABELED_DIR / cls

        if not class_dir.is_dir():
            raise FileNotFoundError(
                f"Thieu thu muc nhan: {class_dir}"
            )

    # =====================================================
    # 3. DOC ANH VA LAY NHAN TU THU MUC
    # =====================================================

    samples = []
    unmatched_files = []
    invalid_images = []
    seen_paths = set()

    print("\nDang doc anh da duyet va kiem tra...")

    for cls in CLASSES:
        class_dir = LABELED_DIR / cls

        for image_path in sorted(class_dir.iterdir()):
            if not image_path.is_file():
                continue

            if image_path.suffix.lower() not in VALID_EXTENSIONS:
                continue

            resolved = image_path.resolve()

            if resolved in seen_paths:
                raise ValueError(
                    f"File bi dua vao nhieu lan: {image_path}"
                )

            seen_paths.add(resolved)

            # Doi chieu voi export_report.csv bang ten file
            filename = image_path.name

            if filename not in filename_to_crop:
                unmatched_files.append(str(image_path))
                continue

            crop_key = filename_to_crop[filename]

            # Kiem tra anh co doc duoc khong truoc khi tao output
            img = cv2.imread(str(image_path))

            if img is None:
                invalid_images.append(str(image_path))
                continue

            h, w = img.shape[:2]

            if h < 2 or w < 2:
                invalid_images.append(str(image_path))
                continue

            samples.append({
                "image_path": image_path.resolve(),
                "label": cls,
                "crop_key": crop_key,
            })

    if unmatched_files:
        raise ValueError(
            "Co anh khong doi chieu duoc voi export_report.csv.\n"
            "Co the anh da bi doi ten sau khi export.\n"
            "Vi du:\n"
            + "\n".join(unmatched_files[:10])
        )

    if invalid_images:
        raise ValueError(
            "Phat hien anh hong hoac khong doc duoc:\n"
            + "\n".join(invalid_images[:20])
        )

    if not samples:
        raise ValueError(
            "Khong tim thay anh hop le trong cac thu muc nhan."
        )

    sample_df = pd.DataFrame(samples)

    # Mot crop goc khong duoc xuat hien nhieu lan
    duplicated_crops = sample_df[
        sample_df["crop_key"].duplicated(keep=False)
    ]

    if not duplicated_crops.empty:
        raise ValueError(
            "Mot crop goc xuat hien nhieu lan trong dataset da duyet.\n"
            + duplicated_crops[
                ["image_path", "label", "crop_key"]
            ].head(20).to_string(index=False)
        )

    # =====================================================
    # 4. GAN SPLIT TU MANIFEST
    # =====================================================

    sample_df = sample_df.merge(
        split_map,
        on="crop_key",
        how="left",
        validate="many_to_one",
    )

    invalid_split = (
        sample_df["split"].isna()
        | ~sample_df["split"].isin(SPLITS)
    )

    if invalid_split.any():
        bad = sample_df.loc[
            invalid_split,
            ["image_path", "crop_key", "split"],
        ].head(20)

        raise ValueError(
            "Anh da duyet khong co split hop le trong manifest:\n"
            + bad.to_string(index=False)
        )

    # =====================================================
    # 5. KIEM TRA RO RI THEO ANH NGUON
    # =====================================================

    if "source_image" in manifest.columns:
        source_info = manifest[
            ["crop_key", "source_image", "split"]
        ].copy()

        source_info["source_key"] = (
            source_info["source_image"].map(normalize_source_key)
        )

        source_info = source_info[
            source_info["source_key"] != ""
        ].drop_duplicates(["source_key", "split"])

        source_splits = (
            source_info.groupby("source_key")["split"].nunique()
        )

        leaking_sources = source_splits[source_splits > 1]

        if not leaking_sources.empty:
            examples = leaking_sources.index.tolist()[:10]

            raise ValueError(
                "RO RI DU LIEU: cung mot source_image nam "
                "trong nhieu split.\n"
                f"Vi du: {examples}\n\n"
                "Khong nen tiep tuc train/test voi split nay. "
                "Can chia lai split theo anh nguon source_image."
            )

    elif REQUIRE_SOURCE_SPLIT_CHECK:
        raise ValueError(
            "REQUIRE_SOURCE_SPLIT_CHECK=True nhung "
            "crop_manifest.csv khong co cot source_image.\n"
            "Hay them cot source_image hoac tat co kiem tra sau "
            "khi tu xac minh split khong bi ro ri."
        )

    # =====================================================
    # 6. KIEM TRA ANH TRUNG BYTE GIUA CAC SPLIT
    # =====================================================

    print("\nDang kiem tra hash anh goc...")

    hash_to_records = defaultdict(list)

    for _, row in sample_df.iterrows():
        digest = sha256_file(row["image_path"])

        hash_to_records[digest].append({
            "path": str(row["image_path"]),
            "split": row["split"],
            "label": row["label"],
        })

    cross_split_duplicates = []

    for digest, records in hash_to_records.items():
        unique_splits = {r["split"] for r in records}

        if len(unique_splits) > 1:
            cross_split_duplicates.append((digest, records))

    if cross_split_duplicates:
        examples = []

        for _, records in cross_split_duplicates[:5]:
            examples.extend(
                f"{r['split']} | {r['label']} | {r['path']}"
                for r in records
            )

        raise ValueError(
            "Phat hien noi dung anh giong het nhau nam trong "
            "nhieu split.\n"
            + "\n".join(examples[:20])
        )

    # =====================================================
    # 7. KIEM TRA SO LUONG ANH MOI LOP
    # =====================================================

    print("\n========== SO LUONG ANH DA DUYET ==========")

    counts = (
        sample_df.groupby(["split", "label"])
        .size()
        .unstack(fill_value=0)
        .reindex(index=SPLITS, columns=CLASSES, fill_value=0)
    )

    print(counts.to_string())

    for split in SPLITS:
        for cls in CLASSES:
            if counts.loc[split, cls] == 0:
                raise ValueError(
                    f"TAP {split.upper()} khong co anh lop {cls}."
                )

    # =====================================================
    # 8. TAO OUTPUT VA COPY ANH GOC
    # =====================================================

    # Chi tao output sau khi cac kiem tra dau vao da qua.
    for split in SPLITS:
        for cls in CLASSES:
            (OUTPUT_DIR / split / cls).mkdir(
                parents=True,
                exist_ok=True,
            )

    train_files = {cls: [] for cls in CLASSES}
    original_counts = {
        split: {cls: 0 for cls in CLASSES}
        for split in SPLITS
    }

    try:
        for index, row in sample_df.reset_index(drop=True).iterrows():
            src = Path(row["image_path"])
            split = row["split"]
            cls = row["label"]

            dst = (
                OUTPUT_DIR / split / cls
                / f"original_{index + 1:06d}{src.suffix.lower()}"
            )

            shutil.copy2(src, dst)
            original_counts[split][cls] += 1

            if split == "train":
                train_files[cls].append(dst)

    except Exception:
        # Xoa output tao dang do neu copy that bai.
        # Chi xoa thu muc output neu no chua ton tai truoc khi chay.
        if OUTPUT_DIR.exists():
            shutil.rmtree(OUTPUT_DIR, ignore_errors=True)
        raise

    print("\n========== ANH GOC SAU KHI CHIA ==========")

    for split in SPLITS:
        print(f"\n--- {split.upper()} ---")

        for cls in CLASSES:
            print(f"{cls:20s}: {original_counts[split][cls]}")

    # =====================================================
    # 9. AUGMENT CHI TAP TRAIN
    # =====================================================

    print("\n========== AUGMENT TRAIN ==========")

    for cls in CLASSES:
        files = train_files[cls]
        current = len(files)

        print(f"\n[{cls}] Anh train goc: {current}")

        if current == 0:
            raise ValueError(f"Lop {cls} khong co anh train.")

        if current >= TARGET_PER_CLASS:
            print(
                f"Da co {current} anh goc, "
                "khong tao them anh augment."
            )
            continue

        needed = TARGET_PER_CLASS - current
        created = 0
        attempts = 0
        max_attempts = max(needed * 5, 100)

        while created < needed and attempts < max_attempts:
            attempts += 1

            src = files[int(rng.integers(0, len(files)))]
            img = cv2.imread(str(src))

            if img is None:
                continue

            aug = augment_image(img)

            # Ten anh dua tren so dem da tao thanh cong,
            # tranh ghi de anh khi mot lan ghi file that bai.
            dst = (
                OUTPUT_DIR / "train" / cls
                / f"aug_{created + 1:05d}.jpg"
            )

            success = cv2.imwrite(
                str(dst),
                aug,
                [cv2.IMWRITE_JPEG_QUALITY, 95],
            )

            if success:
                created += 1

        if created < needed:
            raise RuntimeError(
                f"Lop {cls}: chi tao duoc {created}/{needed} "
                "anh augment sau {attempts} lan thu."
            )

        print(
            f"Anh goc: {current} | "
            f"Anh augment: {created} | "
            f"Tong train: {current + created}"
        )

    # =====================================================
    # 10. BAO CAO CUOI
    # =====================================================

    print("\n========== DATASET RESNET HOAN TAT ==========")

    for split in SPLITS:
        total = 0
        print(f"\n--- {split.upper()} ---")

        for cls in CLASSES:
            folder = OUTPUT_DIR / split / cls

            n_original = len(list(folder.glob("original_*")))
            n_aug = len(list(folder.glob("aug_*")))
            n = n_original + n_aug

            print(
                f"{cls:20s}: {n:5d} "
                f"(goc={n_original}, augment={n_aug})"
            )

            total += n

        print(f"TONG {split.upper()}: {total}")

    print(f"\nOutput: {OUTPUT_DIR}")
    print("Chi tap train duoc augment.")
    print("Valid va test giu anh goc.")


if __name__ == "__main__":
    main()
