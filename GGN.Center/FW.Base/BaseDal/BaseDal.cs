using FW.Base.BaseEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;

namespace FW.Base.BaseDal
{
    /// <summary>
    /// 基本的数据库操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseDal<T> where T : class, IEntityBase
    {
        #region 数据对象指定

        /// <summary>
        /// 实体数据库
        /// </summary>
        protected abstract DbContext ActiveContext { get; }

        /// <summary>
        /// 实体对象集合(查询)
        /// </summary>
        protected virtual DbSet<T> DbQuery
        {
            get { return DbSet; }
        }

        /// <summary>
        /// 实体对象集合(操作)
        /// </summary>
        protected abstract DbSet<T> DbSet { get; }

        #endregion

        #region 插入对象
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual OperateStatus Insert(T entity)
        {
            //操作状态信息
            var status = new OperateStatus();
            try
            {
                //指定状态
                ActiveContext.Entry(entity).State = EntityState.Added;
                //提交数据库
                int k = ActiveContext.SaveChanges();
                //成功
                status.IsSuccessful = true;
            }
            catch (DbEntityValidationException ex)
            {
                //错误信息
                status.Message = ex.Message;
                //获取详细验证错误
                status.MultipleMessage = GetDetailErrorString(ex);
            }
            return status;
        }
        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entitylist"></param>
        /// <returns></returns>
        public virtual OperateStatus InsertBatchs(IList<T> entitylist)
        {
            //操作状态信息
            var status = new OperateStatus();
            try
            {
                if (entitylist.Count < 1)
                {
                    status.Message = "无添加的数据";
                    return status;
                }
                foreach (var entity in entitylist)
                {
                    //指定状态
                    ActiveContext.Entry(entity).State = EntityState.Added;
                }

                //提交数据库
                int k = ActiveContext.SaveChanges();
                //成功
                status.IsSuccessful = true;
            }
            catch (DbEntityValidationException ex)
            {
                //错误信息
                status.Message = ex.Message;
                //获取详细验证错误
                status.MultipleMessage = GetDetailErrorString(ex);
            }
            return status;
        }
        #endregion

        #region 获取详细验证错误
        
        /// <summary>
        /// 获取详细验证错误
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDetailErrorString(DbEntityValidationException dbex)
        {
            try
            {
                //没有验证错误
                if (dbex.EntityValidationErrors.Count() < 1)
                {
                    return new Dictionary<string, string>();
                }
                //验证错误集合
                Dictionary<string, string> dics = new Dictionary<string, string>();

                foreach (var item in dbex.EntityValidationErrors)
                {
                    //错误信息
                    DbValidationError error = item.ValidationErrors.FirstOrDefault();
                    dics.Add(error.PropertyName, error.ErrorMessage);
                }
                return dics;
            }
            catch (Exception ex)
            {
                Dictionary<string, string> errorMsg = new Dictionary<string, string>();
                errorMsg.Add("获取错误信息出错", ex.Message);
                return errorMsg;
            }
        }
        #endregion

        #region 修改对象
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual OperateStatus Update(T entity)
        {
            //操作状态信息
            var status = new OperateStatus();
            try
            {
                var entry = ActiveContext.Entry(entity);
                T attachedEntity = DbSet.Find(entity.Id);
                if (attachedEntity != null)
                {
                    var attachedEntry = ActiveContext.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(entity);
                }
                else
                {
                    entry.State = EntityState.Modified;
                }
                ActiveContext.SaveChanges();

                //成功
                status.IsSuccessful = true;
            }
            catch (DbEntityValidationException ex)
            {
                //错误信息
                status.Message = ex.Message;
                //获取详细验证错误
                status.MultipleMessage = GetDetailErrorString(ex);
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
            var status = new OperateStatus();
            if (entity != null && fileds != null)
            {
                try
                {
                    //附加数据
                    DbSet.Attach(entity);
                    //绑定
                    var SetEntry = ((IObjectContextAdapter)ActiveContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);
                    foreach (var t in fileds)
                    {
                        //指定具体更新字段
                        SetEntry.SetModifiedProperty(t);
                    }
                    ActiveContext.SaveChanges();

                    //成功
                    status.IsSuccessful = true;
                }
                catch (DbEntityValidationException ex)
                {
                    //错误信息
                    status.Message = ex.Message;
                    status.Data = ex;
                }
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
            var status = new OperateStatus();
            if (entity != null && exceptFileds != null)
            {
                try
                {
                    //附加数据
                    DbSet.Attach(entity);
                    //绑定
                    var SetEntry = ((IObjectContextAdapter)ActiveContext).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);

                    //实体信息
                    Type t = entity.GetType();
                    PropertyInfo[] properties = t.GetProperties();
                    //变量实体字段
                    foreach (var filedName in properties)
                    {
                        //排除不更新字段
                        if (!exceptFileds.Contains(filedName.Name))
                        {
                            //指定具体更新字段
                            SetEntry.SetModifiedProperty(filedName.Name);
                        }
                    }

                    ActiveContext.SaveChanges();

                    //成功
                    status.IsSuccessful = true;
                }
                catch (Exception ex)
                {
                    //错误信息
                    status.Message = ex.Message;
                }
            }
            return status;
        }
        #endregion

        #region 删除对象
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        public virtual OperateStatus Delete(T entity)
        {
            //状态信息
            var status = new OperateStatus();
            try
            {
                //设置状态
                ActiveContext.Entry(entity).State = EntityState.Deleted;
                //提交数据库
                ActiveContext.SaveChanges();

                //成功
                status.IsSuccessful = true;
            }
            catch (Exception ex)
            {
                //错误信息
                status.Message = ex.Message;
            }
            return status;
        }

        public virtual OperateStatus Delete(Guid Id)
        {
            //状态信息
            var status = new OperateStatus();
            try
            {
                //读取批量删除数据集合
                var entity = DbQuery.FirstOrDefault(p => p.Id == Id);

                if (entity ==  null)
                {
                    status.Message = "删除的数据不存在";
                    return status;
                }
                ActiveContext.Entry(entity).State = EntityState.Deleted;

                //提交数据库
                ActiveContext.SaveChanges();

                //执行成功
                status.IsSuccessful = true;
                status.Message = "成功";
            }
            catch (Exception ex)
            {
                //错误信息
                status.Message = ex.Message;
            }
            return status;
        }


        #endregion

        #region 根据Id集合批量删除
        /// <summary>
        /// 根据Id集合批量删除
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public virtual OperateStatus DeleteBatchs(IList<Guid> ids)
        {
            //状态信息
            var status = new OperateStatus();
            try
            {
                //读取批量删除数据集合
                var entityList = DbQuery.Where(p => ids.Contains(p.Id));

                if (entityList.Count() < 1)
                {
                    status.Message = "删除的数据不存在";
                    return status;
                }

                foreach (var entity in entityList)
                {
                    //设置状态
                    ActiveContext.Entry(entity).State = EntityState.Deleted;
                }

                //提交数据库
                ActiveContext.SaveChanges();

                //执行成功
                status.IsSuccessful = true;
                status.Message = "成功";
            }
            catch (Exception ex)
            {
                //错误信息
                status.Message = ex.Message;
            }
            return status;
        }
        #endregion        

        #region 根据Id获取实体对象
        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public virtual T GetById(Guid Id)
        {
            return DbQuery.SingleOrDefault(p => p.Id == Id);
        }
        #endregion

        #region 获取所有对象
        /// <summary>
        /// 获取所有对象
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>实体对象清单</returns>
        public virtual IList<T> GetAll()
        {
            IList<T> result = DbQuery.ToList();
            return result;
        }
        #endregion

    }
}
