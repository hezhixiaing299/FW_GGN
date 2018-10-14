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
    public partial class Auth_UserFeatureRole : IEntityBase
    {
    }
    public partial class vAuth_UserFeatureRole : Auth_UserFeatureRole
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_UserFeatureRoleQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_UserFeatureRoleQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
