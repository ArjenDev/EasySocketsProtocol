using EasySocketsProtocol.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation
{   
    public class Header : IHeader, ISerializable
    {
        public PacketType PacketType { get; set; }
        public string CallBackID { get; set; }

        public Header()
        {
            this.CallBackID = Guid.NewGuid().ToString("N");
        }
    }
}
