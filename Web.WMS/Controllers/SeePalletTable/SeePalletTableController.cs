using BILBasic.Interface;
using BILBasic.JSONUtil;
using SqlSugarDAL;
using SqlSugarDAL.wareHouse;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.SeePalletTable
{
    public class SeePalletTableController : Controller
    {
        /// <summary>
        /// 跳转主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(string houseNo = null, string orderType = null)
        {
            var houses = new WareHouseService().GetList();
            var refreshTime = Convert.ToInt32(ConfigurationManager.AppSettings["RefreshTime"]) * 1000;

            ViewBag.refreshTime = refreshTime;
            ViewBag.houseNo = houseNo == null ? "" : houseNo;
            ViewBag.orderType = orderType == null ? "" : orderType;
            return View(houses);
        }

        /// <summary>
        /// 表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int limit, int page, string houseId, string OrderType)
        {
            List<Kanban> kanbans = new List<Kanban>();
            T_Interface_Func TIF = new T_Interface_Func();
            string json = "";
            if (OrderType == "0")//发货看板
            {
                json = "{\"data_no\":\"" + houseId + "\",\"VoucherType\":\"6000\"}";
            }
            else if (OrderType == "1")//形态转换看板
            {
                json = "{\"data_no\":\"" + houseId + "\",\"VoucherType\":\"6001\"}";
            }
            else if (OrderType == "2")
            {

            }
            else
            {

            }
            string ERPJson = TIF.GetModelListByInterface(json);
            kanbans = JSONHelper.JsonToObject<List<Kanban>>(ERPJson);
            var jsonReturn = new
            {
                Result = 1,
                ResultValue = (kanbans == null || kanbans.Count() == 0) ? "没有符合条件的数据" : "",
                Data = kanbans.Skip(limit * (page - 1)).Take(limit).ToList(),
                PageData = new
                {
                    totalCount = kanbans.Count(),
                    pageSize = limit,
                    currentPage = page,
                    totalPages = kanbans.Count() % limit > 0
                    ? (Math.Floor(Convert.ToDouble(kanbans.Count() / limit)) + 1)
                    : (kanbans.Count() / limit)
                }
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }
    }
}