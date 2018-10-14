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
    public partial class Auth_DataRoleDirectory : IEntityBase
    {
    }
    public partial class vAuth_DataRoleDirectory : Auth_DataRoleDirectory
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_DataRoleDirectoryQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_DataRoleDirectoryQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
