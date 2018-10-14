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
    public partial class Auth_FeatureRoleDirectory : IEntityBase
    {
    }
    public partial class vAuth_FeatureRoleDirectory : Auth_FeatureRoleDirectory
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_FeatureRoleDirectoryQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_FeatureRoleDirectoryQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
