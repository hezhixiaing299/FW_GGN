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
    public class Auth_DataRoleDal : BaseDal<Auth_DataRole>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Auth_DataRoleDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Auth_DataRole> DbSet
        {
            get
            {
                return activeContext.Auth_DataRole;
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
        public  vAuth_DataRole GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vAuth_DataRole>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Auth_DataRole model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Auth_DataRole
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
        
        public ListByPages<vAuth_DataRole> QuickQuery(Auth_DataRoleQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataRole
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vAuth_DataRole
                        {
                             Id = temp.Id,
                             DataRoleDirectoryId = temp.DataRoleDirectoryId,
                             PositionId = temp.PositionId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Sort = temp.Sort,
                             Remark = temp.Remark,
                        };
            var resutls = query.ToListByPages(queryParam);
            return resutls;
        }
        /// <summary>
        /// 全查询分页
        /// </summary>
        /// <param name="queryParam">自定义扩展查询参数</param>
        /// <returns></returns>
        public ListByPages<vAuth_DataRole> Query(Auth_DataRoleQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_DataRole
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vAuth_DataRole
                        {
                             Id = temp.Id,
                             DataRoleDirectoryId = temp.DataRoleDirectoryId,
                             PositionId = temp.PositionId,
                             Code = temp.Code,
                             Name = temp.Name,
                             Sort = temp.Sort,
                             Remark = temp.Remark,
                        }
                        ;
            var tempquery = query.ToListByPages(queryParam);
            return tempquery;
        }
        #endregion
    }
}

