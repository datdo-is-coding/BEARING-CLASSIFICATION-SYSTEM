
import csv
import json
import random
import shutil
from datetime import datetime
from pathlib import Path

import matplotlib
matplotlib.use("Agg")
import matplotlib.pyplot as plt

import numpy as np
import torch
import torch.nn as nn
from torch.optim import AdamW
from torch.optim.lr_scheduler import ReduceLROnPlateau
from torchvision import models

from dataset import create_loaders, CLASSES


# ============================================================
# 1. CAU HINH
# ============================================================

PROJECT_ROOT = Path(__file__).resolve().parents[2]

# Thu muc chua ket qua ResNet-34
OUTPUT_DIR = PROJECT_ROOT / "weights" / "resnet34"
RUNS_DIR = OUTPUT_DIR / "runs"

OUTPUT_DIR.mkdir(parents=True, exist_ok=True)
RUNS_DIR.mkdir(parents=True, exist_ok=True)

# Moi lan chay tao mot thu muc rieng
RUN_ID = datetime.now().strftime("run_%Y%m%d_%H%M%S")
RUN_DIR = RUNS_DIR / RUN_ID
RUN_DIR.mkdir(parents=True, exist_ok=False)

# File ket qua rieng cua lan train hien tai
BEST_LOSS_PATH = RUN_DIR / "best_resnet34.pth"
BEST_ACC_PATH = RUN_DIR / "best_resnet34_accuracy.pth"
HISTORY_PATH = RUN_DIR / "training_history.csv"
CONFIG_PATH = RUN_DIR / "config.json"
PLOT_PATH = RUN_DIR / "training_curves.png"

# Cac file mac dinh, luon cap nhat sau khi train thanh cong
LATEST_BEST_LOSS = OUTPUT_DIR / "best_resnet34.pth"
LATEST_BEST_ACC = OUTPUT_DIR / "best_resnet34_accuracy.pth"
LATEST_HISTORY = OUTPUT_DIR / "training_history.csv"
LATEST_CONFIG = OUTPUT_DIR / "config.json"
LATEST_PLOT = OUTPUT_DIR / "training_curves.png"

MAX_EPOCHS = 20

# Giai doan 1: chi train lop phan loai
HEAD_EPOCHS = 2
HEAD_LR = 3e-4

# Giai doan 2: fine-tune layer4 va lop phan loai
BACKBONE_LR = 1e-5
CLASSIFIER_LR = 3e-5

WEIGHT_DECAY = 5e-4
DROPOUT = 0.30
LABEL_SMOOTHING = 0.05

PATIENCE = 4
MIN_DELTA = 0.002
SEED = 42


# ============================================================
# 2. RANDOM SEED
# ============================================================

def set_seed(seed=42):
    random.seed(seed)
    np.random.seed(seed)
    torch.manual_seed(seed)

    if torch.cuda.is_available():
        torch.cuda.manual_seed_all(seed)

    # Ho tro tinh tai lap ket qua tren CUDA.
    # Mot so phep toan co the khong deterministic tuy phien ban.
    torch.backends.cudnn.deterministic = True
    torch.backends.cudnn.benchmark = False


# ============================================================
# 3. TAO MODEL RESNET-34
# ============================================================

def build_model(num_classes):
    try:
        weights = models.ResNet34_Weights.DEFAULT
        model = models.resnet34(weights=weights)
        print("Da tai ResNet-34 pretrained ImageNet.")

    except Exception as exc:
        raise RuntimeError(
            "Khong tai duoc pretrained weights ResNet-34. "
            "Hay kiem tra mang hoac cache torchvision. "
            "Khong nen am tham train tu dau."
        ) from exc

    in_features = model.fc.in_features

    model.fc = nn.Sequential(
        nn.Dropout(p=DROPOUT),
        nn.Linear(in_features, num_classes),
    )

    return model


# ============================================================
# 4. DONG BANG / MO CAC TANG
# ============================================================

def freeze_backbone(model):
    for param in model.parameters():
        param.requires_grad = False

    for param in model.fc.parameters():
        param.requires_grad = True


def unfreeze_layer4(model):
    for param in model.parameters():
        param.requires_grad = False

    for param in model.layer4.parameters():
        param.requires_grad = True

    for param in model.fc.parameters():
        param.requires_grad = True


