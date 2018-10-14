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
    public class Schedule_TaskDal : BaseDal<Schedule_Task>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Schedule_TaskDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Schedule_Task> DbSet
        {
            get
            {
                return activeContext.Schedule_Task;
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
        public  vSchedule_Task GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSchedule_Task>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Schedule_Task model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Schedule_Task
                            //where
                            //temp.GroupMark.Contains(model.GroupMark)
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
        
        public ListByPages<vSchedule_Task> QuickQuery(Schedule_TaskQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Task
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.GroupMark.Contains(queryParam.KeyWords)
                          || temp.GroupName.Contains(queryParam.KeyWords)
                          || temp.ScheduleTypeMark.Contains(queryParam.KeyWords)
                          || temp.ScheduleTypeName.Contains(queryParam.KeyWords)
                          || temp.JobCode.Contains(queryParam.KeyWords)
                          || temp.JobName.Contains(queryParam.KeyWords)
                          || temp.JobRemark.Contains(queryParam.KeyWords)
                          || temp.JobExpress.Contains(queryParam.KeyWords)
                          || temp.AppUrl.Contains(queryParam.KeyWords)
                          || temp.RequestMode.Contains(queryParam.KeyWords)
                          || temp.RequestContentType.Contains(queryParam.KeyWords)
                          || temp.RequestParam.Contains(queryParam.KeyWords)
                          || temp.LastUserName.Contains(queryParam.KeyWords))
                        select new vSchedule_Task
                        {
                             Id = temp.Id,
                             GroupMark = temp.GroupMark,
                             GroupName = temp.GroupName,
                             ScheduleTypeMark = temp.ScheduleTypeMark,
                             ScheduleTypeName = temp.ScheduleTypeName,
                             JobCode = temp.JobCode,
                             JobName = temp.JobName,
                             JobRemark = temp.JobRemark,
                             JobExpress = temp.JobExpress,
                             JobBeginTime = temp.JobBeginTime,
                             JobEndTime = temp.JobEndTime,
                             Sort = temp.Sort,
                             Status = temp.Status,
                             AppUrl = temp.AppUrl,
                             RequestMode = temp.RequestMode,
                             RequestContentType = temp.RequestContentType,
                             RequestParam = temp.RequestParam,
                             LastUserId = temp.LastUserId,
                             LastUserName = temp.LastUserName,
                             LastTime = temp.LastTime,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSchedule_Task> Query(Schedule_TaskQueryParam queryParam)
        {
            var query = from temp in activeContext.Schedule_Task
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.GroupMark.Contains(queryParam.KeyWords)
                          || temp.GroupName.Contains(queryParam.KeyWords)
                          || temp.ScheduleTypeMark.Contains(queryParam.KeyWords)
                          || temp.ScheduleTypeName.Contains(queryParam.KeyWords)
                          || temp.JobCode.Contains(queryParam.KeyWords)
                          || temp.JobName.Contains(queryParam.KeyWords)
                          || temp.JobRemark.Contains(queryParam.KeyWords)
                          || temp.JobExpress.Contains(queryParam.KeyWords)
                          || temp.AppUrl.Contains(queryParam.KeyWords)
                          || temp.RequestMode.Contains(queryParam.KeyWords)
                          || temp.RequestContentType.Contains(queryParam.KeyWords)
                          || temp.RequestParam.Contains(queryParam.KeyWords)
                          || temp.LastUserName.Contains(queryParam.KeyWords))
                        select new vSchedule_Task
                        {
                             Id = temp.Id,
                             GroupMark = temp.GroupMark,
                             GroupName = temp.GroupName,
                             ScheduleTypeMark = temp.ScheduleTypeMark,
                             ScheduleTypeName = temp.ScheduleTypeName,
                             JobCode = temp.JobCode,
                             JobName = temp.JobName,
                             JobRemark = temp.JobRemark,
                             JobExpress = temp.JobExpress,
                             JobBeginTime = temp.JobBeginTime,
                             JobEndTime = temp.JobEndTime,
                             Sort = temp.Sort,
                             Status = temp.Status,
                             AppUrl = temp.AppUrl,
                             RequestMode = temp.RequestMode,
                             RequestContentType = temp.RequestContentType,
                             RequestParam = temp.RequestParam,
                             LastUserId = temp.LastUserId,
                             LastUserName = temp.LastUserName,
                             LastTime = temp.LastTime,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

