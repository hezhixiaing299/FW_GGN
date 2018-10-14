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
    public class Sys_MenuController : BaseController
    {
        Sys_MenuDal dalSM = new Sys_MenuDal();

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
        public JsonResult QuickQuery(Sys_MenuQuickQueryParam queryParam)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSM.QuickQuery(queryParam);
            return result;
        }
        /// <summary>
        ///  获取数据列表，高级查询
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public JsonResult Query(Sys_MenuQueryParam queryParam)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSM.Query(queryParam);
            return result;
        }

        /// <summary>
        /// 获取所有应用系统
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllApplication()
        {
            return new JsonResult { Data = dalSM.GetAll() };
        }
        #endregion

        #region 删除数据单条
        [HttpPost]
        public JsonResult Delete(Guid Id)
        {
            JsonResult result = new JsonResult();
            result.Data = dalSM.Delete(Id);
            return result;
        }
        #endregion

        #region 快速保存
        [HttpPost]
        public JsonResult QuickSave(Sys_Menu model)
        {
            JsonResult result = new JsonResult();
            if (model.Id == Guid.Empty)
            {
                result.Data = dalSM.Insert(model);
            }
            else
            {
                result.Data = dalSM.Update(model);
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
                result.Data = dalSM.GetById(Id.Value);
            }
            return result;
        }
        #endregion

    }
}
