using BILWeb.Material;
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

        public ActionResult PdfInit(string ErpVoucherNo)
        {
            //根据生产订单获取SOP地址
            //string ErpVoucherNo = "MO04012012090002";
            T_Material_Batch_DB db = new T_Material_Batch_DB();
            List<MoReport> moReports = db.GetSopList(ErpVoucherNo);
            return Json(moReports, JsonRequestBehavior.AllowGet);
        }


    }
}