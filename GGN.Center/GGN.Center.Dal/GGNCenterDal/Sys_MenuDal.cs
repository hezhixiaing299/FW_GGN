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
    public class Sys_MenuDal : BaseDal<Sys_Menu>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_MenuDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_Menu> DbSet
        {
            get
            {
                return activeContext.Sys_Menu;
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
        public  vSys_Menu GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_Menu>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Sys_Menu model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_Menu
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
        
        public ListByPages<vSys_Menu> QuickQuery(Sys_MenuQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Menu
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords)
                          || temp.Icon.Contains(queryParam.KeyWords))
                        select new vSys_Menu
                        {
                             Id = temp.Id,
                             ApplicationModuleId = temp.ApplicationModuleId,
                             FeatureId = temp.FeatureId,
                             ParentId = temp.ParentId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             Url = temp.Url,
                             Remark = temp.Remark,
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
        public ListByPages<vSys_Menu> Query(Sys_MenuQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Menu
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Url.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords)
                          || temp.Icon.Contains(queryParam.KeyWords))
                        select new vSys_Menu
                        {
                             Id = temp.Id,
                             ApplicationModuleId = temp.ApplicationModuleId,
                             FeatureId = temp.FeatureId,
                             ParentId = temp.ParentId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Level = temp.Level,
                             Sort = temp.Sort,
                             Url = temp.Url,
                             Remark = temp.Remark,
                             Icon = temp.Icon,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

