using System;
using System.IO.Ports;

namespace DashBoardBearing
{
    public class SerialManager
    {
        private SerialPort serialPort;

        public Action<bool> SerialStatus;

        
        public string CurrentPortName { get; private set; }
        public int CurrentBaudRate { get; private set; }
        public int CurrentDataBits { get; private set; }
        public Parity CurrentParity { get; private set; }
        public StopBits CurrentStopBits { get; private set; }

        public bool IsRunning
        {
            get
            {
                return serialPort != null &&
                       serialPort.IsOpen;
            }
        }

        public string[] GetPorts()
        {
            return SerialPort.GetPortNames();
        }

        public bool Connect(
            string portName,
            int baudRate,
            int dataBits,
            Parity parity,
            StopBits stopBits)
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

                // Lưu cấu hình hiện tại
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
            catch
            {
            }

            serialPort.Dispose();
            serialPort = null;

            CurrentPortName = null;
            CurrentBaudRate = 0;
            CurrentDataBits = 0;
            CurrentParity = Parity.None;
            CurrentStopBits = StopBits.None;

            SerialStatus?.Invoke(false);
        }
    }
}