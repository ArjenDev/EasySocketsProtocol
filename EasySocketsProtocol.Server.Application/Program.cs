using EasySocketsProtocol.Protocol;
using EasySocketsProtocol.Protocol.Extensions;
using EasySocketsProtocol.Protocol.Implementation;
using EasySocketsProtocol.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Server.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            SocketServer<Header, BasePacket<Header>> server = new SocketServer<Header, BasePacket<Header>>(OnNewPacket);
            server.StartListening(12000);
            
            Console.ReadLine();
        }

        static void OnNewPacket(Socket socket, IPacket<Header> packet)
        {
            //Determine type of packet so th
            Console.WriteLine(packet.Header.PacketType);

            switch (packet.Header.PacketType)
            {
                case PacketType.Ack:
                    //Sends an ack back to the client
                    new SocketSender(socket).Send(new AckMessage());
                    break;

                case PacketType.Hello:
                    //Get the specific type of packet
                    var helloPacket = PacketFactory.GetPacket<HelloMessage>(packet);

                    //Write packet body to console
                    Console.WriteLine(helloPacket.Payload.Message);

                    //Send some response
                    new SocketSender(socket).Send(new HelloMessage() { Message = "Hi to you too :)" });
                    break;
            }
            
        }
    }    
}
