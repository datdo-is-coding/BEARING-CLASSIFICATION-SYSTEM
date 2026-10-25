
import csv
import json
import sys
from pathlib import Path

import numpy as np
import torch
import torch.nn as nn
import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt

from torchvision import models
from sklearn.metrics import (
    accuracy_score,
    classification_report,
    confusion_matrix,
    precision_recall_fscore_support,
)

from dataset import create_loaders, CLASSES


# ============================================================
# 1. CAU HINH
# ============================================================

PROJECT_ROOT = Path(__file__).resolve().parents[2]
WEIGHTS_DIR = PROJECT_ROOT / "weights" / "resnet34"

# Mac dinh danh gia checkpoint co validation loss tot nhat
CHECKPOINT_PATH = WEIGHTS_DIR / "best_resnet34.pth"

IMAGE_SIZE = 224
BATCH_SIZE = 8
DROPOUT = 0.30

# Neu danh gia checkpoint trong runs/, ket qua duoc luu
# vao thu muc evaluation_test ben canh checkpoint.
# Neu dung checkpoint mac dinh, ket qua nam trong weights/resnet34.
OUTPUT_DIR = CHECKPOINT_PATH.parent / "evaluation_test"
OUTPUT_DIR.mkdir(parents=True, exist_ok=True)

DEVICE = torch.device(
    "cuda" if torch.cuda.is_available() else "cpu"
)


# ============================================================
# 2. TAO MODEL
# ============================================================

def build_model(num_classes, dropout=0.30):
    model = models.resnet34(weights=None)

    in_features = model.fc.in_features
    model.fc = nn.Sequential(
        nn.Dropout(p=dropout),
        nn.Linear(in_features, num_classes),
    )

    return model


# ============================================================
# 3. LOAD CHECKPOINT
# ============================================================

def load_checkpoint(model, checkpoint_path, device):
    if not checkpoint_path.is_file():
        raise FileNotFoundError(
            f"Khong tim thay checkpoint: {checkpoint_path}\n"
            "Hay train ResNet-34 truoc khi danh gia."
        )

    # Checkpoint do train.py tao ra, chua state_dict va metadata.
    try:
        checkpoint = torch.load(
            checkpoint_path,
            map_location=device,
            weights_only=True,
        )
    except TypeError:
        # Tuong thich voi phien ban PyTorch cu
        checkpoint = torch.load(
            checkpoint_path,
            map_location=device,
        )

    if "model_state_dict" not in checkpoint:
        raise KeyError(
            "Checkpoint khong co model_state_dict. "
            "Hay kiem tra dung file best_resnet34.pth."
        )

    saved_classes = checkpoint.get("classes")

    if saved_classes != list(CLASSES):
        raise ValueError(
            "Thu tu lop trong checkpoint khong khop dataset.py.\n"
            f"Checkpoint: {saved_classes}\n"
            f"Dataset: {list(CLASSES)}"
        )

    model.load_state_dict(checkpoint["model_state_dict"])
    model.eval()

    return checkpoint


# ============================================================
# 4. DU DOAN TREN TAP TEST
# ============================================================

def predict_test(model, test_loader, device):
    all_labels = []
    all_predictions = []
    all_probabilities = []
    all_paths = []

    # ImageFolder luu danh sach (duong_dan, nhan)
    samples = test_loader.dataset.samples

    sample_index = 0

    model.eval()

    with torch.inference_mode():
        for images, labels in test_loader:
            images = images.to(device, non_blocking=True)
            labels_device = labels.to(device, non_blocking=True)

            if device.type == "cuda":
                with torch.amp.autocast(
                    device_type="cuda",
                    enabled=True,
                ):
                    outputs = model(images)
            else:
                outputs = model(images)

            probabilities = torch.softmax(outputs.float(), dim=1)
            predictions = probabilities.argmax(dim=1)

            batch_size = labels.size(0)

            all_labels.extend(labels_device.cpu().tolist())
            all_predictions.extend(predictions.cpu().tolist())
            all_probabilities.extend(probabilities.cpu().tolist())

            for _ in range(batch_size):
                all_paths.append(samples[sample_index][0])
                sample_index += 1

    if not all_labels:
        raise RuntimeError("Tap test khong co anh de danh gia.")

    return (
        np.asarray(all_labels, dtype=int),
        np.asarray(all_predictions, dtype=int),
        np.asarray(all_probabilities, dtype=float),
        all_paths,
    )


# ============================================================
# 5. VE CONFUSION MATRIX
# ============================================================

