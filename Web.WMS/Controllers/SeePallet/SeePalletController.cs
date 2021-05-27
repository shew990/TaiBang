using BILWeb.Material;
using Newtonsoft.Json;
using SqlSugarDAL.compter;
using SqlSugarDAL.station;
using SqlSugarDAL.Until;
using SqlSugarDAL.view_product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.WMS.Controllers.SeePallet
{
    public class SeePalletController : Controller
    {
        /// <summary>
        /// 返回主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string result = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
                result = Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(result))
                result = Request.UserHostAddress;
            ViewBag.ipAddress = result;
            return View();
        }

        /// <summary>
        /// 触发 获取pdf文件地址 操作
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public ActionResult PdfInit(string ErpVoucherNo, string ipAddress)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                var productOrder = new View_ProductService().GetProduct(ErpVoucherNo);
                if (productOrder == null)
                {
                    successResult.Msg = "没有该生产订单!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }

                //根据生产订单获取SOP地址
                var moReport = new T_Material_Batch_DB().GetSopList(ErpVoucherNo).FirstOrDefault();
                if (moReport == null)
                {
                    successResult.Msg = "该erp单号没有pdf地址数据,请核实!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }

                View_CompterService compterService = new View_CompterService();
                var compter = compterService.GetCompter(ipAddress);
                if (compter == null)
                {
                    successResult.Msg = "该电脑不能使用该功能,请联系管理员添加!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }
                var stations = new View_StationService().GetList(x => x.LineId == compter.LineId);
                var objStation = stations
                    .Find(x => x.Id == compter.StationId && x.OrderNo == ErpVoucherNo);
                string pdfAddress = "";
                //该工位&&该订单号&&pdf地址 不为空=>不做更新
                if (objStation != null && !string.IsNullOrEmpty(objStation.PdfAddress))
                {
                    pdfAddress = objStation.PdfAddress;
                }
                else//做更新
                {
                    var count = stations.FindAll(x => x.OrderNo == ErpVoucherNo
                    && !string.IsNullOrEmpty(x.PdfAddress) && x.Id != compter.StationId).Count();
                    pdfAddress = count == 0 ? moReport.Sop1 : count == 1 ? moReport.Sop2
                        : count == 2 ? moReport.Sop3 : count == 3 ? moReport.Sop4 : count == 4
                        ? moReport.Sop5 : moReport.Sop6;
                    var station = new StationService()
                        .GetList(x => x.Id == compter.StationId).FirstOrDefault();
                    station.OrderNo = ErpVoucherNo;
                    station.PdfAddress = pdfAddress;
                    new StationService().Update(station);
                }
                successResult.Data = new { productOrder = productOrder, pdfAddress = pdfAddress };
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }


    }
}