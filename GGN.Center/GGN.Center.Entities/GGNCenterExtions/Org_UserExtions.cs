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
    public partial class Org_User : IEntityBase
    {
    }
    public partial class vOrg_User : Org_User
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Org_UserQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
        public string q { get; set; }
        public Guid Id { get; set; }
        public string LoginName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IdCard { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Org_UserQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }

}
