using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace FW.Base.BaseEntity
{
    /// <summary>
    /// 分页查询接口
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// 起始页码
        /// </summary>
        int page { get; set; }

        /// <summary>
        /// 每页记录数，默认10
        /// </summary>
        int limit { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        int total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int totalpagecount { get; set; }
    }
    public class sort
    {
        public string property { get; set; }
        public string direction { get; set; }
    }

    /// <summary>
    /// 查询基本参数
    /// 针对前端的,每个前端的参数名称都不同,所以在前端决定之后,这里才能定
    /// 但是,如果能做到和前端参数的转换,或者映射，那这里就可以看心情命名了
    /// </summary>
    [Serializable]
    public class BaseSearchParam : IPagination
    {
        /// <summary>
        /// 需要进行合计的字段，多个值用英文逗号分隔“,”，如传入参数非数值类型，则不予合计
        /// </summary>
        [DataMember]
        public string sumfield { get; set; } 

        [DataMember]
        public string sort { get; set; }  
      
        /// <summary>
        /// 当前页序号
        /// </summary>
        [DataMember]
        public int page { get; set; }

        /// <summary>
        /// 页大小，默认10
        /// </summary>
        [DataMember]
        public int limit { get; set; }

        /// <summary>
        /// 返回的时候需要用到的总记录数
        /// </summary>
        [DataMember]
        public int totalRowsCount { get; set; }

        /// <summary>
        /// 是否为导出
        /// </summary>
        [DataMember]
        public bool IsExport { get; set; }

        /// <summary>
        /// 排序字段及排序方式组合字符串
        /// </summary>
        [DataMember]
        public string orderString
        {
            get
            {
                if (string.IsNullOrEmpty(sort))
                {
                    //throw new InvalidOperationException("未指定排序字段");
                    return "id asc";//默认id
                }
                else
                {
                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    var sortList = Serializer.Deserialize<List<sort>>(sort);
                    var returnString = "";
                    foreach (var model in sortList)
                    {
                        returnString += string.Format("{0} {1},", model.property, model.direction);
                    }
                    if (returnString.Length > 0)
                    {
                        returnString = returnString.Substring(0, returnString.Length - 1);
                    }
                    return returnString;
                }
            }
        }
        [DataMember]
        public bool IsAllPage { get; set; }
        /// <summary>
        /// 获取起始记录数(跳过的数据条数)
        /// <para>从0开始</para>
        /// </summary>
        [DataMember]
        public int startIndex
        {
            get
            {
                return (page - 1) * limit;
            }
        }
        /// <summary>
        /// 总记录数
        /// </summary>
        [DataMember]
        public int total { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        [DataMember]
        public int totalpagecount { get; set; }
    }

    /// <summary>
    /// 通用代码|名称查询参数
    /// </summary>
    public class CodeNameCommonQueryParam : BaseSearchParam
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }


    /// <summary>
    /// 通用单一分页查询参数
    /// </summary>
    public class ValueCommonQueryParam : BaseSearchParam
    {
        /// <summary>
        /// 查询值
        /// </summary>
        public string Value { get; set; }
    }


    /// <summary>
    /// 单一不分页查询参数
    /// </summary>
    public class ValueQueryParam
    {
        /// <summary>
        /// 查询值
        /// </summary>
        public string Value { get; set; }
    }
}
