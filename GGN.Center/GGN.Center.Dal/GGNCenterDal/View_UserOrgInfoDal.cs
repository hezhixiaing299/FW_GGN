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
    public class View_UserOrgInfoDal : BaseDal<View_UserOrgInfo>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public View_UserOrgInfoDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<View_UserOrgInfo> DbSet
        {
            get
            {
                return activeContext.View_UserOrgInfo;
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

        #region 方法

        /// <summary>
        /// 获取用户组织机构信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_UserOrgInfo> GetUserOrgInfos(Org_UserQueryParam queryParam)
        {
            var query = from temp in activeContext.View_UserOrgInfo
                        where (queryParam.Id == Guid.Empty || temp.Id == queryParam.Id)
                           && (string.IsNullOrEmpty(queryParam.LoginName) || temp.LoginName == queryParam.LoginName)
                           && (string.IsNullOrEmpty(queryParam.Phone) || temp.Phone.Contains(queryParam.Phone))
                           && (string.IsNullOrEmpty(queryParam.Phone) || temp.Telephone.Contains(queryParam.Phone))
                           && (string.IsNullOrEmpty(queryParam.Email) || temp.Email.Contains(queryParam.Email))
                           && (string.IsNullOrEmpty(queryParam.IdCard) || temp.IdCard.Contains(queryParam.IdCard))
                        select temp;
            var result = query.ToList();
            return result;
        }
        /// <summary>
        /// 获取所有组织机构员工信息(在职员工)
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_UserOrgInfo> GetAllUserInfos()
        {
            var query = from temp in activeContext.View_UserOrgInfo
                        where temp.IsFreeze == false
                        select temp;
            var result = query.ToList();
            return result;
        }
        /// <summary>
        /// 获取所有组织机构员工信息(在职员工)
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_UserOrgInfo> GetAllUserInfosList(Org_UserQueryParam queryParam)
        {
            var query = from temp in activeContext.View_UserOrgInfo
                        where (
                         string.IsNullOrEmpty(queryParam.KeyWords)
                         || temp.LoginName.Contains(queryParam.KeyWords)
                         || temp.Phone.Contains(queryParam.KeyWords)
                         || temp.Telephone.Contains(queryParam.KeyWords)
                         || temp.Email.Contains(queryParam.KeyWords)
                         || temp.IdCard.Contains(queryParam.KeyWords)
                        )
                        &&
                        temp.IsFreeze == false
                        select temp;
            var result = query.ToList();
            return result;
        }

        #endregion
    }
}

