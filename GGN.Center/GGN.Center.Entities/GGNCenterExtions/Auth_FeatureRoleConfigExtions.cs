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
    public partial class Auth_FeatureRoleConfig : IEntityBase
    {
    }
    public partial class vAuth_FeatureRoleConfig : Auth_FeatureRoleConfig
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_FeatureRoleConfigQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_FeatureRoleConfigQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
