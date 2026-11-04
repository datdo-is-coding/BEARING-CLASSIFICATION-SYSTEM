using System;

namespace DashBoardBearing
{
    /// <summary>
    /// Định nghĩa giao thức truyền thông nhị phân Dat Protocol [SOF][CMD][LEN][DATA][CHECKSUM][EOF]
    /// </summary>
    public static class DatProtocol
    {
        public const byte DAT_SOF = 0xAA;
        public const byte DAT_EOF = 0x55;

        // Command Identifiers
        public const byte CMD_JOG            = 0x01; // PC -> Robot: Điều khiển góc khớp J1..J4 (16 bytes float)
        public const byte CMD_AUTO_START     = 0x02; // PC -> Robot: Bật chu trình tự động (1 byte speed)
        public const byte CMD_AUTO_STOP      = 0x03; // PC -> Robot: Dừng chu trình tự động (0 byte)
        public const byte CMD_DETECT_TRIGGER = 0x04; // Robot -> PC: Cảm biến quang phát hiện vòng bi (2 bytes)
        public const byte CMD_INSPECT_RESULT = 0x05; // PC -> Robot: Kết quả nhận diện AI (2 bytes: isOk, defectType)
        public const byte CMD_TELEMETRY      = 0x06; // Robot -> PC: Phản hồi góc J1..J4 và XYZ thời gian thực (28 bytes)
        public const byte CMD_SET_TEACH      = 0x07; // PC -> Robot: Lưu điểm Teach Point (7 bytes: index, x, y, z)
        public const byte CMD_MANUAL_ACT     = 0x08; // PC -> Robot: Bật/tắt thiết bị thủ công (2 bytes: dev, val)
        public const byte CMD_ACK            = 0x0A; // Phản hồi xác nhận lệnh (2 bytes)
        public const byte CMD_GET_TEACH      = 0x0B; // PC -> Robot: Yêu cầu lấy 5 điểm Teach từ Flash ESP32 (0 byte)
        public const byte CMD_TEACH_ALL_RESP = 0x0C; // Robot -> PC: Phản hồi 5 điểm Teach từ Flash ESP32 (30 bytes)

        // ACK Status Codes
        public const byte ACK_STATUS_OK           = 0x00;
        public const byte ACK_STATUS_ERR_CHECKSUM = 0x01;
        public const byte ACK_STATUS_ERR_LEN      = 0x02;
        public const byte ACK_STATUS_ERR_CMD      = 0x03;
        public const byte ACK_STATUS_ERR_SAFETY   = 0x04;

        // Defect Codes
        public const byte DEFECT_OK              = 0x00; // Đạt chuẩn (Bình thường)
        public const byte DEFECT_RUSTED          = 0x01; // Gỉ sét bề mặt
        public const byte DEFECT_DAMAGED_SHIELD  = 0x02; // Hư nắp che
        public const byte DEFECT_SURFACE_DAMAGE  = 0x03; // Trầy xước

        /// <summary>
        /// Tính mã kiểm tra XOR Checksum: CMD ^ LEN ^ DATA[0] ^ ... ^ DATA[n-1]
        /// </summary>
        public static byte CalculateChecksum(byte cmd, byte len, byte[]? data)
        {
            byte csum = (byte)(cmd ^ len);
            if (data != null && len > 0)
            {
                for (int i = 0; i < len && i < data.Length; i++)
                {
                    csum ^= data[i];
                }
            }
            return csum;
        }

        /// <summary>
        /// Đóng gói khung truyền đầy đủ: [SOF][CMD][LEN][DATA][CHECKSUM][EOF]
        /// </summary>
        public static byte[] BuildPacket(byte cmd, byte[]? payload = null)
        {
            byte len = (byte)(payload != null ? Math.Min(payload.Length, 255) : 0);
            byte checksum = CalculateChecksum(cmd, len, payload);

            byte[] packet = new byte[len + 5];
            packet[0] = DAT_SOF;
            packet[1] = cmd;
            packet[2] = len;
            if (len > 0 && payload != null)
            {
                Array.Copy(payload, 0, packet, 3, len);
            }
            packet[3 + len] = checksum;
            packet[4 + len] = DAT_EOF;

            return packet;
        }

        /// <summary>
        /// Đóng gói lệnh JOG 4 góc khớp (J1..J4 dạng IEEE-754 Little Endian 16 bytes)
        /// </summary>
        public static byte[] BuildJogPacket(float j1, float j2, float j3, float j4)
        {
            byte[] payload = new byte[16];
            Buffer.BlockCopy(BitConverter.GetBytes(j1), 0, payload, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(j2), 0, payload, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(j3), 0, payload, 8, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(j4), 0, payload, 12, 4);
            return BuildPacket(CMD_JOG, payload);
        }

        /// <summary>
        /// Đóng gói lệnh bắt đầu chế độ tự động kèm tốc độ băng tải
        /// </summary>
        public static byte[] BuildAutoStartPacket(byte speed = 180)
        {
            return BuildPacket(CMD_AUTO_START, new byte[] { speed });
        }

        /// <summary>
        /// Đóng gói lệnh dừng chế độ tự động
        /// </summary>
        public static byte[] BuildAutoStopPacket()
        {
            return BuildPacket(CMD_AUTO_STOP, Array.Empty<byte>());
        }

