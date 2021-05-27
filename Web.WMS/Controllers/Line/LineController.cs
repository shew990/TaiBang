using SqlSugarDAL.line;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.Line
{
    public class LineController : Controller
    {
        /// <summary>
        /// 返回主视图
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
        public ActionResult List(int limit, int page, string lineName)
        {
            var stations = new LineService().GetLinesPage(limit, page, lineName);
            return Json(stations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public ActionResult DeleteSave(T_Line line)
        {
            var successResult = new LineService().DeleteSave(line);
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
        public ActionResult Submit(T_Line line)
        {
            var successResult = new LineService().Submit(line);
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }
    }
}