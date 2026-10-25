"""TODO: triển khai chức năng của module này."""

from pathlib import Path
from ultralytics import YOLO
import torch


def main():
    project_dir = Path(__file__).resolve().parents[2]

    # Neu thu muc train co them so, sua duong dan nay
    weights = (
        project_dir
        / "experiments"
        / "yolo"
        / "yolo_v8n_rtx2050"
        / "weights"
        / "best.pt"
    )

    data_yaml = (
        project_dir
        / "data"
        / "yolo_dataset"
        / "data.yaml"
    )

    if not weights.exists():
        raise FileNotFoundError(
            f"Khong tim thay best.pt: {weights}"
        )

    if not data_yaml.exists():
        raise FileNotFoundError(
            f"Khong tim thay data.yaml: {data_yaml}"
        )

    if not torch.cuda.is_available():
        raise RuntimeError(
            "CUDA chua san sang. Kiem tra PyTorch/GPU."
        )

    print("GPU:", torch.cuda.get_device_name(0))
    print("Weights:", weights)

    model = YOLO(str(weights))

    metrics = model.val(
        data=str(data_yaml),
        split="val",
        imgsz=640,
        batch=4,
        device=0,
        workers=2,
        plots=True,
        project=str(project_dir / "experiments" / "yolo"),
        name="yolo_v8n_validation",
        exist_ok=False,
    )

    print("\n===== KET QUA VALIDATION =====")
    print(f"Precision: {metrics.box.mp:.4f}")
    print(f"Recall:    {metrics.box.mr:.4f}")
    print(f"mAP50:     {metrics.box.map50:.4f}")
    print(f"mAP50-95:  {metrics.box.map:.4f}")


if __name__ == "__main__":
    main()
