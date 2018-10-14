using FW.Base.BaseCommon;
using FW.Base.BaseEntity;
using GGN.Center.Dal;
using GGN.Center.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GGN.Center.UI
{
    public static class UserHelper
    {
        /// <summary>
        /// 用户令牌CookieKey,写死定义叫"DCTokenCookie"
        /// </summary>
        private static readonly string userGGNTokenCookie = "GGNTokenCookie";

        private static readonly Org_UserDal dalUser = new Org_UserDal();

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static PrincipalUser GetCurrentUser()
        {
            PrincipalUser principalUser;
            //通过Cookie获取当前登陆名,如果没有,则需要登录
            string loginName = GetCurrentUserLoginName();
            if (string.IsNullOrEmpty(loginName))
            {
                principalUser = null;
                HttpContext.Current.Session[loginName] = null;

                HttpCookie cookies = HttpContext.Current.Request.Cookies[userGGNTokenCookie];
                if (cookies != null)
                {
                    cookies.Expires = DateTime.Today.AddDays(-1);
                    HttpContext.Current.Response.Cookies.Add(cookies);
                    HttpContext.Current.Request.Cookies.Remove(userGGNTokenCookie);
                }
            }
            else
            {
                //Session里面不存在
                if (HttpContext.Current.Session[loginName] == null)
                {
                    //检查数据库是否有此用户
                    Org_User user = GetOnlineUser(loginName);
                    if (user == null) //没有说明有问题
                    {
                        principalUser = (PrincipalUser)null;
                        HttpContext.Current.Session[loginName] = null;

                        HttpCookie cookies = HttpContext.Current.Request.Cookies[userGGNTokenCookie];
                        if (cookies != null)
                        {
                            cookies.Expires = DateTime.Today.AddDays(-1);
                            HttpContext.Current.Response.Cookies.Add(cookies);
                            HttpContext.Current.Request.Cookies.Remove(userGGNTokenCookie);
                        }
                    }
                    else
                    {
                        //如果有此用户
                        principalUser = new PrincipalUser
                        {
                            Id = user.Id,
                            LoginName = user.LoginName,
                            UserName = user.UserName,
                            UserCode = user.Code,
                            IsManager = false,
                            IsOutSide = user.IsOutSide,
                            Phone = user.Phone
                        };
                        //获取用户全信息数据
                        Org_UserQueryParam query = new Org_UserQueryParam { LoginName = loginName };
                        var userdatainfo = dalUser.GetUserFullInfo(query);

                        //重建此session和缓存数据
                        principalUser.IsManager = userdatainfo.BaseInfo.IsSuperMgr;

                        RedisHelper.Set("GGNCenterUser_" + loginName, userdatainfo, null);
                        HttpContext.Current.Session[principalUser.LoginName] = userdatainfo;
                    }
                }
                else //Session里面存在
                {
                    var sessionUser = (UserBackFullInfo)(HttpContext.Current.Session[loginName]);
                    principalUser = new PrincipalUser();
                    principalUser.Id = sessionUser.BaseInfo.Id;
                    principalUser.LoginName = sessionUser.BaseInfo.LoginName;
                    principalUser.UserName = sessionUser.BaseInfo.UserName;
                    principalUser.UserCode = sessionUser.BaseInfo.Code;
                    principalUser.IsManager = sessionUser.BaseInfo.IsSuperMgr;
                    principalUser.IsOutSide = sessionUser.BaseInfo.IsOutSide;
                    principalUser.Phone = sessionUser.BaseInfo.Phone;
                }
            }
            return principalUser;
        }

        /// <summary>
        /// 写入用户令牌Cookie
        /// </summary>
        public static void WriteUserTokenCookie(string loginName)
        {
            string securityKey = GetSecurityKey();
            int loginExpiresTime = Convert.ToInt32(GlobalStaticParam.GetByCode("LoginStateTime"));
            DateTime expirationTime = DateTime.Now.AddHours(loginExpiresTime);
            //创建用户令牌Cookie值
            string value = CreateUserTokenCookieValue(loginName, securityKey, expirationTime);
            var cookie = new HttpCookie(userGGNTokenCookie, value) { Expires = expirationTime };
            //设置域
            SetCookieDomain(cookie);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 清除用户Cookie
        /// </summary>
        public static void ClearUserTokenCookie()
        {
            //cookie包含在request和response中
            var responseCookie = HttpContext.Current.Response.Cookies[userGGNTokenCookie];
            var loginname = DecryptLoginName(responseCookie.Value);
            if (responseCookie != null)
            {
                //HttpContext.Current.Response.Cookies.Remove(responseCookie.Name);
                string[] cookies = { responseCookie.Name };
                ClearCookies(HttpContext.Current, cookies);
            }
            HttpContext.Current.Session[loginname] = null;
            //清除用户信息缓存
            RedisHelper.Delete("GGNCenterUser_" + loginname);
        }

        /// <summary>
        ///设置cookie的域 
        /// </summary>
        private static void SetCookieDomain(HttpCookie cookie)
        {
            if (HttpContext.Current == null || !(HttpContext.Current.Request.Url.Host != "localhost"))
            {
                return;
            }
            else
            {
                cookie.Domain = (string)GlobalStaticParam.GetByCode("Domain");
            }
        }

        /// <summary>
        /// 清除指定cookie
        /// </summary>
        /// <param name="contenxt">The contenxt.</param>
        /// <param name="cookies">The cookies.</param>
        public static void ClearCookies(HttpContext contenxt, params string[] cookies)
        {
            //清除cookies
            foreach (string key in cookies)
            {
                contenxt.Response.Cookies.Remove(key);
                contenxt.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
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
        /// 检测过期时间是否有效
        /// </summary>
        /// <param name="expirationTimeString"></param>
        /// <returns></returns>
        private static bool IsExpirationTimeValid(string expirationTimeString)
        {
            DateTime expirationTime = GetDateTimeFromString(expirationTimeString);
            return expirationTime > DateTime.Now;
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
        /// 获取解码Base64后的格式
        /// </summary>
        /// <returns></returns>
        private static string DecodeFromBase64(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(bytes);
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
        /// 根据用户登录名获取用户
        /// </summary>
        /// <param name="loginName">登录名</param>       
        /// <returns></returns>
        private static Org_User GetOnlineUser(string loginName)
        {
            Org_User user = dalUser.GetByLoginName(loginName);
            return user;
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
        
        #endregion

        #region 验证用户功能权限

        /// <summary>
        /// 验证用户功能权限
        /// 先对用户的登录状态进行验证，如果未登录则重定向到系统配置中配置的登录页面，并且终止当前请求Action的执行。
        /// 如果已登录，则继续进行功能项权限验证，如果用户没有所请求Action的权限则重定向到权限验证失败页面，并且终止当前请求Action的执行。
        /// 如果权限验证通过则继续执行所请求的Action
        /// </summary>
        public static bool ValidateUserFeatureAuthority(ActionExecutingContext actionExecutingContext, PrincipalUser currentUser)
        {
            IgnoreAuthorityAttribute authorityAttribute = UserHelper.GetIgnoreAuthorityAttribute(actionExecutingContext);
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
            {
                return (IgnoreAuthorityAttribute)customAttributes[0];
            }
            return (IgnoreAuthorityAttribute)null;
        }

        #endregion
    }
}