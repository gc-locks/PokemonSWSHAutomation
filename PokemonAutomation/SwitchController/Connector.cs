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
                byte[] data = new byte[1];
                data[0] = (byte)(0xc0 | ((byte)state << 4) | ((byte)button));

                serialPort.Write(data, 0, 1);
            }
        }

        public void InputHat(HatState hatState)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[1];
                data[0] = (byte)(0xe0 | ((byte)HatStateUtil.ToNativeHatState(hatState)));

                serialPort.Write(data, 0, 1);
            }
        }

        public void InputStick(Stick hatSwitch, StickState hatStateX, StickState hatStateY)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[3];
                data[0] = (byte)hatSwitch;
                data[1] = (byte)(0x7f & ((byte)hatStateX >> 1));
                data[2] = (byte)(0x7f & ((byte)hatStateY >> 1));

                serialPort.Write(data, 0, 3);
            }
        }

        public static string[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
