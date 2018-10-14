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
    public class Sys_UserLoginStateDal : BaseDal<Sys_UserLoginState>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Sys_UserLoginStateDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Sys_UserLoginState> DbSet
        {
            get
            {
                return activeContext.Sys_UserLoginState;
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
        public  vSys_UserLoginState GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vSys_UserLoginState>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Sys_UserLoginState model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Sys_UserLoginState
                            //where
                            //temp.EqpMark.Contains(model.EqpMark)
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
        
        public ListByPages<vSys_UserLoginState> QuickQuery(Sys_UserLoginStateQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_UserLoginState
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.LoginName.Contains(queryParam.KeyWords)
                          || temp.UserCode.Contains(queryParam.KeyWords)
                          || temp.UserName.Contains(queryParam.KeyWords)
                          || temp.EqpMark.Contains(queryParam.KeyWords)
                          || temp.EqpName.Contains(queryParam.KeyWords))
                        select new vSys_UserLoginState
                        {
                             Id = temp.Id,
                             SessionId = temp.SessionId,
                             UserId = temp.UserId,
                             LoginName = temp.LoginName,
                             UserCode = temp.UserCode,
                             UserName = temp.UserName,
                             EqpMark = temp.EqpMark,
                             EqpName = temp.EqpName,
                             LastTime = temp.LastTime,
                             PeriodTime = temp.PeriodTime,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vSys_UserLoginState> Query(Sys_UserLoginStateQueryParam queryParam)
        {
            var query = from temp in activeContext.Sys_UserLoginState
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.LoginName.Contains(queryParam.KeyWords)
                          || temp.UserCode.Contains(queryParam.KeyWords)
                          || temp.UserName.Contains(queryParam.KeyWords)
                          || temp.EqpMark.Contains(queryParam.KeyWords)
                          || temp.EqpName.Contains(queryParam.KeyWords))
                        select new vSys_UserLoginState
                        {
                             Id = temp.Id,
                             SessionId = temp.SessionId,
                             UserId = temp.UserId,
                             LoginName = temp.LoginName,
                             UserCode = temp.UserCode,
                             UserName = temp.UserName,
                             EqpMark = temp.EqpMark,
                             EqpName = temp.EqpName,
                             LastTime = temp.LastTime,
                             PeriodTime = temp.PeriodTime,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