def plot_confusion_matrix(cm, class_names, output_dir):
    # So luong du doan
    fig, ax = plt.subplots(figsize=(9, 7))
    image = ax.imshow(cm, interpolation="nearest")
    fig.colorbar(image, ax=ax)

    ax.set(
        xticks=np.arange(len(class_names)),
        yticks=np.arange(len(class_names)),
        xticklabels=class_names,
        yticklabels=class_names,
        xlabel="Predicted label",
        ylabel="True label",
        title="Confusion Matrix - Test Set",
    )

    plt.setp(
        ax.get_xticklabels(),
        rotation=30,
        ha="right",
        rotation_mode="anchor",
    )

    threshold = cm.max() / 2 if cm.size and cm.max() > 0 else 0

    for i in range(cm.shape[0]):
        for j in range(cm.shape[1]):
            ax.text(
                j,
                i,
                str(cm[i, j]),
                ha="center",
                va="center",
                color="white" if cm[i, j] > threshold else "black",
            )

    fig.tight_layout()
    fig.savefig(
        output_dir / "confusion_matrix.png",
        dpi=200,
        bbox_inches="tight",
    )
    plt.close(fig)

    # Confusion matrix chuan hoa theo tung lop that
    row_sums = cm.sum(axis=1, keepdims=True)
    cm_normalized = np.divide(
        cm,
        row_sums,
        out=np.zeros_like(cm, dtype=float),
        where=row_sums != 0,
    )

    fig, ax = plt.subplots(figsize=(9, 7))
    image = ax.imshow(
        cm_normalized,
        interpolation="nearest",
        vmin=0,
        vmax=1,
    )
    fig.colorbar(image, ax=ax, format="%.0f%%")

    ax.set(
        xticks=np.arange(len(class_names)),
        yticks=np.arange(len(class_names)),
        xticklabels=class_names,
        yticklabels=class_names,
        xlabel="Predicted label",
        ylabel="True label",
        title="Normalized Confusion Matrix - Test Set",
    )

    plt.setp(
        ax.get_xticklabels(),
        rotation=30,
        ha="right",
        rotation_mode="anchor",
    )

    for i in range(cm_normalized.shape[0]):
        for j in range(cm_normalized.shape[1]):
            ax.text(
                j,
                i,
                f"{cm_normalized[i, j] * 100:.1f}%",
                ha="center",
                va="center",
                color=(
                    "white"
                    if cm_normalized[i, j] > 0.5
                    else "black"
                ),
            )

    fig.tight_layout()
    fig.savefig(
        output_dir / "confusion_matrix_normalized.png",
        dpi=200,
        bbox_inches="tight",
    )
    plt.close(fig)


# ============================================================
# 6. VE PRECISION / RECALL / F1
# ============================================================

def plot_class_metrics(report, class_names, output_dir):
    precision = [
        report[name]["precision"] for name in class_names
    ]
    recall = [
        report[name]["recall"] for name in class_names
    ]
    f1 = [
        report[name]["f1-score"] for name in class_names
    ]

    x = np.arange(len(class_names))
    width = 0.24

    fig, ax = plt.subplots(figsize=(11, 6))

    ax.bar(x - width, precision, width, label="Precision")
    ax.bar(x, recall, width, label="Recall")
    ax.bar(x + width, f1, width, label="F1-score")

    ax.set_ylim(0, 1.05)
    ax.set_ylabel("Score")
    ax.set_title("Per-Class Metrics - Test Set")
    ax.set_xticks(x)
    ax.set_xticklabels(class_names, rotation=20, ha="right")
    ax.grid(axis="y", alpha=0.3)
    ax.legend()

    fig.tight_layout()
    fig.savefig(
        output_dir / "class_metrics.png",
        dpi=200,
        bbox_inches="tight",
    )
    plt.close(fig)


# ============================================================
# 7. LUU DANH SACH ANH DU DOAN SAI
# ============================================================

def save_misclassified(
    paths,
    y_true,
    y_pred,
    probabilities,
    output_path,
):
    with open(
        output_path,
        "w",
        newline="",
        encoding="utf-8-sig",
    ) as file:
        writer = csv.writer(file)

        writer.writerow([
            "image_path",
            "true_label",
            "predicted_label",
            "confidence_percent",
        ])

        for i in range(len(y_true)):
            if y_true[i] == y_pred[i]:
                continue

            confidence = probabilities[i, y_pred[i]] * 100

            writer.writerow([
                paths[i],
                CLASSES[y_true[i]],
                CLASSES[y_pred[i]],
                round(float(confidence), 2),
            ])


# ============================================================
# 8. MAIN
# ============================================================

