using BILWeb.Material;
using Newtonsoft.Json;
using SqlSugarDAL.compter;
using SqlSugarDAL.station;
using SqlSugarDAL.Until;
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
            return View();
        }

        public ActionResult PdfInit(string ErpVoucherNo, string ipAddress)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                StationService stationService = new StationService();
                CompterService compterService = new CompterService();
                //根据生产订单获取SOP地址
                //string ErpVoucherNo = "MO04012012090002";
                var moReport = new T_Material_Batch_DB().GetSopList(ErpVoucherNo).FirstOrDefault();
                var isFirstStation = compterService.IsFirstStation(ipAddress);
                if (isFirstStation)
                {
                    var stations = stationService.GetStations();
                    stations[0].PDFAddress = moReport.Sop1;
                    stations[1].PDFAddress = moReport.Sop2;
                    stations[2].PDFAddress = moReport.Sop3;
                    stations[3].PDFAddress = moReport.Sop4;
                    stations[4].PDFAddress = moReport.Sop5;
                    stations[5].PDFAddress = moReport.Sop6;
                    stationService.UpdateRange(stations);
                }
                else
                {
                    var compter = compterService.GetCompter(ipAddress);
                    var station = stationService.GetById(compter.StationId);
                    stationService.Update(station);
                }
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