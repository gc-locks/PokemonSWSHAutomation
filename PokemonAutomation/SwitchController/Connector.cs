using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace SwitchController
{
    public class Connector : IDisposable, IController
    {
        private readonly System.IO.Ports.SerialPort serialPort;

        public Connector(string comPort)
        {
            serialPort = new SerialPort
            {
                BaudRate = 115200,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                PortName = comPort
            };

            serialPort.Open();
        }

        public void Dispose()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        public bool Available()
        {
            return serialPort.IsOpen;
        }

        public void InputButton(Button button, ButtonState state)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[2];
                data[0] = (byte)button;
                data[1] = (byte)state;

                serialPort.Write(data, 0, 2);
            }
        }

        public void InputHatSwitch(HatSwitch hatSwitch, HatState hatStateX, HatState hatStateY)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[3];
                data[0] = (byte)hatSwitch;
                data[1] = (byte)hatStateX;
                data[2] = (byte)hatStateY;

                serialPort.Write(data, 0, 3);
            }
        }

        public static string[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
