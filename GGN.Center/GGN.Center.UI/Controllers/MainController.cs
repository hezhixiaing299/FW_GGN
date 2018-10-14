using FW.Base.BaseEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GGN.Center.UI.Controllers
{
    public class MainController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [IgnoreAuthorityAttribute(IgnoreType.SameAs)]
        public ActionResult First()
        {
            return View();
        }

        //[IgnoreAuthority(IgnoreType.IgnoreFeature)] //忽略功能权限
        //[IgnoreAuthority(IgnoreType.SameAs, "QuickQuery")] //要求匹配的功能权限名
        [IgnoreAuthority(IgnoreType.SameAs)]
        public JsonResult aaa()
        {
            return new JsonResult();
        }
    }
}