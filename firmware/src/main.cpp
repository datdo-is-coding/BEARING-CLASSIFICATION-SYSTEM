#include <Arduino.h>
#include "control.hpp"
#include "String.h"
#include "CLI.hpp"


uint32_t last_time;
uint32_t send_time;
bool instructionStarted = false;
uint32_t waitStartTime = 0;




void runProgram()
{
    if (instructionIndex >= programSize)
    {
        robotState = IDLE;
        return;
    }
    Instruction &cmd = program[instructionIndex];
    if (cmd.type == MOV)
    {
        if (!instructionStarted)
        {
            moveToCoordinateStart(
                cmd.x,
                cmd.y,
                cmd.z
            );
            instructionStarted = true;
        }
        if (moveToCoordinateStep())
        {
            instructionIndex++;
            instructionStarted = false;
        }
        return;
    }


    if (cmd.type == WAIT)
    {
        if (!instructionStarted)
        {
            waitStartTime = millis();
            instructionStarted = true;
        }
        if (millis() - waitStartTime >= cmd.value)
        {
            instructionIndex++;
            instructionStarted = false;
        }
        return;
    }
    if (cmd.type == MAG)
    {
        if (cmd.value)
            digitalWrite(A0, HIGH);
        else
            digitalWrite(A0, LOW);

        instructionIndex++;
        return;
    }
    if (cmd.type == HOME)
    {
        moveToStartPos();

        instructionIndex++;
        return;
    }

    if (cmd.type == GOTO)
    {
        

        instructionIndex = cmd.value - 1;
        return;
    }
}

void setup() {
  Serial.begin(9600); 

  initServo();

  moveToStartPos();

  pinMode(A0, OUTPUT);
  digitalWrite(A0, LOW);
  delay(5000);
  last_time = millis();
  send_time = millis();
}

void loop()
{
    // =============================================
    // SERIAL
    // =============================================

    if (Serial.available() > 0)
    {
        String data = Serial.readStringUntil('\n');

        parseCommand(data);


        if (robotState == UPLOAD)
        {
            parseProgram(data);
        }
        else if (robotState == JOG)
        {
            parseAngles(data);
        }
    }


    if (robotState == RUN)
    {
        if (millis() - last_time >= 5)
        {
            last_time = millis();

            runProgram();
        }

        if(millis() - send_time >= 50){
          send_time = millis();
          String data = "A " + String(currentJ1) + " " + 
                    String(currentJ2) + " " + 
                    String(currentJ3) + " " + 
                    String(currentJ4);

          Serial.println(data);
          
        }
    }


    
}