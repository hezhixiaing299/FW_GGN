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
    public class Sys_RegionDal : BaseDal<Sys_Region>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_RegionDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_Region> DbSet
        {
            get
            {
                return activeContext.Sys_Region;
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
        public  vSys_Region GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_Region>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Sys_Region model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_Region
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
        
        public ListByPages<vSys_Region> QuickQuery(Sys_RegionQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Region
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.RegionName.Contains(queryParam.KeyWords)
                          || temp.RegionCode.Contains(queryParam.KeyWords)
                          || temp.PinYin.Contains(queryParam.KeyWords)
                          || temp.ShortPinYin.Contains(queryParam.KeyWords)
                          || temp.FullPath.Contains(queryParam.KeyWords))
                        select new vSys_Region
                        {
                             Id = temp.Id,
                             RegionName = temp.RegionName,
                             RegionCode = temp.RegionCode,
                             PinYin = temp.PinYin,
                             ShortPinYin = temp.ShortPinYin,
                             FullPath = temp.FullPath,
                             HaveChild = temp.HaveChild,
                             Level = temp.Level,
                             Sort = temp.Sort,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_Region> Query(Sys_RegionQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_Region
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.RegionName.Contains(queryParam.KeyWords)
                          || temp.RegionCode.Contains(queryParam.KeyWords)
                          || temp.PinYin.Contains(queryParam.KeyWords)
                          || temp.ShortPinYin.Contains(queryParam.KeyWords)
                          || temp.FullPath.Contains(queryParam.KeyWords))
                        select new vSys_Region
                        {
                             Id = temp.Id,
                             RegionName = temp.RegionName,
                             RegionCode = temp.RegionCode,
                             PinYin = temp.PinYin,
                             ShortPinYin = temp.ShortPinYin,
                             FullPath = temp.FullPath,
                             HaveChild = temp.HaveChild,
                             Level = temp.Level,
                             Sort = temp.Sort,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

