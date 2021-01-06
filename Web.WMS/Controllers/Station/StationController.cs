using SqlSugarDAL.line;
using SqlSugarDAL.station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.Station
{
    public class StationController : Controller
    {
        /// <summary>
        /// 跳转主页
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
        public ActionResult List(int limit, int page, string lineName, string stationName, string ipAddress)
        {
            var stations = new View_StationService()
                          .GetStationsPage(limit, page, lineName, stationName, ipAddress);
            return Json(stations, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            var lines = new LineService().GetList();
            return View(lines);
        }

        /// <summary>
        /// 提交（新增/编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit(View_Station station)
        {
            var successResult = new View_StationService().Submit(station);
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

    }
}