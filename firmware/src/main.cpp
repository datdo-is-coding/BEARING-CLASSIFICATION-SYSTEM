#include <Arduino.h>
#include "control.hpp"
#include "CLI.hpp"
#include "robot_system_flow.hpp"
#include "config_storage.hpp"
#include "dat_protocol.hpp"

uint32_t last_time;
uint32_t send_time;
bool instructionStarted = false;
uint32_t waitStartTime = 0;
String asciiLineBuffer = "";

void handleDatPacket(const DatPacket &packet)
{
    switch (packet.cmd)
    {
        case CMD_JOG:
            if (packet.len == 16)
            {
                float j1, j2, j3, j4;
                memcpy(&j1, &packet.data[0], sizeof(float));
                memcpy(&j2, &packet.data[4], sizeof(float));
                memcpy(&j3, &packet.data[8], sizeof(float));
                memcpy(&j4, &packet.data[12], sizeof(float));

                robotJogAngles(j1, j2, j3, j4);
            }
            else
            {
                datSendAck(CMD_JOG, ACK_STATUS_ERR_LEN);
            }
            break;

        case CMD_AUTO_START:
            {
                uint8_t speed = (packet.len > 0) ? packet.data[0] : 0;
                systemFlowStart(speed);
                datSendAck(CMD_AUTO_START, ACK_STATUS_OK);
            }
            break;

        case CMD_AUTO_STOP:
            systemFlowStop();
            datSendAck(CMD_AUTO_STOP, ACK_STATUS_OK);
            break;

        case CMD_INSPECT_RESULT:
            if (packet.len >= 2)
            {
                bool isOk = (packet.data[0] != 0);
                uint8_t defectType = packet.data[1];
                systemFlowSetAiResult(isOk, defectType);
                datSendAck(CMD_INSPECT_RESULT, ACK_STATUS_OK);
            }
            else
            {
                datSendAck(CMD_INSPECT_RESULT, ACK_STATUS_ERR_LEN);
            }
            break;

        case CMD_SET_TEACH:
            if (packet.len >= 7)
            {
                uint8_t idx = packet.data[0];
                int16_t x, y, z;
                memcpy(&x, &packet.data[1], sizeof(int16_t));
                memcpy(&y, &packet.data[3], sizeof(int16_t));
                memcpy(&z, &packet.data[5], sizeof(int16_t));

                if (idx == 1) saveStartCoordinate(x, y, z);
                else if (idx == 2) savePickCoordinate(x, y, z);
                else if (idx >= 3 && idx <= 5) savePlaceCoordinate(idx - 2, x, y, z);

                datSendAck(CMD_SET_TEACH, ACK_STATUS_OK);
            }
            else
            {
                datSendAck(CMD_SET_TEACH, ACK_STATUS_ERR_LEN);
            }
            break;

        case CMD_GET_TEACH:
            datSendTeachPoints(currentConfig.startPoint.x, currentConfig.startPoint.y, currentConfig.startPoint.z,
                               currentConfig.pickPoint.x, currentConfig.pickPoint.y, currentConfig.pickPoint.z,
                               currentConfig.placePoint1.x, currentConfig.placePoint1.y, currentConfig.placePoint1.z,
                               currentConfig.placePoint2.x, currentConfig.placePoint2.y, currentConfig.placePoint2.z,
                               currentConfig.placePoint3.x, currentConfig.placePoint3.y, currentConfig.placePoint3.z);
            Serial.println("[TEACH SYNC] Da gui 5 diem Teach tu Flash ve PC theo yeu cau.");
            break;

        case CMD_MANUAL_ACT:
            if (packet.len >= 2)
            {
                uint8_t dev = packet.data[0];
                uint8_t val = packet.data[1];
                if (dev == 1)
                {
                    if (val > 0) startConveyor(val);
                    else stopConveyor();
                }
                else if (dev == 2)
                {
                    setMagnet(val > 0);
                }
                datSendAck(CMD_MANUAL_ACT, ACK_STATUS_OK);
            }
            break;

        default:
            datSendAck(packet.cmd, ACK_STATUS_ERR_CMD);
            break;
    }
}

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
            moveToCoordinateStart(cmd.x, cmd.y, cmd.z);
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
        setMagnet(cmd.value != 0);
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
    Serial.begin(115200); 

    // 1. Nạp cấu hình từ Flash (Preferences)
    configStorageInit();

    // 2. Khởi tạo Robot
    initServo();
    moveToStartPos();

    // 3. Khởi tạo giao thức Dat Protocol
    datProtocolInit();

    // 4. Khởi tạo hệ thống ngoại vi với cảm biến đã lưu trong Flash
    systemFlowInit(currentConfig.activeSensorPin);

    last_time = millis();
    send_time = millis();
    Serial.println(">>> ESP32 ROBOT & CONVEYOR SYSTEM READY [DAT PROTOCOL ACTIVE] <<<");
}

void loop()
{
    // Giám sát liên tục CB1 và CB2
    checkSensorsAndNotify();

    // Cập nhật chu trình Flow tự động (băng tải -> cảm biến -> dừng -> trigger C# -> AI -> gắp/thả -> telemetry)
    systemFlowUpdate();

    // Xử lý dữ liệu nhận từ Serial qua máy trạng thái Dat Protocol
    while (Serial.available() > 0)
    {
        uint8_t b = (uint8_t)Serial.read();

        DatPacket packet;
        if (datProtocolProcessByte(b, packet))
        {
            handleDatPacket(packet);
        }
        else
        {
            // Dự phòng: Thu thập buffer ký tự ASCII nếu người dùng gõ Serial Monitor
            if (b == '\n' || b == '\r')
            {
                if (asciiLineBuffer.length() > 0)
                {
                    parseCommand(asciiLineBuffer);
                    if (robotState == UPLOAD)
                    {
                        parseProgram(asciiLineBuffer);
                    }
                    else if (robotState == JOG)
                    {
                        parseAngles(asciiLineBuffer);
                    }
                    asciiLineBuffer = "";
                }
            }
            else if (b >= 32 && b <= 126)
            {
                if (asciiLineBuffer.length() < 120)
                {
                    asciiLineBuffer += (char)b;
                }
            }
        }
    }

    if (robotState == RUN)
    {
        if (millis() - last_time >= 5)
        {
            last_time = millis();
            runProgram();
        }

        if (millis() - send_time >= 50)
        {
            send_time = millis();
            robotSendCurrentTelemetry();
        }
    }
}