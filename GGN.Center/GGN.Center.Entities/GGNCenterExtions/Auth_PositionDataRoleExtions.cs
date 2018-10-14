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
    public partial class Auth_PositionDataRole : IEntityBase
    {
    }
    public partial class vAuth_PositionDataRole : Auth_PositionDataRole
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_PositionDataRoleQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_PositionDataRoleQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
