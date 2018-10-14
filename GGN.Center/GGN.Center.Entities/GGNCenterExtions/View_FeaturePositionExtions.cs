using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseEntity;
using System.Runtime.Serialization;

namespace GGN.Center.Entities
{
    /// <summary>
    /// 扩展属性
    /// </summary>
    [Serializable]
    public partial class View_FeaturePosition : IEntityBase
    {
    }
    public partial class vView_FeaturePosition : View_FeaturePosition
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class View_FeaturePositionQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
        public List<Guid> PositionIds { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class View_FeaturePositionQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
