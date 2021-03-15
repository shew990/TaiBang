using SqlSugarDAL.setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;

namespace Web.WMS.Controllers.Setting
{
    public class SettingController : Controller
    {
        /// <summary>
        /// 跳转主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int limit, int page)
        {
            var settings = new SettingService().GetSettingsPage(limit, page);
            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public ActionResult DeleteSave(T_Setting setting)
        {
            var successResult = new SettingService().DeleteSave(setting);
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转保存页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 提交（新增/编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit(T_Setting setting)
        {
            string userNo = Commom.ReadUserInfo().UserNo;
            var successResult = new SettingService().Submit(userNo, setting);
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }
    }
}