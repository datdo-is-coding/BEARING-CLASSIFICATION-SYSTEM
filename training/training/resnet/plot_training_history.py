
from pathlib import Path

import pandas as pd
import matplotlib.pyplot as plt


# Đường dẫn project
PROJECT_ROOT = Path(__file__).resolve().parents[2]

HISTORY_FILE = (
    PROJECT_ROOT
    / "weights"
    / "resnet34"
    / "training_history.csv"
)

OUTPUT_DIR = PROJECT_ROOT / "weights" / "resnet34"
OUTPUT_DIR.mkdir(parents=True, exist_ok=True)


def main():
    if not HISTORY_FILE.exists():
        raise FileNotFoundError(
            f"Không tìm thấy file lịch sử huấn luyện:\n{HISTORY_FILE}"
        )

    df = pd.read_csv(HISTORY_FILE)

    required_columns = [
        "epoch",
        "train_loss",
        "val_loss",
        "train_accuracy",
        "val_accuracy",
    ]

    missing = [c for c in required_columns if c not in df.columns]

    if missing:
        raise ValueError(
            f"File CSV thiếu các cột: {missing}\n"
            f"Các cột hiện có: {list(df.columns)}"
        )

    # Loại bỏ dòng thiếu dữ liệu cần thiết
    df = df.dropna(subset=required_columns)

    if df.empty:
        raise ValueError("Không có dữ liệu hợp lệ để vẽ biểu đồ.")

    # ==============================
    # 1. LOSS CURVE
    # ==============================
    plt.figure(figsize=(10, 6))

    plt.plot(
        df["epoch"],
        df["train_loss"],
        marker="o",
        label="Train Loss",
    )

    plt.plot(
        df["epoch"],
        df["val_loss"],
        marker="s",
        label="Validation Loss",
    )

    plt.xlabel("Epoch")
    plt.ylabel("Loss")
    plt.title("ResNet-34 Training and Validation Loss")
    plt.xticks(df["epoch"].astype(int))
    plt.grid(True, alpha=0.3)
    plt.legend()
    plt.tight_layout()

    loss_path = OUTPUT_DIR / "loss_curve.png"
    plt.savefig(loss_path, dpi=300, bbox_inches="tight")
    plt.show()
    plt.close()

    # ==============================
    # 2. ACCURACY CURVE
    # ==============================
    plt.figure(figsize=(10, 6))

    plt.plot(
        df["epoch"],
        df["train_accuracy"] * 100,
        marker="o",
        label="Train Accuracy",
    )

    plt.plot(
        df["epoch"],
        df["val_accuracy"] * 100,
        marker="s",
        label="Validation Accuracy",
    )

    plt.xlabel("Epoch")
    plt.ylabel("Accuracy (%)")
    plt.title("ResNet-34 Training and Validation Accuracy")
    plt.xticks(df["epoch"].astype(int))
    plt.grid(True, alpha=0.3)
    plt.legend()
    plt.tight_layout()

    accuracy_path = OUTPUT_DIR / "accuracy_curve.png"
    plt.savefig(accuracy_path, dpi=300, bbox_inches="tight")
    plt.show()
    plt.close()

    # ==============================
    # 3. THỐNG KÊ
    # ==============================
    best_loss_row = df.loc[df["val_loss"].idxmin()]
    best_acc_row = df.loc[df["val_accuracy"].idxmax()]

    print("\n========== TRAINING SUMMARY ==========")
    print(f"So epoch đã chạy: {len(df)}")
    print(
        f"Validation loss thấp nhất: "
        f"{best_loss_row['val_loss']:.4f} "
        f"(Epoch {int(best_loss_row['epoch'])})"
    )
    print(
        f"Validation accuracy cao nhất: "
        f"{best_acc_row['val_accuracy'] * 100:.2f}% "
        f"(Epoch {int(best_acc_row['epoch'])})"
    )

    print("\nĐã lưu biểu đồ:")
    print(loss_path)
    print(accuracy_path)


if __name__ == "__main__":
    main()
