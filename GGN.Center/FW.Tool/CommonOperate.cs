using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FW.Tool
{
    /// <summary>
    /// 基本操作
    /// </summary>
    public class CommonOperate
    {
        private static string AESKey = "Test|Hahaha";
        #region 获取GUID的Long型字符串
        /// <summary>
        /// 获取GUID的Long型字符串
        /// </summary>
        /// <returns></returns>
        public string GetGuidLong()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0).ToString();
        }
        #endregion

        #region 对象转换静态
        /// <summary>
        ///  通过反射将传入的对象转换为指定类型的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static TResult ConvertObj<TResult>(object input) where TResult : class
        {
            if (input == null)
                return null;
            //类型相等时直接返回
            if (input.GetType() == typeof(TResult))
                return (TResult)input;
            CommonOperate com = new CommonOperate();
            return (TResult)com.Convert(input, typeof(TResult));
        }
        #endregion

        #region 对象转换
        /// <summary>
        ///  通过反射将传入的对象转换为指定类型的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public TResult Convert<TResult>(object input) where TResult : class
        {
            if (input == null)
                return null;
            //类型相等时直接返回
            if (input.GetType() == typeof(TResult))
                return (TResult)input;
            return (TResult)Convert(input, typeof(TResult));
        }
        /// <summary>
        ///  通过反射将传入的对象转换为指定类型的对象
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public TResult ConvertClone<TResult>(object input) where TResult : class
        {
            if (input == null)
                return null;

            return (TResult)Convert(input, typeof(TResult));
        }
        /// <summary>
        /// 通过反射将传入的对象转换为指定类型的对象
        /// </summary>
        /// <param name="input"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public  object Convert(object input, Type targetType)
        {
            object result = Activator.CreateInstance(targetType);
            var properties = targetType.GetProperties();
            var inputType = input.GetType();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (!propertyInfo.CanWrite)
                    continue;
                PropertyInfo inputPropertyInfo = inputType.GetProperty(propertyInfo.Name);
                if (inputPropertyInfo == null)
                    continue;
                object propertyValue = inputPropertyInfo.GetValue(input, null);
                propertyInfo.SetValue(result, propertyValue, null);
            }
            return result;
        }

        #endregion

        #region 传化AES加密
        /// <summary>
        /// AES加密，带初始向量
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESEncrypt(string sourceString, string key, string iv)
        {
            try
            {
                byte[] btKey = Encoding.UTF8.GetBytes(key);

                byte[] btIV = Encoding.UTF8.GetBytes(iv);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIV), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return System.Convert.ToBase64String(ms.ToArray());
                    }
                    catch
                    {
                        return sourceString;
                    }
                }
            }
            catch { }

            return "";
        }


        /// <summary>
        /// AES解密，带向量
        /// </summary>
        /// <param name="encryptedString"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static string AESDecrypt(string encryptedString, string key, string iv)
        {
            byte[] btKey = Encoding.UTF8.GetBytes(key);

            byte[] btIV = Encoding.UTF8.GetBytes(iv);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = System.Convert.FromBase64String(encryptedString);
                try
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIV), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return encryptedString;
                }
            }
            //return "";
        }


        #endregion 

        #region AES加解密
        public static String Hex_2To16(Byte[] bytes)
        {
            String hexString = String.Empty;
            Int32 iLength = 65535;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();
                if (bytes.Length < iLength)
                {
                    iLength = bytes.Length;
                }
                for (int i = 0; i < iLength; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
       public static string Encrypt(string toEncrypt)
       {
           try
           {
               Byte[] _Key = Encoding.ASCII.GetBytes(AESKey);
               Byte[] _Source = Encoding.UTF8.GetBytes(toEncrypt);
               Aes aes = Aes.Create("AES");
               aes.Mode = CipherMode.ECB;
               aes.Padding = PaddingMode.PKCS7;
               aes.Key = _Key;
               ICryptoTransform cTransform = aes.CreateEncryptor();
               Byte[] cryptData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
               String HexCryptString = Hex_2To16(cryptData);
               Byte[] HexCryptData = Encoding.UTF8.GetBytes(HexCryptString);
               String CryptString = System.Convert.ToBase64String(HexCryptData);
               return CryptString;
           }
           catch (Exception ex)
           {
               return "";
           }
       }
      public static Byte[] Hex_16To2(String hexString)
       {
           if ((hexString.Length % 2) != 0)
           {
               hexString += " ";
           }
           Byte[] returnBytes = new Byte[hexString.Length / 2];
           for (Int32 i = 0; i < returnBytes.Length; i++)
           {
               returnBytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
           }
           return returnBytes;
       }
        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
      public static string Decrypt(string toDecrypt)
      {
          Byte[] _Key = Encoding.ASCII.GetBytes(AESKey);
          Aes aes = Aes.Create("AES");
          aes.Mode = CipherMode.ECB;
          aes.Padding = PaddingMode.PKCS7;
          aes.Key = _Key;
          ICryptoTransform cTransform = aes.CreateDecryptor();
          Byte[] encryptedData = System.Convert.FromBase64String(toDecrypt);
          String encryptedString = Encoding.UTF8.GetString(encryptedData);
          Byte[] _Source = Hex_16To2(encryptedString);
          Byte[] riginalSrouceData = cTransform.TransformFinalBlock(_Source, 0, _Source.Length);
          String riginalString = Encoding.UTF8.GetString(riginalSrouceData);
          return riginalString;
      }
        #endregion

        #region 加密为MD5字符串
        /// <summary>
        /// 密码加密为MD5字符串
        /// </summary>
        /// <param name="pswd"></param>
        /// <returns></returns>
        public static string PassWordByMd5(string pswd)
        {
            if (string.IsNullOrEmpty(pswd))
            {
                pswd = "";
            }
            byte[] result = Encoding.Default.GetBytes(pswd);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }
        /// <summary>
        /// 根据密码和随机密码进行二次MD5加密
        /// </summary>
        /// <param name="pswd"></param>
        /// <returns></returns>
        public static string Md5ByRandom(string pwd,string randomPwd)
        {
            return PassWordByMd5(PassWordByMd5(pwd) + randomPwd);
        }
        #endregion

        #region 根据当前的集合生成Sql含In的语法结构字符串
        /// <summary>
        /// 根据当前的集合生成Sql含In的语法结构字符串|用于SQL语句
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static string GenerateSqlInParameterBySql(string field, IList<String> list)
        {
            var inStr = string.Empty;
            //集合为空
            if (list.Count < 1)
            {
                return " 1<>1";
            }

            foreach (var p in list)
            {
                if (p != null)
                {
                    inStr += "'" + p.ToString() + "',";
                }
            }

            if (!string.IsNullOrEmpty(inStr))
            {
                //去除最后一个"，"
                inStr = inStr.Substring(0, inStr.Length - 1);

                //构建In参数条件
                inStr = string.Format("  {0} in ({1})", field, inStr);
            }
            else
            {
                //空值
                inStr = string.Format(" 1<> 1");
            }
            return inStr;
        }
        #endregion 

        #region 加密号码

        public static string EncrypNumber(string Number)
        {
            if (string.IsNullOrEmpty(Number) || Number.Length < 3)
            {
                return Number;

            }
            string Result = "";
            var iCenter = Math.Ceiling((decimal)Number.Length / 3);
            for (int i = 0; i < Number.Length; i++)
            {
                if (i > iCenter - 1 & i < iCenter * 2 - 1)
                {
                    Result += "*";
                }
                else
                {
                    Result += Number[i];
                }
            }
            return Result;
        }
        #endregion

        #region 加密电话号码，中间部分*号表示
        public static string EncrypPhone(string Phone)
        {
            if (string.IsNullOrEmpty(Phone))
            {
                return "";
            }
            if (Phone.Length < 7)
            {
                return "";
            }
            string Result = "";
            for (int i = 0; i < Phone.Length; i++)
            {
                if (i > 2 & i < 7)
                {
                    Result += "*";
                }
                else
                {
                    Result += Phone[i];
                }
            }

            return Result;
        }
        #endregion

        #region 获取Ip地址

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIPAddress()
        {
            string user_IP = string.Empty;
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    user_IP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    user_IP = System.Web.HttpContext.Current.Request.UserHostAddress;
                }
            }
            else
            {
                user_IP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return user_IP;
        }

        #endregion

    }
}
