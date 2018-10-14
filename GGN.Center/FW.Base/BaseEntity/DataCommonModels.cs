using System;
using System.Collections.Generic;

namespace FW.Base.BaseEntity
{

    /// <summary>
    /// 文件上传基础数据模型
    /// </summary>
    public partial class BaseFileUploadModel
    {
        /// <summary>
        /// 获取一个值，表示返回标记是否成功
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// 消息字符串
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 上传Key值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 原图保存路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 缩略图路径
        /// </summary>
        public string SmallImgPath { get; set; }
        /// <summary>
        /// 原图HTTP路径
        /// </summary>
        public string UrlPath { get; set; }
        /// <summary>
        /// 缩略图HTTP路径
        /// </summary>
        public string SmallUrlPath { get; set; }
        /// <summary>
        /// 附件记录表id
        /// </summary>
        public Guid FileAttachId { get; set; }
    }

    /// <summary>
    /// 数据权限统一树结构模型(Tree)
    /// </summary>
    public class BaseDataPowerCommonTreeModels
    {
        public Guid id { get; set; }
        public string text { get; set; }

        public IList<BaseDataPowerCommonTreeModels> children = new List<BaseDataPowerCommonTreeModels>();
    }
}
