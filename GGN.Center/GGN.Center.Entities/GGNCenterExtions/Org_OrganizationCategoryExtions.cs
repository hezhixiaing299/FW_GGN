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
    public partial class Org_OrganizationCategory : IEntityBase
    {
    }
    public partial class vOrg_OrganizationCategory : Org_OrganizationCategory
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Org_OrganizationCategoryQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Org_OrganizationCategoryQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
