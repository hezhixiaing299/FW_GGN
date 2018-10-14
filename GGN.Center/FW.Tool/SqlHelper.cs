using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace FW.Tool
{
    public class SqlHelper
    {
        /// <summary>
        /// 是否从配置文件读取超时
        /// </summary>
        public static bool TimeoutHasBeenDetermined { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string _connectionString
        {
            get
            {
                NameValueCollection nameValueCollection = ConfigurationManager.GetSection("SystemConfig") as NameValueCollection;
                if (nameValueCollection == null)
                {
                    throw new ConfigurationErrorsException("配置文件中没有配置");
                }
                return nameValueCollection.Get("connectionStrings");
            }
        }
        private static int _commandTimeout;
        /// <summary>
        /// 设置连接超时，默认读配置文件
        /// </summary>
        public static int CommandTimeout
        {
            get
            {
                if (!SqlHelper.TimeoutHasBeenDetermined)
                {
                    GetCommandTimeout();
                }
                return SqlHelper._commandTimeout;
            }
            set
            {
                SqlHelper._commandTimeout = value;
            }
        }

        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <param name="commandText">Sql命令文本</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQueryAsync(commandText, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <param name="commandText">Sql命令文本</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType)
        {
            return await ExecuteNonQueryAsync<DbParameter>(commandText, null, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <typeparam name="TParameter">参数类型</typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters">参数集合</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery<TParameter>(string commandText, IEnumerable<TParameter> commandParameters) where TParameter : DbParameter
        {
            return ExecuteNonQueryAsync<TParameter>(commandText, commandParameters, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <typeparam name="TParameter">参数类型</typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters">参数集合</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync<TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("connectionString can not null");
            }
            int result;
            using (DbConnection cn = CreateConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                result = await ExecuteNonQueryAsync<TParameter>(cn, commandText, commandParameters, commandType).ConfigureAwait(false);
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <param name="connection">数据库链接</param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbConnection connection, string commandText)
        {
            return ExecuteNonQueryAsync(connection, commandText, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <param name="connection">数据库链接</param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(DbConnection connection, string commandText, CommandType commandType)
        {
            return await ExecuteNonQueryAsync<DbParameter>(connection, commandText, null, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <param name="connection">数据库链接</param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync<TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters) where TParameter : DbParameter
        {
            return await ExecuteNonQueryAsync<TParameter>(connection, commandText, commandParameters, CommandType.Text).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <typeparam name="TParameter">参数类型</typeparam>
        /// <param name="connection">数据库链接</param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters">参数集合</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery<TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteNonQueryAsync<TParameter>(connection, commandText, commandParameters, commandType).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数
        /// </summary>
        /// <typeparam name="TParameter">参数类型</typeparam>
        /// <param name="connection">数据库链接</param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters">参数集合</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync<TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            int result;
            using (DbCommand cmd = CreateCommand())
            {
                await PrepareCommandAsync<TParameter>(connection, null, cmd, commandText, commandParameters, commandType).ConfigureAwait(false);
                int retval = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                cmd.Parameters.Clear();
                result = retval;
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回影响的行数(事务)
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandText">命令文本</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction transaction, string commandText)
        {
            return ExecuteNonQueryAsync(transaction, commandText, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数(事务)
        /// </summary>
        /// <param name="transaction">事务</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return await ExecuteNonQueryAsync<DbParameter>(transaction, commandText, null, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回影响的行数(事务)
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction">事务</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandParameters">参数列表</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteNonQueryAsync<TParameter>(transaction, commandText, commandParameters, commandType).Result;
        }
        /// <summary>
        /// 执行命令返回影响的行数(事务)
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction">事务</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandParameters">参数列表</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteNonQueryAsync<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction", "Parameter {0} cannot be null.");
            }
            int result;
            using (DbCommand cmd = CreateCommand())
            {
                await PrepareCommandAsync<TParameter>(transaction.Connection, transaction, cmd, commandText, commandParameters, commandType).ConfigureAwait(false);
                int retval = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                cmd.Parameters.Clear();
                result = retval;
            }
            return result;
        }

        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, null, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, IEnumerable<DbParameter> commandParameters)
        {
            return ExecuteDataSet(commandText, commandParameters, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <param name="commandParameters">参数</param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string commandText, IEnumerable<DbParameter> commandParameters, CommandType commandType)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("connectionString can not null");
            }
            DataSet result;
            using (DbConnection dbConnection = CreateConnection(_connectionString))
            {
                dbConnection.Open();
                result = ExecuteDataSet(dbConnection, commandText, commandParameters, commandType);
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(DbConnection connection, string commandText)
        {
            return ExecuteDataSet(connection, commandText, null, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(DbConnection connection, string commandText, IEnumerable<DbParameter> commandParameters)
        {
            return ExecuteDataSet(connection, commandText, commandParameters, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(DbConnection connection, string commandText, CommandType commandType)
        {
            return ExecuteDataSet(connection, commandText, null, commandType);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(DbConnection connection, string commandText, IEnumerable<DbParameter> commandParameters, CommandType commandType)
        {
            DataSet result;
            using (DbCommand dbCommand = CreateCommand())
            {
                PrepareCommand(connection, null, dbCommand, commandText, commandParameters, commandType);
                using (DbDataAdapter dbDataAdapter = CreateDataAdapter(dbCommand))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Locale = CultureInfo.CurrentCulture;
                    dbDataAdapter.Fill(dataSet);
                    dbCommand.Parameters.Clear();
                    result = dataSet;
                }
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(DbTransaction transaction, string commandText)
        {
            return ExecuteDataSet<DbParameter>(transaction, commandText, null, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回DataSet
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction can not null");
            }
            DataSet result;
            using (DbCommand dbCommand = CreateCommand())
            {
                PrepareCommand(transaction.Connection, transaction, dbCommand, commandText, commandParameters, commandType);
                using (DbDataAdapter dbDataAdapter = CreateDataAdapter(dbCommand))
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Locale = CultureInfo.CurrentCulture;
                    dbDataAdapter.Fill(dataSet);
                    dbCommand.Parameters.Clear();
                    result = dataSet;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="connectionOwnership"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader, TParameter>(DbConnection connection, DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, SqlHelper.SqlConnectionOwnership connectionOwnership, CommandType commandType) where TReader : DbDataReader where TParameter : DbParameter
        {
            DbCommand cmd = CreateCommand();
            await PrepareCommandAsync<TParameter>(connection, transaction, cmd, commandText, commandParameters, commandType).ConfigureAwait(false);
            DbDataReader dr;
            if (connectionOwnership == SqlHelper.SqlConnectionOwnership.External)
            {
                dr = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            }
            else
            {
                dr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
            }
            cmd.Parameters.Clear();
            return dr as TReader;
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader>(string commandText) where TReader : DbDataReader
        {
            return ExecuteReaderAsync<TReader>(commandText, CommandType.Text).Result;
        }

        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader>(string commandText, CommandType commandType) where TReader : DbDataReader
        {
            return await ExecuteReaderAsync<TReader, DbParameter>(commandText, null, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader, TParameter>(CommandType commandType, string commandText, IEnumerable<TParameter> commandParameters) where TReader : DbDataReader where TParameter : DbParameter
        {
            return ExecuteReaderAsync<TReader, TParameter>(commandText, commandParameters, commandType).Result;
        }

        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader, TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TReader : DbDataReader where TParameter : DbParameter
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("connectionString can not null");
            }
            DbConnection cn = CreateConnection(_connectionString);
            await cn.OpenAsync().ConfigureAwait(false);
            TReader result;
            try
            {
                TReader reader = await ExecuteReaderAsync<TReader, TParameter>(cn, null, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.Internal, commandType).ConfigureAwait(false);
                result = reader;
            }
            catch
            {
                cn.Close();
                throw;
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader>(DbConnection connection, CommandType commandType, string commandText) where TReader : DbDataReader
        {
            return ExecuteReaderAsync<TReader>(connection, commandType, commandText).Result;
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader>(DbConnection connection, CommandType commandType, string commandText) where TReader : DbDataReader
        {
            return await ExecuteReaderAsync<TReader, DbParameter>(connection, commandType, commandText, null).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader, TParameter>(DbConnection connection, CommandType commandType, string commandText, IEnumerable<TParameter> commandParameters) where TReader : DbDataReader where TParameter : DbParameter
        {
            return ExecuteReaderAsync<TReader, TParameter>(connection, commandType, commandText, commandParameters).Result;
        }

        /// <summary>
        ///  执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader, TParameter>(DbConnection connection, CommandType commandType, string commandText, IEnumerable<TParameter> commandParameters) where TReader : DbDataReader where TParameter : DbParameter
        {
            return await ExecuteReaderAsync<TReader, TParameter>(connection, null, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External, commandType).ConfigureAwait(false);
        }
        /// <summary>
        ///  执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader>(DbTransaction transaction, CommandType commandType, string commandText) where TReader : DbDataReader
        {
            return ExecuteReaderAsync<TReader>(transaction, commandType, commandText).Result;
        }
        /// <summary>
        /// 执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader>(DbTransaction transaction, CommandType commandType, string commandText) where TReader : DbDataReader
        {
            return await ExecuteReaderAsync<TReader, DbParameter>(transaction, commandType, commandText, null).ConfigureAwait(false);
        }
        /// <summary>
        ///  执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static TReader ExecuteReader<TReader, TParameter>(DbTransaction transaction, CommandType commandType, string commandText, IEnumerable<TParameter> commandParameters) where TReader : DbDataReader where TParameter : DbParameter
        {
            return ExecuteReaderAsync<TReader, TParameter>(transaction, commandType, commandText, commandParameters).Result;
        }
        /// <summary>
        ///  执行命令返回DataReader
        /// </summary>
        /// <typeparam name="TReader"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static async Task<TReader> ExecuteReaderAsync<TReader, TParameter>(DbTransaction transaction, CommandType commandType, string commandText, IEnumerable<TParameter> commandParameters) where TReader : DbDataReader where TParameter : DbParameter
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction can not null");
            }
            return await ExecuteReaderAsync<TReader, TParameter>(transaction.Connection, transaction, commandText, commandParameters, SqlHelper.SqlConnectionOwnership.External, commandType).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar<object>(commandText, CommandType.Text);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static TResult ExecuteScalar<TResult>(string commandText, CommandType commandType)
        {
            return ExecuteScalarAsync<TResult, DbParameter>(commandText, null, commandType).Result;
        }

        /// <summary>
        ///  执行命令返回1*1数据
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync(string commandText)
        {
            return await ExecuteScalarAsync<DbParameter>(commandText, null, CommandType.Text).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandType"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static object ExecuteScalar<TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalar<object, TParameter>(commandText, commandParameters, commandType);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static TResult ExecuteScalar<TResult, TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalarAsync<TResult, TParameter>(commandText, commandParameters, commandType).Result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync<TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return await ExecuteScalarAsync<object, TParameter>(commandText, commandParameters, commandType).ConfigureAwait(false);
        }

        /// <summary>
        ///  执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteScalarAsync<TResult, TParameter>(string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                throw new ArgumentException("connectionString can not null");
            }
            TResult result;
            using (DbConnection cn = CreateConnection(_connectionString))
            {
                await cn.OpenAsync().ConfigureAwait(false);
                result = await ExecuteScalarAsync<TResult, TParameter>(cn, commandText, commandParameters, commandType).ConfigureAwait(false);
            }
            return result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbConnection connection, string commandText, CommandType commandType)
        {
            return ExecuteScalar<object>(connection, commandText, commandType);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(DbConnection connection, string commandText, CommandType commandType)
        {
            return ExecuteScalarAsync<T>(connection, commandText, commandType).Result;
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync(DbConnection connection, string commandText, CommandType commandType)
        {
            return await ExecuteScalarAsync<object>(connection, commandText, commandType).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteScalarAsync<TResult>(DbConnection connection, string commandText, CommandType commandType)
        {
            return await ExecuteScalarAsync<TResult, DbParameter>(connection, commandText, null, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar<TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalar<object, TParameter>(connection, commandText, commandParameters, commandType);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static TResult ExecuteScalar<TResult, TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalarAsync<TResult, TParameter>(connection, commandText, commandParameters, commandType).Result;
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync<TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return await ExecuteScalarAsync<object, TParameter>(connection, commandText, commandParameters, commandType).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="connection"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteScalarAsync<TResult, TParameter>(DbConnection connection, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            TResult result;
            using (DbCommand cmd = CreateCommand())
            {
                await PrepareCommandAsync<TParameter>(connection, null, cmd, commandText, commandParameters, commandType).ConfigureAwait(false);
                object retval = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                cmd.Parameters.Clear();
                result = (TResult)((object)retval);
            }
            return result;
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return ExecuteScalar<object>(transaction, commandText, commandType);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(DbTransaction transaction, string commandText)
        {
            return ExecuteScalarAsync<T>(transaction, commandText, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T>(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return ExecuteScalarAsync<T>(transaction, commandText, commandType).Result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return await ExecuteScalarAsync<object>(transaction, commandText, commandType).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteScalarAsync<TResult>(DbTransaction transaction, string commandText, CommandType commandType)
        {
            return await ExecuteScalarAsync<TResult, DbParameter>(transaction, commandText, null, CommandType.Text).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static object ExecuteScalar<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalar<object, TParameter>(transaction, commandText, commandParameters, commandType);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T, TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return ExecuteScalarAsync<T, TParameter>(transaction, commandText, commandParameters, commandType).Result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static T ExecuteScalar<T, TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters) where TParameter : DbParameter
        {
            return ExecuteScalarAsync<T, TParameter>(transaction, commandText, commandParameters, CommandType.Text).Result;
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            return await ExecuteScalarAsync<object, TParameter>(transaction, commandText, commandParameters, commandType).ConfigureAwait(false);
        }

        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static async Task<object> ExecuteScalarAsync<TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters) where TParameter : DbParameter
        {
            return await ExecuteScalarAsync<object, TParameter>(transaction, commandText, commandParameters, CommandType.Text).ConfigureAwait(false);
        }
        /// <summary>
        /// 执行命令返回1*1数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public static async Task<T> ExecuteScalarAsync<T, TParameter>(DbTransaction transaction, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction can not null");
            }
            T result;
            using (DbCommand cmd = CreateCommand())
            {
                await PrepareCommandAsync<TParameter>(transaction.Connection, transaction, cmd, commandText, commandParameters, commandType).ConfigureAwait(false);
                object retval = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                cmd.Parameters.Clear();
                result = (T)((object)retval);
            }
            return result;
        }
        /// <summary>
        /// 向Command附加参数
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="command"></param>
        /// <param name="commandParameters"></param>
        private static void AttachParameters<TParameter>(DbCommand command, IEnumerable<TParameter> commandParameters) where TParameter : DbParameter
        {
            foreach (TParameter tparameter in commandParameters)
            {
                if ((tparameter.Direction == ParameterDirection.Input || tparameter.Direction == ParameterDirection.InputOutput) && tparameter.Value == null)
                {
                    tparameter.Value = DBNull.Value;
                }
                command.Parameters.Add(tparameter);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandParameters"></param>
        /// <param name="parameterValues"></param>
        private static void AssignParameterValues(IList<SqlParameter> commandParameters, IList<object> parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
            {
                return;
            }
            if (commandParameters.Count != parameterValues.Count)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }
            for (int i = 0; i < commandParameters.Count; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParameter"></typeparam>
        /// <param name="command"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        private static async Task PrepareCommandAsync<TParameter>(DbConnection connection, DbTransaction transaction, DbCommand command, string commandText, IEnumerable<TParameter> commandParameters, CommandType commandType) where TParameter : DbParameter
        {
            if (connection == null)
            {
                throw new ArgumentNullException("connection can not null");
            }
            if (string.IsNullOrWhiteSpace(commandText))
            {
                throw new ArgumentException("commandText can not null");
            }
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync().ConfigureAwait(false);
            }
            command.Connection = connection;
            command.CommandTimeout = CommandTimeout;
            command.CommandText = commandText;
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                SqlHelper.AttachParameters<TParameter>(command, commandParameters);
            }
        }
        /// <summary>
        /// 准备Command
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="command"></param>
        /// <param name="commandText"></param>
        /// <param name="commandParameters"></param>
        /// <param name="commandType"></param>
        private static void PrepareCommand(DbConnection connection, DbTransaction transaction, DbCommand command, string commandText, IEnumerable<DbParameter> commandParameters, CommandType commandType)
        {
            PrepareCommandAsync<DbParameter>(connection, transaction, command, commandText, commandParameters, commandType).ConfigureAwait(false);
        }
        /// <summary>
        /// 获取超时
        /// </summary>
        private static void GetCommandTimeout()
        {
            if (!SqlHelper.TimeoutHasBeenDetermined)
            {
                if (ConfigurationManager.AppSettings["SqlCommandTimeout"] != null)
                {
                    string text = ConfigurationManager.AppSettings["SqlCommandTimeout"];
                    int commandTimeout;
                    if (!string.IsNullOrEmpty(text) && int.TryParse(text, out commandTimeout))
                    {
                        CommandTimeout = commandTimeout;
                        SqlHelper.TimeoutHasBeenDetermined = true;
                    }
                }
                if (!SqlHelper.TimeoutHasBeenDetermined)
                {
                    using (DbCommand dbCommand = CreateCommand())
                    {
                        CommandTimeout = dbCommand.CommandTimeout;
                        SqlHelper.TimeoutHasBeenDetermined = true;
                    }
                }
            }
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static DbConnection CreateConnection(string connectionString)
        {
            try
            {
                return new SqlConnection(connectionString);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 创建数据库SqlCommand对象.
        /// </summary>
        /// <returns></returns>
        private static DbCommand CreateCommand()
        {
            return new SqlCommand();
        }
        /// <summary>
        /// 创建数据库SqlCommand对象.
        /// </summary>
        /// <param name="command">the <see cref="T:System.Data.Common.DbCommand" />.</param>
        /// <returns></returns>
        private static DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            return new SqlDataAdapter(command as SqlCommand);
        }
        public enum SqlConnectionOwnership
        {
            Internal,
            External
        }
    }
}
