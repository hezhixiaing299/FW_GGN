//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------
using System.Runtime.Serialization;
using System.ComponentModel;
using System;
using System.Collections.Generic;


namespace GGN.Center.Entities
{
    /// <summary>
    /// 
    /// </summary> 
    [DataContract(IsReference = true)]
    [Description("")]
    public partial class Schedule_Task
    {
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public System.Guid Id { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string GroupMark { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string GroupName { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string ScheduleTypeMark { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string ScheduleTypeName { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string JobCode { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string JobName { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string JobRemark { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string JobExpress { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public Nullable<System.DateTime> JobBeginTime { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public Nullable<System.DateTime> JobEndTime { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public int Sort { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public short Status { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string AppUrl { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string RequestMode { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string RequestContentType { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string RequestParam { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public System.Guid LastUserId { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public string LastUserName { get; set; }
    
    	[DataMember]
    	[Description("")]
        /// <summary>
    	/// 
    	/// </summary>
    	public System.DateTime LastTime { get; set; }
    
    }
}