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
    public partial class Auth_PositionFeatureRole : IEntityBase
    {
    }
    public partial class vAuth_PositionFeatureRole : Auth_PositionFeatureRole
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_PositionFeatureRoleQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_PositionFeatureRoleQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
