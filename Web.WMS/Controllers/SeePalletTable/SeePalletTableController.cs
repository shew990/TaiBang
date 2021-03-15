using BILBasic.Interface;
using BILBasic.JSONUtil;
using SqlSugarDAL;
using SqlSugarDAL.t_taskdetails;
using SqlSugarDAL.t_tasktrans;
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
            T_Interface_Func TIF = new T_Interface_Func();
            string json = "";
            List<Kanban> kanbansOrder = new List<Kanban>();
            if (OrderType == "0")//出货单看板
            {
                json = "{\"data_no\":\"" + houseId + "\",\"VoucherType\":\"6000\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                var kanbans = JSONHelper.JsonToObject<List<Kanban>>(ERPJson);

                var orderByEmergencyFlag = kanbans.FindAll(x => x.EmergencyFlag == "true")
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                var orderByBusinessDate = kanbans.FindAll(x => !IsToday(x.BusinessDate))
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                var others = kanbans.FindAll(x => x.EmergencyFlag != "true" && IsToday(x.BusinessDate))
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                kanbansOrder.AddRange(orderByEmergencyFlag);
                kanbansOrder.AddRange(orderByBusinessDate);
                kanbansOrder.AddRange(others);
                kanbansOrder.ForEach(x =>
                {
                    var details = new TaskDetailsService().GetList(y => y.ERPVOUCHERNO == x.DocNo);
                    var taskTran = new TasktransService()
                        .GetList(z => z.ERPVOUCHERNO == x.DocNo && z.TASKTYPE == 2).FirstOrDefault();
                    x.SHELVEQTY = details.Sum(a => a.SHELVEQTY);
                    x.CREATER = taskTran.CREATER;
                });
            }
            else if (OrderType == "1")//形态转换看板
            {
                json = "{\"data_no\":\"" + houseId + "\",\"VoucherType\":\"6001\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                var kanbans = JSONHelper.JsonToObject<List<Kanban>>(ERPJson);

                var orderByEmergencyFlag = kanbans.FindAll(x => x.EmergencyFlag == "true")
                    .OrderByDescending(x => x.BusinessDate);
                var orderByBusinessDate = kanbans.FindAll(x => !IsToday(x.BusinessDate))
                    .OrderByDescending(x => x.BusinessDate);
                var orderByStatus = kanbans.FindAll(x => x.Status == "Approved")
                    .OrderByDescending(x => x.BusinessDate);
                var others = kanbans.FindAll(x => x.EmergencyFlag != "true" && IsToday(x.BusinessDate)
                            && x.Status != "Approved").OrderByDescending(x => x.BusinessDate);
                kanbansOrder.AddRange(orderByEmergencyFlag);
                kanbansOrder.AddRange(orderByBusinessDate);
                kanbansOrder.AddRange(orderByStatus);
                kanbansOrder.AddRange(others);
            }
            else if (OrderType == "2")
            {

            }
            else
            {

            }
            var jsonReturn = new
            {
                Result = 1,
                ResultValue = (kanbansOrder == null || kanbansOrder.Count() == 0) ? "没有符合条件的数据" : "",
                Data = kanbansOrder.Skip(limit * (page - 1)).Take(limit).ToList(),
                PageData = new
                {
                    totalCount = kanbansOrder.Count(),
                    pageSize = limit,
                    currentPage = page,
                    totalPages = kanbansOrder.Count() % limit > 0
                    ? (Math.Floor(Convert.ToDouble(kanbansOrder.Count() / limit)) + 1)
                    : (kanbansOrder.Count() / limit)
                }
            };
            return Json(jsonReturn, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否当天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public Boolean IsToday(string dt)
        {
            var dtTime = Convert.ToDateTime(dt);
            DateTime today = DateTime.Today;
            DateTime tempToday = new DateTime(dtTime.Year, dtTime.Month, dtTime.Day);
            if (today == tempToday)
                return true;
            return false;
        }
    }
}