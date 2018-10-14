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
    public partial class Auth_UserDataRole : IEntityBase
    {
    }
    public partial class vAuth_UserDataRole : Auth_UserDataRole
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_UserDataRoleQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_UserDataRoleQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
