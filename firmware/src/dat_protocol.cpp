#include "dat_protocol.hpp"

// =============================================================================
// TRẠNG THÁI MÁY GIẢI MÃ (PARSER FINITE STATE MACHINE)
// =============================================================================
enum ParseState {
    PARSE_WAIT_SOF,
    PARSE_READ_CMD,
    PARSE_READ_LEN,
    PARSE_READ_DATA,
    PARSE_READ_CHECKSUM,
    PARSE_READ_EOF
};

static ParseState parseState = PARSE_WAIT_SOF;
static DatPacket rxPacket;
static uint8_t rxDataIdx = 0;
static uint8_t rxChecksum = 0;

void datProtocolInit()
{
    parseState = PARSE_WAIT_SOF;
    rxDataIdx = 0;
    rxChecksum = 0;
    memset(&rxPacket, 0, sizeof(DatPacket));
}

uint8_t datCalculateChecksum(uint8_t cmd, uint8_t len, const uint8_t *data)
{
    uint8_t csum = cmd ^ len;
    if (data != nullptr && len > 0)
    {
        for (uint8_t i = 0; i < len; i++)
        {
            csum ^= data[i];
        }
    }
    return csum;
}

bool datProtocolProcessByte(uint8_t b, DatPacket &outPacket)
{
    switch (parseState)
    {
        case PARSE_WAIT_SOF:
            if (b == DAT_SOF)
            {
                rxDataIdx = 0;
                memset(&rxPacket, 0, sizeof(DatPacket));
                parseState = PARSE_READ_CMD;
            }
            break;

        case PARSE_READ_CMD:
            rxPacket.cmd = b;
            parseState = PARSE_READ_LEN;
            break;

        case PARSE_READ_LEN:
            rxPacket.len = b;
            if (rxPacket.len > DAT_MAX_PAYLOAD)
            {
                // Độ dài vượt quá bộ đệm cho phép -> Hủy gói và quay về chờ SOF
                parseState = PARSE_WAIT_SOF;
            }
            else if (rxPacket.len == 0)
            {
                parseState = PARSE_READ_CHECKSUM;
            }
            else
            {
                rxDataIdx = 0;
                parseState = PARSE_READ_DATA;
            }
            break;

        case PARSE_READ_DATA:
            rxPacket.data[rxDataIdx++] = b;
            if (rxDataIdx >= rxPacket.len)
            {
                parseState = PARSE_READ_CHECKSUM;
            }
            break;

        case PARSE_READ_CHECKSUM:
            rxChecksum = b;
            parseState = PARSE_READ_EOF;
            break;

        case PARSE_READ_EOF:
            parseState = PARSE_WAIT_SOF;
            if (b == DAT_EOF)
            {
                // Kiểm tra tính toàn vẹn gói tin bằng mã XOR Checksum
                uint8_t expectedChecksum = datCalculateChecksum(rxPacket.cmd, rxPacket.len, rxPacket.data);
                if (rxChecksum == expectedChecksum)
                {
                    outPacket = rxPacket;
                    return true; // Gói tin hoàn toàn chính xác!
                }
                else
                {
                    // Checksum không khớp, gửi thông báo lỗi về C#
                    datSendAck(rxPacket.cmd, ACK_STATUS_ERR_CHECKSUM);
                }
            }
            break;

        default:
            parseState = PARSE_WAIT_SOF;
            break;
    }

    return false;
}

void datSendPacket(uint8_t cmd, const uint8_t *data, uint8_t len)
{
    Serial.write(DAT_SOF);
    Serial.write(cmd);
    Serial.write(len);
    if (len > 0 && data != nullptr)
    {
        Serial.write(data, len);
    }
    uint8_t csum = datCalculateChecksum(cmd, len, data);
    Serial.write(csum);
    Serial.write(DAT_EOF);
}

void datSendAck(uint8_t originalCmd, uint8_t statusCode)
{
    uint8_t payload[2] = { originalCmd, statusCode };
    datSendPacket(CMD_ACK, payload, 2);
}

void datSendDetectTrigger(uint8_t sensorId)
{
    uint8_t payload[2] = { sensorId, 0x00 };
    datSendPacket(CMD_DETECT_TRIGGER, payload, 2);
}

void datSendTelemetry(float j1, float j2, float j3, float j4, float x, float y, float z)
{
    uint8_t payload[28];
    memcpy(&payload[0],  &j1, sizeof(float));
    memcpy(&payload[4],  &j2, sizeof(float));
    memcpy(&payload[8],  &j3, sizeof(float));
    memcpy(&payload[12], &j4, sizeof(float));
    memcpy(&payload[16], &x,  sizeof(float));
    memcpy(&payload[20], &y,  sizeof(float));
    memcpy(&payload[24], &z,  sizeof(float));

    datSendPacket(CMD_TELEMETRY, payload, 28);
}

void datSendTeachPoints(int16_t x1, int16_t y1, int16_t z1,
                        int16_t x2, int16_t y2, int16_t z2,
                        int16_t x3, int16_t y3, int16_t z3,
                        int16_t x4, int16_t y4, int16_t z4,
                        int16_t x5, int16_t y5, int16_t z5)
{
    uint8_t payload[30];
    memcpy(&payload[0],  &x1, 2); memcpy(&payload[2],  &y1, 2); memcpy(&payload[4],  &z1, 2);
    memcpy(&payload[6],  &x2, 2); memcpy(&payload[8],  &y2, 2); memcpy(&payload[10], &z2, 2);
    memcpy(&payload[12], &x3, 2); memcpy(&payload[14], &y3, 2); memcpy(&payload[16], &z3, 2);
    memcpy(&payload[18], &x4, 2); memcpy(&payload[20], &y4, 2); memcpy(&payload[22], &z4, 2);
    memcpy(&payload[24], &x5, 2); memcpy(&payload[26], &y5, 2); memcpy(&payload[28], &z5, 2);

    datSendPacket(CMD_TEACH_ALL_RESP, payload, 30);
}


