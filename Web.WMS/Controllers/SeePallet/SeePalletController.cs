using BILWeb.Material;
using Newtonsoft.Json;
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

                StationService stationService = new StationService();
                //根据生产订单获取SOP地址
                //string ErpVoucherNo = "MO04012012090002";
                var moReport = new T_Material_Batch_DB().GetSopList(ErpVoucherNo).FirstOrDefault();
                if (moReport == null)
                {
                    successResult.Msg = "没有pdf地址数据,请联系管理员!";
                    return Json(successResult, JsonRequestBehavior.AllowGet);
                }
                var index = stationService.GetStationIndex(ipAddress);
                var stations = stationService.GetStations();
                string pdaAddress = index == 0 ? moReport.Sop1 : index == 1
                        ? moReport.Sop2 : index == 2 ? moReport.Sop3 : index == 3
                        ? moReport.Sop4 : index == 4 ? moReport.Sop5 : moReport.Sop6;
                if (index == 0)//工位1
                {
                    stations[0].PDFAddress = moReport.Sop1;
                    stations[1].PDFAddress = moReport.Sop2;
                    stations[2].PDFAddress = moReport.Sop3;
                    stations[3].PDFAddress = moReport.Sop4;
                    stations[4].PDFAddress = moReport.Sop5;
                    stations[5].PDFAddress = moReport.Sop6;
                    stationService.UpdateRange(stations);
                }
                else//其他工位
                {
                    var station = stations.Find(x => x.Id == stations[index].Id);
                    station.PDFAddress = pdaAddress;
                    stationService.Update(station);
                }

                successResult.Data = new { productOrder = productOrder, pdfAddress = pdaAddress };
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