def set_training_mode(model, stage):
    """
    Giu BatchNorm o eval de khong cap nhat running statistics.
    Giai doan 1: chi train classifier.
    Giai doan 2: train layer4 va classifier.
    """
    model.train()

    for module in model.modules():
        if isinstance(module, nn.BatchNorm2d):
            module.eval()

    if stage == 1:
        model.conv1.eval()
        model.bn1.eval()
        model.layer1.eval()
        model.layer2.eval()
        model.layer3.eval()
        model.layer4.eval()
        model.fc.train()

    elif stage == 2:
        model.conv1.eval()
        model.bn1.eval()
        model.layer1.eval()
        model.layer2.eval()
        model.layer3.eval()

        model.layer4.train()
        model.fc.train()

        # BatchNorm trong layer4 van giu eval
        for module in model.layer4.modules():
            if isinstance(module, nn.BatchNorm2d):
                module.eval()


# ============================================================
# 5. OPTIMIZER VA SCHEDULER
# ============================================================

def build_optimizer(model, stage):
    if stage == 1:
        return AdamW(
            model.fc.parameters(),
            lr=HEAD_LR,
            weight_decay=WEIGHT_DECAY,
        )

    return AdamW(
        [
            {
                "params": model.layer4.parameters(),
                "lr": BACKBONE_LR,
            },
            {
                "params": model.fc.parameters(),
                "lr": CLASSIFIER_LR,
            },
        ],
        weight_decay=WEIGHT_DECAY,
    )


def build_scheduler(optimizer):
    return ReduceLROnPlateau(
        optimizer,
        mode="min",
        factor=0.5,
        patience=1,
        min_lr=1e-7,
    )


# ============================================================
# 6. TRAIN / VALIDATION
# ============================================================

def run_epoch(
    model,
    loader,
    criterion,
    device,
    optimizer=None,
    scaler=None,
    stage=2,
):
    is_training = optimizer is not None

    if is_training:
        set_training_mode(model, stage)
    else:
        model.eval()

    total_loss = 0.0
    total_correct = 0
    total_samples = 0

    for images, labels in loader:
        images = images.to(device, non_blocking=True)
        labels = labels.to(device, non_blocking=True)

        if is_training:
            optimizer.zero_grad(set_to_none=True)

        with torch.set_grad_enabled(is_training):
            with torch.amp.autocast(
                device_type=device.type,
                enabled=(device.type == "cuda"),
            ):
                outputs = model(images)
                loss = criterion(outputs, labels)

            if is_training:
                scaler.scale(loss).backward()
                scaler.step(optimizer)
                scaler.update()

        batch_size = labels.size(0)

        total_loss += loss.item() * batch_size
        total_correct += (
            outputs.argmax(dim=1) == labels
        ).sum().item()
        total_samples += batch_size

    if total_samples == 0:
        raise RuntimeError(
            "DataLoader khong co du lieu. "
            "Hay kiem tra dataset.py va duong dan dataset."
        )

    return (
        total_loss / total_samples,
        total_correct / total_samples,
    )


# ============================================================
# 7. LUU CHECKPOINT VA HISTORY
# ============================================================

def save_checkpoint(
    path,
    model,
    epoch,
    val_loss,
    val_accuracy,
    stage,
):
    torch.save(
        {
            "model_name": "resnet34",
            "model_state_dict": model.state_dict(),
            "classes": list(CLASSES),
            "epoch": epoch,
            "stage": stage,
            "best_val_loss": float(val_loss),
            "best_val_accuracy": float(val_accuracy),
            "dropout": DROPOUT,
            "label_smoothing": LABEL_SMOOTHING,
        },
        path,
    )


def save_history(history):
    if not history:
        return

    with open(
        HISTORY_PATH,
        "w",
        newline="",
        encoding="utf-8-sig",
    ) as file:
        writer = csv.DictWriter(
            file,
            fieldnames=list(history[0].keys()),
        )
        writer.writeheader()
        writer.writerows(history)


# ============================================================
# 8. VE DO THI TRAINING
# ============================================================

def plot_training_history(history, save_path):
    if not history:
        print("Khong co history de ve do thi.")
        return

    epochs = [row["epoch"] for row in history]

    train_loss = [row["train_loss"] for row in history]
    val_loss = [row["val_loss"] for row in history]

    train_acc = [
        row["train_accuracy"] * 100 for row in history
    ]
    val_acc = [
        row["val_accuracy"] * 100 for row in history
    ]

    fig, axes = plt.subplots(1, 2, figsize=(14, 5))

    # Loss
    axes[0].plot(
        epochs, train_loss,
        marker="o", label="Train loss"
    )
    axes[0].plot(
        epochs, val_loss,
        marker="o", label="Validation loss"
    )
    axes[0].set_title("Training and Validation Loss")
    axes[0].set_xlabel("Epoch")
    axes[0].set_ylabel("Loss")
    axes[0].grid(True, alpha=0.3)
    axes[0].legend()

    # Accuracy
    axes[1].plot(
        epochs, train_acc,
        marker="o", label="Train accuracy"
    )
    axes[1].plot(
        epochs, val_acc,
        marker="o", label="Validation accuracy"
    )
    axes[1].set_title("Training and Validation Accuracy")
    axes[1].set_xlabel("Epoch")
    axes[1].set_ylabel("Accuracy (%)")
    axes[1].grid(True, alpha=0.3)
    axes[1].legend()

    fig.suptitle("ResNet-34 Training Curves")
    fig.tight_layout()
    fig.savefig(save_path, dpi=200, bbox_inches="tight")
    plt.close(fig)

    print("Da luu do thi:", save_path)


