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
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;


namespace GGN.Center.Dal
{
    public class View_DataRolePositionDal : BaseDal<View_DataRolePosition>
    {
        #region 必备

        //数据集
        private readonly GGNCenterEntities activeContext;

        public View_DataRolePositionDal()
        {
            activeContext = new GGNCenterEntities();
        }
        protected override DbSet<View_DataRolePosition> DbSet
        {
            get
            {
                return activeContext.View_DataRolePosition;
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
        /// 
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public List<View_DataRolePosition> GetUserDataRoleInfos(View_DataRolePositionQueryParam queryParam)
        {
            var result = activeContext.View_DataRolePosition.Where(p => p.PositionId.HasValue
                            && queryParam.PositionIds.Contains(p.PositionId.Value)).ToList();
            return result;
        }
        /// <summary>
        /// 根据登录名和数据对象Code获取数据对象权限
        /// </summary>
        /// <param name="LogonName"></param>
        /// <param name="DataObjectCode"></param>
        /// <returns></returns>
        public List<Guid> GetUserDataInfosByLogonName(string LoginName, string DataObjectCode)
        {
            var PositionIds = (from user in activeContext.Org_User
                               join position in activeContext.Org_UserPosition on user.Id equals position.UserId
                               where user.LoginName.Equals(LoginName)
                               select position.PositionId).ToList();
            //取得所有岗位下的所有数据权限数据
            var dataitems = activeContext.View_DataRolePosition.Where(p => p.PositionId.HasValue
                            && PositionIds.Contains(p.PositionId.Value) && p.DataObjectCode.Equals(DataObjectCode)
                            ).ToList();

            return dataitems.Select(p => p.DataItemId).Distinct().ToList();
        }
        /// <summary>
        /// 获取用户数据权限信息
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public Hashtable GetUserDataInfos(View_DataRolePositionQueryParam queryParam)
        {
            Hashtable result = new Hashtable();
            //取得所有岗位下的所有数据权限数据
            var dataitems = activeContext.View_DataRolePosition.Where(p => p.PositionId.HasValue
                            && queryParam.PositionIds.Contains(p.PositionId.Value)).ToList();

            //取得数据对象列表
            var dataobjects = dataitems.Select(p => p.DataObjectId).Distinct().ToList();

            
            //循环处理每一个数据对象
            foreach (var dataobject in dataobjects)
            {
                //取得该数据对象下的dataitem清单
                var tempdataitems = dataitems.Where(p => p.DataObjectId == dataobject).ToList();
                //var tempdataobject = new AuthDataObject();
                //tempdataobject.Id = tempdataitems[0].DataObjectId;
                //tempdataobject.Code = tempdataitems[0].DataObjectCode;
                //tempdataobject.Name = tempdataitems[0].DataObjectName;
                //tempdataobject.ApplicationId = tempdataitems[0].DataObjectApplicationId.Value;
                //tempdataobject.DataBase = tempdataitems[0].DataObjectDataBase;
                //tempdataobject.TableName = tempdataitems[0].DataObjectTableName;
                //tempdataobject.Assembly = tempdataitems[0].DataObjectAssembly;
                //tempdataobject.ClassFullName = tempdataitems[0].DataObjectClassFullName;
                //tempdataobject.Sort = tempdataitems[0].DataObjectSort.Value;

                result.Add(tempdataitems[0].DataObjectClassFullName, tempdataitems);
            }

            //var aa = new List<Guid>();
            //foreach (DictionaryEntry bb in result)
            //{
            //    if (bb.Key.ToString().Contains("OrgOrganization"))
            //    {
            //        var cc = bb.Value as List<ViewDataRolePosition>;
            //        var dd = cc.Select(p=>p.Id).ToList();
            //        aa = aa.Concat(dd).ToList();
            //    }
            //}
            //var ee = aa.Count;

            return result;
        }

        #endregion
    }
}
