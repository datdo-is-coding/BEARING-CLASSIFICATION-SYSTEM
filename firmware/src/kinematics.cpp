
#include "kinematics.hpp"
#include <Arduino.h>
#include <math.h>


float theta1 = 0.0f;
float theta2 = 0.0f;
float theta3 = 0.0f;
float theta4 = 0.0f;




bool IK(int32_t x, int32_t y, int32_t z)
{
    // -------------------------------------------------
    // L4 offset
    // -------------------------------------------------

    float z_offset = (float)z + L4;

    // -------------------------------------------------
    // THETA 1
    // -------------------------------------------------

    theta1 =
        TO_DEGREE(
            atan2((float)y,(float)x ));


    float r =sqrt((float)x * x +(float)y * y );

    float Z =(float)L1 - z_offset;


    float denominator = 2.0f * L2 * L3;
    float c3 =
        (
            r * r +
            Z * Z -
            L2 * L2 -
            L3 * L3
        ) / denominator;


    if (c3 < -1.0f || c3 > 1.0f)
    {
        Serial.println("IK ERROR: OUT OF WORKSPACE");
        return false;
    }


    c3 = constrain(c3, -1.0f, 1.0f);


    float s3 =
        sqrt(
            1.0f - c3 * c3
        );


    theta3 =
        TO_DEGREE(
            atan2(
                s3,
                c3
            )
        );



    float A =
        L2 +
        L3 * c3;

    float B =
        L3 * s3;



    float d =
        r * r +
        Z * Z;

    if (d < 0.000001f)
    {
        Serial.println("IK ERROR: SINGULARITY");
        return false;
    }

    float c2 =
        (
            r * A +
            Z * B
        ) / d;

    float s2 =
        (
            Z * A -
            r * B
        ) / d;


    theta2 =
        TO_DEGREE(
            atan2(
                s2,
                c2
            )
        );


    theta4 =
        90.0f -
        theta2 -
        theta3;

    return true;
}


void FK()
{


    float t1 =
        TO_RADIAN(theta1);

    float t2 =
        TO_RADIAN(theta2);

    float t23 =
        TO_RADIAN(
            theta2 +
            theta3
        );


    float r =
        L2 * cos(t2) +
        L3 * cos(t23);


    float x =
        cos(t1) * r;

    float y =
        sin(t1) * r;

    float z =
        L1 -
        L2 * sin(t2) -
        L3 * sin(t23) -
        L4;

    Serial.print("FK: ");

    Serial.print(x, 3);
    Serial.print(" ");

    Serial.print(y, 3);
    Serial.print(" ");

    Serial.println(z, 3);
}







