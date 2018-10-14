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
    public class Auth_DataRoleConfigDal : BaseDal<Auth_DataRoleConfig>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Auth_DataRoleConfigDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Auth_DataRoleConfig> DbSet
        {
            get
            {
                return activeContext.Auth_DataRoleConfig;
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
        public  vAuth_DataRoleConfig GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vAuth_DataRoleConfig>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Auth_DataRoleConfig model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Auth_DataRoleConfig
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
        
        public ListByPages<vAuth_DataRoleConfig> QuickQuery(Auth_DataRoleConfigQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataRoleConfig
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  )
                        select new vAuth_DataRoleConfig
                        {
                             Id = temp.Id,
                             DataRoleId = temp.DataRoleId,
                             DataObjectId = temp.DataObjectId,
                             DataItemId = temp.DataItemId,
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
        public ListByPages<vAuth_DataRoleConfig> Query(Auth_DataRoleConfigQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataRoleConfig
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  )
                        select new vAuth_DataRoleConfig
                        {
                             Id = temp.Id,
                             DataRoleId = temp.DataRoleId,
                             DataObjectId = temp.DataObjectId,
                             DataItemId = temp.DataItemId,
                             Sort = temp.Sort,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

