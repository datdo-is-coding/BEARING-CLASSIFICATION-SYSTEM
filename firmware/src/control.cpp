#include "control.hpp"
#include "kinematics.hpp"

// =====================================================
// PCA9685
// =====================================================

Adafruit_PWMServoDriver pca(PCA9685_ADDRESS);


// =====================================================
// CURRENT SERVO POSITION
// =====================================================

float currentJ1 = 0.0f;
float currentJ2 = 0.0f;
float currentJ3 = 0.0f;
float currentJ4 = 0.0f;


// =====================================================
// CURRENT CARTESIAN POSITION
// =====================================================

int32_t current_x = 164;
int32_t current_y = 0;
int32_t current_z = 104;


// =====================================================
// SERVO
// =====================================================

// Chuyển góc 0-180° sang microsecond
int angleToMicroseconds(
    float angle,
    uint16_t min_us,
    uint16_t max_us
)
{
    angle = constrain(angle, 0.0f, 180.0f);

    return (int)(
        min_us +
        (angle / 180.0f) *
        (max_us - min_us)
    );
}


// =====================================================
// MICROSECOND → PCA9685
// =====================================================

void setServoMicroseconds(
    uint8_t channel,
    uint16_t us
)
{
    // PCA9685 = 50 Hz
    //
    // Chu kỳ:
    // 1 / 50 = 20 ms = 20000 us
    //
    // PCA9685:
    // 4096 bước / 20000 us

    uint16_t ticks =
        ((uint32_t)us * 4096UL) / 20000UL;

    pca.setPWM(
        channel,
        0,
        ticks
    );
}


// =====================================================
// WRITE SERVO ANGLE
// =====================================================

void writeServoFloat(
    uint8_t channel,
    float angle,
    uint16_t min_us,
    uint16_t max_us
)
{
    int us = angleToMicroseconds(
        angle,
        min_us,
        max_us
    );

    setServoMicroseconds(
        channel,
        us
    );
}


// =====================================================
// INIT SERVO
// =====================================================

void initServo()
{
    // ESP32
    // GPIO25 = SDA
    // GPIO26 = SCL

    Wire.begin(
        PCA_SDA,
        PCA_SCL
    );

    // Khởi động PCA9685
    pca.begin();

    // Tần số oscillator mặc định PCA9685
    pca.setOscillatorFrequency(25000000);

    // Servo = 50 Hz
    pca.setPWMFreq(PCA_FREQUENCY);

    delay(10);
}


// =====================================================
// HOME
// =====================================================

void moveToStartPos()
{
    int32_t x = 164;
    int32_t y = 0;
    int32_t z = 104;

    // Tính IK
    if (!IK(x, y, z))
        return;


    // =================================================
    // IK ANGLE
    // =================================================

    Serial.printf("T1=%.2f | T2=%.2f | T3=%.2f | T4=%.2f\n",
              theta1, theta2, theta3, theta4);
    
    float targetJ1 = ServoAngleJ1(theta1);
    float targetJ2 = ServoAngleJ2(theta2);
    float targetJ3 = ServoAngleJ3(theta3);
    float targetJ4 = ServoAngleJ4(theta4);
Serial.printf("T1=%.2f | T2=%.2f | T3=%.2f | T4=%.2f\n",
              targetJ1, targetJ2, targetJ3, targetJ4);

    // =================================================
    // J1 - MG996R - CH0
    // =================================================

    writeServoFloat(
        J1_CHANNEL,
        targetJ1,
        J1_MIN_US,
        J1_MAX_US
    );


    // =================================================
    // J2 - MG996R - CH4
    // =================================================

    writeServoFloat(
        J2_CHANNEL,
        targetJ2,
        J2_MIN_US,
        J2_MAX_US
    );


    // =================================================
    // J3 - MG996R - CH8
    // =================================================

    writeServoFloat(
        J3_CHANNEL,
        targetJ3,
        J3_MIN_US,
        J3_MAX_US
    );


    // =================================================
    // J4 - SG90 - CH12
    // =================================================

    writeServoFloat(
        J4_CHANNEL,
        targetJ4,
        J4_MIN_US,
        J4_MAX_US
    );


    // =================================================
    // SAVE CURRENT ANGLE
    // =================================================

    currentJ1 = targetJ1;
    currentJ2 = targetJ2;
    currentJ3 = targetJ3;
    currentJ4 = targetJ4;


    // =================================================
    // SAVE CURRENT POSITION
    // =================================================

    current_x = x;
    current_y = y;
    current_z = z;

}


