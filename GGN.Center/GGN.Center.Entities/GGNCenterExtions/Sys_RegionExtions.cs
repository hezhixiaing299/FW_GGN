using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseEntity;

namespace GGN.Center.Entities
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    public partial class Sys_Region : IEntityBase
    {
    }
    public partial class vSys_Region : Sys_Region
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Sys_RegionQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Sys_RegionQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
