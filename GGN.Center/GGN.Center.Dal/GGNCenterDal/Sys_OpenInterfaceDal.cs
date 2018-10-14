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
    public class Sys_OpenInterfaceDal : BaseDal<Sys_OpenInterface>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_OpenInterfaceDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_OpenInterface> DbSet
        {
            get
            {
                return activeContext.Sys_OpenInterface;
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
        public  vSys_OpenInterface GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_OpenInterface>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Sys_OpenInterface model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_OpenInterface
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
        #endregion

        #region 方法
        
        public ListByPages<vSys_OpenInterface> QuickQuery(Sys_OpenInterfaceQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_OpenInterface
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.HttpType.Contains(queryParam.KeyWords)
                          || temp.Assembly.Contains(queryParam.KeyWords)
                          || temp.Params.Contains(queryParam.KeyWords)
                          || temp.Class.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vSys_OpenInterface
                        {
                             Id = temp.Id,
                             ApplicationId = temp.ApplicationId,
                             OpenInterfaceTypeId = temp.OpenInterfaceTypeId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Url = temp.Url,
                             HttpType = temp.HttpType,
                             Assembly = temp.Assembly,
                             Params = temp.Params,
                             Class = temp.Class,
                             StartTime = temp.StartTime,
                             EndTime = temp.EndTime,
                             Remark = temp.Remark,
                             Sort = temp.Sort,
                             IsFreeze = temp.IsFreeze,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_OpenInterface> Query(Sys_OpenInterfaceQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_OpenInterface
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.HttpType.Contains(queryParam.KeyWords)
                          || temp.Assembly.Contains(queryParam.KeyWords)
                          || temp.Params.Contains(queryParam.KeyWords)
                          || temp.Class.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vSys_OpenInterface
                        {
                             Id = temp.Id,
                             ApplicationId = temp.ApplicationId,
                             OpenInterfaceTypeId = temp.OpenInterfaceTypeId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Url = temp.Url,
                             HttpType = temp.HttpType,
                             Assembly = temp.Assembly,
                             Params = temp.Params,
                             Class = temp.Class,
                             StartTime = temp.StartTime,
                             EndTime = temp.EndTime,
                             Remark = temp.Remark,
                             Sort = temp.Sort,
                             IsFreeze = temp.IsFreeze,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

