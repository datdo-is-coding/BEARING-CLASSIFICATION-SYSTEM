
const names = {
    OK: "Đạt chuẩn",
    NG_RI_SET: "Rỉ sét",
    NG_MAT_NAP: "Mất nắp",
    NG_HU_HAI_BE_MAT: "Hư hại bề mặt"
};

const colors = {
    OK: "#4bdf9b",
    NG_RI_SET: "#fb7185",
    NG_MAT_NAP: "#f5ad5b",
    NG_HU_HAI_BE_MAT: "#a78bfa"
};

const $ = id => document.getElementById(id);

let running = false;

function safe(value) {
    return String(value).replace(
        /[&<>"']/g,
        c => ({
            "&": "&amp;",
            "<": "&lt;",
            ">": "&gt;",
            '"': "&quot;",
            "'": "&#39;"
        }[c])
    );
}

function setText(id, value) {
    const element = $(id);
    if (element) {
        element.textContent = value;
    }
}

function setOnline(id, online) {
    const element = $(id);

    if (element) {
        element.classList.toggle("online", online);
        element.style.backgroundColor =
            online ? "#34d399" : "#64748b";
    }
}

function updateRunState(isRunning) {
    running = isRunning;

    const label = $("run-label");
    const sub = $("run-sub");
    const dot = $("run-dot");

    if (label) {
        label.textContent = running
            ? "Đang hoạt động"
            : "Đang dừng";

        label.style.color = running
            ? "#34d399"
            : "#ffffff";
    }

    if (sub) {
        sub.textContent = running
            ? "Hệ thống đang phân loại vòng bi"
            : "Nhấn Bắt đầu để vận hành";
    }

    if (dot) {
        dot.classList.toggle("online", running);

        dot.style.backgroundColor = running
            ? "#34d399"
            : "#64748b";

        dot.style.boxShadow = running
            ? "0 0 12px rgba(52,211,153,0.65)"
            : "none";
    }
}

async function refresh() {
    try {
        const response = await fetch("/api/status", {
            cache: "no-store"
        });

        if (!response.ok) {
            throw new Error("Không tải được trạng thái");
        }

        const data = await response.json();
        const state = data.state || {};

        updateRunState(Boolean(state.running));

        const devices = [
            ["ai", state.ai_online],
            ["cam", state.camera_online],
            ["esp", state.esp_online]
        ];

        for (const [id, online] of devices) {
            setOnline("dot-" + id, Boolean(online));

            setText(
                id + "-status",
                online ? "Giả lập đang chạy" : "Chưa kết nối"
            );
        }

        const total = Number(data.total) || 0;
        const ok = Number(data.ok) || 0;
        const ng = Number(data.ng) || 0;

        setText("total", total);
        setText("ok", ok);
        setText("ng", ng);

        const rate = total > 0
            ? (ok / total * 100).toFixed(1)
            : "0.0";

        setText("rate", rate + "%");

        const last = state.last_result;

        setText(
            "last-result",
            last
                ? (names[last.category] || last.category)
                : "Chưa có dữ liệu"
        );

        setText(
            "last-confidence",
            last
                ? (Number(last.confidence) * 100).toFixed(1) + "%"
                : "—"
        );

        const bars = $("bars");
        const counts = data.counts || {};

        if (bars) {
            bars.innerHTML = Object.entries(names)
                .map(([key, label]) => {
                    const count = Number(counts[key]) || 0;

                    const percent = total > 0
                        ? count / total * 100
                        : 0;

                    return `
                        <div>
                            <div class="bar-top">
                                <span>${label}</span>
                                <span>${count} sản phẩm</span>
                            </div>
                            <div class="track">
                                <div class="fill"
                                     style="width:${percent}%;
                                     background:${colors[key]}">
                                </div>
                            </div>
                        </div>
                    `;
                }).join("");
        }

        const history = $("history");
        const items = data.history || [];

        if (history) {
            history.innerHTML = items.length
                ? items.map(item => `
                    <tr>
                        <td>${safe(
                            String(item.timestamp || "")
                                .replace("T", " ")
                        )}</td>
                        <td>
                            <span class="chip ${
                                item.category === "OK"
                                    ? "OK" : "NG"
                            }">
                                ${
                                    names[item.category] ||
                                    safe(item.category)
                                }
                            </span>
                        </td>
                        <td>${
                            (
                                Number(item.confidence) * 100
                            ).toFixed(1)
                        }%</td>
                        <td>${safe(item.source || "")}</td>
                    </tr>
                `).join("")
                : `
                    <tr>
                        <td colspan="4">
                            Chưa có dữ liệu
                        </td>
                    </tr>
                `;
        }

        const cameraImg = $("camera-img");
        const placeholder = $("camera-placeholder");

        if (cameraImg && placeholder) {
            if (state.camera_online) {
                cameraImg.hidden = false;
                placeholder.hidden = true;

                cameraImg.src =
                    "/api/camera?t=" + Date.now();

                cameraImg.onerror = () => {
                    cameraImg.hidden = true;
                    placeholder.hidden = false;
                };
            } else {
                cameraImg.hidden = true;
                placeholder.hidden = false;
            }
        }

        setText(
            "camera-tag",
            state.camera_online
                ? "CAMERA THỰC"
                : "CHƯA KẾT NỐI"
        );

    } catch (error) {
        setText(
            "feedback",
            "Lỗi kết nối máy chủ: " + error.message
        );
        console.error(error);
    }
}

async function control(action) {
    try {
        setText("feedback", "Đang xử lý lệnh...");

        const response = await fetch("/api/control", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ action })
        });

        const data = await response.json();

        if (!response.ok || data.ok === false) {
            throw new Error(
                data.error || "Không thực hiện được lệnh"
            );
        }

        setText(
            "feedback",
            data.message || "Đã thực hiện: " + action
        );

        await refresh();

    } catch (error) {
        setText(
            "feedback",
            "Lỗi điều khiển: " + error.message
        );
    }
}

function resetCounter() {
    if (confirm("Bạn có chắc muốn xóa toàn bộ thống kê?")) {
        control("reset");
    }
}

function clock() {
    setText(
        "clock",
        new Date().toLocaleString("vi-VN")
    );
}

clock();
refresh();

setInterval(clock, 1000);
setInterval(refresh, 2500);
