#include "robot_system_flow.hpp"
#include "config_storage.hpp"

// =============================================================================
// BIẾN NỘI BỘ QUẢN LÝ THIẾT BỊ & FSM
// =============================================================================
static uint8_t currentSpeed = 0;
static bool magnetActive = false;
static SystemFlowState currentFlowState = FLOW_STATE_IDLE;
static uint32_t stateTimer = 0;
static uint32_t lastRobotStepTime = 0;
static uint32_t lastTelemetryTime = 0;
static bool robotIsBusy = false;

static bool pendingIsOk = false;
static uint8_t pendingDefectType = 0;
static bool aiResultReady = false;

// =============================================================================
// 1. ĐIỀU KHIỂN MOTOR BĂNG TẢI (PWM ĐIỀU TỐC)
// =============================================================================
void initConveyorMotor()
{
    ledcSetup(MOTOR_PWM_CHANNEL, MOTOR_PWM_FREQ, MOTOR_PWM_RES);
    ledcAttachPin(PIN_MOTOR_PWM, MOTOR_PWM_CHANNEL);
    setMotorSpeed(0);
    Serial.printf("[MOTOR] Khoi tao thanh cong tren GPIO%d (Channel %d, Freq %dHz, Res %d-bit)\n",
                  PIN_MOTOR_PWM, MOTOR_PWM_CHANNEL, MOTOR_PWM_FREQ, MOTOR_PWM_RES);
}

void setMotorSpeed(uint8_t speed)
{
    currentSpeed = speed;
    ledcWrite(MOTOR_PWM_CHANNEL, currentSpeed);
}

void startConveyor(uint8_t speed)
{
    setMotorSpeed(speed);
    Serial.printf("[MOTOR] Bat bang tai voi toc do PWM: %d/255\n", speed);
}

void stopConveyor()
{
    setMotorSpeed(0);
    Serial.println("[MOTOR] Da dung bang tai (PWM = 0).");
}

bool isConveyorRunning()
{
    return (currentSpeed > 0);
}

uint8_t getMotorSpeed()
{
    return currentSpeed;
}

// =============================================================================
// 2. ĐIỀU KHIỂN NAM CHÂM ĐIỆN (MAGNET)
// =============================================================================
void initMagnet()
{
    pinMode(PIN_MAGNET, OUTPUT);
    setMagnet(false);
    Serial.printf("[MAGNET] Khoi tao thanh cong tren GPIO%d (Default OFF)\n", PIN_MAGNET);
}

void setMagnet(bool state)
{
    magnetActive = state;
    digitalWrite(PIN_MAGNET, state ? MAGNET_ON : MAGNET_OFF);
    Serial.printf("[MAGNET] Nam cham dien: %s\n", state ? "BAT (ON)" : "TAT (OFF)");
}

bool getMagnetState()
{
    return magnetActive;
}

// =============================================================================
// 3. ĐỌC CẢM BIẾN QUANG PHÁT HIỆN VÒNG BI & LỌC NHIỄU
// =============================================================================
static uint8_t activeSensorPin = DEFAULT_ACTIVE_CB;
static int lastStateCB1 = -1;
static int lastStateCB2 = -1;

void setActiveSensor(uint8_t sensorPin)
{
    activeSensorPin = sensorPin;
    Serial.printf("[SENSOR CONFIG] Da chon cam bien dung bang tai: %s (GPIO%d)\n",
                  (activeSensorPin == CB1) ? "CB1" : "CB2", activeSensorPin);
}

uint8_t getActiveSensor()
{
    return activeSensorPin;
}

static bool filterSensorState(uint8_t pin, int targetLevel, uint8_t sampleCount = 5, uint16_t intervalUs = 1000)
{
    for (uint8_t i = 0; i < sampleCount; i++)
    {
        delayMicroseconds(intervalUs);
        if (digitalRead(pin) != targetLevel)
        {
            return false;
        }
    }
    return true;
}

void initSensors(uint8_t selectedTriggerSensor)
{
    pinMode(CB1, INPUT_PULLDOWN);
    pinMode(CB2, INPUT_PULLDOWN);

    activeSensorPin = selectedTriggerSensor;
    lastStateCB1 = digitalRead(CB1);
    lastStateCB2 = digitalRead(CB2);

    Serial.printf("[SENSOR INIT] GPIO%d (CB1)=%s | GPIO%d (CB2)=%s [INPUT_PULLDOWN]\n",
                  CB1, lastStateCB1 ? "HIGH" : "LOW",
                  CB2, lastStateCB2 ? "HIGH" : "LOW");
}

