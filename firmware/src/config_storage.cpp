#include "config_storage.hpp"
#include "robot_system_flow.hpp"

static Preferences prefs;
static const char* PREF_NAMESPACE = "bearing_sys";

// Biến toàn cục lưu cấu hình hiện hành trong RAM (5 Teach Points)
SystemConfig currentConfig = {
    MOTOR_DEFAULT_SPEED, // conveyorSpeed (180)
    DEFAULT_ACTIVE_CB,   // activeSensorPin (CB1 = 18)
    { 164,   0, 104 },   // startPoint (Điểm 1: Start / Home)
    { 164,   0,  40 },   // pickPoint (Điểm 2: Gắp phôi)
    { 120, 120,  40 },   // placePoint1 (Điểm 3: Gỉ sét - Rusted)
    {   0, 160,  40 },   // placePoint2 (Điểm 4: Hư nắp che - Damaged Shield)
    {-120, 120,  40 }    // placePoint3 (Điểm 5: Trầy xước - Surface Damage)
};

// =============================================================================
// KHỞI TẠO VÀ NẠP CẤU HÌNH TỪ FLASH (GỌI TRONG SETUP)
// =============================================================================
void configStorageInit()
{
    prefs.begin(PREF_NAMESPACE, false);

    currentConfig.conveyorSpeed   = prefs.getUChar("conv_spd", MOTOR_DEFAULT_SPEED);
    currentConfig.activeSensorPin = prefs.getUChar("act_cb",   DEFAULT_ACTIVE_CB);

    // 1. Tọa độ Bắt Đầu (Start / Home)
    currentConfig.startPoint.x = prefs.getShort("st_x", 164);
    currentConfig.startPoint.y = prefs.getShort("st_y",   0);
    currentConfig.startPoint.z = prefs.getShort("st_z", 104);

    // 2. Tọa độ Gắp (Pick Point)
    currentConfig.pickPoint.x = prefs.getShort("pk_x", 164);
    currentConfig.pickPoint.y = prefs.getShort("pk_y",   0);
    currentConfig.pickPoint.z = prefs.getShort("pk_z",  40);

    // 3. Tọa độ Thả 1 (Gỉ sét)
    currentConfig.placePoint1.x = prefs.getShort("p1_x", 120);
    currentConfig.placePoint1.y = prefs.getShort("p1_y", 120);
    currentConfig.placePoint1.z = prefs.getShort("p1_z",  40);

    // 4. Tọa độ Thả 2 (Hư nắp che)
    currentConfig.placePoint2.x = prefs.getShort("p2_x",   0);
    currentConfig.placePoint2.y = prefs.getShort("p2_y", 160);
    currentConfig.placePoint2.z = prefs.getShort("p2_z",  40);

    // 5. Tọa độ Thả 3 (Trầy xước)
    currentConfig.placePoint3.x = prefs.getShort("p3_x", -120);
    currentConfig.placePoint3.y = prefs.getShort("p3_y",  120);
    currentConfig.placePoint3.z = prefs.getShort("p3_z",   40);

    prefs.end();

    Serial.println("[FLASH] >>> Da nap thanh cong 5 diem Teach tu Flash NVS <<<");
    printConfigStorage();
}

// =============================================================================
// LƯU TOÀN BỘ CẤU HÌNH HIỆN HÀNH VÀO FLASH
// =============================================================================
void saveAllConfig()
{
    prefs.begin(PREF_NAMESPACE, false);

    prefs.putUChar("conv_spd", currentConfig.conveyorSpeed);
    prefs.putUChar("act_cb",   currentConfig.activeSensorPin);

    // Start
    prefs.putShort("st_x", currentConfig.startPoint.x);
    prefs.putShort("st_y", currentConfig.startPoint.y);
    prefs.putShort("st_z", currentConfig.startPoint.z);

    // Pick
    prefs.putShort("pk_x", currentConfig.pickPoint.x);
    prefs.putShort("pk_y", currentConfig.pickPoint.y);
    prefs.putShort("pk_z", currentConfig.pickPoint.z);

    // Place 1
    prefs.putShort("p1_x", currentConfig.placePoint1.x);
    prefs.putShort("p1_y", currentConfig.placePoint1.y);
    prefs.putShort("p1_z", currentConfig.placePoint1.z);

    // Place 2
    prefs.putShort("p2_x", currentConfig.placePoint2.x);
    prefs.putShort("p2_y", currentConfig.placePoint2.y);
    prefs.putShort("p2_z", currentConfig.placePoint2.z);

    // Place 3
    prefs.putShort("p3_x", currentConfig.placePoint3.x);
    prefs.putShort("p3_y", currentConfig.placePoint3.y);
    prefs.putShort("p3_z", currentConfig.placePoint3.z);

    prefs.end();
    Serial.println("[FLASH] Da luu toan bo 5 diem Teach vao Flash thanh cong!");
}

// =============================================================================
// KHÔI PHỤC CẤU HÌNH MẶC ĐỊNH
// =============================================================================
void resetDefaultConfig()
{
    currentConfig.conveyorSpeed   = MOTOR_DEFAULT_SPEED;
    currentConfig.activeSensorPin = DEFAULT_ACTIVE_CB;

    currentConfig.startPoint  = { 164,   0, 104 };
    currentConfig.pickPoint   = { 164,   0,  40 };
    currentConfig.placePoint1 = { 120, 120,  40 };
    currentConfig.placePoint2 = {   0, 160,  40 };
    currentConfig.placePoint3 = {-120, 120,  40 };

    saveAllConfig();
    Serial.println("[FLASH] Da reset toan bo ve mac dinh nha san xuat!");
}

