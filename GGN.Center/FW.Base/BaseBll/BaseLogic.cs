using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FW.Base.BaseBll
{
    /// <summary>
    /// 实体业务逻辑基类
    /// </summary>
    /// <typeparam name="T">业务实体类</typeparam>
    public abstract class BaseLogic<T> where T : class, IEntityBase
    {
        #region 数据对象指定
        private readonly BaseDal<T> repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository">注入数据访问接口</param>
        protected BaseLogic(BaseDal<T> repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("BaseDal", "BaseDal cannot be null");
            }
            this.repository = repository;
        }

        #endregion

        #region 插入对象
        /// <summary>
        /// 创建实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        public virtual OperateStatus Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            //插入数据
            var status = repository.Insert(entity);
            //操作结果
            if (status.IsSuccessful)
            {
                //指定成功默认返回结果值
                status.Message = "成功";
            }
            return status;
        }
        #endregion

        #region 批量创建实体对象
        /// <summary>
        /// 批量创建实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        public virtual OperateStatus InsertBatchs(IList<T> entitylist)
        {
            if (entitylist == null || entitylist.Count <= 0)
            {
                throw new ArgumentNullException("entity");
            }
            //插入数据
            var status = repository.InsertBatchs(entitylist);
            //操作结果
            if (status.IsSuccessful)
            {
                //指定成功默认返回结果值
                status.Message = "成功";
            }
            return status;
        }

        #endregion

        #region 修改对象
        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">操作状态</param>
        public virtual OperateStatus Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentException("实体的Id为空");
            }
            //操作状态信息
            var status = new OperateStatus();
            var original = GetById(entity.Id);
            if (original == null)
            {
                status = new OperateStatus
                {
                    Message = "更新的数据不存在"
                };
            }
            else
            {
                //执行更新
                status = repository.Update(entity);
                if (status.IsSuccessful)
                {
                    //指定成功默认返回结果值
                    status.Message = "成功";
                    status.Data = entity;
                }
            }
            return status;
        }
        #endregion

        #region 指定字段更新
        /// <summary>
        /// 指定字段更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fileds">指定字段集合</param>
        public virtual OperateStatus UpdateByEntityFields(T entity, List<string> fileds)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //更新
            var status = repository.UpdateByEntityFields(entity, fileds);

            if (status.IsSuccessful)
            {
                //指定成功默认返回结果值
                status.Message = "成功";
            }
            return status;
        }
        #endregion

        #region 更新除指定字段外的数据
        /// <summary>
        /// 更新除指定字段外的数据
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="fileds">指定字段集合</param>
        public virtual OperateStatus UpdateByEntityExceptFields(T entity, List<string> exceptFileds)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            //更新
            var status = repository.UpdateByEntityExceptFields(entity, exceptFileds);

            if (status.IsSuccessful)
            {
                //指定成功默认返回结果值
                status.Message = "成功";
            }
            return status;
        }
        #endregion

        #region 删除对象
        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="id">实体对象Id</param>
        /// <param name="status">操作状态</param>
        public virtual OperateStatus Delete(Guid id)
        {
            //操作状态信息
            var status = new OperateStatus { IsSuccessful = false };
            if (id == Guid.Empty)
            {
                status.Message = "该数据不允许删除或不存在!";
                return status;
            }
            T entity = GetById(id);
            if (entity == null)
            {
                status.Message = "删除的数据不存在";
            }
            else
            {
                status = repository.Delete(entity);
                if (status.IsSuccessful)
                {
                    //指定成功默认返回结果值
                    status.Message = "成功";
                }
            }
            return status;
        }
        #endregion

        #region 根据Id集合批量删除
        /// <summary>
        /// 根据Id集合批量删除
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="entity">指定实体(表名)</param>
        /// <returns></returns>
        public virtual OperateStatus DeleteBatchs(IList<Guid> ids)
        {
            //状态信息
            var status = new OperateStatus();
            if (ids.Count < 1 || ids == null)
            {
                status.Message = "无删除数据";
            }

            //批量删除
            status = repository.DeleteBatchs(ids);

            if (status.IsSuccessful)
            {
                //指定成功默认返回结果值
                status.Message = "成功";
            }
            return status;
        }
        #endregion

        #region 根据Id获取实体对象
        /// <summary>
        /// 通过ID获取实体对象
        /// </summary>
        /// <param name="id">实体对象Id</param>
        /// <returns>实体对象</returns>
        protected virtual T GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("id为空");
            }
            return repository.GetById(id);
        }
        #endregion

        #region 获取所有对象

        /// <summary>
        /// 获取所有实体对象
        /// </summary>
        /// <returns>实体对象清单</returns>
        protected IList<T> GetAll()
        {
            return repository.GetAll();
        }

        #endregion
    }
}