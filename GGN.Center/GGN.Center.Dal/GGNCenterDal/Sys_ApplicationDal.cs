using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using FW.Tool;
using GGN.Center.Entities;

namespace GGN.Center.Dal
{
    public class Sys_ApplicationDal : BaseDal<Sys_Application>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_ApplicationDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_Application> DbSet
        {
            get
            {
                return activeContext.Sys_Application;
            }
        }
        /// <summary>
        /// 实体对象上下文
        /// </summary>
        protected override DbContext ActiveContext
        {
            get
            {
                return activeContext;
            }
        }
        #endregion

        #region 根据Id获取扩展对象
        /// <summary>
        /// 根据Id获取扩展对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public vSys_Application GetById(Guid Id)
        {
            var model = base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_Application>(model);
            return returnModel;
        }
        #endregion

        #region 检查mark是否重复

        public OperateStatus CheckMark(Sys_Application model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_Application
                                //where
                                //temp..Contains(model.)
                            select temp;

                var Count = query.Count();
                if (model.Id == Guid.Empty || model.Id == null)
                {
                    if (Count == 0)
                    {
                        op.IsSuccessful = true;
                    }
                    else
                    {
                        op.IsSuccessful = false;
                        op.Message = "标示不能重复";
                    }
                }
                else
                {
                    Count = query.Where(f => f.Id != model.Id).Count();
                    if (Count == 0)
                    {
                        op.IsSuccessful = true;
                    }
                    else
                    {
                        op.IsSuccessful = false;
                        op.Message = "标示不能重复";
                    }
                }
            }
            catch (Exception ex)
            {
                op.IsSuccessful = false;
                op.Message = ex.Message;
            }
            return op;
        }

        /// <summary>
        /// 检查是否合法(code是否已存在)
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True:合法;False:不合法</returns>
        public OperateStatus CheckValidate(Sys_Application entity)
        {
            OperateStatus result = new OperateStatus { Message = "编码Code已经存在!", IsSuccessful = false };
            var templist = activeContext.Sys_Application.Where(p => p.Code == entity.Code).FirstOrDefault();
            //如果没有记录,通过
            if (templist == null)
            {
                result.Message = "检查通过!";
                result.IsSuccessful = true;
                return result;
            }
            //如果有记录,Id相同,说明是编辑,通过
            if (entity.Id == templist.Id)
            {
                result.Message = "检查通过!";
                result.IsSuccessful = true;
                return result;
            }

            return result;
        }

        #endregion

        #region 方法

        public ListByPages<vSys_Application> QuickQuery(Sys_ApplicationQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Application
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)
                          || temp.Domain.Contains(queryParam.KeyWords)
                          || temp.VisitUrl.Contains(queryParam.KeyWords)
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Icon.Contains(queryParam.KeyWords))
                        select new vSys_Application
                        {
                            Id = temp.Id,
                            Domain = temp.Domain,
                            VisitUrl = temp.VisitUrl,
                            Code = temp.Code,
                            Name = temp.Name,
                            IsFreeze = temp.IsFreeze,
                            Sort = temp.Sort,
                            Icon = temp.Icon,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_Application> Query(Sys_ApplicationQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Application
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)
                          || temp.Domain.Contains(queryParam.KeyWords)
                          || temp.VisitUrl.Contains(queryParam.KeyWords)
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Icon.Contains(queryParam.KeyWords))
                        select new vSys_Application
                        {
                            Id = temp.Id,
                            Domain = temp.Domain,
                            VisitUrl = temp.VisitUrl,
                            Code = temp.Code,
                            Name = temp.Name,
                            IsFreeze = temp.IsFreeze,
                            Sort = temp.Sort,
                            Icon = temp.Icon,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

