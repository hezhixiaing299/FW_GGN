using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FW.Base.BaseCommon;
using FW.Base.BaseEntity;
using FW.Tool;
using GGN.Center.Dal;
using GGN.Center.Entities;

namespace GGN.Center.UI.Controllers
{
    public class LoginController : Controller
    {
        private Org_UserDal ouDal = new Org_UserDal();

        public ActionResult Index()
        {
            //var orgdata = RedisHelper.Get<List<Org_Organization>>("GGNCenterOrgs");
            //var allorguserdata = RedisHelper.Get<List<View_UserOrgInfo>>("GGNCenterOrgAllPersons");
            //var GGNCenterUser_sysadmin = RedisHelper.Get("GGNCenterUser_sysadmin");
            //var aa = RedisHelper.Get<UserBackFullInfo>("GGNCenterUser_sysadmin");
            //var bb = RedisHelper.Get("GGNCenterOrgAllPersons");
            return View();
        }

        public ActionResult GetAuthCode()
        {
            return File(VerifyCode.GetVerifyCode(), @"image/Gif");
        }

        public JsonResult CheckLogin(BaseUser loginuser)
        {
            JsonResult json = new JsonResult();
            OperateStatus op = new OperateStatus();

            string checkVerify = DEncrypt.Get16_Md5Lower(loginuser.Code, null);
            if (Session["Login_VerifyCode"] == null || checkVerify != Session["Login_VerifyCode"].ToString())
            {
                op.IsSuccessful = false;
                op.Message = "验证码不正确，请重新输入!";
            }
            else
            {
                op = ouDal.CheckLogin(loginuser);
                if (op.IsSuccessful)
                {
                    //记录Cookie
                    //UserHelper.WrriteUserTokenCookie(loginuser.LoginName);
                    op.IsSuccessful = true;
                    op.Message = "登录成功!欢迎您!";
                    //记录Cookie
                    UserHelper.WriteUserTokenCookie(loginuser.LoginName);
                }
                else
                {
                    op.IsSuccessful = false;
                    op.Message = op.Message ?? "用户名或密码错误！";
                }
            }
            json.Data = op;
            return json;
        }
    }
}