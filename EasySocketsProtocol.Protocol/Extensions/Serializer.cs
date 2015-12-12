using EasySocketsProtocol.Protocol.Serialization;
using EasySocketsProtocol.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySocketsProtocol.Protocol.Extensions
{
    public static class Serializer
    {
        public static T Deserialize<T>(this T obj, byte[] data) where T : ISerializable, new()
        {
            int index = 0;
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.Name == "RawData") continue; //UBER DIRTY

                if (property.PropertyType == typeof(string))
                    property.SetValue(obj, SerializerHelper.DeserializeString(data, ref index));
                else if (property.PropertyType == typeof(int))
                    property.SetValue(obj, SerializerHelper.Deserialize<int>(BitConverter.ToInt32, data, ref index));
                else if (property.PropertyType == typeof(byte))
                    property.SetValue(obj, data[index++]);
                else if (property.PropertyType == typeof(double))
                    property.SetValue(obj, SerializerHelper.Deserialize<double>(BitConverter.ToDouble, data, ref index));
                else if (property.PropertyType == typeof(float))
                    property.SetValue(obj, SerializerHelper.Deserialize<float>(BitConverter.ToSingle, data, ref index));
                else if (property.PropertyType == typeof(long))
                    property.SetValue(obj, SerializerHelper.Deserialize<long>(BitConverter.ToInt64, data, ref index));
                else if (property.PropertyType == typeof(bool))
                    property.SetValue(obj, SerializerHelper.Deserialize<bool>(BitConverter.ToBoolean, data, ref index));
                else if (property.PropertyType == typeof(DateTime))
                    property.SetValue(obj, new DateTime(SerializerHelper.Deserialize<long>(BitConverter.ToInt64, data, ref index)));
                else if (property.PropertyType == typeof(short))
                    property.SetValue(obj, SerializerHelper.Deserialize<short>(BitConverter.ToInt16, data, ref index));
                else if (property.PropertyType.BaseType == typeof(Enum))
                    property.SetValue(obj, data[index++]); //TEMPORARY ASSUMPTION ENUM IS BYTE
                //TODO:
                //Collections
                //Nested objects


                else
                    throw new NotImplementedException();
            }

            return obj;
        }

        public static byte[] Serialize<T>(this T obj) where T : ISerializable, new()
        {
            List<byte> bytes = new List<byte>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.Name == "RawData") continue; //UBER DIRTY

                var value = property.GetValue(obj);

                if (property.PropertyType == typeof(string))
                    bytes.AddRange(SerializerHelper.Serialize((string)value));
                else if (property.PropertyType == typeof(int))
                    bytes.AddRange(BitConverter.GetBytes((int)value));
                else if (property.PropertyType == typeof(byte))
                    bytes.Add((byte)value);
                else if (property.PropertyType == typeof(double))
                    bytes.AddRange(BitConverter.GetBytes((double)value));
                else if (property.PropertyType == typeof(float))
                    bytes.AddRange(BitConverter.GetBytes((float)value));
                else if (property.PropertyType == typeof(long))
                    bytes.AddRange(BitConverter.GetBytes((long)value));
                else if (property.PropertyType == typeof(bool))
                    bytes.AddRange(BitConverter.GetBytes((bool)value));
                else if (property.PropertyType == typeof(DateTime))
                    bytes.AddRange(BitConverter.GetBytes(((DateTime)value).Ticks));
                else if (property.PropertyType == typeof(short))
                    bytes.AddRange(BitConverter.GetBytes((short)value));
                else if (property.PropertyType.BaseType == typeof(Enum))
                    bytes.Add((byte)value);//TEMPORARY ASSUMPTION ENUM IS BYTE
                else
                    throw new NotImplementedException();

            }

            return bytes.ToArray();
        }
    }
}