void saveConveyorSpeed(uint8_t speed)
{
    currentConfig.conveyorSpeed = speed;
    prefs.begin(PREF_NAMESPACE, false);
    prefs.putUChar("conv_spd", speed);
    prefs.end();
    Serial.printf("[FLASH] Da luu Toc do bang tai moi: %d/255\n", speed);
}

void saveActiveSensor(uint8_t sensorPin)
{
    currentConfig.activeSensorPin = sensorPin;
    setActiveSensor(sensorPin);
    prefs.begin(PREF_NAMESPACE, false);
    prefs.putUChar("act_cb", sensorPin);
    prefs.end();
    Serial.printf("[FLASH] Da luu Cam bien kick hoat: %s (GPIO%d)\n",
                  (sensorPin == CB1) ? "CB1" : "CB2", sensorPin);
}

void saveStartCoordinate(int16_t x, int16_t y, int16_t z)
{
    currentConfig.startPoint.x = x;
    currentConfig.startPoint.y = y;
    currentConfig.startPoint.z = z;

    prefs.begin(PREF_NAMESPACE, false);
    prefs.putShort("st_x", x);
    prefs.putShort("st_y", y);
    prefs.putShort("st_z", z);
    prefs.end();
    Serial.printf("[FLASH] Da luu Diem 1 (START/HOME): X=%d, Y=%d, Z=%d\n", x, y, z);
}

void savePickCoordinate(int16_t x, int16_t y, int16_t z)
{
    currentConfig.pickPoint.x = x;
    currentConfig.pickPoint.y = y;
    currentConfig.pickPoint.z = z;

    prefs.begin(PREF_NAMESPACE, false);
    prefs.putShort("pk_x", x);
    prefs.putShort("pk_y", y);
    prefs.putShort("pk_z", z);
    prefs.end();
    Serial.printf("[FLASH] Da luu Diem 2 (PICK/GAP): X=%d, Y=%d, Z=%d\n", x, y, z);
}

void savePlaceCoordinate(uint8_t index, int16_t x, int16_t y, int16_t z)
{
    prefs.begin(PREF_NAMESPACE, false);

    if (index == 1)
    {
        currentConfig.placePoint1 = { x, y, z };
        prefs.putShort("p1_x", x);
        prefs.putShort("p1_y", y);
        prefs.putShort("p1_z", z);
        Serial.printf("[FLASH] Da luu Diem 3 (THA 1 - GI SET): X=%d, Y=%d, Z=%d\n", x, y, z);
    }
    else if (index == 2)
    {
        currentConfig.placePoint2 = { x, y, z };
        prefs.putShort("p2_x", x);
        prefs.putShort("p2_y", y);
        prefs.putShort("p2_z", z);
        Serial.printf("[FLASH] Da luu Diem 4 (THA 2 - HU NAP CHE): X=%d, Y=%d, Z=%d\n", x, y, z);
    }
    else if (index == 3)
    {
        currentConfig.placePoint3 = { x, y, z };
        prefs.putShort("p3_x", x);
        prefs.putShort("p3_y", y);
        prefs.putShort("p3_z", z);
        Serial.printf("[FLASH] Da luu Diem 5 (THA 3 - TRAY XUOC): X=%d, Y=%d, Z=%d\n", x, y, z);
    }

    prefs.end();
}

// =============================================================================
// CÁC HÀM GETTER
// =============================================================================
Point3D getStartCoordinate()
{
    return currentConfig.startPoint;
}

Point3D getPickCoordinate()
{
    return currentConfig.pickPoint;
}

Point3D getPlaceCoordinate(uint8_t index)
{
    if (index == 1) return currentConfig.placePoint1;
    if (index == 2) return currentConfig.placePoint2;
    if (index == 3) return currentConfig.placePoint3;
    return currentConfig.placePoint1;
}

// =============================================================================
// IN BẢNG THÔNG SỐ ĐANG LƯU TRONG FLASH
// =============================================================================
void printConfigStorage()
{
    Serial.println("=================================================");
    Serial.println("      5 DIEM TEACH LUU TRONG FLASH ESP32 (NVS)   ");
    Serial.println("=================================================");
    Serial.printf(" 1. Diem 1 - START/HOME : X=%d, Y=%d, Z=%d\n",
                  currentConfig.startPoint.x, currentConfig.startPoint.y, currentConfig.startPoint.z);
    Serial.printf(" 2. Diem 2 - GAP (Pick) : X=%d, Y=%d, Z=%d\n",
                  currentConfig.pickPoint.x, currentConfig.pickPoint.y, currentConfig.pickPoint.z);
    Serial.printf(" 3. Diem 3 - THA GI SET : X=%d, Y=%d, Z=%d\n",
                  currentConfig.placePoint1.x, currentConfig.placePoint1.y, currentConfig.placePoint1.z);
    Serial.printf(" 4. Diem 4 - THA NAP CHE: X=%d, Y=%d, Z=%d\n",
                  currentConfig.placePoint2.x, currentConfig.placePoint2.y, currentConfig.placePoint2.z);
    Serial.printf(" 5. Diem 5 - THA TRAY   : X=%d, Y=%d, Z=%d\n",
                  currentConfig.placePoint3.x, currentConfig.placePoint3.y, currentConfig.placePoint3.z);
    Serial.println("=================================================");
}
