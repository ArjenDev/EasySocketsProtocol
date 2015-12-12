using EasySocketsProtocol.Protocol.Serialization;
using EasySocketsProtocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySocketsProtocol.Protocol.Extensions;

namespace EasySocketsProtocol.Protocol
{
    public class Packet<HeaderType, PayloadType> : BasePacket<HeaderType>, IPacket<HeaderType, PayloadType> where HeaderType : ISerializable, new() where PayloadType : ISerializable, new()
    {
        public PayloadType Payload { get; set; }

        public Packet(byte[] data) : base()
        {
            this.SetData(data);
        }

        public Packet(HeaderType header, PayloadType payload) : base(header)
        {
            this.Payload = payload;

            this.PacketSize = sizeof(int) + sizeof(int) + this.HeaderSize + this.Payload.Serialize<PayloadType>().Length;          
        }

        public override void SetData(byte[] data)
        {
            base.SetData(data);

            this.Payload = new PayloadType().Deserialize<PayloadType>(data.Skip(8 + this.HeaderSize).Take(this.PacketSize - 8 - this.HeaderSize).ToArray());
        }
                
        public override byte[] GetMessage()
        {
            byte[] bytes = new byte[this.PacketSize];
            int offset = 0;

            //Serialize packetsize
            Array.Copy(BitConverter.GetBytes(this.PacketSize), 0, bytes, offset, sizeof(int));
            offset += sizeof(int);

            //Serialize headersize
            Array.Copy(BitConverter.GetBytes(this.HeaderSize), 0, bytes, offset, sizeof(int));
            offset += sizeof(int);

            //Serialize header
            byte[] headerBytes = this.Header.Serialize();
            Array.Copy(headerBytes, 0, bytes, offset, headerBytes.Length);
            offset += headerBytes.Length;

            //Serialize payload
            byte[] payloadBytes = this.Payload.Serialize();
            Array.Copy(payloadBytes, 0, bytes, offset, payloadBytes.Length);

            return bytes;            
        }
    }
}