# ============================================================
# 9. CAP NHAT CAC FILE MAC DINH
# ============================================================

def publish_latest_results():
    """
    Chi cap nhat file mac dinh sau khi train hoan tat.
    Cac run cu van duoc giu nguyen trong runs/.
    """
    file_pairs = [
        (BEST_LOSS_PATH, LATEST_BEST_LOSS),
        (BEST_ACC_PATH, LATEST_BEST_ACC),
        (HISTORY_PATH, LATEST_HISTORY),
        (CONFIG_PATH, LATEST_CONFIG),
        (PLOT_PATH, LATEST_PLOT),
    ]

    for source, destination in file_pairs:
        if not source.is_file():
            raise FileNotFoundError(
                f"Khong tim thay ket qua: {source}"
            )
        shutil.copy2(source, destination)


# ============================================================
# 10. MAIN
# ============================================================

def main():
    set_seed(SEED)

    device = torch.device(
        "cuda" if torch.cuda.is_available() else "cpu"
    )

    print("=" * 70)
    print("RESNET-34 - TWO-STAGE FINE-TUNING")
    print("=" * 70)
    print("Device:", device)
    print("Run ID:", RUN_ID)
    print("Run directory:", RUN_DIR)
    print("Dataset:", PROJECT_ROOT / "data" / "resnet_dataset")

    if device.type == "cuda":
        print("GPU:", torch.cuda.get_device_name(0))
        print(
            "VRAM:",
            round(
                torch.cuda.get_device_properties(
                    0
                ).total_memory / (1024 ** 3),
                2,
            ),
            "GB",
        )

    # Lay train, validation va test loaders.
    # Test loader khong duoc dung de chon checkpoint.
    train_loader, valid_loader, test_loader = create_loaders()

    if len(train_loader.dataset) == 0:
        raise RuntimeError("Tap train dang rong.")

    if len(valid_loader.dataset) == 0:
        raise RuntimeError("Tap validation dang rong.")

    print("\nTrain images:", len(train_loader.dataset))
    print("Validation images:", len(valid_loader.dataset))
    print("Test images:", len(test_loader.dataset))
    print("Classes:", CLASSES)

    model = build_model(len(CLASSES)).to(device)

    criterion = nn.CrossEntropyLoss(
        label_smoothing=LABEL_SMOOTHING,
    )

    scaler = torch.amp.GradScaler(
        "cuda",
        enabled=(device.type == "cuda"),
    )

    # Khoi dong giai doan 1
    stage = 1
    freeze_backbone(model)

    optimizer = build_optimizer(model, stage)
    scheduler = build_scheduler(optimizer)

    best_val_loss = float("inf")
    best_val_loss_epoch = 0

    best_val_accuracy = -1.0
    best_val_acc_epoch = 0

    epochs_without_improvement = 0
    history = []

    for epoch in range(1, MAX_EPOCHS + 1):

        # Chuyen sang fine-tune layer4 sau HEAD_EPOCHS
        if epoch == HEAD_EPOCHS + 1:
            stage = 2
            unfreeze_layer4(model)

            optimizer = build_optimizer(model, stage)
            scheduler = build_scheduler(optimizer)

            print("\n" + "=" * 70)
            print("CHUYEN SANG GIAI DOAN 2")
            print("Mo layer4 va classifier")
            print("Layer4 LR:", BACKBONE_LR)
            print("Classifier LR:", CLASSIFIER_LR)
            print("=" * 70)

        train_loss, train_acc = run_epoch(
            model=model,
            loader=train_loader,
            criterion=criterion,
            device=device,
            optimizer=optimizer,
            scaler=scaler,
            stage=stage,
        )

        val_loss, val_acc = run_epoch(
            model=model,
            loader=valid_loader,
            criterion=criterion,
            device=device,
            stage=stage,
        )

        scheduler.step(val_loss)

        row = {
            "epoch": epoch,
            "stage": stage,
            "train_loss": train_loss,
            "train_accuracy": train_acc,
            "val_loss": val_loss,
            "val_accuracy": val_acc,
            "learning_rate_backbone": (
                optimizer.param_groups[0]["lr"]
                if stage == 2
                else 0.0
            ),
            "learning_rate_classifier": (
                optimizer.param_groups[-1]["lr"]
            ),
        }

        history.append(row)

        print("\n" + "-" * 70)
        print(f"Epoch [{epoch}/{MAX_EPOCHS}] | Stage {stage}")
        print(
            f"Train loss={train_loss:.4f} | "
            f"Train acc={train_acc * 100:.2f}%"
        )
        print(
            f"Val loss={val_loss:.4f} | "
            f"Val acc={val_acc * 100:.2f}%"
        )
        print(
            "LR:",
            [f"{group['lr']:.2e}" for group in optimizer.param_groups],
        )

        # Luu checkpoint theo validation loss
        if val_loss < best_val_loss - MIN_DELTA:
            best_val_loss = val_loss
            best_val_loss_epoch = epoch
            epochs_without_improvement = 0

            save_checkpoint(
                BEST_LOSS_PATH,
                model,
                epoch,
                val_loss,
                val_acc,
                stage,
            )

            print("Da luu best checkpoint theo val_loss.")

        else:
            epochs_without_improvement += 1

        # Luu checkpoint theo validation accuracy
        if val_acc > best_val_accuracy:
            best_val_accuracy = val_acc
            best_val_acc_epoch = epoch

            save_checkpoint(
                BEST_ACC_PATH,
                model,
                epoch,
                val_loss,
                val_acc,
                stage,
            )

            print("Da luu best checkpoint theo val_accuracy.")

        # Luu history moi epoch de tranh mat du lieu neu dung som
        save_history(history)

        # Cap nhat do thi trong thu muc run hien tai
        plot_training_history(history, PLOT_PATH)

        if epochs_without_improvement >= PATIENCE:
            print(
                "\nEarly stopping: validation loss "
                f"khong cai thien du {PATIENCE} epoch."
            )
            break

    # Luu cau hinh cua lan train nay
    config = {
        "run_id": RUN_ID,
        "run_dir": str(RUN_DIR),
        "model": "ResNet-34",
        "training_strategy": "two_stage_finetuning",
        "classes": list(CLASSES),
        "dataset_dir": str(
            PROJECT_ROOT / "data" / "resnet_dataset"
        ),
        "train_images": len(train_loader.dataset),
        "validation_images": len(valid_loader.dataset),
        "test_images": len(test_loader.dataset),
        "max_epochs": MAX_EPOCHS,
        "epochs_completed": len(history),
        "head_epochs": HEAD_EPOCHS,
        "head_lr": HEAD_LR,
        "backbone_lr": BACKBONE_LR,
        "classifier_lr": CLASSIFIER_LR,
        "weight_decay": WEIGHT_DECAY,
        "dropout": DROPOUT,
        "label_smoothing": LABEL_SMOOTHING,
        "patience": PATIENCE,
        "min_delta": MIN_DELTA,
        "seed": SEED,
        "best_val_loss": best_val_loss,
        "best_val_loss_epoch": best_val_loss_epoch,
        "best_val_accuracy": best_val_accuracy,
        "best_val_accuracy_epoch": best_val_acc_epoch,
        "best_loss_checkpoint": str(BEST_LOSS_PATH),
        "best_accuracy_checkpoint": str(BEST_ACC_PATH),
        "training_history": str(HISTORY_PATH),
        "training_plot": str(PLOT_PATH),
    }

    with open(CONFIG_PATH, "w", encoding="utf-8") as file:
        json.dump(
            config,
            file,
            indent=4,
            ensure_ascii=False,
        )

    # Chi cap nhat file mac dinh khi toan bo qua trinh thanh cong
    publish_latest_results()

    print("\n" + "=" * 70)
    print("HUAN LUYEN HOAN TAT")
    print(
        f"Best val loss: {best_val_loss:.4f} "
        f"(Epoch {best_val_loss_epoch})"
    )
    print(
        f"Best val accuracy: {best_val_accuracy * 100:.2f}% "
        f"(Epoch {best_val_acc_epoch})"
    )
    print("\nKet qua rieng cua lan train:")
    print("Run directory:", RUN_DIR)
    print("Checkpoint loss:", BEST_LOSS_PATH)
    print("Checkpoint accuracy:", BEST_ACC_PATH)
    print("History:", HISTORY_PATH)
    print("Config:", CONFIG_PATH)
    print("Plot:", PLOT_PATH)

    print("\nFile mac dinh da cap nhat:")
    print("Checkpoint loss:", LATEST_BEST_LOSS)
    print("Checkpoint accuracy:", LATEST_BEST_ACC)
    print("History:", LATEST_HISTORY)
    print("Config:", LATEST_CONFIG)
    print("Plot:", LATEST_PLOT)
    print("=" * 70)


if __name__ == "__main__":
    main()
