using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol
{
    /// <summary>
    /// This interface extends the IPacket<HeaderType> with a Payload
    /// </summary>
    /// <typeparam name="HeaderType"></typeparam>
    /// <typeparam name="PayloadType"></typeparam>
    public interface IPacket<HeaderType, PayloadType> : IPacket<HeaderType>
    {
        PayloadType Payload { get; set; }
    }

    /// <summary>
    /// This interface defines a basic message without a payload
    /// </summary>
    /// <typeparam name="HeaderType"></typeparam>
    public interface IPacket<HeaderType>
    {
        int PacketSize { get; set; }
        void SetData(byte[] data);
        HeaderType Header { get; set; }
        byte[] GetMessage();
        byte[] RawData { get; set; }
    }
}
