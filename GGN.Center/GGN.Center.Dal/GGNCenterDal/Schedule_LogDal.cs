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
    public class Schedule_LogDal : BaseDal<Schedule_Log>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Schedule_LogDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Schedule_Log> DbSet
        {
            get
            {
                return activeContext.Schedule_Log;
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
        public  vSchedule_Log GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSchedule_Log>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Schedule_Log model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Schedule_Log
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
        
        public ListByPages<vSchedule_Log> QuickQuery(Schedule_LogQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Log
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.JobCode.Contains(queryParam.KeyWords)
                          || temp.JobName.Contains(queryParam.KeyWords)
                          || temp.ResultMessage.Contains(queryParam.KeyWords))
                        select new vSchedule_Log
                        {
                             Id = temp.Id,
                             JobId = temp.JobId,
                             JobCode = temp.JobCode,
                             JobName = temp.JobName,
                             TriggerTime = temp.TriggerTime,
                             ExecuteStatus = temp.ExecuteStatus,
                             ResultMessage = temp.ResultMessage,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSchedule_Log> Query(Schedule_LogQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Log
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.JobCode.Contains(queryParam.KeyWords)
                          || temp.JobName.Contains(queryParam.KeyWords)
                          || temp.ResultMessage.Contains(queryParam.KeyWords))
                        select new vSchedule_Log
                        {
                             Id = temp.Id,
                             JobId = temp.JobId,
                             JobCode = temp.JobCode,
                             JobName = temp.JobName,
                             TriggerTime = temp.TriggerTime,
                             ExecuteStatus = temp.ExecuteStatus,
                             ResultMessage = temp.ResultMessage,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

