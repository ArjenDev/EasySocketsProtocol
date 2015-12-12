using EasySocketsProtocol.Protocol.Extensions;
using EasySocketsProtocol.Protocol.Implementation;
using EasySocketsProtocol.Protocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Implementation
{
    public class PacketFactory
    {
        public static IPacket<Header> CreateBasePacket(byte[] data)
        {
            return new BasePacket<Header>(data);
        }

        public static IPacket<Header, PayloadType> CreatePacket<PayloadType>(byte[] data) where PayloadType : ISerializable, new()
        {
            return new Packet<Header, PayloadType>(data);
        }

        /// <summary>
        /// Unpacks a basePacket into a Packet
        /// </summary>
        /// <typeparam name="PayloadType"></typeparam>
        /// <param name="basePacket"></param>
        /// <returns></returns>
        public static IPacket<Header, PayloadType> GetPacket<PayloadType>(IPacket<Header> basePacket) where PayloadType : ISerializable, new()
        {
            return CreatePacket<PayloadType>(basePacket.RawData);
        }

        public static IPacket<Header, PayloadType> CreatePacket<PayloadType>(Header header, PayloadType payload) where PayloadType : ISerializable , new()
        {
            return new Packet<Header, PayloadType>(header, payload);
        }
    }
}
