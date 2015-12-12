# EasySocketsProtocol
A lightweight sockets C# library with configurable protocol that supports serialization of objects. 

In 3 little steps you can add a new type of message to the socket communication and directly build the logic inside the server and client.

<h2>How to use the example Protocol implementation:</h2><br>
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
<h3>
Future goals:</h3><br>
-Implement a callback pattern by extending the header with a callbackID. This way the client can request something from the server and when the response packet comes back to the client it can act upon it.
