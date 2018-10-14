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
    public class View_FeaturePositionDal : BaseDal<View_FeaturePosition>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public View_FeaturePositionDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<View_FeaturePosition> DbSet
        {
            get
            {
                return activeContext.View_FeaturePosition;
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
        /// 获取用户多岗位下的功能项信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_FeaturePosition> GetUserFeatureInfos(View_FeaturePositionQueryParam queryParam)
        {
            var result = activeContext.View_FeaturePosition.Where(p => p.PositionId.HasValue
                            && queryParam.PositionIds.Contains(p.PositionId.Value)).ToList();
            return result;
        }

        /// <summary>
        /// 获取管理员的数据
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_FeaturePosition> GetSystemManagerFeatures()
        {
            var result = new List<View_FeaturePosition>();
            var featurelist = activeContext.Sys_Feature.ToList();
            foreach (var item in featurelist)
            {
                var temp = new View_FeaturePosition
                {
                    Id = Guid.Empty,
                    FeatureId = item.Id,
                    FeatureCode = item.Code,
                    FeatureName = item.Name,
                    FeatureControllerName = item.ControllerName,
                    FeatureActionName = item.ActionName,
                    FeatureUrl = item.Url,
                    FeatureIsMenu = item.IsMenu,
                    FeatureIsShortCut = item.IsShortCut,
                    FeatureLevel = item.Level,
                    FeatureSort = item.Sort,
                    FeatureRemark = item.Remark,
                    FeatureApplicationModuleId = item.ApplicationModuleId,
                    //PositionId = null,
                    //PositionCode = string.Empty,
                    //PositionName = string.Empty,
                    //PositionCategoryId = main.Code,
                    //OrganizationId = main.Code,
                    //FeatureRoleId = main.Code,
                    //FeatureRoleCode = main.Code,
                    //FeatureRoleName = main.Code,
                    //FeatureRoleRemark = main.Code,
                    //FeatureRoleDirectoryId = main.Code,
                    //FeatureRoleSort = main.Code
                };
                result.Add(temp);
            }
            //var result = (from main in activeContext.SysFeature
            //              select new ViewFeaturePosition
            //              {
            //                  Id = Guid.Empty,
            //                  FeatureId = main.Id,
            //                  FeatureCode = main.Code,
            //                  FeatureName = main.Name,
            //                  FeatureControllerName = main.Code,
            //                  FeatureActionName = main.Code,
            //                  FeatureUrl = main.Code,
            //                  FeatureIsMenu = main.IsMenu,
            //                  FeatureIsShortCut = main.IsShortCut,
            //                  FeatureLevel = main.Level,
            //                  FeatureSort = main.Sort,
            //                  FeatureRemark = main.Remark,
            //                  FeatureApplicationModuleId = main.ApplicationModuleId,
            //                  //PositionId = null,
            //                  //PositionCode = string.Empty,
            //                  //PositionName = string.Empty,
            //                  //PositionCategoryId = main.Code,
            //                  //OrganizationId = main.Code,
            //                  //FeatureRoleId = main.Code,
            //                  //FeatureRoleCode = main.Code,
            //                  //FeatureRoleName = main.Code,
            //                  //FeatureRoleRemark = main.Code,
            //                  //FeatureRoleDirectoryId = main.Code,
            //                  //FeatureRoleSort = main.Code
            //              }).ToList();
            return result;
        }

        #endregion
    }
}

