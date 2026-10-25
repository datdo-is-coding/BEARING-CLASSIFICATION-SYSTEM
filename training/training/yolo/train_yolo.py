
from pathlib import Path
import torch
from ultralytics import YOLO


def main():
    # ==========================================
    # 1. XAC DINH DUONG DAN DU AN
    # ==========================================
    project_dir = Path(__file__).resolve().parents[2]
    data_yaml = project_dir / "data" / "yolo_dataset" / "data.yaml"
    experiment_dir = project_dir / "experiments" / "yolo"

    # ==========================================
    # 2. KIEM TRA DATASET
    # ==========================================
    if not data_yaml.exists():
        raise FileNotFoundError(
            f"Khong tim thay data.yaml: {data_yaml}"
        )

    required_dirs = [
        "train/images",
        "train/labels",
        "valid/images",
        "valid/labels",
        "test/images",
        "test/labels",
    ]

    dataset_dir = project_dir / "data" / "yolo_dataset"

    for relative_dir in required_dirs:
        folder = dataset_dir / relative_dir
        if not folder.is_dir():
            raise FileNotFoundError(
                f"Khong tim thay thu muc: {folder}"
            )

    # ==========================================
    # 3. KIEM TRA GPU
    # ==========================================
    print("=" * 60)
    print("HUAN LUYEN YOLOv8n")
    print("=" * 60)
    print(f"PyTorch: {torch.__version__}")
    print(f"CUDA: {torch.cuda.is_available()}")

    if not torch.cuda.is_available():
        raise RuntimeError(
            "PyTorch chua nhan CUDA. "
            "Hay sua moi truong GPU truoc khi huan luyen."
        )

    print(f"GPU: {torch.cuda.get_device_name(0)}")
    print(f"Dataset: {data_yaml}")

    # ==========================================
    # 4. TAI MO HINH
    # ==========================================
    # Phai khoi tao model truoc khi goi model.train()
    pretrained_weights = project_dir / "weights" / "yolov8n.pt"

    if not pretrained_weights.is_file():
        raise FileNotFoundError(
            f"Khong tim thay trong so YOLOv8n: {pretrained_weights}"
        )

    model = YOLO(str(pretrained_weights))
    

    # ==========================================
    # 5. HUAN LUYEN TREN RTX 2050
    # ==========================================
    results = model.train(
        data=str(data_yaml),
        epochs=100,
        imgsz=640,
        batch=4,
        device=0,
        workers=2,
        amp=True,
        cache=False,
        patience=20,
        project=str(experiment_dir),
        name="yolo_v8n_rtx2050",
        exist_ok=False,
        save=True,
        plots=True,
        seed=42,
        deterministic=True,
        close_mosaic=10,
    )

    # ==========================================
    # 6. THONG BAO KET QUA
    # ==========================================
    save_dir = Path(results.save_dir)

    print("\n" + "=" * 60)
    print("HUAN LUYEN HOAN TAT")
    print("=" * 60)
    print(f"Thu muc ket qua: {save_dir}")
    print(f"Best weights: {save_dir / 'weights' / 'best.pt'}")
    print(f"Last weights: {save_dir / 'weights' / 'last.pt'}")


if __name__ == "__main__":
    main()
