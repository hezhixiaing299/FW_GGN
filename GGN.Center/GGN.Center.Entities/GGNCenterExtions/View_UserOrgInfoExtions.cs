using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseEntity;
using System.Runtime.Serialization;

namespace GGN.Center.Entities
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    [Serializable]
    public partial class View_UserOrgInfo : IEntityBase
    {
    }
    public partial class vView_UserOrgInfo : View_UserOrgInfo
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class View_UserOrgInfoQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class View_UserOrgInfoQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
