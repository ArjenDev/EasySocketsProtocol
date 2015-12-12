# EasySocketsProtocol
A lightweight sockets C# library with the following key features:
-Easily customizable/extendable protocol
-Generic serialization of objects optimized for less bandwidth usage
-Fast (creating/parsing packets: 1 million packets in 2 seconds)

I made this library as I could not find another library that supported optimized object serialization and an automatic translation between the byte stream and c# objects.

The following projects are the implementation of the EasySocketsProtocol, these are the only files that need to be adapted to your situation:<br>
-EasyProtocol.Client<br>
-EasyProtocol.Server<br>
-EasyProtocol.Protocol.Implementation

<h2>How to use the protocol implementation example:</h2><br>
-Define messages that implements the ISerializable interface.<br>
-Create the PacketType enum values that correspond to these messages<br>
-Create the mapping between packet types and messages in HeaderPacketMapping<br>

<h2>How to use the client:</h2>

CallBacks are optional. If an incoming message is new and/or not attached to a callBack, then OnNewPacket will be called.

```
static void Main(string[] args)
        {
            client = new SocketClient<Header, BasePacket<Header>>(OnNewPacket);
            client.StartClient(12000);

            client.Send(new HelloMessage() { Message = "Hi there" }, (responsePacket) =>
            {
                var helloMessageCallBack = PacketFactory.GetPacket<HelloMessage>(responsePacket);

                Console.WriteLine("I'm a callback! The server messaged: " + helloMessageCallBack.Payload.Message);
            });
            
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
```
        
<h2>How to use the server:</h2>
```

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
```
