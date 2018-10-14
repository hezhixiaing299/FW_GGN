using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseEntity;

namespace GGN.Center.Entities
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    public partial class Org_Organization : IEntityBase
    {
    }
    public partial class vOrg_Organization : Org_Organization
    {
        [DataMember]
        public Guid OrganizationRelationshipId { get; set; }

        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public string OrganizationCategoryName { get; set; }
        [DataMember]
        public string OrganizationCategoryCode { get; set; }
        [DataMember]
        public int OrganizationCategorySort { get; set; }
        [DataMember]
        public Guid PositionId { get; set; }
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Org_OrganizationQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Org_OrganizationQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
