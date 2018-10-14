using FW.Base.BaseCommon;
using FW.Base.BaseEntity;
using GGN.Center.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GGN.TestChildA.Controllers
{
    /// <summary>
    /// 所有Controller类的基类
    /// 
    /// </summary>
    public class ChildSysBaseController : Controller
    {        

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// 定义为虚属性，便于子类在调试时设置不同的用户
        /// </remarks>
        protected virtual PrincipalUser CurrentUser { get; private set; }

        protected virtual UserBackFullInfo CurrentUserFullInfo { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestContext"/>
        protected override void Initialize(RequestContext requestContext)
        {
            this.CurrentUser = ChildSysUserHelper.GetCurrentUser();
            if (CurrentUser != null)
            {
                requestContext.HttpContext.User = (IPrincipal)this.CurrentUser;
                this.CurrentUserFullInfo = (UserBackFullInfo)(requestContext.HttpContext.Session[this.CurrentUser.LoginName]);                
            }
            base.Initialize(requestContext);
        }

        /// <summary>
        /// Action开始执行
        /// 验证用户功能项权限
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (this.CurrentUser == null)
            {
                //为空处理. 比如记录日志,跳转登录页等等
                //这个要和前台约定返回数据
            }
            else
            {
                ChildSysUserHelper.ValidateUserFeatureAuthority(filterContext, this.CurrentUser);
                base.OnActionExecuting(filterContext);
            }
        }

        /// <summary>
        /// Action执行完毕
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result != null)
            {
                if (filterContext.Result.GetType() == typeof(JsonResult))
                {
                    var result = (JsonResult)filterContext.Result;
                    result.MaxJsonLength = int.MaxValue;
                    filterContext.Result = result;
                }
            }
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 开始执行Action的返回结果
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            if (!(filterContext.Result.GetType() == typeof(RedirectResult)))
            {
                return;
            }
        }

        /// <summary>
        /// Action的返回结果执行完毕
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// 出现了异常
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
    }

    public class ChildSysUserHelper
    {
        /// <summary>
        /// 用户令牌CookieKey,写死定义叫"DCTokenCookie"
        /// </summary>
        private static readonly string userGGNTokenCookie = "GGNTokenCookie";

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static PrincipalUser GetCurrentUser()
        {
            PrincipalUser principalUser;
            //通过Cookie获取当前登陆名
            string loginName = GetCurrentUserLoginName();
            var currentUser = RedisHelper.Get<UserBackFullInfo>("GGNCenterUser_" + loginName);
            //if (string.IsNullOrEmpty(loginName))
            if (currentUser == null)
            {
                principalUser = (PrincipalUser)null;
                HttpContext.Current.Session[loginName] = null;
            }
            else
            {
                var orgslist = RedisHelper.Get<List<Org_Organization>>("GGNCenterOrgs");
                if (orgslist == null)
                {
                    //看该如何处理吧
                }
                //把全部员工数据放入缓存,如果取不到值(没有设置或者过期)
                var allorguserdata = RedisHelper.Get<List<View_UserOrgInfo>>("GGNCenterOrgAllPersons");
                if (allorguserdata == null || allorguserdata.Count == 0)
                {
                    //看该如何处理吧
                }

                principalUser = new PrincipalUser();
                principalUser.Id = currentUser.BaseInfo.Id;
                principalUser.LoginName = currentUser.BaseInfo.LoginName;
                principalUser.UserName = currentUser.BaseInfo.UserName;
                principalUser.UserCode = currentUser.BaseInfo.Code;
                principalUser.IsOutSide = currentUser.BaseInfo.IsOutSide;
                principalUser.IsManager = currentUser.BaseInfo.IsSuperMgr;
                //Session已有
                if (HttpContext.Current.Session[principalUser.LoginName] == null)
                {
                    //设置Session对象                        
                    HttpContext.Current.Session[principalUser.LoginName] = currentUser;
                }
            }
            return principalUser;
        }

        /// <summary>
        /// 写入用户令牌Cookie
        /// </summary>
        public static void WrriteUserTokenCookie(string loginName)
        {
            string securityKey = GetSecurityKey();
            int logonExpiresTime = Convert.ToInt32(GlobalStaticParam.GetByCode("LoginExpiresTime"));
            DateTime expirationTime = DateTime.Now.AddMinutes(logonExpiresTime);
            //创建用户令牌Cookie值
            string value = CreateUserTokenCookieValue(loginName, securityKey, expirationTime);
            var cookie = new HttpCookie(userGGNTokenCookie, value) { Expires = expirationTime };
            //设置域
            SetCookieDomain(cookie);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        ///设置cookie的域 
        /// </summary>
        private static void SetCookieDomain(HttpCookie cookie)
        {
            //lpmf.weijinggroup.com
            var hostPath = HttpContext.Current.Request.Url.Host;
            //if (hostPath != "localhost")
            //{
            //    cookie.Domain = (string)GlobalParam.GetByCode("Domain");
            //}            
            //////检查是否配置为域名
            //if (hostPath.IndexOf(".com") > 0)
            //{
            //}
            cookie.Domain = hostPath;
        }

        #region 用户TokenCookie操作

        /// <summary>
        /// 创建用户令牌Cookie值
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="securityKey"></param>
        /// <param name="expirationTime"></param>
        /// <returns></returns>
        public static string CreateUserTokenCookieValue(string loginName, string securityKey, DateTime expirationTime)
        {
            //加密过程：
            //1.构造加密前格式：登录名(loginName)+密钥(securityKey)+到期时间(expirationTime,yyyyMMddHHmmss)
            //2.用MD5Hash
            //3.构造编码格式：hash值（32位）+到期时间（endTime,yyyyMMddHHmmss，14位）+登录名
            //4.用Base64编码
            string expirationTimeString = expirationTime.ToString("yyyyMMddHHmmss");
            string md5Key = string.Format("{0}{1}{2}", loginName, securityKey, expirationTimeString);
            string md5Result = GetMd5Hash(md5Key);
            //Console.WriteLine("MD5 Result is:"+ md5Result);
            string encodeKey = string.Format("{0}{1}{2}", md5Result, expirationTimeString, loginName);
            return EncodeToBase64(encodeKey);
        }

        /// <summary>
        /// 获取按Base64编码后的字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string EncodeToBase64(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 获取解码Base64后的格式
        /// </summary>
        /// <returns></returns>
        private static string DecodeFromBase64(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 获取Md5哈希值
        /// </summary>
        /// <param name="value">返回值为32位的字符串</param>
        /// <returns></returns>
        private static string GetMd5Hash(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(bytes);

            //md5以后为16字节的数组，128位，将其按16进制编码后，转换为32位的字符串
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 获取用户登录名
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentUserLoginName()
        {
            //根据浏览器的cookie信息解密出用户名。
            //var hostPath = HttpContext.Current.Request.Url.Host;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[userGGNTokenCookie];
            if (cookie == null)
            {
                return null;
            }
            return DecryptLoginName(cookie.Value);
        }

        /// <summary>
        /// 解密Cookie获取用户名
        /// </summary>
        /// <returns></returns>
        public static string DecryptLoginName(string userToken)
        {
            //cookie长度为76位
            if (string.IsNullOrEmpty(userToken))
            {
                return null;
            }
            //解码Base64编码
            string decodeString = DecodeFromBase64(userToken);
            //验证过期时间是否有效
            string expirationTimeString = decodeString.Substring(32, 14);
            if (!IsExpirationTimeValid(expirationTimeString))
                return null;
            //验证登录名是否有效
            if (!IsLoginNameValid(decodeString))
                return null;
            string loginName = decodeString.Substring(46);
            return loginName;
        }

        /// <summary>
        /// 获取加密key
        /// </summary>
        /// <returns></returns>
        private static string GetSecurityKey()
        {
            //todo 从系统配置中获取加密key,但机制没确定,暂时静态表中不加这个code
            return "";
        }

        /// <summary>
        /// 检测登录名是否有效
        /// </summary>
        /// <param name="decodeString"></param>
        /// <returns></returns>
        private static bool IsLoginNameValid(string decodeString)
        {
            string md5Result = decodeString.Substring(0, 32);
            string expirationTimeString = decodeString.Substring(32, 14);
            string loginName = decodeString.Substring(46);
            string md5Key = string.Format("{0}{1}{2}", loginName, GetSecurityKey(), expirationTimeString);
            return md5Result == GetMd5Hash(md5Key);
        }

        private static DateTime GetDateTimeFromString(string timeString)
        {
            int year = int.Parse(timeString.Substring(0, 4));
            int month = int.Parse(timeString.Substring(4, 2));
            int day = int.Parse(timeString.Substring(6, 2));
            int hour = int.Parse(timeString.Substring(8, 2));
            int minute = int.Parse(timeString.Substring(10, 2));
            int second = int.Parse(timeString.Substring(12, 2));
            DateTime result = new DateTime(year, month, day, hour, minute, second);
            return result;
        }

        /// <summary>
        /// 检测过期时间是否有效
        /// </summary>
        /// <param name="expirationTimeString"></param>
        /// <returns></returns>
        private static bool IsExpirationTimeValid(string expirationTimeString)
        {
            DateTime expirationTime = GetDateTimeFromString(expirationTimeString);
            return expirationTime > DateTime.Now;
        }

        #endregion


        #region 验证用户功能权限

        /// <summary>
        /// 用户是否有Controller和Action对应功能的权限
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="controllerName"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        //public static bool HasFeatureAuthority(string loginName, string controllerName, string actionName)
        //{
        //    OrgUserDal dal = new OrgUserDal();
        //    return dal.HasFeatureAuthority(loginName, controllerName, actionName);
        //}

        /// <summary>
        /// 验证用户功能权限
        /// 先对用户的登录状态进行验证，如果未登录则重定向到系统配置中配置的登录页面，并且终止当前请求Action的执行。
        /// 如果已登录，则继续进行功能项权限验证，如果用户没有所请求Action的权限则重定向到权限验证失败页面，并且终止当前请求Action的执行。
        /// 如果权限验证通过则继续执行所请求的Action
        /// </summary>
        public static bool ValidateUserFeatureAuthority(ActionExecutingContext actionExecutingContext, PrincipalUser currentUser)
        {
            IgnoreAuthorityAttribute authorityAttribute = ChildSysUserHelper.GetIgnoreAuthorityAttribute(actionExecutingContext);
            if (authorityAttribute != null && authorityAttribute.IgnoreType == IgnoreType.IgnoreLogin) //是否有验证特性
            {
                return true;
            }
            if (currentUser == null)
            {
                //页面跳转
                return false;
            }
            if (currentUser.IsManager)  //管理员
            {
                return true;
            }
            string logonName = currentUser.LoginName;
            WriteUserTokenCookie(logonName);
            string controllerName = actionExecutingContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;
            string actionName = actionExecutingContext.ActionDescriptor.ActionName;
            if (authorityAttribute != null)
            {
                if (authorityAttribute.IgnoreType == IgnoreType.IgnoreFeature)
                {
                    return true;
                }
                if (authorityAttribute.IgnoreType == IgnoreType.SameAs)
                {
                    if (string.IsNullOrEmpty(authorityAttribute.SameActionName))  //如果没有复制SameActionName,则用当前ActionName
                    {
                        authorityAttribute.SameActionName = actionName;
                    }
                    actionName = authorityAttribute.SameActionName;
                    if (!string.IsNullOrEmpty(authorityAttribute.SameControllerName))
                    {
                        controllerName = authorityAttribute.SameControllerName;
                    }
                    var userinfo = (UserBackFullInfo)(actionExecutingContext.HttpContext.Session[currentUser.LoginName]);
                    var FeatureCheck = userinfo.UserFeatureInfoList.Where(p => p.FeatureControllerName == controllerName && p.FeatureActionName == actionName).ToList();
                    if (FeatureCheck.Count == 1)
                    {
                        return true;
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Controller：{0}上的Action：{1}配置异常,请检查配置!", (object)controllerName, (object)actionName));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 指定的Action是否需要权限检查
        /// </summary>
        /// <param name="actionExecutingContext"/>
        /// <returns/>
        private static IgnoreAuthorityAttribute GetIgnoreAuthorityAttribute(ActionExecutingContext actionExecutingContext)
        {
            object[] customAttributes = actionExecutingContext.ActionDescriptor.GetCustomAttributes(typeof(IgnoreAuthorityAttribute), false);
            if (customAttributes.Length != 0)
                return (IgnoreAuthorityAttribute)customAttributes[0];
            return (IgnoreAuthorityAttribute)null;
        }

        /// <summary>
        /// 写入用户令牌Cookie
        /// 如果当前存在cookie，则每小时延时一次.
        /// </summary>
        public static void WriteUserTokenCookie(string loginName, bool forceWrite = false)
        {
            if (HttpContext.Current == null)
            {
                return;
            }
            string securityKey = GetSecurityKey();
            int logonExpiresTime = Convert.ToInt32(GlobalStaticParam.GetByCode("LoginExpiresTime"));
            DateTime expirationTime = DateTime.Now.AddHours(logonExpiresTime);
            //创建用户令牌Cookie值
            string value = CreateUserTokenCookieValue(loginName, securityKey, expirationTime);
            var cookie = new HttpCookie(userGGNTokenCookie, value) { Expires = expirationTime };
            //设置域
            SetCookieDomain(cookie);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }


        #endregion
    }

}