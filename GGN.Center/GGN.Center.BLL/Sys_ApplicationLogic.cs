using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseBll;
using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using GGN.Center.Entities;
using GGN.Center.Dal;

namespace GGN.Center.BLL
{
    public class Sys_ApplicationLogic : BaseLogic<Sys_Application>
    {
        #region 必备

        //数据操作对象
        Sys_ApplicationDal Dal = new Sys_ApplicationDal();

        private readonly BaseDal<Sys_Application> repository;

        public Sys_ApplicationLogic(Sys_ApplicationDal repository)
            : base(repository)
        {
            this.repository = repository;
        }

        public Sys_ApplicationLogic()
            : base(new Sys_ApplicationDal() { })
        {
        }

        #endregion

        #region 数据操作

        /// <summary>
        /// BLL层重写Create方法(示例)
        /// 默认可以调用base的Create方法
        /// </summary>
        /// <param name="entity">提交数据实体</param>
        /// <returns>返回消息对象</returns>
        public override OperateStatus Create(Sys_Application entity)
        {
            var check = Dal.CheckValidate(entity);
            if (!check.IsSuccessful)
            {
                return check;
            }
            //实现逻辑
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
            //创建
            return base.Create(entity);
        }

        /// <summary>
        /// BLL层重写Update方法(示例)
        /// 默认可以调用base的Update方法
        /// </summary>
        /// <param name="entity">提交数据实体</param>
        /// <returns>返回消息对象</returns>
        public override OperateStatus Update(Sys_Application entity)
        {
            var check = Dal.CheckValidate(entity);
            if (!check.IsSuccessful)
            {
                return check;
            }
            return base.Update(entity);
        }

        /// <summary>
        /// BLL层重写Delete方法(示例)
        /// 默认可以调用base的Delete方法
        /// 单个删除
        /// </summary>
        /// <param name="entity">提交数据实体</param>
        /// <returns>返回消息对象</returns>
        public override OperateStatus Delete(Guid id)
        {
            return Dal.Delete(id);
        }

        /// <summary>
        /// BLL层重写DeleteBatchs方法(示例)
        /// 默认可以调用base的DeleteBatchs方法
        /// 批量删除
        /// </summary>
        /// <param name="entity">提交数据实体</param>
        /// <returns>返回消息对象</returns>
        public override OperateStatus DeleteBatchs(IList<Guid> ids)
        {
            return base.DeleteBatchs(ids);
        }

        #endregion

        #region 自定义方法
        
         /// <summary>
         /// 根据Id获取扩展对象
         /// </summary>
         /// <param name="Id"></param>
         /// <returns></returns>
        public vSys_Application GetById(Guid Id)
        {
            return Dal.GetById(Id);
        }

        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public IList<Sys_Application> GetAll()
        {
            return Dal.GetAll();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_Application> Query(Sys_ApplicationQueryParam queryParam)
        {
            return Dal.Query(queryParam);
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_Application> QuickQuery(Sys_ApplicationQuickQueryParam queryParam)
        {
            return Dal.QuickQuery(queryParam);
        }

        #endregion
    }
}
