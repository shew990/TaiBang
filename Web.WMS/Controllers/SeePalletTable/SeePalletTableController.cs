using BILBasic.Interface;
using BILBasic.JSONUtil;
using Newtonsoft.Json;
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
            ViewBag.houseNo = houseNo == null ? "" : houseNo.ToString();
            ViewBag.orderType = orderType == null ? "" : orderType.ToString();
            return View(houses);
        }

        /// <summary>
        /// 表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult List(int limit, int page, string houseNo, string OrderType)
        {
            T_Interface_Func TIF = new T_Interface_Func();
            string json = "";
            List<Kanban> kanbansOrder = new List<Kanban>();
            var orderByEmergencyFlag = new List<Kanban>();
            if (OrderType == "0")//出货单看板
            {
                json = "{\"data_no\":\"" + houseNo + "\",\"VoucherType\":\"6000\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                var returnKanban = JSONHelper.JsonToObject<ReturnKanban>(ERPJson);
                var kanbans = returnKanban.data == null ? new List<Kanban>() : returnKanban.data;

                LogNet.LogInfo("---------------------调用ERP接口:返回出货单看板参数：" + kanbans);

                orderByEmergencyFlag = kanbans.FindAll(x => x.EmergencyFlag == "True")
                    .OrderBy(x => x.BusinessDate).ThenBy(x => x.TransportModeCode).ToList();
                orderByEmergencyFlag.ForEach(x => x.BackColor = "red");
                var orderByBusinessDate = kanbans.FindAll(x => !IsToday(x.BusinessDate)
                    && Convert.ToDateTime(x.BusinessDate) < DateTime.Now
                    && x.EmergencyFlag != "True").OrderBy(x => x.BusinessDate)
                    .ThenBy(x => x.TransportModeCode).ToList();
                orderByBusinessDate.ForEach(x => x.BackColor = "yellow");
                var others = kanbans.FindAll(x => x.EmergencyFlag != "True"
                && (IsToday(x.BusinessDate) || Convert.ToDateTime(x.BusinessDate) > DateTime.Now))
                    .OrderBy(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                kanbansOrder.AddRange(orderByEmergencyFlag);
                kanbansOrder.AddRange(orderByBusinessDate);
                kanbansOrder.AddRange(others);
            }
            else if (OrderType == "1")//形态转换看板
            {
                json = "{\"data_no\":\"" + houseNo + "\",\"VoucherType\":\"6001\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                var returnKanban = JSONHelper.JsonToObject<ReturnKanban>(ERPJson);
                var kanbans = returnKanban.data;
                if (kanbans == null)
                    kanbans = new List<Kanban>();

                LogNet.LogInfo("---------------------调用ERP接口:返回形态看板参数：" + kanbans);

                orderByEmergencyFlag = kanbans.FindAll(x => x.EmergencyFlag == "True"
                    && x.Status != "Approved").OrderBy(x => x.BusinessDate).ToList();
                orderByEmergencyFlag.ForEach(x => x.BackColor = "red");
                var orderByBusinessDate = kanbans.FindAll(x => !IsToday(x.BusinessDate)
                     && x.Status != "Approved" && Convert.ToDateTime(x.BusinessDate) < DateTime.Now
                    && x.EmergencyFlag != "True").OrderBy(x => x.BusinessDate).ToList();
                orderByBusinessDate.ForEach(x => x.BackColor = "yellow");
                var orderByStatus = kanbans.FindAll(x => x.Status == "Approved")
                    .OrderBy(x => x.BusinessDate).ToList();
                orderByStatus.ForEach(x => x.BackColor = "blue");
                var others = kanbans.FindAll(x => x.EmergencyFlag != "True"
                && (IsToday(x.BusinessDate) || Convert.ToDateTime(x.BusinessDate) > DateTime.Now)
                            && x.Status != "Approved").OrderBy(x => x.BusinessDate);
                kanbansOrder.AddRange(orderByEmergencyFlag);
                kanbansOrder.AddRange(orderByBusinessDate);
                kanbansOrder.AddRange(others);
                kanbansOrder.AddRange(orderByStatus);
            }
            else if (OrderType == "2")
            {

            }
            else
            {

            }

            //发货看板赋值
            var datas = new List<Kanban>();
            if (orderByEmergencyFlag.Count() >= limit || page == 1)
            {
                datas = kanbansOrder.Take(limit).ToList();
            }
            else
            {
                var pageDatas = kanbansOrder.Skip(limit * (page - 1)).Take(limit).ToList();
                datas.AddRange(orderByEmergencyFlag);
                datas.AddRange(pageDatas.Take(limit - orderByEmergencyFlag.Count()));
            }
            if (OrderType == "0")
            {
                datas.ForEach(x =>
                {
                    var details = new TaskDetailsService().GetList(y => y.ERPVOUCHERNO == x.DocNo);
                    var taskTran = new TasktransService()
                        .GetList(z => z.ERPVOUCHERNO == x.DocNo && z.TASKTYPE == 2).FirstOrDefault();
                    x.SHELVEQTY = details.Sum(a => a.UNSHELVEQTY);
                    x.CREATER = taskTran == null ? "" : taskTran.CREATER;
                });
            }

            var jsonReturn = new
            {
                Result = 1,
                ResultValue = (datas == null || datas.Count() == 0) ? "没有符合条件的数据" : "",
                Data = datas,
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