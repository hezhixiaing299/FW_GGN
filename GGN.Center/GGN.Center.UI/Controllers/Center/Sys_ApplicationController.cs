using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FW.Base.BaseEntity;
using GGN.Center.Dal;
using GGN.Center.Entities;

namespace GGN.Center.UI.Controllers
{
    public class Sys_ApplicationController : BaseController
    {
        Sys_ApplicationDal dalSY = new Sys_ApplicationDal();

        #region 主页
        [IgnoreAuthority(IgnoreType.IgnoreFeature)]
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 列表页面
        /// <summary>
        ///  获取数据列表，快速查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public JsonResult QuickQuery(Sys_ApplicationQuickQueryParam queryParam)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSY.QuickQuery(queryParam);
            return result;
        }
        /// <summary>
        ///  获取数据列表，高级查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public JsonResult Query(Sys_ApplicationQueryParam queryParam)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSY.Query(queryParam);
            return result;
        }

        /// <summary>
        /// 获取所有应用系统
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllApplication()
        {
            return new JsonResult { Data = dalSY.GetAll() };
        }
        #endregion

        #region 删除数据单条
        [HttpPost]
        public JsonResult Delete(Guid Id)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSY.Delete(Id);
            return result;
        }
        #endregion

        #region 快速保存
        [HttpPost]
        public JsonResult QuickSave(Sys_Application model)
        {
            JsonResult result = new JsonResult();
            if (model.Id == Guid.Empty)
            {
                result.Data = dalSY.Insert(model);
            }
            else
            {
                result.Data = dalSY.Update(model);
            }
            return result;
        }
        #endregion

        #region 编辑/展示页面
        public ActionResult Update()
        {
            return View();
        }
        /// <summary>
        ///  获取详细信息
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public JsonResult GetById(Guid? Id)
        {
            JsonResult result = new JsonResult();
            if (Id.HasValue)
            {
                result.Data = dalSY.GetById(Id.Value);
            }
            return result;
        }
        #endregion

    }
}
