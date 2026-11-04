#ifndef DAT_PROTOCOL_HPP
#define DAT_PROTOCOL_HPP

#include <Arduino.h>

// =============================================================================
// CÁC HẰNG SỐ ĐỒNG BỘ KHUNG TRUYỀN (DAT PROTOCOL FRAME CONSTANTS)
// Cấu trúc khung: [SOF][CMD][LEN][DATA][CHECKSUM][EOF]
// =============================================================================
#define DAT_SOF 0xAA // Byte bắt đầu khung (Start of Frame)
#define DAT_EOF 0x55 // Byte kết thúc khung (End of Frame)

#define DAT_MAX_PAYLOAD 64 // Kích thước vùng đệm payload tối đa

// =============================================================================
// BẢNG MÃ LỆNH (COMMAND IDENTIFIERS)
// =============================================================================
#define CMD_JOG            0x01 // PC -> Robot: Điều khiển góc khớp J1..J4 (16 bytes float)
#define CMD_AUTO_START     0x02 // PC -> Robot: Bắt đầu chu trình tự động (1 byte speed)
#define CMD_AUTO_STOP      0x03 // PC -> Robot: Dừng chu trình tự động (0 byte)
#define CMD_DETECT_TRIGGER 0x04 // Robot -> PC: Cảm biến quang phát hiện vòng bi (2 bytes: sensorId, reserved)
#define CMD_INSPECT_RESULT 0x05 // PC -> Robot: Kết quả nhận diện AI (2 bytes: isOk, defectType)
#define CMD_TELEMETRY      0x06 // Robot -> PC: Phản hồi góc J1..J4 và XYZ thời gian thực (28 bytes)
#define CMD_SET_TEACH      0x07 // PC -> Robot: Lưu điểm Teach Point (7 bytes: index 1..5, x, y, z)
#define CMD_MANUAL_ACT     0x08 // PC -> Robot: Điều khiển thiết bị thủ công (2 bytes: dev, val)
#define CMD_ACK            0x0A // Phản hồi xác nhận lệnh (2 bytes: originalCmd, statusCode)
#define CMD_GET_TEACH      0x0B // PC -> Robot: Yêu cầu lấy 5 điểm Teach từ Flash ESP32 (0 byte)
#define CMD_TEACH_ALL_RESP 0x0C // Robot -> PC: Phản hồi 5 điểm Teach từ Flash ESP32 (30 bytes: 5x int16 X,Y,Z)

// =============================================================================
// MÃ TRẠNG THÁI PHẢN HỒI (ACK STATUS CODES)
// =============================================================================
#define ACK_STATUS_OK           0x00 // Lệnh hợp lệ và đã tiếp nhận
#define ACK_STATUS_ERR_CHECKSUM 0x01 // Lỗi Checksum
#define ACK_STATUS_ERR_LEN      0x02 // Sai chiều dài dữ liệu LEN
#define ACK_STATUS_ERR_CMD      0x03 // Lệnh không được hỗ trợ
#define ACK_STATUS_ERR_SAFETY   0x04 // Bị chặn do vi phạm an toàn động học (h < 10mm)

// =============================================================================
// MÃ KHUYẾT TẬT VÒNG BI (DEFECT CODES)
// =============================================================================
#define DEFECT_OK              0x00 // Đạt chuẩn (Normal) -> Băng tải chạy tiếp
#define DEFECT_RUSTED          0x01 // Gỉ sét bề mặt -> Gắp thả Teach 3
#define DEFECT_DAMAGED_SHIELD  0x02 // Hư nắp che -> Gắp thả Teach 4
#define DEFECT_SURFACE_DAMAGE  0x03 // Trầy xước -> Gắp thả Teach 5

// =============================================================================
// CẤU TRÚC GÓI TIN ĐÃ GIẢI MÃ
// =============================================================================
struct DatPacket {
    uint8_t cmd;
    uint8_t len;
    uint8_t data[DAT_MAX_PAYLOAD];
};

// =============================================================================
// CÁC HÀM KHỞI TẠO, PARSE VÀ TRUYỀN DỮ LIỆU
// =============================================================================
void datProtocolInit();

// Xử lý từng byte non-blocking qua máy trạng thái (FSM)
bool datProtocolProcessByte(uint8_t b, DatPacket &outPacket);

// Tính mã kiểm tra XOR Checksum: CMD ^ LEN ^ DATA[0] ^ ... ^ DATA[n-1]
uint8_t datCalculateChecksum(uint8_t cmd, uint8_t len, const uint8_t *data);

// Gửi gói tin đầy đủ theo chuẩn Dat Protocol: [SOF][CMD][LEN][DATA][CHECKSUM][EOF]
void datSendPacket(uint8_t cmd, const uint8_t *data, uint8_t len);

// Các hàm đóng gói chuyên dụng thuận tiện
void datSendAck(uint8_t originalCmd, uint8_t statusCode);
void datSendDetectTrigger(uint8_t sensorId);
void datSendTelemetry(float j1, float j2, float j3, float j4, float x, float y, float z);
void datSendTeachPoints(int16_t x1, int16_t y1, int16_t z1,
                        int16_t x2, int16_t y2, int16_t z2,
                        int16_t x3, int16_t y3, int16_t z3,
                        int16_t x4, int16_t y4, int16_t z4,
                        int16_t x5, int16_t y5, int16_t z5);

#endif // DAT_PROTOCOL_HPP
