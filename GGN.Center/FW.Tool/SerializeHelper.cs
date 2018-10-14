using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace FW.Tool
{
    /// <summary>
    /// 序列化和反序列化
    /// </summary>
    public class SerializeHelper
    {
        public static byte[] SerializeObject(object pObj)
        {
            if (pObj == null)
                return null;
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, pObj);
            memoryStream.Position = 0;
            byte[] read = new byte[memoryStream.Length];
            memoryStream.Read(read, 0, read.Length);
            memoryStream.Close();
            return read;
        }
        
        public static object DeserializeObject(byte[] pBytes)
        {
            object newOjb = null;
            if (pBytes == null)
            {
                return newOjb;
            }
            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(pBytes);
            memoryStream.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            newOjb = formatter.Deserialize(memoryStream);
            memoryStream.Close();
            return newOjb;
        }
    }
}
