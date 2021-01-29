using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.SyncService;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "InStock/InStock")]
    public class InStockController : BaseController<T_InStockInfo>
    {

        private IInStockService inStockService;
        public InStockController()
        {
            inStockService = (IInStockService)ServiceFactory.CreateObject("InStock.T_InStock_Func");
            baseservice = inStockService;
        }

        T_InStock_Func tfunc = new T_InStock_Func();
        T_InStockDetail_Func tfunc_detail = new T_InStockDetail_Func();

        //public ActionResult GetModelList(DividPage page, T_InStockInfo model)
        //{
        //    List<T_InStockInfo> modelList = new List<T_InStockInfo>();
        //    string strError = "";
        //    tfunc.GetModelListByPage(ref modelList, Common.Commom.currentUser, model, ref page, ref strError);
        //    ViewData["PageData"] = new PageData<T_InStockInfo> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/OutStock/GetModelList") };
        //    return View("InStockList", model);
        //}

        public JsonResult GetDetail(Int32 ID)
        {
            List<T_InStockDetailInfo> modelList = new List<T_InStockDetailInfo>();
            string strError = "";
            //tfunc.GetModelListByHeaderID(ref modelList,ID,ref strError);
            tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Sync(string ErpVoucherNo)
        {
            string strMsg = "";
            ParamaterFiled_DB PDB = new ParamaterFiled_DB();
            if (PDB.GetVoucherNo(ErpVoucherNo, ref strMsg, "1"))
            {
                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
            }

            ////if (ErpVoucherNo.Length < 10)
            ////{
            ////    return Json(new { state = false, obj = "ERP单据长度不能少于10位！" }, JsonRequestBehavior.AllowGet);
            ////}
            //string ErrorMsg = ""; int WmsVoucherType = 32; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            //BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            ////10:入库单据
            //if (PFunc.Sync(10, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            //{

            //    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            //}
        }

        //关闭单据
        [HttpPost]
        public JsonResult CloseInstock(string ID)
        {
            try
            {
                string strError = "";
                T_InStock_Func tfunc = new T_InStock_Func();
                if (tfunc.CloseInStockVoucherNo(Convert.ToInt32(ID), currentUser, ref strError))
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

        //关闭单据
        [HttpPost]
        public JsonResult OpenInstock(string ID)
        {
            try
            {
                string strError = "";
                T_InStock_Func tfunc = new T_InStock_Func();
                if (tfunc.OpenInStockVoucherNo(Convert.ToInt32(ID), currentUser, ref strError))
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


        //关闭单据
        [HttpGet]
        public JsonResult DeleteNo(string ErpVoucherno, string flag)
        {
            string strError = "";
            List<string> list = new List<string>();
            try
            {
                T_InStock_Func inStockFunc = new T_InStock_Func();
                if (inStockFunc.DeleteNo(ErpVoucherno, flag, ref strError))
                {
                    return Json(new { state = true, obj = "删除成功！" }, JsonRequestBehavior.AllowGet);
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

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public ActionResult OutExcel(string jsonString)
        {
            T_InStockInfo model = JsonConvert.DeserializeObject<T_InStockInfo>(jsonString);

            string strErrMsg = string.Empty;
            List<T_InStockInfo> lstExport = new List<T_InStockInfo>();
            DividPage page = new DividPage { CurrentPageShowCounts = 10000000 };
            tfunc.GetModelListByPage(ref lstExport, currentUser, model, ref page, ref strErrMsg);

            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet();
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("单据号");
            row1.CreateCell(1).SetCellValue("据点编号");
            row1.CreateCell(2).SetCellValue("据点名");
            row1.CreateCell(3).SetCellValue("ERP单号");
            row1.CreateCell(4).SetCellValue("单据类型");
            row1.CreateCell(5).SetCellValue("状态");
            row1.CreateCell(6).SetCellValue("仓库");
            row1.CreateCell(7).SetCellValue("创建时间");
            row1.CreateCell(8).SetCellValue("供应商");
            for (int i = 0; i < lstExport.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);//给sheet添加一行
                row.CreateCell(0).SetCellValue(lstExport[i].VoucherNo);
                row.CreateCell(1).SetCellValue(lstExport[i].StrongHoldCode);
                row.CreateCell(2).SetCellValue(lstExport[i].StrongHoldName);
                row.CreateCell(3).SetCellValue(lstExport[i].ErpVoucherNo);
                row.CreateCell(4).SetCellValue(lstExport[i].StrVoucherType);
                row.CreateCell(5).SetCellValue(lstExport[i].StrStatus);
                row.CreateCell(6).SetCellValue(lstExport[i].FromErpWareHouse);
                row.CreateCell(7).SetCellValue(lstExport[i].CreateTime.ToString());
                row.CreateCell(8).SetCellValue(lstExport[i].SupplierName);
            }
            string fileName = "入库单据列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream memoryStream = new MemoryStream();
            book.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.ms-excel", fileName);
        }

    }
}