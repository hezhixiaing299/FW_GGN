using System;

namespace FW.Base.BaseEntity
{
    /// <summary>
    /// 定义实体的规则
    /// </summary>
    public interface IEntityBase
    {
        /// <summary>
        /// 实体的唯一标识
        /// </summary>
        Guid Id { get; set; }
    }


    public interface IRepository<T> where T : class, new()
    {
        OperateStatus Insert(T entityBase);
    }
}
