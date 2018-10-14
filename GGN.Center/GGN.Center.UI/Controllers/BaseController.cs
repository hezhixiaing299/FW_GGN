using FW.Base.BaseCommon;
using FW.Base.BaseDal;
using FW.Base.BaseEntity;
using GGN.Center.Dal;
using GGN.Center.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GGN.Center.UI.Controllers
{

    /// <summary>
    /// 所有Controller类的基类
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        protected virtual PrincipalUser CurrentUser { get; set; }

        #region 基础

        /// <summary>
        /// 获取当前用户
        /// 定义为虚属性，便于子类在调试时设置不同的用户
        /// </summary>
        /// </remarks>
        protected virtual UserBackFullInfo CurrentUserFullInfo { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestContext"/>
        protected override void Initialize(RequestContext requestContext)
        {
            this.CurrentUser = UserHelper.GetCurrentUser();

            //核对用户信息和安全信息 / 登录是否有效 / 缓存用户数据 / 记录日志 / 异常返回信息定义(和前台约定页面跳转)
            if (CurrentUser != null)
            {
                requestContext.HttpContext.User = (IPrincipal)this.CurrentUser;
                this.CurrentUserFullInfo = (UserBackFullInfo)(requestContext.HttpContext.Session[this.CurrentUser.LoginName]);
                //把组织机构全部数据放入缓存,如果取不到值(没有设置或者过期)
                var orgdata = RedisHelper.Get<List<Org_Organization>>("GGNCenterOrgs");
                if (orgdata == null || orgdata.Count == 0)
                {
                    Org_OrganizationDal oobll = new Org_OrganizationDal();
                    var orgall = oobll.GetAll().ToList();
                    RedisHelper.Set("GGNCenterOrgs", orgall, new TimeSpan(3650,0,0,0,0)); //失效时间3650天,10年
                }
                //把全部员工数据放入缓存,如果取不到值(没有设置或者过期)
                var allorguserdata = RedisHelper.Get<List<View_UserOrgInfo>>("GGNCenterOrgAllPersons");
                if (allorguserdata == null || allorguserdata.Count == 0)
                {
                    Org_UserDal orguserbll = new Org_UserDal();
                    var orguserall = orguserbll.GetAllUserInfos();
                    RedisHelper.Set("GGNCenterOrgAllPersons", orguserall, new TimeSpan(3650, 0, 0, 0, 0)); //失效时间3650天,10年
                }
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
            //整体权限检查/缓存用户数据/记录日志/异常返回信息定义(和前台约定页面跳转)
            if (this.CurrentUser == null)
            {
                //为空处理. 比如记录日志,跳转登录页等等
                //这个要和前台约定返回数据
            }
            else
            {
                //开始检查权限这些
                UserHelper.ValidateUserFeatureAuthority(filterContext, this.CurrentUser);
                base.OnActionExecuting(filterContext);
            }
        }

        /// <summary>
        /// Action执行完毕
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //结果转json
            if (filterContext.Result != null)
            {
                if (filterContext.Result.GetType() == typeof(JsonResult))
                {
                    var result = (JsonResult)filterContext.Result;
                    result.MaxJsonLength = int.MaxValue;
                    filterContext.Result = result;
                }
            }
            //可以记录日志
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// 开始执行Action的返回结果
        /// </summary>
        /// <param name="filterContext"/>
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            //结果类型过滤
            //这里要结合权限检查和前台确定如何做，可以考虑结果类型过滤
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
            //加入自定义异常操作
            base.OnException(filterContext);
        }
        #endregion

        #region 常用方法
        
        #endregion
    }

}