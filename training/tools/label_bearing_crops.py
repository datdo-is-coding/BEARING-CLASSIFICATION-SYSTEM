
import cv2
import pandas as pd
from pathlib import Path

# =====================================================
# GAN NHAN CROP VONG BI - 4 LOAI
# =====================================================

PROJECT_ROOT = Path(__file__).resolve().parent.parent

MANIFEST_PATH = (
    PROJECT_ROOT / "data" / "bearing_crops" / "crop_manifest.csv"
)

OUTPUT_PATH = (
    PROJECT_ROOT / "data" / "bearing_crops" / "bearing_crop_labels.csv"
)

LABELS = {
    ord("1"): "normal",
    ord("2"): "rusted",
    ord("3"): "surface_damage",
    ord("4"): "damaged_shield",
}

WINDOW_NAME = "Bearing Labeling - 1/2/3/4 | S=Skip | Q=Quit"


def resolve_path(path):
    """Tim anh crop tu duong dan trong manifest."""
    path = Path(str(path))

    if path.is_absolute() and path.exists():
        return path

    candidate = PROJECT_ROOT / path
    if candidate.exists():
        return candidate

    candidate = PROJECT_ROOT / "data" / path
    if candidate.exists():
        return candidate

    return path


def main():
    if not MANIFEST_PATH.exists():
        raise FileNotFoundError(
            f"Khong tim thay manifest:\n{MANIFEST_PATH}"
        )

    df = pd.read_csv(MANIFEST_PATH)

    if "crop_path" not in df.columns:
        raise ValueError("Manifest khong co cot crop_path.")

    # Doc nhan cu de co the tiep tuc cong viec
    if OUTPUT_PATH.exists():
        old = pd.read_csv(OUTPUT_PATH)

        if {"crop_path", "defect_label"}.issubset(old.columns):
            saved_labels = dict(
                zip(
                    old["crop_path"].astype(str),
                    old["defect_label"].fillna("").astype(str)
                )
            )
        else:
            saved_labels = {}
    else:
        saved_labels = {}

    # Chi bo qua cac anh da duoc gan mot trong bon nhan hop le
    valid_labels = set(LABELS.values())

    pending = []
    for i, row in df.iterrows():
        image_key = str(row["crop_path"])
        old_label = saved_labels.get(image_key, "")

        if old_label not in valid_labels:
            pending.append(i)

    print("=" * 60)
    print("GAN NHAN VONG BI")
    print("1 = normal         : Binh thuong")
    print("2 = rusted         : Bi gi")
    print("3 = surface_damage : Hu hong be mat")
    print("4 = damaged_shield : Hu hong nap chan")
    print("S = Skip           : Bo qua, xem lai sau")
    print("Q = Quit           : Luu va thoat")
    print(f"Tong so crop: {len(df)}")
    print(f"So anh con can gan nhan: {len(pending)}")
    print(f"File ket qua: {OUTPUT_PATH}")
    print("=" * 60)

    if not pending:
        print("Tat ca anh da co nhan hop le.")
        return

    def save_progress():
        output = df.copy()
        output["defect_label"] = (
            output["crop_path"]
            .astype(str)
            .map(saved_labels)
            .fillna("")
        )
        output.to_csv(
            OUTPUT_PATH,
            index=False,
            encoding="utf-8-sig"
        )

    cv2.namedWindow(WINDOW_NAME, cv2.WINDOW_NORMAL)

    position = 0

    while position < len(pending):
        idx = pending[position]
        row = df.loc[idx]
        image_key = str(row["crop_path"])
        image_path = resolve_path(image_key)

        image = cv2.imread(str(image_path))

        if image is None:
            print(f"[LOI] Khong doc duoc anh: {image_path}")
            position += 1
            continue

        # Thu nho anh neu qua lon, giu nguyen ti le
        h, w = image.shape[:2]
        scale = min(1000 / w, 650 / h, 1.0)

        if scale < 1.0:
            image = cv2.resize(
                image,
                (int(w * scale), int(h * scale)),
                interpolation=cv2.INTER_AREA
            )

        # Hien thi thong tin tren anh
        display = image.copy()

        instructions = [
            f"Crop {idx + 1}/{len(df)} | Pending {position + 1}/{len(pending)}",
            "1: NORMAL | 2: RUSTED",
            "3: SURFACE DAMAGE | 4: DAMAGED SHIELD",
            "S: SKIP | Q: SAVE AND QUIT",
        ]

        y = 25
        for line in instructions:
            cv2.putText(
                display, line, (10, y),
                cv2.FONT_HERSHEY_SIMPLEX,
                0.6, (0, 255, 255), 2, cv2.LINE_AA
            )
            y += 27

        cv2.imshow(WINDOW_NAME, display)
        key = cv2.waitKey(0) & 0xFF

        if key in LABELS:
            label = LABELS[key]
            saved_labels[image_key] = label

            print(f"[{idx + 1}/{len(df)}] {image_key} -> {label}")

            save_progress()
            position += 1

        elif key in (ord("s"), ord("S")):
            # De trong nhan de co the xem lai khi chay lai
            saved_labels.pop(image_key, None)
            position += 1

        elif key in (ord("q"), ord("Q"), 27):
            save_progress()
            print("Da luu tien do. Thoat chuong trinh.")
            break

        # Phim khac: hien lai anh hien tai

    cv2.destroyAllWindows()
    save_progress()

    final_labels = [
        value for value in saved_labels.values()
        if value in valid_labels
    ]

    print("\n========== TONG KET ==========")
    for label in valid_labels:
        print(f"{label}: {final_labels.count(label)}")

    print(f"Da luu tai: {OUTPUT_PATH}")
    print("Chay lai script de tiep tuc gan nhan.")


if __name__ == "__main__":
    main()
