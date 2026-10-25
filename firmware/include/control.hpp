#ifndef CONTROL_HPP
#define CONTROL_HPP

#include <Arduino.h>
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

// =====================================================
// PCA9685
// =====================================================

#define PCA9685_ADDRESS 0x40

#define PCA_SDA 25
#define PCA_SCL 26

#define PCA_FREQUENCY 50

extern Adafruit_PWMServoDriver pca;

// =====================================================
// SERVO CHANNEL
// =====================================================

#define J1_CHANNEL  0
#define J2_CHANNEL  4
#define J3_CHANNEL  8
#define J4_CHANNEL 12

// =====================================================
// SERVO MICROSECONDS
// =====================================================

// MG996R
#define J1_MIN_US 500
#define J1_MAX_US 2500

#define J2_MIN_US 500
#define J2_MAX_US 2500

#define J3_MIN_US 500
#define J3_MAX_US 2500

// SG90
#define J4_MIN_US 600
#define J4_MAX_US 2400

// =====================================================
// CURRENT SERVO ANGLE
// =====================================================

extern float currentJ1;
extern float currentJ2;
extern float currentJ3;
extern float currentJ4;

// =====================================================
// CURRENT CARTESIAN POSITION
// =====================================================

extern int32_t current_x;
extern int32_t current_y;
extern int32_t current_z;

// =====================================================
// SERVO ANGLE OFFSET
// =====================================================

#define ServoAngleJ1(theta) (theta + 90.0f)
#define ServoAngleJ2(theta) (theta + 180.0f)
#define ServoAngleJ3(theta) (180.0f - theta)
#define ServoAngleJ4(theta) (theta - 15.0f)

// =====================================================
// FUNCTION
// =====================================================

void initServo();

void setServoMicroseconds(
    uint8_t channel,
    uint16_t us
);

void writeServoFloat(
    uint8_t channel,
    float angle,
    uint16_t min_us,
    uint16_t max_us
);

void moveToStartPos();

void moveToCoordinateStart(
    int16_t x,
    int16_t y,
    int16_t z
);

bool moveToCoordinateStep();

#endif