using BILBasic.Common;
using BILWeb.Query;
using BILWeb.Stock;
using Newtonsoft.Json;
using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using Web.WMS.Common;
using Web.WMS.Models;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Query
{
    [RoleActionFilter(Message = "Query/StockDetail")]
    public class StockDetailController : Controller
    {
        Query_DB queryDB = new Query_DB();

        public ActionResult PageView(PageData<T_StockInfoEX> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, T_StockInfoEX model)
        {
            List<T_StockInfoEX> modelList = new List<T_StockInfoEX>();
            string strError = "";
            queryDB.GetStockDetInfo(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<T_StockInfoEX> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/StockDetail/GetModelList") };
            return View("GetModelList", model);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult GetData()
        {
            Stream stream = Request.InputStream;
            string json = string.Empty;
            if (stream.Length != 0)
            {
                StreamReader streamreader = new StreamReader(stream);
                json = streamreader.ReadToEnd();
            }
            PageRequest<T_StockInfoEX> pageRequest = JsonConvert.DeserializeObject<PageRequest<T_StockInfoEX>>(json);

            List<T_StockInfoEX> modelList = new List<T_StockInfoEX>();
            DividPage page = new DividPage { CurrentPageRecordCounts = pageRequest.CurrentPageRecordCounts, CurrentPageShowCounts = pageRequest.CurrentPageShowCounts, PagesCount = pageRequest.PagesCount, RecordCounts = pageRequest.RecordCounts,CurrentPageNumber=pageRequest.CurrentPageNumber };
            T_StockInfoEX model = pageRequest.model;
            string strError = "";
            queryDB.GetStockDetInfo(model, ref page, ref modelList, ref strError);
            BaseModel<List<T_StockInfoEX>> returnmodel = new BaseModel<List<T_StockInfoEX>>() { Result = 1, ResultValue = strError, Data = modelList, PageData = new R_Pagedata() { totalCount = page.RecordCounts, pageSize = page.CurrentPageShowCounts, currentPage = page.CurrentPageNumber, totalPages = page.PagesCount } };
            return Json(returnmodel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转表单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Form()
        {
            return View();
        }

        /// <summary>
        /// 拆分打印
        /// </summary>
        /// <returns></returns>
        public ActionResult SplitStamp(string serialno, decimal qty)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            string strErrMsg = "";
            DateTime time = DateTime.Now;
            if (!queryDB.Chai(serialno, qty, Commom.ReadUserInfo(), ref strErrMsg, ref time))
            {
                successResult.Msg = strErrMsg;
                return Json(successResult,JsonRequestBehavior.AllowGet);
            }

            successResult.Data = time.ToString();
            successResult.Success = true;
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public FileResult Excel(string jsonString)
        {
            T_StockInfoEX model = JsonConvert.DeserializeObject<T_StockInfoEX>(jsonString);

            string strErrMsg = string.Empty;
            List<T_StockInfoEX> lstExport = new List<T_StockInfoEX>();
            string strError = "";
            DividPage page = new DividPage
            {
                CurrentPageShowCounts = 1000000
            };
            queryDB.GetStockDetInfo(model, ref page, ref lstExport, ref strError);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("据点");
            row1.CreateCell(1).SetCellValue("物料号");
            row1.CreateCell(2).SetCellValue("物料名称");
            row1.CreateCell(3).SetCellValue("EAN");
            row1.CreateCell(4).SetCellValue("效期");
            row1.CreateCell(5).SetCellValue("序列号");
            row1.CreateCell(6).SetCellValue("批次");
            row1.CreateCell(7).SetCellValue("仓库");
            row1.CreateCell(8).SetCellValue("库区");
            row1.CreateCell(9).SetCellValue("库位");
            row1.CreateCell(10).SetCellValue("数量");
            row1.CreateCell(11).SetCellValue("项目跟踪号");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);

                rowtemp.CreateCell(0).SetCellValue(lstExport[i].StrongHoldCode == null ? "" : lstExport[i].StrongHoldCode.ToString());
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].MaterialNo == null ? "" : lstExport[i].MaterialNo.ToString());
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].MaterialDesc == null ? "" : lstExport[i].MaterialDesc.ToString());
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].EAN == null ? "" : lstExport[i].EAN.ToString());
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].EDate == null ? "" : lstExport[i].EDate.ToString());
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].SerialNo == null ? "" : lstExport[i].SerialNo.ToString());
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].BatchNo == null ? "" : lstExport[i].BatchNo.ToString());
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].WarehouseNo == null ? "" : lstExport[i].WarehouseNo.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].HouseNo == null ? "" : lstExport[i].HouseNo.ToString());
                rowtemp.CreateCell(9).SetCellValue(lstExport[i].AreaNo == null ? "" : lstExport[i].AreaNo.ToString());
                rowtemp.CreateCell(10).SetCellValue(lstExport[i].Qty.ToString());
                rowtemp.CreateCell(11).SetCellValue(lstExport[i].TracNo == null ? "" : lstExport[i].TracNo.ToString());
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

    }
}



    