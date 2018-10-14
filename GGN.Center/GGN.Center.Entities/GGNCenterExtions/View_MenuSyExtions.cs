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
    public partial class View_MenuSys : IEntityBase
    {
    }
    public partial class vView_MenuSys : View_MenuSys
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class View_MenuSysQueryParam : BaseSearchParam
    {
        public List<Guid> Ids { get; set; }
        public string KeyWords { get; set; }
        public List<Guid> FeatureIds { get; set; }
        public Guid ApplicationId { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class View_MenuSysQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
