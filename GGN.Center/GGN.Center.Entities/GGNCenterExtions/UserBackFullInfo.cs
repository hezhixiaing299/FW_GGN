using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GGN.Center.Entities
{
    [Serializable]
    public partial class UserBackFullInfo
    {
        /// <summary>
        /// 用户基础信息
        /// </summary>
        [DataMember]
        public UserBaseInfo BaseInfo { get; set; }

        /// <summary>
        /// 用户组织机构信息
        /// </summary>
        [DataMember]
        public List<View_UserOrgInfo> UserOrgInfoList { get; set; }

        /// <summary>
        /// 用户数据权限信息
        /// </summary>
        [DataMember]
        public Hashtable UserDataInfoList { get; set; }

        /// <summary>
        /// 用户功能权限信息
        /// </summary>
        [DataMember]
        public List<View_FeaturePosition> UserFeatureInfoList { get; set; }

        /// <summary>
        /// 用户菜单信息
        /// </summary>
        [DataMember]
        public List<View_MenuSys> UserMenuInfoList { get; set; }
        
        public UserBackFullInfo()
        {
            BaseInfo = new UserBaseInfo();
            UserOrgInfoList = new List<View_UserOrgInfo>();
            UserDataInfoList = new Hashtable();
            UserFeatureInfoList = new List<View_FeaturePosition>();
            UserMenuInfoList = new List<View_MenuSys>();
        }        
    }

    [Serializable]
    public class UserBaseInfo : Org_User
    {
    }
}
