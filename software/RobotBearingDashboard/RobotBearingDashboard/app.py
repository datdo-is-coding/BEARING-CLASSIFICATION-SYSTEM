import os
import sqlite3
import threading
import random
import time
import json
from pathlib import Path
from datetime import datetime
from flask import Flask, render_template, jsonify, request

BASE_DIR = Path(__file__).resolve().parent
DB_PATH = BASE_DIR / "bearing.db"
CONFIG_PATH = BASE_DIR / "config.json"

app = Flask(__name__)
lock = threading.RLock()

config = {
    "flask": {"host": "0.0.0.0", "port": 5000},
    "system": {"mode": "simulation", "simulation_interval": 3}
}

if CONFIG_PATH.exists():
    with open(CONFIG_PATH, "r", encoding="utf-8") as f:
        loaded = json.load(f)
    for key, value in loaded.items():
        if isinstance(value, dict) and isinstance(config.get(key), dict):
            config[key].update(value)
        else:
            config[key] = value

HOST = config["flask"]["host"]
PORT = int(config["flask"]["port"])
MODE = config["system"].get("mode", "simulation").lower()
SIM_INTERVAL = float(config["system"].get("simulation_interval", 3))
API_KEY = os.environ.get("BEARING_API_KEY")

if MODE not in ("simulation", "real"):
    raise ValueError("Mode must be simulation or real")

if SIM_INTERVAL <= 0:
    raise ValueError("Simulation interval must be positive")

CLASSES = {
    "OK": "Đạt chuẩn",
    "NG_RI_SET": "Rỉ sét",
    "NG_MAT_NAP": "Mất nắp",
    "NG_HU_HAI_BE_MAT": "Hư hại bề mặt"
}

state = {
    "running": False,
    "ai_online": False,
    "esp_online": False,
    "camera_online": False,
    "last_result": None,
    "last_command": None,
    "updated_at": None
}


def now():
    return datetime.now().isoformat(timespec="seconds")


def get_db():
    conn = sqlite3.connect(str(DB_PATH), timeout=10)
    conn.row_factory = sqlite3.Row
    return conn


def init_db():
    with get_db() as conn:
        conn.execute("""
            CREATE TABLE IF NOT EXISTS detections (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                timestamp TEXT NOT NULL,
                category TEXT NOT NULL,
                confidence REAL NOT NULL,
                source TEXT NOT NULL
            )
        """)


def save_detection(category, confidence, source):
    timestamp = now()

    with get_db() as conn:
        cursor = conn.execute("""
            INSERT INTO detections
            (timestamp, category, confidence, source)
            VALUES (?, ?, ?, ?)
        """, (timestamp, category, confidence, source))
        record_id = cursor.lastrowid

    result = {
        "id": record_id,
        "category": category,
        "confidence": confidence,
        "timestamp": timestamp,
        "source": source
    }

    state["last_result"] = result
    state["updated_at"] = timestamp
    return result


def simulation_worker():
    categories = list(CLASSES.keys())
    weights = [70, 12, 10, 8]

    while True:
        time.sleep(SIM_INTERVAL)

        with lock:
            if MODE != "simulation" or not state["running"]:
                continue

            category = random.choices(
                categories, weights=weights, k=1
            )[0]

            confidence = round(random.uniform(0.85, 0.99), 4)
            result = save_detection(category, confidence, "SIMULATION")

            state["ai_online"] = True
            state["esp_online"] = True

            print("[SIMULATION]", result["category"], result["confidence"])


@app.get("/")
def index():
    return render_template("index.html")


@app.get("/api/status")
def api_status():
    with lock:
        snapshot = dict(state)

        with get_db() as conn:
            rows = conn.execute("""
                SELECT category, COUNT(*) AS n
                FROM detections
                GROUP BY category
            """).fetchall()

            history = [
                dict(row)
                for row in conn.execute("""
                    SELECT *
                    FROM detections
                    ORDER BY id DESC
                    LIMIT 60
                """).fetchall()
            ]

    counts = {key: 0 for key in CLASSES}

    for row in rows:
        if row["category"] in counts:
            counts[row["category"]] = row["n"]

    total = sum(counts.values())
    ok = counts["OK"]

    return jsonify({
        "state": snapshot,
        "counts": counts,
        "total": total,
        "ok": ok,
        "ng": total - ok,
        "history": history,
        "classes": CLASSES,
        "mode": MODE
    })


@app.post("/api/control")
def api_control():
    data = request.get_json(silent=True) or {}
    action = str(data.get("action", "")).lower()

    if action not in ("start", "stop", "reset"):
        return jsonify(error="Lệnh không hợp lệ"), 400

    with lock:
        if action == "start":
            state["running"] = True

            if MODE == "simulation":
                state["ai_online"] = True
                state["esp_online"] = True

        elif action == "stop":
            state["running"] = False

            if MODE == "simulation":
                state["ai_online"] = False
                state["esp_online"] = False

        elif action == "reset":
            if state["running"]:
                return jsonify(error="Hãy dừng trước khi reset"), 409

            with get_db() as conn:
                conn.execute("DELETE FROM detections")

            state["last_result"] = None

        state["last_command"] = {"action": action}
        state["updated_at"] = now()
        snapshot = dict(state)

    return jsonify(ok=True, state=snapshot, mode=MODE)


def check_api_key():
    return bool(API_KEY) and request.headers.get("X-API-Key") == API_KEY


@app.post("/api/detection")
def api_detection():
    if MODE != "real":
        return jsonify(error="Đang ở chế độ giả lập"), 409

    if not check_api_key():
        return jsonify(error="API key không hợp lệ"), 401

    data = request.get_json(silent=True) or {}
    category = data.get("category")

    if category not in CLASSES:
        return jsonify(error="Nhãn không hợp lệ"), 400

    try:
        confidence = float(data.get("confidence"))
    except (TypeError, ValueError):
        return jsonify(error="Confidence không hợp lệ"), 400

    if not 0 <= confidence <= 1:
        return jsonify(error="Confidence phải từ 0 đến 1"), 400

    with lock:
        if not state["running"]:
            return jsonify(error="Hệ thống đang dừng"), 409

        result = save_detection(category, confidence, "AI_CAMERA")
        state["ai_online"] = True

    return jsonify(ok=True, result=result)


@app.post("/api/heartbeat")
def api_heartbeat():
    if MODE != "real":
        return jsonify(error="Đang ở chế độ giả lập"), 409

    if not check_api_key():
        return jsonify(error="API key không hợp lệ"), 401

    with lock:
        state["ai_online"] = True
        state["updated_at"] = now()

    return jsonify(ok=True)


@app.get("/api/camera")
def api_camera():
    return jsonify(error="Camera chưa được tích hợp"), 404


init_db()

if MODE == "simulation":
    threading.Thread(target=simulation_worker, daemon=True).start()

if __name__ == "__main__":
    print("ROBOT BEARING DASHBOARD")
    print("Mode:", MODE)
    print("Database:", DB_PATH)
    app.run(host=HOST, port=PORT, debug=False, threaded=True)