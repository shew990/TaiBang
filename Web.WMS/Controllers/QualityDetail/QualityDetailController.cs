using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.Quality;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "QualityDetail/QualityDetail")]
    public class QualityDetailController : BaseController<T_QualityDetailInfo>
    {

        private IQualityDetailService qualityDetailService;
        public QualityDetailController()
        {
            qualityDetailService = (IQualityDetailService)ServiceFactory.CreateObject("Quality.T_QualityDetail_Func");
            baseservice = qualityDetailService;
        }

        T_InStockDetail_Func tfunc_detail = new T_InStockDetail_Func();


        public JsonResult Sync(string ErpVoucherNo)
        {
            string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            //10:入库单据
            if (PFunc.Sync(10, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
           
        }

        //关闭单据
        [HttpPost]
        public JsonResult CloseQuality(string ID)
        {
            try
            {
                string strError = "";
                T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
                if (tfunc.CloseQualityDetailVoucherNo(Convert.ToInt32(ID), currentUser, ref strError))
                {
                    return Json(new { state = true, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}