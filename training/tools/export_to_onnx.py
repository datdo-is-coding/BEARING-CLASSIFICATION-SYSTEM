
from pathlib import Path
import torch
import torch.nn as nn
from torchvision import models
from ultralytics import YOLO

PROJECT_DIR = Path(__file__).resolve().parents[1]
CSHARP_DIR = PROJECT_DIR / "csharp"
WEIGHTS_ONNX_DIR = PROJECT_DIR / "weights" / "onnx"

CSHARP_DIR.mkdir(parents=True, exist_ok=True)
WEIGHTS_ONNX_DIR.mkdir(parents=True, exist_ok=True)


def export_yolo():
    print("=" * 60)
    print("1. XUAT MODEL YOLOv8 SANG ONNX")
    print("=" * 60)
    
    yolo_candidates = list(
        (PROJECT_DIR / "experiments" / "yolo").glob("*/weights/best.pt")
    )
    if not yolo_candidates:
        raise FileNotFoundError("Khong tim thay checkpoint YOLO best.pt")
    
    best_yolo = max(yolo_candidates, key=lambda p: p.stat().st_mtime)
    print(f"YOLO weights nguon: {best_yolo}")
    
    model = YOLO(str(best_yolo))
    exported_path = model.export(
        format="onnx",
        imgsz=640,
        dynamic=False,
        opset=12,
        simplify=True,
    )
    
    dest_csharp = CSHARP_DIR / "yolo_bearing.onnx"
    dest_weights = WEIGHTS_ONNX_DIR / "yolo_bearing.onnx"
    
    import shutil
    shutil.copyfile(exported_path, dest_csharp)
    Path(exported_path).replace(dest_weights)
    
    print(f"[OK] Da xuat YOLO ONNX tai: {dest_csharp}")
    print(f"[OK] Da sao chep tai: {dest_weights}")
    return dest_csharp


def export_resnet():
    print("\n" + "=" * 60)
    print("2. XUAT MODEL RESNET-34 SANG ONNX")
    print("=" * 60)
    
    resnet_weights = PROJECT_DIR / "weights" / "resnet34" / "best_resnet34.pth"
    if not resnet_weights.is_file():
        raise FileNotFoundError(f"Khong tim thay ResNet checkpoint: {resnet_weights}")
        
    print(f"ResNet weights nguon: {resnet_weights}")
    
    # Khoi tao model voi 4 classes
    model = models.resnet34(weights=None)
    model.fc = nn.Sequential(
        nn.Dropout(p=0.3),
        nn.Linear(model.fc.in_features, 4),
    )
    
    checkpoint = torch.load(resnet_weights, map_location="cpu", weights_only=True)
    if isinstance(checkpoint, dict) and "model_state_dict" in checkpoint:
        state_dict = checkpoint["model_state_dict"]
    elif isinstance(checkpoint, dict) and "state_dict" in checkpoint:
        state_dict = checkpoint["state_dict"]
    else:
        state_dict = checkpoint

    state_dict = {
        k.removeprefix("module."): v for k, v in state_dict.items()
    }
    model.load_state_dict(state_dict, strict=True)
    model.eval()
    
    dummy_input = torch.randn(1, 3, 224, 224, requires_grad=False)
    dest_csharp = CSHARP_DIR / "resnet34_bearing.onnx"
    dest_weights = WEIGHTS_ONNX_DIR / "resnet34_bearing.onnx"
    
    torch.onnx.export(
        model,
        dummy_input,
        str(dest_csharp),
        export_params=True,
        opset_version=12,
        do_constant_folding=True,
        input_names=["input"],
        output_names=["output"],
        dynamic_axes=None,
    )
    import shutil
    shutil.copyfile(dest_csharp, dest_weights)

    print(f"[OK] Da xuat ResNet-34 ONNX tai: {dest_csharp}")
    print(f"[OK] Da sao chep tai: {dest_weights}")
    return dest_csharp


def main():
    print("BAT DAU XUAT CAC MODEL CHO C# WINFORMS ONNX RUNTIME")
    export_yolo()
    export_resnet()
    print("\n" + "=" * 60)
    print(f"HOAN TAT! Ca 2 file ONNX da duoc luu tai tap: {CSHARP_DIR}")
    print("1. csharp/yolo_bearing.onnx     (Input: 1x3x640x640)")
    print("2. csharp/resnet34_bearing.onnx (Input: 1x3x224x224)")
    print("=" * 60)


if __name__ == "__main__":
    main()
