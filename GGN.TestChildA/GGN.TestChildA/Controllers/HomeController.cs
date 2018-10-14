using FW.Base.BaseCommon;
using GGN.Center.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GGN.TestChildA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var a1 = (ConfigurationManager.GetSection("SystemConfig") as NameValueCollection).Get("RedisIp");
            //var aa = RedisHelper.Get<UserBackFullInfo>("GGNCenterUser_sysadmin"); 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}