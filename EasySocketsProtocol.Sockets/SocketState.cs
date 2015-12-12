using EasySocketsProtocol.Protocol;
using EasySocketsProtocol.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Sockets
{
    class SocketState<HeaderType> where HeaderType : ISerializable, new()
    {
        public Socket workSocket = null;
        public const int BufferSize = 8192;
        public IPacket<HeaderType> Packet;
        public byte[] buffer = new byte[BufferSize];
    }
}
