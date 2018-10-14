using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace FW.Base.BaseCommon
{
    /// <summary>
    /// 读取应用程序域名前缀
    /// </summary>
    public static class GlobalApplicationParam
    {
        #region 读取应用程序域名前缀
        /// <summary>
        /// 配置表
        /// </summary>
        private static Dictionary<string, object> configs;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static GlobalApplicationParam()
        {
            Load();
        }

        /// <summary>
        /// 根据传入的键获取域名前缀值
        /// </summary>
        /// <param name="key">配置的系统键值</param>
        /// <returns></returns>
        public static string GetByCode(string key)
        {
            //不抛出异常
            return GetByCode(key, false).ToString();
        }

        /// <summary>
        /// 从数据库加载配置表
        /// </summary>
        private static void Load()
        {
            configs = new Dictionary<string, object>();

            //读取数据库
            IEnumerable<string[]> configRecords = ReadConfigRecords();

            //添加记录
            foreach (string[] record in configRecords)
            {
                string key = record[0];
                string strValue = record[1];
                configs.Add(key, strValue);
            }
        }

        /// <summary>
        /// 根据传入的key获取值
        /// </summary>
        /// <param name="key">配置的键</param>
        /// <param name="throwOnError">键值不存在时是否抛出异常</param>
        /// <returns></returns>
        public static object GetByCode(string key, bool throwOnError)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key");
            }
            var targetKey = key.ToUpper();
            object result;
            if (configs.TryGetValue(targetKey, out result))
            {
                if (result is Exception)
                {
                    throw (Exception)result;
                }
                return result;
            }
            if (throwOnError)
            {
                throw new ConfigurationErrorsException(string.Format("键值{0}没有在系统配置中配置。", key));
            }
            return string.Format("键值{0}没有配置。", key);
        }

        /// <summary>
        /// 从数据库读取记录
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<string[]> ReadConfigRecords()
        {
            string connectionString = ReadConnectionString();
            string queryString = GetQueryString();

            var data = new List<string[]>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        data.Add(new[] { reader[0].ToString().ToUpper(), reader[1].ToString() });
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return data;
        }

        /// <summary>
        /// 获取查询字符串
        /// </summary>
        /// <returns></returns>
        private static string GetQueryString()
        {
            var queryBuilder = new StringBuilder();
            queryBuilder.Append(" SELECT Code,Domain");
            queryBuilder.Append(" FROM  Sys_Application ");
            return queryBuilder.ToString();
        }

        /// <summary>
        /// 获取系统配置数据库连接字符串
        /// </summary>
        /// <returns></returns>
        private static string ReadConnectionString()
        {
            var SystemConfig = ConfigurationManager.GetSection("SystemConfig") as NameValueCollection;
            if (SystemConfig == null)
            {
                throw new ConfigurationErrorsException("没有在配置文件中配置系统配置组件");
            }
            string connectionString = SystemConfig.Get("MainConStrings");
            return connectionString;
        }

        #endregion

        #region 根据参数读取访问地址前缀
        /// <summary>
        /// 获取客户端根据参数读取访问地址前缀地址
        /// </summary>
        /// <returns></returns>
        public static string GetBeginUrlBySystemName(string systemName)
        {
            //1读webconfig

            //2读静态
            var str = GlobalStaticParam.GetByCode(systemName);
            if (str != null)
            {
                return str.ToString();
            }
            return "";
        }
        #endregion
    }
}
