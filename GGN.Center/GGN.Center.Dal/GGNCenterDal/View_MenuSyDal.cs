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
    public class View_MenuSyDal : BaseDal<View_MenuSys>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public View_MenuSyDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<View_MenuSys> DbSet
        {
            get
            {
                return activeContext.View_MenuSys;
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

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_MenuSys> GetUserMenuInfoList(View_MenuSysQueryParam queryParam)
        {
            //取出对应功能项下的菜单
            var result = activeContext.View_MenuSys.Where(p => p.FeatureId.HasValue
                            && queryParam.FeatureIds.Contains(p.FeatureId.Value)).DistinctBy(p => new { p.Id }).ToList();
            return result;
        }

        /// <summary>
        /// 获取用户菜单
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_MenuSys> GetMenuByIds(View_MenuSysQueryParam queryParam)
        {
            var result = activeContext.View_MenuSys.Where(p => queryParam.Ids.Contains(p.Id)).ToList();
            return result;
        }

        /// <summary>
        /// 检查每个菜单的上级菜单
        /// </summary>
        /// <param name="all"></param>
        /// <param name="everyone"></param>
        private void CheckEveryOneMenuParent(IList<View_MenuSys> all, View_MenuSys everyone, List<View_MenuSys> last)
        {
            //首先添加该记录
            last.Add(everyone);

            //如果菜单有父级
            if (everyone.MenuParentId.HasValue && everyone.MenuParentId.Value != Guid.Empty)
            {
                //找出该父级
                var tempParentMenu = all.Where(p => p.Id == everyone.MenuParentId.Value).FirstOrDefault();
                if (tempParentMenu != null)
                {
                    CheckEveryOneMenuParent(all, tempParentMenu, last);
                }
            }
        }

        /// <summary>
        /// 检查每个菜单的下级菜单
        /// </summary>
        /// <param name="all"></param>
        /// <param name="everyone"></param>
        private void CheckEveryOneMenuChlidrens(IList<View_MenuSys> all, View_MenuSys everyone, List<View_MenuSys> last)
        {
            //首先添加该记录
            last.Add(everyone);
            //找出子级清单
            var tempChlidrenList = all.Where(p => p.MenuParentId.HasValue && p.MenuParentId.Value == everyone.Id).ToList();
            last = last.Concat(tempChlidrenList).ToList();
        }

    }
}

