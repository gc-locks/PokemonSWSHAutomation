using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Linq;
using System.Diagnostics;
using SwitchController.SerialConnection;

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
            serialPort.DataReceived += new SerialDataReceivedEventHandler(OnDataReceived);
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            var data = sp.ReadExisting();
            Debug.Print(data);
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

                data = HammingEncoder.Encode(data).ToArray();
                serialPort.Write(data, 0, data.Length);
            }
        }

        public void InputHat(HatState hatState)
        {
            if (serialPort.IsOpen)
            {
                byte[] data = new byte[1];
                data[0] = (byte)(0xe0 | ((byte)HatStateUtil.ToNativeHatState(hatState)));

                data = HammingEncoder.Encode(data).ToArray();
                serialPort.Write(data, 0, data.Length);
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

                data = HammingEncoder.Encode(data).ToArray();
                serialPort.Write(data, 0, data.Length);
            }
        }

        public static string[] GetSerialPorts()
        {
            return SerialPort.GetPortNames();
        }
    }
}
