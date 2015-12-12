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
    public class BasePacket<HeaderType> : IPacket<HeaderType> where HeaderType : ISerializable, new()
    {
        public int PacketSize { get; set; }
        public int HeaderSize { get; set; }
        public HeaderType Header { get; set; }

        /// <summary>
        /// Contains all bytes of the full packet
        /// </summary>
        public byte[] RawData { get; set; }

        public BasePacket() {
            this.Header = new HeaderType();
        }

        public BasePacket(byte[] data)
        {
            this.SetData(data);
        }

        public BasePacket(HeaderType header)
        {
            this.Header = header;
            this.HeaderSize = this.Header.Serialize<HeaderType>().Length;

            this.PacketSize = sizeof(int) + this.HeaderSize;
        }

        public virtual void SetData(byte[] data)
        {
            this.PacketSize = BitConverter.ToInt32(data, 0);
            this.HeaderSize = BitConverter.ToInt32(data, 4);

            this.Header = new HeaderType().Deserialize<HeaderType>(data.Skip(8).Take(this.HeaderSize).ToArray());
            
            this.RawData = data;
        }

        public HeaderType GetHeader()
        {
            return Header;
        }

        public int GetPacketSize()
        {
            return this.PacketSize;
        }

        public virtual byte[] GetMessage()
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

            return bytes;
        }
    }
}
