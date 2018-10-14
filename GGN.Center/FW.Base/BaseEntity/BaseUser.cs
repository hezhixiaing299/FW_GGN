using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FW.Base.BaseEntity
{
    public class BaseUser
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PassWord { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        public string EquipmentMark { get; set; }
        public string SessionId { get; set; }
        public string TokenKey { get; set; }
        public string TokenValue { get; set; }
    }
}
