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
    public class Sys_FeatureDal : BaseDal<Sys_Feature>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_FeatureDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_Feature> DbSet
        {
            get
            {
                return activeContext.Sys_Feature;
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
        public  vSys_Feature GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_Feature>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Sys_Feature model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_Feature
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
        
        public ListByPages<vSys_Feature> QuickQuery(Sys_FeatureQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Feature
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.ControllerName.Contains(queryParam.KeyWords)
                          || temp.ActionName.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vSys_Feature
                        {
                             Id = temp.Id,
                             ApplicationModuleId = temp.ApplicationModuleId,
                             Code = temp.Code,
                             Name = temp.Name,
                             ControllerName = temp.ControllerName,
                             ActionName = temp.ActionName,
                             Url = temp.Url,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             Remark = temp.Remark,
                             IsMenu = temp.IsMenu,
                             IsShortCut = temp.IsShortCut,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_Feature> Query(Sys_FeatureQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Feature
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.ControllerName.Contains(queryParam.KeyWords)
                          || temp.ActionName.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vSys_Feature
                        {
                             Id = temp.Id,
                             ApplicationModuleId = temp.ApplicationModuleId,
                             Code = temp.Code,
                             Name = temp.Name,
                             ControllerName = temp.ControllerName,
                             ActionName = temp.ActionName,
                             Url = temp.Url,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             Remark = temp.Remark,
                             IsMenu = temp.IsMenu,
                             IsShortCut = temp.IsShortCut,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

