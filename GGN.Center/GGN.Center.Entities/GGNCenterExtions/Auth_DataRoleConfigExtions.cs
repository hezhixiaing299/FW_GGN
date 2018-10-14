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
    public partial class Auth_DataRoleConfig : IEntityBase
    {
    }
    public partial class vAuth_DataRoleConfig : Auth_DataRoleConfig
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_DataRoleConfigQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_DataRoleConfigQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
