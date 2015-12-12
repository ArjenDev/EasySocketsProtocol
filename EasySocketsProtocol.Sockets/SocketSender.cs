using EasySocketsProtocol.Protocol.Implementation;
using EasySocketsProtocol.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Sockets
{
    public class SocketSender
    {
        private Socket socket { get; set; }
        public SocketSender(Socket socket)
        {
            this.socket = socket;
        }

        public string Send<T>(T obj) where T : ISerializable, new()
        {
            var header = HeaderFactory.CreateHeader(typeof(T));
            var packet = PacketFactory.CreatePacket<T>(header, obj);

            byte[] byteData = packet.GetMessage();

            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);

            return packet.Header.CallBackID;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
