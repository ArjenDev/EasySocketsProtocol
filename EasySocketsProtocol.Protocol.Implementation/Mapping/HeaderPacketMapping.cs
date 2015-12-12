using EasySocketsProtocol.Protocol.Implementation.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation.Mapping
{
    public class HeaderPacketMapping
    {
        public static Dictionary<Type, PacketType> Mapping = new Dictionary<Type, PacketType>()
        {
            { typeof(HelloMessage), PacketType.Hello },
            { typeof(AckMessage), PacketType.Ack }
        };
    }    
}
