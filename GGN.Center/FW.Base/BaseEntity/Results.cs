using System.Collections.Generic;

namespace FW.Base.BaseEntity
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListByPages<T> where T : new()
    {
        public T sumdata { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public IList<T> rows { get; set; }

        /// <summary>
        /// 获取记录的总条数
        /// </summary>
        public int total { get; set; }

        /// <summary>
        /// 获取记录的总页数
        /// </summary>
        public int totalpagecount { get; set; }

        /// <summary>
        /// 当前页序号
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 页大小，默认10
        /// </summary>
        public int limit { get; set; }

        /// <summary>
        /// 获取起始记录数(跳过的数据条数)
        /// <para>从0开始</para>
        /// </summary>
        public int startIndex
        {
            get
            {
                return (page - 1) * limit;
            }
        }

        public object AttachData { get; set; }
        public object Extend { get; set; }
    }

    /// <summary>
    /// 操作状态
    /// </summary>
    public class OperateStatus
    {
        public OperateStatus()
        {
            MultipleData = new Dictionary<string, object>();
            MultipleMessage = new Dictionary<string, string>();
        }
        /// <summary>
        /// 返回数据（key,Value）
        /// </summary>
        public Dictionary<string, object> MultipleData { get; set; }
        /// <summary>
        /// 返货多行消息
        /// </summary>
        public Dictionary<string, string> MultipleMessage { get; set; }

        // 摘要: 
        //     消息的参数
        public object Data { get; set; }
        //
        // 摘要: 
        //     获取一个值，表示返回标记是否成功
        public bool IsSuccessful { get; set; }
        //
        // 摘要: 
        //     消息字符串
        public string Message { get; set; }

        /// <summary>
        /// 返回编码
        /// </summary>
        public string Code { get; set; }
    }
}
