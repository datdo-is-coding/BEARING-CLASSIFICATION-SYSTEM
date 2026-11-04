#ifndef HARDWARE_CONFIG_H
#define HARDWARE_CONFIG_H

#include <Arduino.h>

// =============================================================================
// CẤU HÌNH CHÂN PHẦN CỨNG (HARDWARE PINOUT DEFINITION)
// Theo sơ đồ mạch nguyên lý PCB ESP32-WROOM-32D của hệ thống
// =============================================================================

// 1. ĐIỀU KHIỂN MOTOR BĂNG TẢI (PWM ĐIỀU TỐC)
#define PIN_MOTOR_PWM       16      // GPIO16 - Net PWM_MOTOR trên PCB
#define MOTOR_PWM_CHANNEL    0      // Kênh LEDC PWM của ESP32 (0-15)
#define MOTOR_PWM_FREQ    5000      // Tần số PWM 5kHz
#define MOTOR_PWM_RES        8      // Độ phân giải 8-bit (0 - 255)
#define MOTOR_DEFAULT_SPEED 180     // Tốc độ mặc định khi chạy băng tải

// 2. ĐIỀU KHIỂN NAM CHÂM ĐIỆN (ELECTROMAGNET)
#define PIN_MAGNET          17      // GPIO17 - Net PWM_MAG / OUT_MAG trên PCB
#define MAGNET_ON         HIGH      // Mức kích hoạt nam châm
#define MAGNET_OFF         LOW

// 3. CẢM BIẾN QUANG PHÁT HIỆN VẬT (OPTICAL SENSOR)
#define CB1                 18      // GPIO18 - Cảm biến quang 1 (IN1 trên PCB)
#define CB2                 19      // GPIO19 - Cảm biến quang 2 (IN2 trên PCB)
#define PIN_SENSOR_1        CB1
#define PIN_SENSOR_2        CB2

// Cảm biến được chọn ban đầu để kích hoạt dừng băng tải (Mặc định chọn CB1)
#define DEFAULT_ACTIVE_CB   CB1

// Trạng thái cảm biến:
// Bình thường (không có vòng bi): Chân ở mức HIGH
// Khi có vòng bi đi qua: Chân bị kéo xuống mức LOW (sườn âm HIGH -> LOW)
#define SENSOR_NORMAL_LEVEL HIGH
#define SENSOR_ACTIVE_LEVEL LOW     // Mức LOW = Phát hiện có vòng bi     

// 4. GIAO TIẾP PCA9685 CHO ROBOT 4-DOF
// GPIO25 = SDA, GPIO26 = SCL (được định nghĩa trong control.hpp)

#endif // HARDWARE_CONFIG_H
