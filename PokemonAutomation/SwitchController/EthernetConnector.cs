using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using SwitchController.SerialConnection;
using System.Net.Sockets;
using System.Net;

namespace SwitchController
{
    public class EthernetConnector : IDisposable, IController
    {
        private readonly TcpClient client;

        public EthernetConnector(string ipAddr)
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddr), 80);
        }


        public void Dispose()
        {
            client.Close();
            client.Dispose();
        }

        public bool Available()
        {
            return client.Connected;
        }

        public void InputButton(Button button, ButtonState state)
        {
            if (!Available())
                return;

            var stream = client.GetStream();
            byte[] data = new byte[1];
            data[0] = (byte)(0xc0 | ((byte)state << 4) | ((byte)button));

            stream.Write(data, 0, data.Length);
        }

        public void InputHat(HatState hatState)
        {
            if (!Available())
                return;

            var stream = client.GetStream();
            byte[] data = new byte[1];
            data[0] = (byte)(0xe0 | ((byte)HatStateUtil.ToNativeHatState(hatState)));

            stream.Write(data, 0, data.Length);
        }

        public void InputStick(Stick hatSwitch, StickState hatStateX, StickState hatStateY)
        {
            if (!Available())
                return;

            var stream = client.GetStream();
            byte[] data = new byte[3];
            data[0] = (byte)hatSwitch;
            data[1] = (byte)(0x7f & ((byte)hatStateX >> 1));
            data[2] = (byte)(0x7f & ((byte)hatStateY >> 1));

            stream.Write(data, 0, data.Length);
        }
    }
}
