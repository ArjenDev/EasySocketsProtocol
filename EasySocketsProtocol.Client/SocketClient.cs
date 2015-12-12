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

namespace EasySocketsProtocol.Client
{
    public class SocketClient<HeaderType, MyBasePacket> where HeaderType : ISerializable, new() where MyBasePacket : BasePacket<Header>, new()
    {
        private Socket socket;
        private SocketListener<HeaderType, BasePacket<HeaderType>> listener;
        private SocketSender sender;

        //Delegate to call when we receive a new packet
        private SocketListener<HeaderType, BasePacket<HeaderType>>.OnNewPacketDelegate onNewPacketDelegate;

        public SocketClient(SocketListener<HeaderType, BasePacket<HeaderType>>.OnNewPacketDelegate onNewPacketDelegate)
        {
            this.onNewPacketDelegate = onNewPacketDelegate;
        }

        /// <summary>
        /// Connect to local ipv4 address
        /// </summary>
        /// <param name="port"></param>
        public void StartClient(int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(i => i.AddressFamily != AddressFamily.InterNetworkV6);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            startClient(remoteEP, port);
        }

        public void StartClient(string inputIPAddress, int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList.FirstOrDefault(i => i.ToString() == inputIPAddress);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);
            startClient(remoteEP, port);
        }

        private void startClient(IPEndPoint remoteEP, int port)
        {
            try
            {
                // Create a TCP/IP socket.
                socket = new Socket(remoteEP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.
                socket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), socket);
                connectDone.WaitOne();

                //Start listening!
                listener = new SocketListener<HeaderType, BasePacket<HeaderType>>(socket, this.onNewPacketDelegate);
                listener.Receive();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                
                client.EndConnect(ar);
                
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Send<T>(T obj) where T : ISerializable, new()
        {
            sender = new SocketSender(socket);
            sender.Send(obj);
        }
    }
}
