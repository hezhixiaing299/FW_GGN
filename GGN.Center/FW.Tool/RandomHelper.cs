using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FW.Tool
{
    public class RandomHelper
    {
        /// <summary>
        /// 描 述:创建加密随机数生成器 生成强随机种子
        /// (new Random(CommonTool.GetRandomSeed()).Next(10000000, 99999999)).ToString(); //8位随机数
        /// </summary>
        /// <returns></returns>
        public static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
