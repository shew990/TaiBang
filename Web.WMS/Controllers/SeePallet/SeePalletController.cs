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
            //根据生产订单获取SOP地址
            string ErpVoucherNo = "MO04012012090002";
            T_Material_Batch_DB db = new T_Material_Batch_DB();
            List<MoReport> MoReports = db.GetSopList(ErpVoucherNo);
       

            return View();
        }


    }
}