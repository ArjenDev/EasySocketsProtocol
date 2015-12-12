using EasySocketsProtocol.Protocol.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation
{
    public class HeaderFactory
    {
        public static Header CreateHeader(Type requestType)
        {
            return new Header
            {
                PacketType = Mapping.HeaderPacketMapping.Mapping[requestType],
                CallBackID = Guid.NewGuid().ToString("N")
            };
        }
    }
}
