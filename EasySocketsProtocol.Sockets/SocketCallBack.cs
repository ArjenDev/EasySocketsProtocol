using EasySocketsProtocol.Protocol;
using EasySocketsProtocol.Protocol.Serialization;
using EasySocketsProtocol.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Sockets
{
    public static class SocketCallBack<HeaderType> where HeaderType : IHeader, ISerializable, new()
    {
        public delegate void OnCallBackDelegate(IPacket<HeaderType> packet);
        public static OnCallBackDelegate OnCallBack { get; set; }


        public static Dictionary<string, OnCallBackDelegate> PendingCallBacks = new Dictionary<string, OnCallBackDelegate>();
        public static void RegisterCallBack(string callBackID, OnCallBackDelegate callBack)
        {
            PendingCallBacks[callBackID] = callBack;
        }

        public static bool OnNewPacket(IPacket<HeaderType> packet)
        {
            OnCallBackDelegate callBack;
            if (PendingCallBacks.TryGetValue(packet.Header.CallBackID, out callBack))
            {
                callBack(packet);

                //Delete callback
                PendingCallBacks.Remove(packet.Header.CallBackID);

                return true;
            }

            return false;
        }
    }
}
