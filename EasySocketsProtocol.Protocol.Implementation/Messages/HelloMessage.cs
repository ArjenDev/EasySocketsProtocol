using EasySocketsProtocol.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation
{
    public class HelloMessage : ISerializable
    {
        public string Message { get; set; }

        public HelloMessage() { }
        public HelloMessage(string message)
        {
            this.Message = message;
        }
            
    }
}
