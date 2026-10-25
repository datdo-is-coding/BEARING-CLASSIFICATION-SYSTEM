#ifndef CLI_H
#define CLI_H



#include <Arduino.h>
#include "String.h"

#include "stdint.h"
#include "control.hpp"
#define PROGRAM_MAX_SIZE 50

typedef enum{
    UPLOAD,
    RUN,
    JOG,
    IDLE,
    PAUSE,
    STOP,
}RobotState;

typedef enum{
    MOV,
    WAIT,
    HOME,
    MAG,
    GOTO
}CMD_TYPE;

typedef struct{
    CMD_TYPE type;
    int x;
    int y;
    int z;

    uint32_t value;
}Instruction;

extern RobotState robotState;

extern Instruction program[PROGRAM_MAX_SIZE]; 

extern uint8_t instructionIndex;

extern uint8_t programSize;

void parseCommand(String command);
void parseProgram(String command);
void GetProgram();
void parseAngles(String command);


#endif