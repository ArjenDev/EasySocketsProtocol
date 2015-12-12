using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Serialization
{
    public class SerializerHelper
    {
        #region Serialization
        public static byte[] Serialize(string str)
        {
            var lengthBytes = BitConverter.GetBytes(str.Length * sizeof(char));

            byte[] bytes = new byte[str.Length * sizeof(char) + sizeof(int)];
            System.Buffer.BlockCopy(lengthBytes, 0, bytes, 0, lengthBytes.Length);
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 4, str.Length * sizeof(char));
            
            return bytes;
        }       

        #endregion

        #region Deserialization
        public static string DeserializeString(byte[] data, ref int index)
        {
            //Read length of string
            var stringLength = BitConverter.ToInt32(data, index);
            index += sizeof(int);
            
            //Read actual string
            char[] chars = new char[stringLength / sizeof(char)];
            System.Buffer.BlockCopy(data, index, chars, 0, stringLength);
            index += stringLength;

            return new string(chars);
        }
        
        public delegate T BitconverterSelector<T>(byte[] data, int index);
        public static T Deserialize<T>(BitconverterSelector<T> func, byte[] data, ref int index)
        {
            T number = func(data, index);
            index += Marshal.SizeOf(number);
            return number;
        }

        #endregion
    }
}