        /// <summary>
        /// Đóng gói kết quả nhận diện AI gửi về Robot
        /// </summary>
        public static byte[] BuildInspectResultPacket(bool isOk, byte defectType)
        {
            return BuildPacket(CMD_INSPECT_RESULT, new byte[] { (byte)(isOk ? 1 : 0), defectType });
        }

        /// <summary>
        /// Đóng gói yêu cầu đọc 5 điểm Teach từ Flash ESP32
        /// </summary>
        public static byte[] BuildGetTeachPacket()
        {
            return BuildPacket(CMD_GET_TEACH, Array.Empty<byte>());
        }

        /// <summary>
        /// Đóng gói lệnh lưu 1 điểm Teach vào Flash ESP32
        /// </summary>
        public static byte[] BuildSetTeachPacket(byte index, short x, short y, short z)
        {
            byte[] payload = new byte[7];
            payload[0] = index;
            Buffer.BlockCopy(BitConverter.GetBytes(x), 0, payload, 1, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(y), 0, payload, 3, 2);
            Buffer.BlockCopy(BitConverter.GetBytes(z), 0, payload, 5, 2);
            return BuildPacket(CMD_SET_TEACH, payload);
        }
    }

    /// <summary>
    /// Máy trạng thái giải mã từng byte luồng Serial theo chuẩn Dat Protocol (Non-blocking FSM Parser)
    /// </summary>
    public class DatProtocolDecoder
    {
        private enum ParseState
        {
            WaitSof,
            ReadCmd,
            ReadLen,
            ReadData,
            ReadChecksum,
            ReadEof
        }

        private ParseState _state = ParseState.WaitSof;
        private byte _cmd;
        private byte _len;
        private readonly byte[] _dataBuffer = new byte[64];
        private int _dataIndex;
        private byte _checksum;

        public event Action<byte, byte[]>? PacketReceived;
        public event Action<float, float, float, float, float, float, float>? TelemetryReceived;
        public event Action<int>? DetectTriggerReceived;
        public event Action<byte, byte>? AckReceived;
        public event Action<short[][]>? TeachPointsReceived;

        public void ProcessByte(byte b)
        {
            switch (_state)
            {
                case ParseState.WaitSof:
                    if (b == DatProtocol.DAT_SOF)
                    {
                        _state = ParseState.ReadCmd;
                    }
                    break;

                case ParseState.ReadCmd:
                    _cmd = b;
                    _state = ParseState.ReadLen;
                    break;

                case ParseState.ReadLen:
                    _len = b;
                    if (_len > 64)
                    {
                        _state = ParseState.WaitSof;
                    }
                    else if (_len == 0)
                    {
                        _state = ParseState.ReadChecksum;
                    }
                    else
                    {
                        _dataIndex = 0;
                        _state = ParseState.ReadData;
                    }
                    break;

                case ParseState.ReadData:
                    _dataBuffer[_dataIndex++] = b;
                    if (_dataIndex >= _len)
                    {
                        _state = ParseState.ReadChecksum;
                    }
                    break;

                case ParseState.ReadChecksum:
                    _checksum = b;
                    _state = ParseState.ReadEof;
                    break;

                case ParseState.ReadEof:
                    _state = ParseState.WaitSof;
                    if (b == DatProtocol.DAT_EOF)
                    {
                        byte expected = DatProtocol.CalculateChecksum(_cmd, _len, _dataBuffer);
                        if (_checksum == expected)
                        {
                            byte[] payload = new byte[_len];
                            if (_len > 0)
                            {
                                Array.Copy(_dataBuffer, payload, _len);
                            }
                            OnPacketParsed(_cmd, payload);
                        }
                    }
                    break;
            }
        }

        private void OnPacketParsed(byte cmd, byte[] payload)
        {
            PacketReceived?.Invoke(cmd, payload);

            if (cmd == DatProtocol.CMD_TELEMETRY && payload.Length >= 28)
            {
                float j1 = BitConverter.ToSingle(payload, 0);
                float j2 = BitConverter.ToSingle(payload, 4);
                float j3 = BitConverter.ToSingle(payload, 8);
                float j4 = BitConverter.ToSingle(payload, 12);
                float x = BitConverter.ToSingle(payload, 16);
                float y = BitConverter.ToSingle(payload, 20);
                float z = BitConverter.ToSingle(payload, 24);

                TelemetryReceived?.Invoke(j1, j2, j3, j4, x, y, z);
            }
            else if (cmd == DatProtocol.CMD_DETECT_TRIGGER && payload.Length >= 1)
            {
                int sensorId = payload[0];
                DetectTriggerReceived?.Invoke(sensorId);
            }
            else if (cmd == DatProtocol.CMD_ACK && payload.Length >= 2)
            {
                AckReceived?.Invoke(payload[0], payload[1]);
            }
            else if (cmd == DatProtocol.CMD_TEACH_ALL_RESP && payload.Length >= 30)
            {
                short[][] points = new short[5][];
                for (int i = 0; i < 5; i++)
                {
                    short px = BitConverter.ToInt16(payload, i * 6);
                    short py = BitConverter.ToInt16(payload, i * 6 + 2);
                    short pz = BitConverter.ToInt16(payload, i * 6 + 4);
                    points[i] = new short[] { px, py, pz };
                }
                TeachPointsReceived?.Invoke(points);
            }
        }
    }
}

