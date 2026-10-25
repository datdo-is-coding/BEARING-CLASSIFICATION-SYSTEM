#include "CLI.hpp"

RobotState robotState = RUN;

Instruction program[PROGRAM_MAX_SIZE];

uint8_t instructionIndex = 0;
uint8_t programSize = 0;


void parseCommand(String command)
{
    command.trim();

    if (command.equals("START"))
    {
        programSize = 0;
        instructionIndex = 0;

        robotState = UPLOAD;
    }

    else if (command.equals("END"))
    {
        robotState = RUN;
    }

    else if (command.startsWith("JOG"))
    {
        robotState = JOG;
    }

    else if (command.equals("PAUSE")){
        robotState = PAUSE;
    }
    else if(command.equals("STOP")){
        robotState = STOP;
        moveToStartPos();
        instructionIndex = 0;
    }

    else if (command.equals("RUN")){
        robotState = RUN;
    }
}


void parseProgram(String command)
{
    command.trim();

    if (programSize >= PROGRAM_MAX_SIZE)
    {
        Serial.println("PROGRAM MEMORY FULL");
        return;
    }


    if (command.startsWith("MOV"))
    {
        Instruction cmd;

        cmd.type = MOV;

        sscanf(command.c_str(),
               "MOV %d %d %d",
               &cmd.x,
               &cmd.y,
               &cmd.z);

        program[programSize++] = cmd;
    }



    else if (command.startsWith("WAIT"))
    {
        Instruction cmd;

        cmd.type = WAIT;

        cmd.value = command.substring(5).toInt();

        program[programSize++] = cmd;
    }


    else if (command.equals("HOME"))
    {
        Instruction cmd;

        cmd.type = HOME;

        program[programSize++] = cmd;
    }

    else if (command.startsWith("MAG"))
    {
        Instruction cmd;

        cmd.type = MAG;

        cmd.value = command.substring(4).toInt();

        program[programSize++] = cmd;
    }

    else if (command.startsWith("GOTO"))
    {
        Instruction cmd;

        cmd.type = GOTO;

        cmd.value = command.substring(5).toInt();

        program[programSize++] = cmd;
    }
}

void parseAngles(String command)
{
    if (!command.startsWith("JOG"))
        return;

    command.remove(0, 4);   // bỏ "JOG "

    int p1 = command.indexOf(' ');
    int p2 = command.indexOf(' ', p1 + 1);
    int p3 = command.indexOf(' ', p2 + 1);

    if (p1 < 0 || p2 < 0 || p3 < 0)
        return;

    float t1 = ServoAngleJ1(command.substring(0, p1).toFloat());
    float t2 = command.substring(p1 + 1, p2).toFloat();
    float t3 = ServoAngleJ3(command.substring(p2 + 1, p3).toFloat());
    float t4 = ServoAngleJ4(command.substring(p3 + 1).toFloat());

    writeServoFloat(
        J1_CHANNEL,
        t1,
        J1_MIN_US,
        J1_MAX_US
    );


    writeServoFloat(
        J2_CHANNEL,
        t2,
        J2_MIN_US,
        J2_MAX_US
    );

    writeServoFloat(
        J3_CHANNEL,
        t3,
        J3_MIN_US,
        J3_MAX_US
    );

    writeServoFloat(
        J4_CHANNEL,
        t4,
        J4_MIN_US,
        J4_MAX_US
    );

}


void GetProgram()
{
    for (uint8_t i = 0; i < programSize; i++)
    {
        char data[40];

        if (program[i].type == MOV)
        {
            sprintf(data,
                    "MOV %d %d %d\n",
                    program[i].x,
                    program[i].y,
                    program[i].z);
        }

        else if (program[i].type == WAIT)
        {
            sprintf(data,
                    "WAIT %d\n",
                    program[i].value);
        }

        else if (program[i].type == HOME)
        {
            strcpy(data, "HOME\n");
        }

        else if (program[i].type == MAG)
        {
            sprintf(data,
                    "MAG %d\n",
                    program[i].value);
        }

        Serial.print(data);
    }
}

void GetInstruction(int i){
    if( i >= programSize) return;
    char data[40];

    if (program[i].type == MOV)
    {
        sprintf(data,
                "MOV %d %d %d\n",
                program[i].x,
                program[i].y,
                program[i].z);
    }

    else if (program[i].type == WAIT)
    {
        sprintf(data,
                "WAIT %d\n",
                program[i].value);
    }

    else if (program[i].type == HOME)
    {
        strcpy(data, "HOME\n");
    }

    else if (program[i].type == MAG)
    {
        sprintf(data,
                "MAG %d\n",
                program[i].value);
    }

    Serial.print(data);
}


