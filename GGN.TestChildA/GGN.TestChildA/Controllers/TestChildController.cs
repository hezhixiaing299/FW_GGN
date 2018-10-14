using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GGN.TestChildA.Controllers
{
    public class TestChildController : ChildSysBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}