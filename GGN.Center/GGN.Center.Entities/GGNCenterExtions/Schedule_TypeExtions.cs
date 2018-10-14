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
    public partial class Schedule_Type : IEntityBase
    {
    }
    public partial class vSchedule_Type : Schedule_Type
    {
    }
    /// <summary>
    /// 扩展查询参数
    /// </summary>
    public class Schedule_TypeQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
       /// <summary>
    /// 扩展快速查询参数
    /// </summary>
    public class Schedule_TypeQuickQueryParam : BaseSearchParam
    {
        public string KeyWords { get; set; }
    }
}