bool readSensorRaw(uint8_t pin)
{
    return digitalRead(pin);
}

bool isObjectDetected(uint8_t pin)
{
    if (digitalRead(pin) == SENSOR_ACTIVE_LEVEL)
    {
        if (filterSensorState(pin, SENSOR_ACTIVE_LEVEL, 5, 1000))
        {
            return true;
        }
    }
    return false;
}

void checkSensorsAndNotify()
{
    int curCB1 = digitalRead(CB1);
    int curCB2 = digitalRead(CB2);

    if (curCB1 != lastStateCB1)
    {
        if (filterSensorState(CB1, curCB1, 4, 1000))
        {
            int prevState = lastStateCB1;
            lastStateCB1 = curCB1;

            if (prevState == SENSOR_NORMAL_LEVEL && curCB1 == SENSOR_ACTIVE_LEVEL)
            {
                Serial.printf(">>> [CB1] PHAT HIEN VONG BI (HIGH -> LOW, GPIO%d)! <<<\n", CB1);
                if (currentFlowState == FLOW_STATE_WAIT_OBJECT && activeSensorPin == CB1)
                {
                    stopConveyor();
                    stateTimer = millis();
                    currentFlowState = FLOW_STATE_STOP_CONVEYOR;
                }
            }
        }
    }

    if (curCB2 != lastStateCB2)
    {
        if (filterSensorState(CB2, curCB2, 4, 1000))
        {
            int prevState = lastStateCB2;
            lastStateCB2 = curCB2;

            if (prevState == SENSOR_NORMAL_LEVEL && curCB2 == SENSOR_ACTIVE_LEVEL)
            {
                Serial.printf(">>> [CB2] PHAT HIEN VONG BI (HIGH -> LOW, GPIO%d)! <<<\n", CB2);
                if (currentFlowState == FLOW_STATE_WAIT_OBJECT && activeSensorPin == CB2)
                {
                    stopConveyor();
                    stateTimer = millis();
                    currentFlowState = FLOW_STATE_STOP_CONVEYOR;
                }
            }
        }
    }
}

// =============================================================================
// 4. ĐIỀU KHIỂN ROBOT, TELEMETRY & JOG
// =============================================================================
void robotSetTarget(int16_t x, int16_t y, int16_t z)
{
    moveToCoordinateStart(x, y, z);
    robotIsBusy = true;
    Serial.printf("[ROBOT] Di chuyen quy dao den: X=%d, Y=%d, Z=%d\n", x, y, z);
}

bool robotUpdateStep()
{
    bool isCompleted = moveToCoordinateStep();
    if (isCompleted)
    {
        robotIsBusy = false;
    }
    return isCompleted;
}

bool isRobotMoving()
{
    return robotIsBusy;
}

void robotSendCurrentTelemetry()
{
    float sliderJ1 = theta1;
    float sliderJ2 = theta2 + 180.0f;
    float sliderJ3 = theta3;
    float sliderJ4 = 270.0f - sliderJ2 - sliderJ3;

    float curX = (float)current_x;
    float curY = (float)current_y;
    float curZ = (float)current_z;

    datSendTelemetry(sliderJ1, sliderJ2, sliderJ3, sliderJ4, curX, curY, curZ);
}

