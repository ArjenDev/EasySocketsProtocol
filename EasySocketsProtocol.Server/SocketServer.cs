using EasySocketsProtocol.Protocol;
using EasySocketsProtocol.Protocol.Implementation;
using EasySocketsProtocol.Protocol.Serialization;
using EasySocketsProtocol.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Server
{
    public class SocketServer<HeaderType, MyBasePacket> where HeaderType : ISerializable, new() where MyBasePacket : BasePacket<HeaderType>, new()
    {
        SocketListener<HeaderType, BasePacket<HeaderType>>.OnNewPacketDelegate onNewPacket;

        public ManualResetEvent allDone = new ManualResetEvent(false);

        public SocketServer(SocketListener<HeaderType, BasePacket<HeaderType>>.OnNewPacketDelegate onNewPacket)
        {
            this.onNewPacket = onNewPacket;
        }

        public void StartListening(int port)
        {
            byte[] bytes = new Byte[8192];
            
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(i => i.AddressFamily != AddressFamily.InterNetworkV6);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset();

                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    
                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            SocketListener<HeaderType, BasePacket<HeaderType>> socketListener = new SocketListener<HeaderType, BasePacket<HeaderType>>(handler, onNewPacket);
            socketListener.Receive();
        }
    }
}