def main():
    print("=" * 70)
    print("RESNET-34 - TEST SET EVALUATION")
    print("=" * 70)
    print("Device:", DEVICE)
    print("Checkpoint:", CHECKPOINT_PATH)
    print("Output:", OUTPUT_DIR)

    _, _, test_loader = create_loaders()

    print("Test images:", len(test_loader.dataset))
    print("Classes:", CLASSES)

    model = build_model(
        num_classes=len(CLASSES),
        dropout=DROPOUT,
    ).to(DEVICE)

    checkpoint = load_checkpoint(
        model,
        CHECKPOINT_PATH,
        DEVICE,
    )

    print("Checkpoint epoch:", checkpoint.get("epoch", "N/A"))
    print(
        "Checkpoint validation loss:",
        checkpoint.get("best_val_loss", "N/A"),
    )
    print(
        "Checkpoint validation accuracy:",
        checkpoint.get("best_val_accuracy", "N/A"),
    )

    y_true, y_pred, probabilities, paths = predict_test(
        model,
        test_loader,
        DEVICE,
    )

    labels = list(range(len(CLASSES)))

    accuracy = accuracy_score(y_true, y_pred)

    precision_macro, recall_macro, f1_macro, _ = (
        precision_recall_fscore_support(
            y_true,
            y_pred,
            labels=labels,
            average="macro",
            zero_division=0,
        )
    )

    precision_weighted, recall_weighted, f1_weighted, _ = (
        precision_recall_fscore_support(
            y_true,
            y_pred,
            labels=labels,
            average="weighted",
            zero_division=0,
        )
    )

    cm = confusion_matrix(
        y_true,
        y_pred,
        labels=labels,
    )

    report_text = classification_report(
        y_true,
        y_pred,
        labels=labels,
        target_names=CLASSES,
        digits=4,
        zero_division=0,
    )

    report_dict = classification_report(
        y_true,
        y_pred,
        labels=labels,
        target_names=CLASSES,
        output_dict=True,
        zero_division=0,
    )

    # Bao cao text
    report_path = OUTPUT_DIR / "classification_report.txt"
    with open(report_path, "w", encoding="utf-8") as file:
        file.write(report_text)

    # Bao cao JSON
    metrics = {
        "model": "ResNet-34",
        "checkpoint": str(CHECKPOINT_PATH),
        "checkpoint_epoch": checkpoint.get("epoch"),
        "test_samples": int(len(y_true)),
        "accuracy": float(accuracy),
        "macro_precision": float(precision_macro),
        "macro_recall": float(recall_macro),
        "macro_f1": float(f1_macro),
        "weighted_precision": float(precision_weighted),
        "weighted_recall": float(recall_weighted),
        "weighted_f1": float(f1_weighted),
        "classes": list(CLASSES),
        "classification_report": report_dict,
        "confusion_matrix": cm.tolist(),
    }

    with open(
        OUTPUT_DIR / "metrics.json",
        "w",
        encoding="utf-8",
    ) as file:
        json.dump(
            metrics,
            file,
            indent=4,
            ensure_ascii=False,
        )

    # Bao cao tung anh
    with open(
        OUTPUT_DIR / "test_predictions.csv",
        "w",
        newline="",
        encoding="utf-8-sig",
    ) as file:
        writer = csv.writer(file)

        writer.writerow([
            "image_path",
            "true_label",
            "predicted_label",
            "correct",
            "confidence_percent",
        ])

        for i in range(len(y_true)):
            confidence = probabilities[i, y_pred[i]] * 100

            writer.writerow([
                paths[i],
                CLASSES[y_true[i]],
                CLASSES[y_pred[i]],
                bool(y_true[i] == y_pred[i]),
                round(float(confidence), 2),
            ])

    save_misclassified(
        paths,
        y_true,
        y_pred,
        probabilities,
        OUTPUT_DIR / "misclassified_images.csv",
    )

    plot_confusion_matrix(cm, CLASSES, OUTPUT_DIR)
    plot_class_metrics(report_dict, CLASSES, OUTPUT_DIR)

    print("\n" + "=" * 70)
    print("KET QUA DANH GIA TEST")
    print("=" * 70)
    print(f"Accuracy          : {accuracy * 100:.2f}%")
    print(f"Macro Precision   : {precision_macro * 100:.2f}%")
    print(f"Macro Recall      : {recall_macro * 100:.2f}%")
    print(f"Macro F1-score    : {f1_macro * 100:.2f}%")
    print(f"Weighted F1-score : {f1_weighted * 100:.2f}%")

    print("\nClassification report:")
    print(report_text)

    print("Confusion matrix:")
    print(cm)

    print("\nDa luu ket qua tai:", OUTPUT_DIR)
    print(" - metrics.json")
    print(" - classification_report.txt")
    print(" - test_predictions.csv")
    print(" - misclassified_images.csv")
    print(" - confusion_matrix.png")
    print(" - confusion_matrix_normalized.png")
    print(" - class_metrics.png")
    print("=" * 70)


if __name__ == "__main__":
    main()