void robotJogAngles(float j1, float j2, float j3, float j4)
{
    // Kiểm tra an toàn độ cao mặt đất qua FK
    float th1_rad = j1 * PI / 180.0f;
    float th2_rad = (j2 - 180.0f) * PI / 180.0f;
    float th3_rad = j3 * PI / 180.0f;
    float r = L2 * cos(th2_rad) + L3 * cos(th2_rad + th3_rad);
    float Z_arm = L2 * sin(th2_rad) + L3 * sin(th2_rad + th3_rad);
    float x = r * cos(th1_rad);
    float y = r * sin(th1_rad);
    float z = L1 - L4 - Z_arm;

    if (z < 10.0f)
    {
        Serial.printf("[SAFETY BLOCK] Chieu cao Z=%.1f mm < 10 mm! Da chan va cham co khi.\n", z);
        datSendAck(CMD_JOG, ACK_STATUS_ERR_SAFETY);
        return;
    }

    // Áp dụng offset servo và xuất xung
    float t1 = ServoAngleJ1(j1);
    float t2 = j2;
    float t3 = ServoAngleJ3(j3);
    float t4 = ServoAngleJ4(j4);

    writeServoFloat(J1_CHANNEL, t1, J1_MIN_US, J1_MAX_US);
    writeServoFloat(J2_CHANNEL, t2, J2_MIN_US, J2_MAX_US);
    writeServoFloat(J3_CHANNEL, t3, J3_MIN_US, J3_MAX_US);
    writeServoFloat(J4_CHANNEL, t4, J4_MIN_US, J4_MAX_US);

    currentJ1 = t1;
    currentJ2 = t2;
    currentJ3 = t3;
    currentJ4 = t4;

    current_x = (int32_t)x;
    current_y = (int32_t)y;
    current_z = (int32_t)z;

    theta1 = j1;
    theta2 = j2 - 180.0f;
    theta3 = j3;
    theta4 = j4;

    // Phản hồi ACK thành công và Telemetry tức thời
    datSendAck(CMD_JOG, ACK_STATUS_OK);
    datSendTelemetry(j1, j2, j3, j4, x, y, z);
}

// =============================================================================
// 5. TIẾN TRÌNH QUẢN LÝ FLOW TỰ ĐỘNG (FINITE STATE MACHINE)
// =============================================================================
void systemFlowInit(uint8_t selectedTriggerSensor)
{
    initConveyorMotor();
    initMagnet();
    initSensors(selectedTriggerSensor);
    currentFlowState = FLOW_STATE_IDLE;
    robotIsBusy = false;
    aiResultReady = false;
    Serial.println("[FLOW] He thong san sang.");
}

void systemFlowStart(uint8_t speed)
{
    uint8_t runSpeed = (speed > 0) ? speed : (currentConfig.conveyorSpeed > 0 ? currentConfig.conveyorSpeed : MOTOR_DEFAULT_SPEED);
    currentConfig.conveyorSpeed = runSpeed;
    currentFlowState = FLOW_STATE_START_CONVEYOR;
    aiResultReady = false;
    Serial.printf("[FLOW] Bat dau chu trinh tu dong! Toc do: %d\n", runSpeed);
}

void systemFlowStop()
{
    stopConveyor();
    setMagnet(false);
    robotIsBusy = false;
    aiResultReady = false;
    currentFlowState = FLOW_STATE_IDLE;
    Serial.println("[FLOW] Da dung he thong tu dong.");
}

void systemFlowSetAiResult(bool isOk, uint8_t defectType)
{
    pendingIsOk = isOk;
    pendingDefectType = defectType;
    aiResultReady = true;
    Serial.printf("[FLOW] Nhan ket qua AI: %s (DefectCode: %d)\n", isOk ? "OK (PASS)" : "NG (FAIL)", defectType);
}

SystemFlowState getSystemFlowState()
{
    return currentFlowState;
}

const char* getSystemFlowStateStr()
{
    switch (currentFlowState)
    {
        case FLOW_STATE_IDLE:                 return "IDLE";
        case FLOW_STATE_START_CONVEYOR:       return "START_CONVEYOR";
        case FLOW_STATE_WAIT_OBJECT:          return "WAIT_OBJECT";
        case FLOW_STATE_STOP_CONVEYOR:        return "STOP_CONVEYOR";
        case FLOW_STATE_TRIGGER_PC:           return "TRIGGER_PC";
        case FLOW_STATE_WAIT_AI_RESULT:       return "WAIT_AI_RESULT";
        case FLOW_STATE_ROBOT_PICK_MOVE:      return "ROBOT_PICK_MOVE";
        case FLOW_STATE_ROBOT_PICK_SUCK:      return "ROBOT_PICK_SUCK";
        case FLOW_STATE_ROBOT_RETRACT:        return "ROBOT_RETRACT";
        case FLOW_STATE_ROBOT_PLACE_MOVE:     return "ROBOT_PLACE_MOVE";
        case FLOW_STATE_ROBOT_PLACE_RELEASE:  return "ROBOT_PLACE_RELEASE";
        case FLOW_STATE_ROBOT_RETURN_HOME:    return "ROBOT_RETURN_HOME";
        case FLOW_STATE_RESUME_CONVEYOR:      return "RESUME_CONVEYOR";
        default:                              return "UNKNOWN";
    }
}

