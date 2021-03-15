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
            if (OrderType == "0")//出货单看板
            {
                json = "{\"data_no\":\"" + houseNo + "\",\"VoucherType\":\"6000\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("---------------------调用ERP接口:cesh回传：" + ERPJson;
                var returnKanban = JSONHelper.JsonToObject<ReturnKanban>(ERPJson);
                var kanbans = returnKanban.data;

                //var jsonString = "[{\"BusinessDate\":\"2020-11-22 00:0:00\",\"DocNo\":\"SM201122019\",\"Customer_Code\":\"9907.0143\",\"TransportModeName\":\"优速快递\",\"TransportModeCode\":\"01\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"钟睿蝶\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123012\",\"Customer_Code\":\"3303.0385\",\"TransportModeName\":\"货运-送货上门\",\"TransportModeCode\":\"02\",\"Qty\":30.00,\"TransferType\":null,\"BussinessMan\":\"钟睿蝶\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123016\",\"Customer_Code\":\"3303.0394\",\"TransportModeName\":\"优速快递\",\"TransportModeCode\":\"01\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"陈浩宇\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123047\",\"Customer_Code\":\"3303.0385\",\"TransportModeName\":\"货运-送货上门\",\"TransportModeCode\":\"02\",\"Qty\":7.00,\"TransferType\":null,\"BussinessMan\":\"钟睿蝶\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123061\",\"Customer_Code\":\"3508.0006\",\"TransportModeName\":\"货运-送货上门\",\"TransportModeCode\":\"02\",\"Qty\":5.00,\"TransferType\":null,\"BussinessMan\":\"连珍梅\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123068\",\"Customer_Code\":\"1309.0057\",\"TransportModeName\":\"货运-送货上门\",\"TransportModeCode\":\"02\",\"Qty\":50.00,\"TransferType\":null,\"BussinessMan\":\"高兴烨\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123073\",\"Customer_Code\":\"3706.0006\",\"TransportModeName\":\"货运-自提\",\"TransportModeCode\":\"02\",\"Qty\":5.00,\"TransferType\":null,\"BussinessMan\":\"张鹏\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123074\",\"Customer_Code\":\"3100.0337\",\"TransportModeName\":\"货运-送货上门\",\"TransportModeCode\":\"02\",\"Qty\":20.00,\"TransferType\":null,\"BussinessMan\":\"张敏\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-11-23 00:0:00\",\"DocNo\":\"SM201123089\",\"Customer_Code\":\"4113.0013\",\"TransportModeName\":\"优速快递\",\"TransportModeCode\":\"01\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"邓雪薇\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-21 00:0:00\",\"DocNo\":\"SM201221003\",\"Customer_Code\":\"1100.0001\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":11.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-21 00:0:00\",\"DocNo\":\"SM201221004\",\"Customer_Code\":\"1100.0001\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":11.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-22 00:0:00\",\"DocNo\":\"SM201222001\",\"Customer_Code\":\"1100.0001\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":4.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-22 00:0:00\",\"DocNo\":\"SM201222004\",\"Customer_Code\":\"1100.0001\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":6.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-22 00:0:00\",\"DocNo\":\"SM201222005\",\"Customer_Code\":\"1100.0001\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-26 00:0:00\",\"DocNo\":\"SM201226003\",\"Customer_Code\":\"3205.0077\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":13.00,\"TransferType\":null,\"BussinessMan\":\"陈春\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227008\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227009\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227010\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227011\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227012\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227013\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-27 00:0:00\",\"DocNo\":\"SM201227014\",\"Customer_Code\":\"3100.0101\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-29 00:0:00\",\"DocNo\":\"SM201229001\",\"Customer_Code\":\"4406.0105\",\"TransportModeName\":\"快递\",\"TransportModeCode\":\"01\",\"Qty\":20.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-30 00:0:00\",\"DocNo\":\"SM201230001\",\"Customer_Code\":\"4406.0105\",\"TransportModeName\":\"快递\",\"TransportModeCode\":\"01\",\"Qty\":20.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-01-11 00:0:00\",\"DocNo\":\"SM210111002\",\"Customer_Code\":\"3205.0077\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-01-12 00:0:00\",\"DocNo\":\"SM210112003\",\"Customer_Code\":\"3205.0077\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-01-13 00:0:00\",\"DocNo\":\"SM210113001\",\"Customer_Code\":\"1100.0015\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":12.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-01-18 00:0:00\",\"DocNo\":\"SM210118001\",\"Customer_Code\":\"3302.0006\",\"TransportModeName\":\"送货上门\",\"TransportModeCode\":\"03\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-23 00:0:00\",\"DocNo\":\"SM210223001\",\"Customer_Code\":\"4419.0042\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-23 00:0:00\",\"DocNo\":\"SM210223002\",\"Customer_Code\":\"4419.0042\",\"TransportModeName\":\"客户自提\",\"TransportModeCode\":\"99\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-25 00:0:00\",\"DocNo\":\"SM210225001\",\"Customer_Code\":\"4419.0042\",\"TransportModeName\":\"德邦物流\",\"TransportModeCode\":\"01\",\"Qty\":11.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-25 00:0:00\",\"DocNo\":\"SM210225002\",\"Customer_Code\":\"4419.0042\",\"TransportModeName\":\"德邦物流\",\"TransportModeCode\":\"01\",\"Qty\":11.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-25 00:0:00\",\"DocNo\":\"SM210225003\",\"Customer_Code\":\"4419.0042\",\"TransportModeName\":\"德邦物流\",\"TransportModeCode\":\"01\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-03-03 00:0:00\",\"DocNo\":\"SM210303001\",\"Customer_Code\":\"4419.0013\",\"TransportModeName\":\"快递\",\"TransportModeCode\":\"01\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"甘慧敏\",\"EmergencyFlag\":\"False\",\"Status\":null},{\"BusinessDate\":\"2020-12-12 00:0:00\",\"DocNo\":\"DCY-20120003\",\"Customer_Code\":\"成品仓(大电机)\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":20.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-14 00:0:00\",\"DocNo\":\"DCY-20120004\",\"Customer_Code\":\"成品仓(大电机)\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":1.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-24 00:0:00\",\"DocNo\":\"DCY-20120032\",\"Customer_Code\":\"成品仓(大电机)\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-24 00:0:00\",\"DocNo\":\"DCY-20120033\",\"Customer_Code\":\"成品仓(大电机)\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":2.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-25 00:0:00\",\"DocNo\":\"DCY-20120036\",\"Customer_Code\":\"成品仓(大电机)\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":8.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2020-12-29 00:0:00\",\"DocNo\":\"DCY-20120061\",\"Customer_Code\":\"东莞成品仓库\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":10.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null},{\"BusinessDate\":\"2021-02-25 00:0:00\",\"DocNo\":\"DCY-21020007\",\"Customer_Code\":\"苏州成品仓库\",\"TransportModeName\":\"\",\"TransportModeCode\":\"\",\"Qty\":3.00,\"TransferType\":null,\"BussinessMan\":\"\",\"EmergencyFlag\":\"\",\"Status\":null}]";
                //var kanbans = JsonConvert.DeserializeObject<List<Kanban>>(jsonString);
                var orderByEmergencyFlag = kanbans.FindAll(x => x.EmergencyFlag == "true")
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                var orderByBusinessDate = kanbans.FindAll(x => !IsToday(x.BusinessDate))
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                var others = kanbans.FindAll(x => x.EmergencyFlag != "true" && IsToday(x.BusinessDate))
                    .OrderByDescending(x => x.BusinessDate).ThenBy(x => x.TransportModeCode);
                kanbansOrder.AddRange(orderByEmergencyFlag);
                kanbansOrder.AddRange(orderByBusinessDate);
                kanbansOrder.AddRange(others);
            }
            else if (OrderType == "1")//形态转换看板
            {
                json = "{\"data_no\":\"" + houseNo + "\",\"VoucherType\":\"6001\"}";
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

            //发货看板赋值
            var datas = kanbansOrder.Skip(limit * (page - 1)).Take(limit).ToList();
            if (OrderType == "0")
            {
                datas.ForEach(x =>
                {
                    var details = new TaskDetailsService().GetList(y => y.ERPVOUCHERNO == x.DocNo);
                    var taskTran = new TasktransService()
                        .GetList(z => z.ERPVOUCHERNO == x.DocNo && z.TASKTYPE == 2).FirstOrDefault();
                    x.SHELVEQTY = details.Sum(a => a.SHELVEQTY);
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
                    totalCount = datas.Count(),
                    pageSize = limit,
                    currentPage = page,
                    totalPages = datas.Count() % limit > 0
                    ? (Math.Floor(Convert.ToDouble(datas.Count() / limit)) + 1)
                    : (datas.Count() / limit)
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