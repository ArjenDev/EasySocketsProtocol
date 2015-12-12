using EasySocketsProtocol.Protocol;
using EasySocketsProtocol.Protocol.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Client.Application
{
    class Program
    {
        static SocketClient<Header, BasePacket<Header>> client;
        static void Main(string[] args)
        {
            client = new SocketClient<Header, BasePacket<Header>>(OnNewPacket);
            client.StartClient(12000);

            client.Send(new HelloMessage() { Message = "Hi there" } );
            
            Console.ReadLine();
        }
        static void OnNewPacket(Socket socket, IPacket<Header> packet)
        {
            //Determine what to do
            switch(packet.Header.PacketType)
            {
                case PacketType.Ack:
                    Console.WriteLine("Received an Ack packet");
                    break;

                case PacketType.Hello:
                    Console.WriteLine("Received a Hello packet");

                    //Let's check what's inside the packet
                    var helloPacket = PacketFactory.GetPacket<HelloMessage>(packet);
                    Console.WriteLine("Packet message: \"" + helloPacket.Payload.Message);

                    //Let's reply to the server
                    client.Send(new HelloMessage() { Message = "Cool stuff!" });
                    break;
            }
        
        }
    }
}
