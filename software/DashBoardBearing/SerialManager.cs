using System;
using System.IO.Ports;

namespace DashBoardBearing
{
    public class SerialManager
    {
        public SerialPort serialPort;

        public Action<bool> SerialStatus;
        public event Action<string>? DataReceivedString;
        public event Action<string>? DataSentString;

        // Các sự kiện thuộc chuẩn nhị phân Dat Protocol
        public DatProtocolDecoder Decoder { get; private set; } = new DatProtocolDecoder();
        public event Action<byte, byte[]>? PacketReceived;
        public event Action<float, float, float, float, float, float, float>? TelemetryReceived;
        public event Action<int>? DetectTriggerReceived;
        public event Action<byte, byte>? AckReceived;
        public event Action<short[][]>? TeachPointsReceived;

        public SerialLogger logger = new SerialLogger();

        public string CurrentPortName { get; private set; }
        public int CurrentBaudRate { get; private set; }
        public int CurrentDataBits { get; private set; }
        public Parity CurrentParity { get; private set; }
        public StopBits CurrentStopBits { get; private set; }

        public bool IsRunning
        {
            get
            {
                return serialPort != null && serialPort.IsOpen;
            }
        }

        public bool IsOpen => IsRunning;

        public SerialManager()
        {
            WireDecoderEvents();
        }

        private void WireDecoderEvents()
        {
            Decoder.PacketReceived += (cmd, p) => PacketReceived?.Invoke(cmd, p);
            Decoder.TelemetryReceived += (j1, j2, j3, j4, x, y, z) => TelemetryReceived?.Invoke(j1, j2, j3, j4, x, y, z);
            Decoder.DetectTriggerReceived += (sensorId) => DetectTriggerReceived?.Invoke(sensorId);
            Decoder.AckReceived += (cmd, status) => AckReceived?.Invoke(cmd, status);
            Decoder.TeachPointsReceived += (pts) => TeachPointsReceived?.Invoke(pts);
        }

        public string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Connect(string portName, int baudRate, 
                            int dataBits, Parity parity, StopBits stopBits)
        {
            try
            {
                Disconnect();

                serialPort = new SerialPort(
                    portName,
                    baudRate,
                    parity,
                    dataBits,
                    stopBits
                );

                serialPort.Open();

                serialPort.DataReceived += (s, e) =>
                {
                    try
                    {
                        if (serialPort != null && serialPort.IsOpen)
                        {
                            int bytesToRead = serialPort.BytesToRead;
                            if (bytesToRead > 0)
                            {
                                byte[] buffer = new byte[bytesToRead];
                                int readCount = serialPort.Read(buffer, 0, bytesToRead);

                                // Nạp từng byte vào bộ giải mã Dat Protocol
                                for (int i = 0; i < readCount; i++)
                                {
                                    Decoder.ProcessByte(buffer[i]);
                                }

                                // Trích xuất text phục vụ hiển thị terminal debug
                                try
                                {
                                    string text = System.Text.Encoding.UTF8.GetString(buffer, 0, readCount);
                                    if (!string.IsNullOrWhiteSpace(text) && !text.Contains('\0'))
                                    {
                                        logger.Write("RX", text.Trim());
                                        DataReceivedString?.Invoke(text.Trim());
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                };

                CurrentPortName = portName;
                CurrentBaudRate = baudRate;
                CurrentDataBits = dataBits;
                CurrentParity = parity;
                CurrentStopBits = stopBits;

                SerialStatus?.Invoke(true);
                return true;
            }
            catch
            {
                serialPort?.Dispose();
                serialPort = null;
                return false;
            }
        }

        public void Disconnect()
        {
            if (serialPort == null)
                return;

            try
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
            }
            catch { }

            serialPort.Dispose();
            serialPort = null;

            CurrentPortName = null;
            CurrentBaudRate = 0;
            CurrentDataBits = 0;
            CurrentParity = Parity.None;
            CurrentStopBits = StopBits.None;

            SerialStatus?.Invoke(false);
        }

        public bool Send(string data)
        {
            try
            {
                if (IsRunning && serialPort != null)
                {
                    serialPort.Write(data);
                    logger.Write("TX", data.Trim());
                    DataSentString?.Invoke(data.Trim());
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Write("ERR_TX", ex.Message);
            }
            return false;
        }

        public bool SendRaw(byte[] buffer)
        {
            try
            {
                if (IsRunning && serialPort != null && buffer != null && buffer.Length > 0)
                {
                    serialPort.Write(buffer, 0, buffer.Length);
                    string hex = BitConverter.ToString(buffer).Replace("-", " ");
                    logger.Write("TX_DAT", hex);
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Write("ERR_TX", ex.Message);
            }
            return false;
        }

        public bool SendPacket(byte cmd, byte[]? payload = null)
        {
            byte[] packet = DatProtocol.BuildPacket(cmd, payload);
            return SendRaw(packet);
        }

        public bool SendJog(float j1, float j2, float j3, float j4)
        {
            byte[] packet = DatProtocol.BuildJogPacket(j1, j2, j3, j4);
            return SendRaw(packet);
        }

        public bool SendAutoStart(byte speed = 180)
        {
            byte[] packet = DatProtocol.BuildAutoStartPacket(speed);
            return SendRaw(packet);
        }

        public bool SendAutoStop()
        {
            byte[] packet = DatProtocol.BuildAutoStopPacket();
            return SendRaw(packet);
        }

        public bool SendInspectResult(bool isOk, byte defectType)
        {
            byte[] packet = DatProtocol.BuildInspectResultPacket(isOk, defectType);
            return SendRaw(packet);
        }

        public bool RequestTeachPoints()
        {
            byte[] packet = DatProtocol.BuildGetTeachPacket();
            return SendRaw(packet);
        }

        public bool SendSetTeach(byte index, short x, short y, short z)
        {
            byte[] packet = DatProtocol.BuildSetTeachPacket(index, x, y, z);
            return SendRaw(packet);
        }

        public bool SendJointAngles(float j1, float j2, float j3, float j4)
        {
            // Ưu tiên gửi gói tin Dat Protocol
            return SendJog(j1, j2, j3, j4);
        }
    }
}