void systemFlowUpdate()
{
    switch (currentFlowState)
    {
        case FLOW_STATE_IDLE:
            break;

        case FLOW_STATE_START_CONVEYOR:
            startConveyor(currentConfig.conveyorSpeed);
            currentFlowState = FLOW_STATE_WAIT_OBJECT;
            break;

        case FLOW_STATE_WAIT_OBJECT:
            if (isObjectDetected(activeSensorPin))
            {
                Serial.printf("[FLOW] >>> PHAT HIEN VONG BI TREN CB%d! DUNG BANG TAI! <<<\n",
                              (activeSensorPin == CB1) ? 1 : 2);
                stopConveyor();
                stateTimer = millis();
                currentFlowState = FLOW_STATE_STOP_CONVEYOR;
            }
            break;

        case FLOW_STATE_STOP_CONVEYOR:
            // Chờ quán tính băng tải ổn định (khoảng 150ms)
            if (millis() - stateTimer >= 150)
            {
                currentFlowState = FLOW_STATE_TRIGGER_PC;
            }
            break;

        case FLOW_STATE_TRIGGER_PC:
            // Gửi gói tin CMD_DETECT_TRIGGER báo cho phần mềm C# chụp ảnh & nhận diện
            datSendDetectTrigger(activeSensorPin == CB1 ? 1 : 2);
            aiResultReady = false;
            stateTimer = millis();
            currentFlowState = FLOW_STATE_WAIT_AI_RESULT;
            Serial.println("[FLOW] Da ban tin TRIGGER len C#. Dang cho ket qua AI...");
            break;

        case FLOW_STATE_WAIT_AI_RESULT:
            if (aiResultReady)
            {
                aiResultReady = false;
                if (pendingIsOk)
                {
                    // Vòng bi ĐẠT CHUẨN -> Bật băng tải chạy thẳng qua khay thành phẩm
                    Serial.println("[FLOW] Vong bi DAT CHUAN -> Bat bang tai chay tiep!");
                    currentFlowState = FLOW_STATE_RESUME_CONVEYOR;
                    stateTimer = millis();
                }
                else
                {
                    // Vòng bi LỖI -> Khởi động cánh tay Robot gắp phân loại
                    Serial.println("[FLOW] Vong bi LOI -> Robot bat dau quy trinh gap!");
                    robotSetTarget(currentConfig.pickPoint.x, currentConfig.pickPoint.y, currentConfig.pickPoint.z);
                    currentFlowState = FLOW_STATE_ROBOT_PICK_MOVE;
                }
            }
            else if (millis() - stateTimer >= 12000)
            {
                // Timeout 12 giây nếu C# không phản hồi
                Serial.println("[FLOW TIMEOUT] C# khong phan hoi ket qua. Tu dong tiep tuc bang tai.");
                currentFlowState = FLOW_STATE_RESUME_CONVEYOR;
                stateTimer = millis();
            }
            break;

        case FLOW_STATE_ROBOT_PICK_MOVE:
            if (!robotIsBusy)
            {
                // Đã hạ tới vị trí gắp -> Bật nam châm hút
                setMagnet(true);
                stateTimer = millis();
                currentFlowState = FLOW_STATE_ROBOT_PICK_SUCK;
            }
            break;

        case FLOW_STATE_ROBOT_PICK_SUCK:
            // Chờ 300ms để nam châm hít chặt vòng bi
            if (millis() - stateTimer >= 300)
            {
                // Nhấc Z lên Safe Z (+60mm)
                int16_t safeZ = currentConfig.pickPoint.z + 60;
                if (safeZ > 220) safeZ = 220;
                robotSetTarget(currentConfig.pickPoint.x, currentConfig.pickPoint.y, safeZ);
                currentFlowState = FLOW_STATE_ROBOT_RETRACT;
            }
            break;

        case FLOW_STATE_ROBOT_RETRACT:
            if (!robotIsBusy)
            {
                // Chọn vị trí thả tương ứng với loại lỗi đã teach
                Point3D targetPlace;
                if (pendingDefectType == DEFECT_RUSTED)
                {
                    targetPlace = currentConfig.placePoint1; // Điểm thả 1: Gỉ sét
                    Serial.println("[FLOW] Robot di chuyen den diem tha: GI SET");
                }
                else if (pendingDefectType == DEFECT_DAMAGED_SHIELD)
                {
                    targetPlace = currentConfig.placePoint2; // Điểm thả 2: Hư nắp che
                    Serial.println("[FLOW] Robot di chuyen den diem tha: HU NAP CHE");
                }
                else
                {
                    targetPlace = currentConfig.placePoint3; // Điểm thả 3: Trầy xước
                    Serial.println("[FLOW] Robot di chuyen den diem tha: TRAY XUOC");
                }

                // Dự phòng an toàn nếu chưa teach điểm
                if (targetPlace.x == 0 && targetPlace.y == 0 && targetPlace.z == 0)
                {
                    targetPlace.x = 120;
                    targetPlace.y = -60;
                    targetPlace.z = 110;
                }

                robotSetTarget(targetPlace.x, targetPlace.y, targetPlace.z);
                currentFlowState = FLOW_STATE_ROBOT_PLACE_MOVE;
            }
            break;

        case FLOW_STATE_ROBOT_PLACE_MOVE:
            if (!robotIsBusy)
            {
                // Đã đến điểm thả -> Tắt nam châm nhả phôi
                setMagnet(false);
                stateTimer = millis();
                currentFlowState = FLOW_STATE_ROBOT_PLACE_RELEASE;
            }
            break;

        case FLOW_STATE_ROBOT_PLACE_RELEASE:
            if (millis() - stateTimer >= 250)
            {
                // Di chuyển robot về vị trí Start/Home đã teach trong Flash (hoặc mặc định 164, 0, 104)
                int16_t homeX = (currentConfig.startPoint.x != 0 || currentConfig.startPoint.y != 0 || currentConfig.startPoint.z != 0) ? currentConfig.startPoint.x : 164;
                int16_t homeY = (currentConfig.startPoint.x != 0 || currentConfig.startPoint.y != 0 || currentConfig.startPoint.z != 0) ? currentConfig.startPoint.y : 0;
                int16_t homeZ = (currentConfig.startPoint.x != 0 || currentConfig.startPoint.y != 0 || currentConfig.startPoint.z != 0) ? currentConfig.startPoint.z : 104;
                robotSetTarget(homeX, homeY, homeZ);
                currentFlowState = FLOW_STATE_ROBOT_RETURN_HOME;
            }
            break;

        case FLOW_STATE_ROBOT_RETURN_HOME:
            if (!robotIsBusy)
            {
                Serial.println("[FLOW] Robot da ve Home an toan.");
                currentFlowState = FLOW_STATE_RESUME_CONVEYOR;
                stateTimer = millis();
            }
            break;

        case FLOW_STATE_RESUME_CONVEYOR:
            // Bật lại băng tải chạy tiếp
            startConveyor(currentConfig.conveyorSpeed);
            // Delay 1.5s để vòng bi cũ hoàn toàn vượt qua vùng cảm biến
            if (millis() - stateTimer >= 1500)
            {
                Serial.println("[FLOW] San sang cho vong bi tiep theo.");
                currentFlowState = FLOW_STATE_WAIT_OBJECT;
            }
            break;

        default:
            currentFlowState = FLOW_STATE_IDLE;
            break;
    }

    // Cập nhật chuyển động nội suy của Robot nếu đang bận
    if (robotIsBusy)
    {
        if (millis() - lastRobotStepTime >= 5)
        {
            lastRobotStepTime = millis();
            robotUpdateStep();
        }
    }

    // Gửi Telemetry chu kỳ 45ms khi Robot đang di chuyển để C# cập nhật mô phỏng 3D
    if (robotIsBusy || currentFlowState >= FLOW_STATE_ROBOT_PICK_MOVE)
    {
        if (millis() - lastTelemetryTime >= 45)
        {
            lastTelemetryTime = millis();
            robotSendCurrentTelemetry();
        }
    }
}
