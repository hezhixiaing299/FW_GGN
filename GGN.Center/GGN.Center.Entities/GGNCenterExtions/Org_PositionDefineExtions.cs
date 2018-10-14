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
    public partial class Org_PositionDefine : IEntityBase
    {
    }
    public partial class vOrg_PositionDefine : Org_PositionDefine
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Org_PositionDefineQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Org_PositionDefineQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
