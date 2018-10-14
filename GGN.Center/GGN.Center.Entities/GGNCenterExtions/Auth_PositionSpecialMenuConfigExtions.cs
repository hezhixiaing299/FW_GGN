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
    public partial class Auth_PositionSpecialMenuConfig : IEntityBase
    {
    }
    public partial class vAuth_PositionSpecialMenuConfig : Auth_PositionSpecialMenuConfig
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Auth_PositionSpecialMenuConfigQueryParam : BaseSearchParam
    {
        public List<Guid> PositionIds { get; set; }
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Auth_PositionSpecialMenuConfigQuickQueryParam : BaseSearchParam
    {
        public Guid MenuId { get; set; }
        public Guid PositionId { get; set; }
        public string KeyWords { get; set; }
    }
}
