using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation
{
    public enum PacketType : byte
    {
        Ack = 0x00,
        Hello = 0x01
    }
}
