﻿using System;
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
    public class Auth_FeatureRoleDal : BaseDal<Auth_FeatureRole>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public Auth_FeatureRoleDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<Auth_FeatureRole> DbSet
        {
            get
            {
                return activeContext.Auth_FeatureRole;
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
        public  vAuth_FeatureRole GetById(Guid Id)
        {
            var model= base.GetById(Id);
            var returnModel = CommonOperate.ConvertObj<vAuth_FeatureRole>(model);
            return returnModel;
        }
        #endregion
        
        #region 检查mark是否重复
        public OperateStatus CheckMark(Auth_FeatureRole model)
        {
            OperateStatus op = new OperateStatus();
            try
            {
                var query = from temp in activeContext.Auth_FeatureRole
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
        
        public ListByPages<vAuth_FeatureRole> QuickQuery(Auth_FeatureRoleQuickQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_FeatureRole
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vAuth_FeatureRole
                        {
                             Id = temp.Id,
                             FeatureRoleDirectoryId = temp.FeatureRoleDirectoryId,
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
        public ListByPages<vAuth_FeatureRole> Query(Auth_FeatureRoleQueryParam queryParam)
        {
            var query = from temp in activeContext.Auth_FeatureRole
                        where
                           (string.IsNullOrEmpty(queryParam.KeyWords)  
                          || temp.Code.Contains(queryParam.KeyWords)
                          || temp.Name.Contains(queryParam.KeyWords)
                          || temp.Remark.Contains(queryParam.KeyWords))
                        select new vAuth_FeatureRole
                        {
                             Id = temp.Id,
                             FeatureRoleDirectoryId = temp.FeatureRoleDirectoryId,
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
