using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseEntity;
using System.Runtime.Serialization;

namespace GGN.Center.Entities
{
    [Serializable]
    public partial class View_DataRolePosition : IEntityBase
    {
    }
    public partial class vView_DataRolePosition : View_DataRolePosition
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class View_DataRolePositionQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
        public List<Guid> PositionIds { get; set; }
    }
    /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class View_DataRolePositionQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
