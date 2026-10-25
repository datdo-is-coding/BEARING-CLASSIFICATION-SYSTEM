"""TODO: triển khai chức năng của module này."""

from pathlib import Path

import torch
from torch.utils.data import DataLoader
from torchvision import datasets, transforms
import sys

if hasattr(sys.stdout, "reconfigure"):
    sys.stdout.reconfigure(encoding="utf-8")
    
PROJECT_ROOT = Path(__file__).resolve().parents[2]
DATA_DIR = PROJECT_ROOT / "data" / "resnet_dataset"

IMAGE_SIZE = 224
BATCH_SIZE = 8
NUM_WORKERS = 0

CLASSES = [
    "damaged_shield",
    "normal",
    "rusted",
    "surface_damage",
]

MEAN = [0.485, 0.456, 0.406]
STD = [0.229, 0.224, 0.225]


def get_transforms():
    train_transform = transforms.Compose([
        transforms.Resize((IMAGE_SIZE, IMAGE_SIZE)),
        transforms.RandomHorizontalFlip(p=0.5),
        transforms.RandomRotation(degrees=8),
        transforms.ColorJitter(
            brightness=0.12,
            contrast=0.12,
            saturation=0.08,
        ),
        transforms.ToTensor(),
        transforms.Normalize(MEAN, STD),
    ])

    eval_transform = transforms.Compose([
        transforms.Resize((IMAGE_SIZE, IMAGE_SIZE)),
        transforms.ToTensor(),
        transforms.Normalize(MEAN, STD),
    ])

    return train_transform, eval_transform


def create_datasets():
    train_transform, eval_transform = get_transforms()

    train_dir = DATA_DIR / "train"
    valid_dir = DATA_DIR / "valid"
    test_dir = DATA_DIR / "test"

    for folder in (train_dir, valid_dir, test_dir):
        if not folder.exists():
            raise FileNotFoundError(
                f"Không tìm thấy thư mục dữ liệu: {folder}"
            )

    train_ds = datasets.ImageFolder(
        str(train_dir),
        transform=train_transform,
    )
    valid_ds = datasets.ImageFolder(
        str(valid_dir),
        transform=eval_transform,
    )
    test_ds = datasets.ImageFolder(
        str(test_dir),
        transform=eval_transform,
    )

    expected = {name: i for i, name in enumerate(CLASSES)}

    for split_name, ds in [
        ("train", train_ds),
        ("valid", valid_ds),
        ("test", test_ds),
    ]:
        if ds.class_to_idx != expected:
            raise ValueError(
                f"Thứ tự lớp ở {split_name} không đúng.\n"
                f"Đang có: {ds.class_to_idx}\n"
                f"Mong đợi: {expected}"
            )

    return train_ds, valid_ds, test_ds


def create_loaders():
    train_ds, valid_ds, test_ds = create_datasets()

    train_loader = DataLoader(
        train_ds,
        batch_size=BATCH_SIZE,
        shuffle=True,
        num_workers=NUM_WORKERS,
        pin_memory=torch.cuda.is_available(),
    )

    valid_loader = DataLoader(
        valid_ds,
        batch_size=BATCH_SIZE,
        shuffle=False,
        num_workers=NUM_WORKERS,
        pin_memory=torch.cuda.is_available(),
    )

    test_loader = DataLoader(
        test_ds,
        batch_size=BATCH_SIZE,
        shuffle=False,
        num_workers=NUM_WORKERS,
        pin_memory=torch.cuda.is_available(),
    )

    return train_loader, valid_loader, test_loader


if __name__ == "__main__":
    train_ds, valid_ds, test_ds = create_datasets()

    print("DATA DIRECTORY:", DATA_DIR)
    print("CLASS TO INDEX:", train_ds.class_to_idx)

    for name, ds in [
        ("TRAIN", train_ds),
        ("VALIDATION", valid_ds),
        ("TEST", test_ds),
    ]:
        counts = {c: 0 for c in CLASSES}

        for _, label in ds.samples:
            counts[CLASSES[label]] += 1

        print(f"\n{name}: {len(ds)} ảnh")
        for class_name, count in counts.items():
            print(f"  {class_name}: {count}")
