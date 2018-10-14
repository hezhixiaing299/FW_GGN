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
    public class Schedule_TypeDal : BaseDal<Schedule_Type>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Schedule_TypeDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Schedule_Type> DbSet
        {
            get
            {
                return activeContext.Schedule_Type;
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
        public  vSchedule_Type GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSchedule_Type>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Schedule_Type model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Schedule_Type
                            //where
                            //temp.TypeMark.Contains(model.TypeMark)
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
        
        public ListByPages<vSchedule_Type> QuickQuery(Schedule_TypeQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Type
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.TypeMark.Contains(queryParam.KeyWords)
                          || temp.TypeName.Contains(queryParam.KeyWords)
                          || temp.ParentMark.Contains(queryParam.KeyWords))
                        select new vSchedule_Type
                        {
                             Id = temp.Id,
                             TypeMark = temp.TypeMark,
                             TypeName = temp.TypeName,
                             ParentMark = temp.ParentMark,
                             Sort = temp.Sort,
                             Status = temp.Status,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSchedule_Type> Query(Schedule_TypeQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Type
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.TypeMark.Contains(queryParam.KeyWords)
                          || temp.TypeName.Contains(queryParam.KeyWords)
                          || temp.ParentMark.Contains(queryParam.KeyWords))
                        select new vSchedule_Type
                        {
                             Id = temp.Id,
                             TypeMark = temp.TypeMark,
                             TypeName = temp.TypeName,
                             ParentMark = temp.ParentMark,
                             Sort = temp.Sort,
                             Status = temp.Status,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

