using BILBasic.Common;
using BILWeb.Query;
using BILWeb.Stock;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Models;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Query
{
    [RoleActionFilter(Message = "Query/StockCombine")]
    public class StockCombineController : Controller
    {
        Query_DB queryDB = new Query_DB();
        // GET: StockCombine
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PageView(PageData<TaskTrans_Model> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        /// <summary>
        /// 库存合并
        /// </summary>
        /// <param name="page"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult GetModelList(DividPage page, T_StockInfoEX model)
        {
            List<T_StockInfoEX> list = new List<T_StockInfoEX>();
            string str = "";
            queryDB.GetStockCombineInfo(model, ref page, ref list, ref str);
            ViewData["PageData"] = new PageData<T_StockInfoEX> { data = list, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/StockCombine/GetModelList") };
            return View("GetModelList", model);
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetData()
        {
            Stream stream = Request.InputStream;
            string json = string.Empty;
            if (stream.Length != 0)
            {
                StreamReader streamreader = new StreamReader(stream);
                json = streamreader.ReadToEnd();
            }
            var pageRequest = JsonConvert.DeserializeObject<PageRequest<T_StockInfoEX>>(json);

            List<T_StockInfoEX> modelList = new List<T_StockInfoEX>();
            DividPage page = new DividPage
            {
                CurrentPageRecordCounts = pageRequest.CurrentPageRecordCounts,
                CurrentPageShowCounts = pageRequest.CurrentPageShowCounts,
                PagesCount = pageRequest.PagesCount,
                RecordCounts = pageRequest.RecordCounts
            };
            T_StockInfoEX model = pageRequest.model;
            string strError = "";
            queryDB.GetStockCombineInfo(model, ref page, ref modelList, ref strError);
            BaseModel<List<T_StockInfoEX>> returnmodel = new BaseModel<List<T_StockInfoEX>>()
            {
                Result = 1,
                ResultValue = strError,
                Data = modelList,
                PageData = new R_Pagedata()
                {
                    totalCount = page.RecordCounts,
                    pageSize = page.CurrentPageShowCounts,
                    currentPage = page.CurrentPageNumber,
                    totalPages = page.PagesCount
                }
            };
            return Json(returnmodel, JsonRequestBehavior.AllowGet);
        }

        public FileResult Excel(string jsonString)
        {
            T_StockInfoEX model = JsonConvert.DeserializeObject<T_StockInfoEX>(jsonString);
            DividPage page = new DividPage();
            page.CurrentPageShowCounts = 1000000;
            List<T_StockInfoEX> list = new List<T_StockInfoEX>();
            string str = "";
            queryDB.GetStockCombineInfo(model, ref page, ref list, ref str);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            //row1.CreateCell(0).SetCellValue("序号");
            //row1.CreateCell(1).SetCellValue("据点编号");
            row1.CreateCell(0).SetCellValue("物料号");
            row1.CreateCell(1).SetCellValue("物料规格");
            row1.CreateCell(2).SetCellValue("批次");
            row1.CreateCell(3).SetCellValue("到期日期");
            row1.CreateCell(4).SetCellValue("数量");
            //row1.CreateCell(9).SetCellValue("检验状态");
            row1.CreateCell(5).SetCellValue("库位");
            row1.CreateCell(6).SetCellValue("库区");
            row1.CreateCell(7).SetCellValue("仓库");
            row1.CreateCell(8).SetCellValue("EAN");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < list.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                //rowtemp.CreateCell(0).SetCellValue(list[i].XH);
                //rowtemp.CreateCell(1).SetCellValue(list[i].StrongHoldCode);
                rowtemp.CreateCell(0).SetCellValue(list[i].MaterialNo);
                rowtemp.CreateCell(1).SetCellValue(list[i].Spec);
                rowtemp.CreateCell(2).SetCellValue(list[i].BatchNo);
                rowtemp.CreateCell(3).SetCellValue(list[i].EDate.ToString());
                rowtemp.CreateCell(4).SetCellValue(list[i].Qty.ToString());
                //rowtemp.CreateCell(7).SetCellValue( list[i].StatusName==null?"": list[i].StatusName);
                rowtemp.CreateCell(5).SetCellValue(list[i].AreaNo.ToString());
                rowtemp.CreateCell(6).SetCellValue(list[i].HouseNo.ToString());
                rowtemp.CreateCell(7).SetCellValue(list[i].WarehouseNo.ToString());
                rowtemp.CreateCell(8).SetCellValue(list[i].EAN.ToString());
            }
            string fileName = "库存合并列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream memoryStream = new MemoryStream();
            book.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return File(memoryStream, "application/vnd.ms-excel", fileName);
        }
    }
}