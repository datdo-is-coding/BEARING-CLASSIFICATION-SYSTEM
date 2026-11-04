#ifndef ROBOT_SYSTEM_FLOW_HPP
#define ROBOT_SYSTEM_FLOW_HPP

#include <Arduino.h>
#include "hardware_config.h"
#include "control.hpp"
#include "kinematics.hpp"
#include "dat_protocol.hpp"

// =============================================================================
// CÁC TRẠNG THÁI CỦA FLOW TỰ ĐỘNG (FSM STATE)
// =============================================================================
enum SystemFlowState {
    FLOW_STATE_IDLE,                 // Trạng thái nghỉ/chờ lệnh
    FLOW_STATE_START_CONVEYOR,       // Bật băng tải chạy (Motor điều tốc PWM)
    FLOW_STATE_WAIT_OBJECT,          // Chờ cảm biến quang phát hiện vật
    FLOW_STATE_STOP_CONVEYOR,        // Đã thấy vật -> Dừng băng tải ngay
    FLOW_STATE_TRIGGER_PC,           // Gửi CMD_DETECT_TRIGGER lên C#
    FLOW_STATE_WAIT_AI_RESULT,       // Đang chờ kết quả nhận diện AI từ C#
    FLOW_STATE_ROBOT_PICK_MOVE,      // Robot di chuyển đến vị trí gắp (Pick Point)
    FLOW_STATE_ROBOT_PICK_SUCK,      // Bật nam châm hút phôi
    FLOW_STATE_ROBOT_RETRACT,        // Nhấc phôi lên cao độ an toàn (Safe Z)
    FLOW_STATE_ROBOT_PLACE_MOVE,     // Robot di chuyển đến vị trí thả lỗi (Place Point)
    FLOW_STATE_ROBOT_PLACE_RELEASE,  // Tắt nam châm nhả phôi vào khay lỗi
    FLOW_STATE_ROBOT_RETURN_HOME,    // Robot quay về vị trí Home an toàn
    FLOW_STATE_RESUME_CONVEYOR       // Bật lại băng tải chạy tiếp, đón vòng bi tiếp theo
};

// =============================================================================
// KHAI BÁO CÁC HÀM ĐIỀU KHIỂN MOTOR BĂNG TẢI (PWM)
// =============================================================================
void initConveyorMotor();
void setMotorSpeed(uint8_t speed);
void startConveyor(uint8_t speed = MOTOR_DEFAULT_SPEED);
void stopConveyor();
bool isConveyorRunning();
uint8_t getMotorSpeed();

// =============================================================================
// KHAI BÁO CÁC HÀM ĐIỀU KHIỂN NAM CHÂM ĐIỆN (MAGNET)
// =============================================================================
void initMagnet();
void setMagnet(bool state);
bool getMagnetState();

// =============================================================================
// KHAI BÁO CÁC HÀM ĐỌC CẢM BIẾN QUANG (OPTICAL SENSOR)
// =============================================================================
void initSensors(uint8_t selectedTriggerSensor = CB1);
void setActiveSensor(uint8_t sensorPin);
uint8_t getActiveSensor();
bool readSensorRaw(uint8_t pin = CB1);
bool isObjectDetected(uint8_t pin = CB1);
void checkSensorsAndNotify();

// =============================================================================
// KHAI BÁO CÁC HÀM ĐIỀU KHIỂN ROBOT & NỘI SUY QUỸ ĐẠO
// =============================================================================
void robotSetTarget(int16_t x, int16_t y, int16_t z);
bool robotUpdateStep();
bool isRobotMoving();
void robotJogAngles(float j1, float j2, float j3, float j4);
void robotSendCurrentTelemetry();

// =============================================================================
// QUẢN LÝ TIẾN TRÌNH TỰ ĐỘNG THEO FLOW
// =============================================================================
void systemFlowInit(uint8_t selectedTriggerSensor = CB1);
void systemFlowStart(uint8_t speed = 0);
void systemFlowStop();
void systemFlowUpdate();
void systemFlowSetAiResult(bool isOk, uint8_t defectType);
SystemFlowState getSystemFlowState();
const char* getSystemFlowStateStr();

#endif // ROBOT_SYSTEM_FLOW_HPP
