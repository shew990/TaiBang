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
                //string ErpVoucherNo = "MO04012012090002";
                var moReport = new T_Material_Batch_DB().GetSopList(ErpVoucherNo).FirstOrDefault();
                if (moReport == null)
                {
                    successResult.Msg = "没有pdf地址数据,请联系管理员!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }

                View_CompterService compterService = new View_CompterService();
                var index = compterService.GetStationIndex(ipAddress);
                if (index == -1)
                {
                    successResult.Msg = "该电脑不能使用该功能,请联系管理员添加!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }
                //var stations = new StationService().GetStations();
                //string pdfAddress = index == 0 ? moReport.Sop1 : index == 1
                //        ? moReport.Sop2 : index == 2 ? moReport.Sop3 : index == 3
                //        ? moReport.Sop4 : index == 4 ? moReport.Sop5 : moReport.Sop6;
                //if (index == 0)//工位1
                //{
                //    stations[0].PDFAddress = moReport.Sop1;
                //    stations[1].PDFAddress = moReport.Sop2;
                //    stations[2].PDFAddress = moReport.Sop3;
                //    stations[3].PDFAddress = moReport.Sop4;
                //    stations[4].PDFAddress = moReport.Sop5;
                //    stations[5].PDFAddress = moReport.Sop6;
                //    stationService.UpdateRange(stations);
                //}
                //else//其他工位
                //{
                //    var station = stations.Find(x => x.Id == stations[index].Id);
                //    station.PDFAddress = pdfAddress;
                //    stationService.Update(station);
                //}

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