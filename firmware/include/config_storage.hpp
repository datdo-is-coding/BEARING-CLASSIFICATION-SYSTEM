#ifndef CONFIG_STORAGE_HPP
#define CONFIG_STORAGE_HPP

#include <Arduino.h>
#include <Preferences.h>
#include "hardware_config.h"

// Cấu trúc tọa độ 3D
struct Point3D {
    int16_t x;
    int16_t y;
    int16_t z;
};

// Cấu trúc toàn bộ cấu hình hệ thống lưu trong Flash (5 Teach Points)
struct SystemConfig {
    uint8_t conveyorSpeed;     // Tốc độ băng tải (0 - 255)
    uint8_t activeSensorPin;   // Cảm biến được chọn (CB1 = 18, CB2 = 19)
    Point3D startPoint;        // Điểm 1: Vị trí bắt đầu (Start / Home)
    Point3D pickPoint;         // Điểm 2: Vị trí gắp (Pick Point)
    Point3D placePoint1;       // Điểm 3: Thả 1 (Gỉ sét - Rusted)
    Point3D placePoint2;       // Điểm 4: Thả 2 (Hư nắp che - Damaged Shield)
    Point3D placePoint3;       // Điểm 5: Thả 3 (Trầy xước - Surface Damage)
};

extern SystemConfig currentConfig;

// =============================================================================
// CÁC HÀM QUẢN LÝ FLASH QUA PREFERENCES
// =============================================================================
void configStorageInit();
void saveAllConfig();
void resetDefaultConfig();

void saveConveyorSpeed(uint8_t speed);
void saveActiveSensor(uint8_t sensorPin);
void saveStartCoordinate(int16_t x, int16_t y, int16_t z);
void savePickCoordinate(int16_t x, int16_t y, int16_t z);
void savePlaceCoordinate(uint8_t index, int16_t x, int16_t y, int16_t z); // index: 1, 2, 3

Point3D getStartCoordinate();
Point3D getPickCoordinate();
Point3D getPlaceCoordinate(uint8_t index); // index: 1, 2, 3

void printConfigStorage();

#endif // CONFIG_STORAGE_HPP
