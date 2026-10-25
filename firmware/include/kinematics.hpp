#ifndef KINEMATICS_HPP
#define KINEMATICS_HPP

#include <stdint.h>
#include <math.h>

// =====================================================
// ROBOT PARAMETERS
// =====================================================

#define L1 80.0f
#define L2 175.0f
#define L3 175.0f
#define L4 35.0f

// =====================================================
// CONVERSION
// =====================================================

#define TO_RADIAN(x) ((x) * PI / 180.0f)
#define TO_DEGREE(x) ((x) * 180.0f / PI)

// =====================================================
// JOINT ANGLES
// =====================================================

extern float theta1;
extern float theta2;
extern float theta3;
extern float theta4;



// =====================================================
// FUNCTIONS
// =====================================================

bool IK(int32_t x, int32_t y, int32_t z);

void FK();

#endif