// =====================================================
// MOV STATE
// =====================================================

#define TRAJECTORY_POINTS 200

static int32_t moveStartX;
static int32_t moveStartY;
static int32_t moveStartZ;

static int32_t moveTargetX;
static int32_t moveTargetY;
static int32_t moveTargetZ;

static uint16_t moveStep = 0;

static bool moving = false;


// =====================================================
// BẮT ĐẦU MỘT LỆNH MOV
// =====================================================

void moveToCoordinateStart(
    int16_t x,
    int16_t y,
    int16_t z
)
{
    moveStartX = current_x;
    moveStartY = current_y;
    moveStartZ = current_z;

    moveTargetX = x;
    moveTargetY = y;
    moveTargetZ = z;

    moveStep = 0;

    moving = true;
}


// =====================================================
// CHẠY ĐÚNG 1 STEP CỦA MOV
// =====================================================

bool moveToCoordinateStep()
{
    // Nếu không chuyển động
    if (!moving)
        return true;


    // =================================================
    // KIỂM TRA ĐÃ ĐẾN ĐÍCH
    // =================================================

    if (moveStep >= TRAJECTORY_POINTS)
    {
        current_x = moveTargetX;
        current_y = moveTargetY;
        current_z = moveTargetZ;

        moving = false;

        return true;
    }


    // =================================================
    // TĂNG STEP
    // =================================================

    moveStep++;


    // =================================================
    // TÍNH XYZ HIỆN TẠI
    // =================================================

    int32_t x =
        moveStartX +
        (moveTargetX - moveStartX) *
        moveStep /
        TRAJECTORY_POINTS;

    int32_t y =
        moveStartY +
        (moveTargetY - moveStartY) *
        moveStep /
        TRAJECTORY_POINTS;

    int32_t z =
        moveStartZ +
        (moveTargetZ - moveStartZ) *
        moveStep /
        TRAJECTORY_POINTS;


    // =================================================
    // TÍNH IK
    // =================================================

    if (!IK(x, y, z))
    {
        moving = false;

        return true;
    }


    // =================================================
    // LẤY GÓC JOINT
    // =================================================

    float targetJ1 = ServoAngleJ1(theta1);
    float targetJ2 = ServoAngleJ2(theta2);
    float targetJ3 = ServoAngleJ3(theta3);
    float targetJ4 = ServoAngleJ4(theta4);


    // =================================================
    // J1 - MG996R - CH0
    // =================================================

    writeServoFloat(
        J1_CHANNEL,
        targetJ1,
        J1_MIN_US,
        J1_MAX_US
    );


    // =================================================
    // J2 - MG996R - CH4
    // =================================================

    writeServoFloat(
        J2_CHANNEL,
        targetJ2,
        J2_MIN_US,
        J2_MAX_US
    );


    // =================================================
    // J3 - MG996R - CH8
    // =================================================

    writeServoFloat(
        J3_CHANNEL,
        targetJ3,
        J3_MIN_US,
        J3_MAX_US
    );


    // =================================================
    // J4 - SG90 - CH12
    // =================================================

    writeServoFloat(
        J4_CHANNEL,
        targetJ4,
        J4_MIN_US,
        J4_MAX_US
    );


    // =================================================
    // SAVE CURRENT JOINT ANGLE
    // =================================================

    currentJ1 = targetJ1;
    currentJ2 = targetJ2;
    currentJ3 = targetJ3;
    currentJ4 = targetJ4;


    // =================================================
    // KIỂM TRA ĐÃ ĐẾN ĐÍCH
    // =================================================

    if (moveStep >= TRAJECTORY_POINTS)
    {
        current_x = moveTargetX;
        current_y = moveTargetY;
        current_z = moveTargetZ;

        moving = false;

        return true;
    }


    return false;
}