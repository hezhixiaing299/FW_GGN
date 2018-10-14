using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FW.Base.BaseEntity
{
    /// <summary>
    /// 在上下文中使用的用户
    /// </summary>
    public class PrincipalUser : IPrincipal
    {
        private string timeFormat;

        ///<summary>
        /// 默认构造函数
        ///</summary>
        public PrincipalUser()
        { }
        
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static PrincipalUser Current
        {
            get
            {
                PrincipalUser principalUser = null;
                //这里以后可以考虑做成枚举,先这样吧
                if (IsControlApp)   //如果是web应用
                {
                    principalUser = GetWebAppUserInfo();  //使用例子,目前IsControlApp这些都是false
                }
                if (IsWCFApp)  //如果是WCF
                {
                    //先不管,返回空
                }
                if (IsAPI)  //如果是API
                {
                    //先不管,返回空
                }
                return principalUser;
            }
        }
        
        /// <summary>
        /// 获取WEB应用的用户信息
        /// </summary>
        /// <returns></returns>
        private static PrincipalUser GetWebAppUserInfo()
        {
            return HttpContext.Current.User as PrincipalUser;
        }

        /// <summary>
        /// 是否WCF应用
        /// 要使用此属性,就得给这个属性赋值,需要和前台约定
        /// </summary>
        public static bool IsWCFApp
        {
            get { return OperationContext.Current != null; }
        }

        /// <summary>
        /// 是否WEB应用
        /// </summary>
        public static bool IsControlApp
        {
            get { return HttpContext.Current != null; }
        }

        /// <summary>
        /// 是否是API
        /// </summary>
        public static bool IsAPI
        {
            get { return HttpContext.Current != null; }
        }

        /// <summary>
        /// 接口要求的,不使用
        /// </summary>
        public IIdentity Identity
        {
            get { return null; }
        }

        /// <summary>
        /// 接口要求的,不使用
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            //return false;
            throw new NotImplementedException();
        }

        #region 用户信息

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户电话
        /// </summary>
        public string Phone { get; set; }

        public string UserCode { get; set; }

        public bool IsManager { get; set; }

        /// <summary>
        /// 是否内部员工
        /// </summary>
        public bool IsOutSide { get; set; }

        /// <summary>
        /// 用户当前组织机构id(关系)
        /// </summary>
        public Guid CurrentOrgId { get; set; }

        /// <summary>
        /// 设备标示
        /// </summary>
        public string EquipmentMark { get; set; }

        #endregion
    }
}
