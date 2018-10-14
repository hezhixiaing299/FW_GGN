﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;

namespace FW.Base.BaseCommon
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public static class GlobalStaticParam
    {
        //private static readonly object objectToLock; //对象锁
        //private static bool loaded; //是否已读取

        /// <summary>
        /// 配置表
        /// </summary>
        private static Dictionary<string, object> configs;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static GlobalStaticParam()
        {
            Load();
        }

        /// <summary>
        /// 根据传入的键获取值
        /// </summary>
        /// <param name="code">配置的键</param>
        /// <returns></returns>
        public static object GetByCode(string code)
        {
            //不抛出异常
            return GetByCode(code, false);
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
        /// <param name="code">配置的键</param>
        /// <param name="throwOnError">键值不存在时是否抛出异常</param>
        /// <returns></returns>
        public static object GetByCode(string code, bool throwOnError)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException("Code");
            }
            var targetKey = code.ToUpper();
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
                throw new ConfigurationErrorsException(string.Format("键值{0}没有在系统配置中配置.", code));
            }
            return string.Format("键值{0}没有配置.", code);
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
            queryBuilder.Append(" SELECT Code,Value FROM  Sys_StaticData");
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
                throw new ConfigurationErrorsException("配置文件中没有配置");
            }
            string connectionString = SystemConfig.Get("MainConStrings");
            return connectionString;
        }

        /// <summary>
        /// 获取系统配置数据库连接字符串
        /// </summary>
        /// <returns></returns>
        public static string ReadConnectionString(string Key)
        {
            var SystemConfig = ConfigurationManager.GetSection("SystemConfig") as NameValueCollection;
            if (SystemConfig == null)
            {
                throw new ConfigurationErrorsException("配置文件中没有找到该Key!");
            }
            string connectionString = SystemConfig.Get(Key);
            return connectionString;
        }

    }
}