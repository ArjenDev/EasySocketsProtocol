using EasySocketsProtocol.Protocol;
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
    public class SocketListener<HeaderType, MyBasePacket> where HeaderType : ISerializable, new() where MyBasePacket : BasePacket<HeaderType>, new()
    {
        public delegate void OnNewPacketDelegate(Socket socket, IPacket<HeaderType> packet);
        public OnNewPacketDelegate OnNewPacket;

        private Socket socket;

        public SocketListener(Socket socket, OnNewPacketDelegate onNewPacket)
        {
            this.socket = socket;
            OnNewPacket += onNewPacket;
        }

        public void Receive()
        {
            try
            {
                // Create the state object.
                SocketState<HeaderType> state = new SocketState<HeaderType>();
                state.workSocket = socket;

                // Begin receiving the data from the remote device.
                socket.BeginReceive(state.buffer, 0, SocketState<HeaderType>.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                SocketState<HeaderType> state = (SocketState<HeaderType>)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead >= 4)
                {
                    if (state.Packet == null)
                        state.Packet = (IPacket<HeaderType>)PacketFactory.CreateBasePacket(state.buffer.Take(bytesRead).ToArray());

                    if (state.Packet.PacketSize == bytesRead)
                    {
                        state.Packet.SetData(state.buffer.Take(bytesRead).ToArray());
                        Console.WriteLine("Received {0} bytes from server.", bytesRead);

                        OnNewPacket(state.workSocket, state.Packet);

                        //Reset for new packets!
                        state = new SocketState<HeaderType>();
                        state.workSocket = client;

                        client.BeginReceive(state.buffer, 0, SocketState<HeaderType>.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                    else
                    {
                        // Get the rest of the data.
                        client.BeginReceive(state.buffer, 0, SocketState<HeaderType>.BufferSize, 0,
                            new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
