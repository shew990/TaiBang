using BILBasic.Common;
using BILWeb.Check;
using BILWeb.Login.User;
using BILWeb.Material;
using BILWeb.Query;
using BILWeb.Stock;
using BILWeb.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "Check/Check")]
    public class CheckController : Controller
    {
        UserInfo currentUser = Common.Commom.ReadUserInfo();
        Check_DB tfun = new Check_DB();

        public ActionResult PageView(PageData<Check_Model> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public ActionResult GetModelList(DividPage page, Check_Model model)
        {
            List<Check_Model> modelList = new List<Check_Model>();
            string strError = "";
            tfun.GetCheckInfo(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<Check_Model> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/GetModelList") };
            return View("GetModelList", model);
        }


        public FileResult Excel(CheckAnalyze model)
        {
            string strErrMsg = string.Empty;
            List<CheckAnalyze> lstExport = new List<CheckAnalyze>();
            string strError = "";
            DividPage page = new DividPage
            {
                CurrentPageShowCounts = 10000000
            };
            tfun.GetCheckAnalyzeExcel(model, ref page, ref lstExport, ref strError);

            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("盈亏情况");
            row1.CreateCell(1).SetCellValue("实盘据点");
            row1.CreateCell(2).SetCellValue("实盘物料号");
            row1.CreateCell(3).SetCellValue("实盘物料描述");
            row1.CreateCell(4).SetCellValue("实盘库位");
            row1.CreateCell(5).SetCellValue("实盘序列号");
            row1.CreateCell(6).SetCellValue("实盘数量");
            row1.CreateCell(7).SetCellValue("实盘跟踪号");
            row1.CreateCell(8).SetCellValue("库存数量");
            row1.CreateCell(9).SetCellValue("库存序列号");
            row1.CreateCell(10).SetCellValue("库存库位");
            row1.CreateCell(11).SetCellValue("库存物料号");
            row1.CreateCell(12).SetCellValue("库存物料描述");
            row1.CreateCell(13).SetCellValue("库存据点");
            row1.CreateCell(14).SetCellValue("库存跟踪号");
            row1.CreateCell(15).SetCellValue("盘盈数量");
            row1.CreateCell(16).SetCellValue("盘亏数量");
            row1.CreateCell(17).SetCellValue("操作人");
            //将数据逐步写入seet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);

                rowtemp.CreateCell(0).SetCellValue(lstExport[i].remark == null ? "" : lstExport[i].remark.ToString());
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].STRONGHOLDCODE == null ? "" : lstExport[i].STRONGHOLDCODE.ToString());
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].MATERIALNO == null ? "" : lstExport[i].MATERIALNO.ToString());
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].MATERIALDESC == null ? "" : lstExport[i].MATERIALDESC.ToString());
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].AREANO == null ? "" : lstExport[i].AREANO.ToString());
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].SERIALNO == null ? "" : lstExport[i].SERIALNO.ToString());
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].QTY == null ? "" : lstExport[i].QTY.ToString());
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].TracNo == null ? "" : lstExport[i].TracNo.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].SQTY == null ? "" : lstExport[i].SQTY.ToString());
                rowtemp.CreateCell(9).SetCellValue(lstExport[i].SSERIALNO.ToString());
                rowtemp.CreateCell(10).SetCellValue(lstExport[i].SAREANO.ToString());
                rowtemp.CreateCell(11).SetCellValue(lstExport[i].SMATERIALNO == null ? "" : lstExport[i].SMATERIALNO.ToString());
                rowtemp.CreateCell(12).SetCellValue(lstExport[i].SMATERIALDESC == null ? "" : lstExport[i].SMATERIALDESC.ToString());
                rowtemp.CreateCell(13).SetCellValue(lstExport[i].SSTRONGHOLDCODE == null ? "" : lstExport[i].SSTRONGHOLDCODE.ToString());
                rowtemp.CreateCell(14).SetCellValue(lstExport[i].sTracNo == null ? "" : lstExport[i].sTracNo.ToString());
                rowtemp.CreateCell(15).SetCellValue(lstExport[i].YQTY == null ? "" : lstExport[i].YQTY.ToString());
                rowtemp.CreateCell(16).SetCellValue(lstExport[i].KQTY == null ? "" : lstExport[i].KQTY.ToString());
                rowtemp.CreateCell(17).SetCellValue(lstExport[i].Creater == null ? "" : lstExport[i].Creater.ToString());


            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }



        [HttpPost]
        public ActionResult Add(Check_Model model, string strAll)
        {
            model.CHECKSTATUS = "新建";
            model.CHECKTYPE = "货位";
            model.CREATER = currentUser.UserName;
            List<CheckArea_Model> list = new List<CheckArea_Model>();
            string[] strsp = strAll.Split(';');
            foreach (string item in strsp)
            {
                if (item.Contains(','))
                {
                    CheckArea_Model m = new CheckArea_Model();
                    string[] strsp2 = item.Split(',');
                    m.AREANO = strsp2[1];
                    m.ID = Convert.ToInt32(strsp2[0]);
                    list.Add(m);
                }
            }
            string ErrMsg = "";
            bool bSucc = tfun.SaveCheck(model, list, ref ErrMsg);
            if (!bSucc)
            {
                ErrMsg = "保存失败！" + ErrMsg;
            }
            else
            {
                ErrMsg = "保存成功！";
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult shanchu(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 0, ref ErrMsg, ""))
            {
                ErrMsg = "删除成功！";
            }
            else
            {
                ErrMsg = "删除失败！" + ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult zhongzhi(string CHECKNO)
        {
            string ErrMsg = "";
            if (tfun.DelCloCheck(CHECKNO, 1, ref ErrMsg, ""))
            {
                ErrMsg = "终止成功！";
            }
            else
            {
                ErrMsg = "终止失败！" + ErrMsg;
            }
            return Json(ErrMsg, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Delete(Check_Model model)
        {
            return RedirectToAction("GetModelList");
        }

        [HttpGet]
        public ActionResult GetModel(Check_Model model)
        {
            Check_Func db = new Check_Func();
            ViewData["Taskno"] = db.GetTableID("SEQ_CHECK_NO").ToString();
            return View();
        }


        public JsonResult GetDetail(int Svalue, string areano, string houseno, string warehouseno)
        {
            Thread.Sleep(5000);
            List<CheckArea_Model> lsttask = new List<CheckArea_Model>();
            tfun.GetCheckArea(Svalue, areano, houseno, warehouseno, ref lsttask);
            return Json(lsttask, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckAnalyze(DividPage page, CheckAnalyze model)
        {
            List<CheckAnalyze> modelList = new List<CheckAnalyze>();
            string strError = "";
            tfun.GetCheckAnalyze(model, ref page, ref modelList, ref strError);
            ViewData["PageData"] = new PageData<CheckAnalyze> { data = modelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Check/CheckAnalyze") };
            return View("CheckAnalyze", model);

        }
        public ActionResult PageView1(PageData<CheckAnalyze> model)
        {
            ViewData["dividPage"] = model.dividPage;
            ViewData["url"] = model.link;
            return View("_ViewPage");
        }

        public JsonResult tiaozheng(string CHECKNO)
        {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！", JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult tiaozheng_ms(string CHECKNO)
        {
            if (currentUser == null)
            {
                return Json("Cookie失效，重新登陆！", JsonRequestBehavior.AllowGet);
            }
            string ErrMsg = "";
            Check_DB db = new Check_DB();
            if (db.DelCloCheck_MsTest(CHECKNO, 2, ref ErrMsg, currentUser.UserNo))
            {
                return Json("调整成功！" + ErrMsg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ErrMsg, JsonRequestBehavior.AllowGet);
            }
        }



        #region 物料盘点
        // 跳转新建明盘单页面
        public ActionResult GetModelMing()
        {
            return View();
        }

        // 跳转明盘单分析页面
        public ActionResult CheckMingAnalyze(string CHECKNO)
        {
            ViewData["checkno"] = CHECKNO;
            
            return View();
        }

        // 获取盘点库存信息
        public JsonResult GetCheckStock(T_StockInfoEX model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.WarehouseNo))
                {
                    return Json(new { Result = 0, ResultValue = "仓库不能为空！" }, JsonRequestBehavior.AllowGet);
                }
                T_WareHouse_DB TWareHouseDB = new T_WareHouse_DB();
                string StrongHoldCode = "";
                string StrongHoldCodeName = "";
                TWareHouseDB.GetStrongholdcode(model.WarehouseNo, ref StrongHoldCode, ref StrongHoldCodeName);
          
                string strErrMsg = string.Empty;
                List<T_StockInfoEX> wms_stock = new List<T_StockInfoEX>();
                Query_DB db = new Query_DB();
                if (db.GetStockCombineInfo2(model, ref wms_stock, ref strErrMsg))
                {
                    if (wms_stock == null || wms_stock.Count == 0)
                    {
                        return Json(new { Result = 0, ResultValue = "查询库存为空！" }, JsonRequestBehavior.AllowGet);
                    }
                    T_Material_Batch_Func FUNC = new T_Material_Batch_Func();
                    List<U9Stock> erp_stock = FUNC.GetStockInfo(model.WarehouseNo, StrongHoldCode, model.MaterialNo);
                    //wms数据和erp数据对账
                    for (int i = 0; i < wms_stock.Count; i++)
                    {
                        if (erp_stock != null && erp_stock.Count > 0)
                        {
                            for (int j = 0; j < erp_stock.Count; j++)
                            {
                                if (wms_stock[i].MaterialNo == erp_stock[j].MaterialNo && wms_stock[i].BatchNo == erp_stock[j].BatchNo)
                                {
                                    wms_stock[i].U9MaterialNo = erp_stock[i].MaterialNo;
                                    wms_stock[i].U9BatchNo = erp_stock[i].BatchNo;
                                    wms_stock[i].U9Qty = erp_stock[j].Qty;
                                    erp_stock[j].IsAmount = 1;
                                }
                            }
                        }

                    }

                    if (erp_stock != null && erp_stock.Count > 0)
                    {
                        //没匹配的u9数据
                        for (int i = 0; i < erp_stock.Count; i++)
                        {
                            if (erp_stock[i].IsAmount != 1)
                            {
                                wms_stock.Add(new T_StockInfoEX()
                                {
                                    U9MaterialNo = erp_stock[i].MaterialNo,
                                    U9BatchNo = erp_stock[i].BatchNo,
                                    U9Qty = erp_stock[i].Qty,
                                    Qty = 0
                                });
                            }
                        }
                    }
                    return Json(new { Result = 1, Data = wms_stock }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = 0, ResultValue = strErrMsg }, JsonRequestBehavior.AllowGet);
                }
                //BaseModel<List<Tmodel>> returnmodel = new BaseModel<List<Tmodel>>() { Result = 1, ResultValue = strError, Data = modelList, PageData = new R_Pagedata() { totalCount = page.RecordCounts, pageSize = page.CurrentPageShowCounts, currentPage = page.CurrentPageNumber, totalPages = page.PagesCount } };

            }
            catch (Exception ex)
            {
                return Json(new { Result = 0, ResultValue = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //保存盘点单
        public JsonResult SaveCheck(T_StockInfoEX model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.WarehouseNo))
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }
                T_WareHouse_DB TWareHouseDB = new T_WareHouse_DB();
                string StrongHoldCode = "";
                string StrongHoldCodeName = "";
                TWareHouseDB.GetStrongholdcode(model.WarehouseNo, ref StrongHoldCode, ref StrongHoldCodeName);

                Check_DB checkdb = new Check_DB();
                string ErrorMsg = "";
                if (!checkdb.CheckIsSvae(model.MaterialNo, model.WarehouseNo,ref ErrorMsg)) {
                    return Json(new { state = false, ErrMsg = "待发或者待收库区有该物料不能生成盘点单！【" + ErrorMsg + "】" }, JsonRequestBehavior.AllowGet);
                }


                string strErrMsg = string.Empty;
                List<T_StockInfoEX> lsttask = new List<T_StockInfoEX>();
                Query_DB db = new Query_DB();
                if (!db.GetStockCombineInfo2(model, ref lsttask, ref strErrMsg))
                {
                    return Json(new { Result = 0, ErrMsg = strErrMsg }, JsonRequestBehavior.AllowGet);
                }
                if (model.Check=="0")
                {
                    //检验WMS库存和U9是否一致
                    T_Material_Batch_Func FUNC = new T_Material_Batch_Func();
                    List<U9Stock> erp_stock = FUNC.GetStockInfo(model.WarehouseNo, StrongHoldCode, model.MaterialNo);
                    bool isOk = true;
                    lsttask.ForEach(itemWMS => {
                        if (erp_stock.FindAll(itemerp => { return itemerp.MaterialNo == itemWMS.MaterialNo && itemerp.BatchNo == itemWMS.BatchNo && itemerp.Qty == itemWMS.Qty; }).Count != 1)
                        {
                            isOk = false;
                        }
                    });
                    if (!isOk)
                    {
                        return Json(new { state = false, ErrMsg = "WMS库存和U9不匹配不能生成盘点单！" }, JsonRequestBehavior.AllowGet);
                    }
                }


                
                Check_Model cm = new Check_Model();
                //cm.REMARKS = strremark + txt_remarkD.Text + txt_remark.Text;
                cm.CHECKSTATUS = "新建";
                cm.CREATER = currentUser.UserNo;
                cm.CHECKDESC = "明盘";
                string ErrMsg = "";
                if (checkdb.SaveCheck2(cm, lsttask, ref ErrMsg))
                    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
                return Json(new { state = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, ErrMsg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        // 物料盘点分析
        public JsonResult GetCheckStockA(string CheckNo)
        {
            try
            {
                if (string.IsNullOrEmpty(CheckNo))
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }
                Check_DB CheckDB = new Check_DB();
                List<T_StockInfoEX> Checklist = new List<T_StockInfoEX>();
                List<T_StockInfoEX> Scanlist = new List<T_StockInfoEX>();
                CheckDB.GetCheckStock2(CheckNo, ref Checklist); //盘点单
                CheckDB.GetCheckSerialno(CheckNo, ref Scanlist); //实盘结果
                if (Checklist.Count == 0 || Scanlist.Count == 0)
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }

                //wms数据和erp数据对账
                for (int i = 0; i < Checklist.Count; i++)
                {
                    for (int j = 0; j < Scanlist.Count; j++)
                    {
                        if (Checklist[i].MaterialNo == Scanlist[j].MaterialNo && Checklist[i].BatchNo == Scanlist[j].BatchNo)
                        {
                            Checklist[i].ScaMaterialNo = Scanlist[j].MaterialNo;
                            Checklist[i].ScaBatchNo = Scanlist[j].BatchNo;
                            Checklist[i].ScanQty = Scanlist[j].Qty;
                            Scanlist[j].IsAmount = 1;
                        }
                    }
                }
                //没匹配的数据
                for (int i = 0; i < Scanlist.Count; i++)
                {
                    if (Scanlist[i].IsAmount != 1)
                    {
                        Checklist.Add(new T_StockInfoEX()
                        {
                            ScaMaterialNo = Scanlist[i].MaterialNo,
                            ScaBatchNo = Scanlist[i].BatchNo,
                            ScanQty = Scanlist[i].Qty,
                            Qty = 0
                        });
                    }
                }

                return Json(new { Result = 1, Data = Checklist }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Result = 0, ResultValue = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //提交U9盘点信息
        public JsonResult SaveCheckToU9(string CheckNo)
        {
            try
            {
                if (string.IsNullOrEmpty(CheckNo))
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }
                Check_DB CheckDB = new Check_DB();
                List<T_StockInfoEX> Checklist = new List<T_StockInfoEX>();
                List<T_StockInfoEX> Scanlist = new List<T_StockInfoEX>();
                CheckDB.GetCheckStock2(CheckNo, ref Checklist); //盘点单
                CheckDB.GetCheckSerialno(CheckNo, ref Scanlist); //实盘结果
                if (Checklist.Count == 0 || Scanlist.Count == 0)
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }

                //wms数据和erp数据对账
                for (int i = 0; i < Checklist.Count; i++)
                {
                    for (int j = 0; j < Scanlist.Count; j++)
                    {
                        if (Checklist[i].MaterialNo == Scanlist[j].MaterialNo && Checklist[i].BatchNo == Scanlist[j].BatchNo)
                        {
                            Checklist[i].ScanQty = Scanlist[j].Qty;
                            Scanlist[j].IsAmount = 1;
                        }
                    }
                }
                //没匹配的数据
                for (int i = 0; i < Scanlist.Count; i++)
                {
                    if (Scanlist[i].IsAmount != 1)
                    {
                        Checklist.Add(new T_StockInfoEX()
                        {
                            U9MaterialNo = Scanlist[i].MaterialNo,
                            U9BatchNo = Scanlist[i].BatchNo,
                            ScanQty = Scanlist[i].Qty,
                            Qty = 0
                        });
                    }
                }
                T_Material_Batch_Func FUNC = new T_Material_Batch_Func();
                string strMsg = "";
                if (FUNC.SaveCheckToU9(Checklist, CheckNo,  currentUser.UserNo, ref strMsg)) {
                    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
                } else {
                    return Json(new { state = false, ErrorMsg = strMsg }, JsonRequestBehavior.AllowGet);
                }
                    
            }
            catch (Exception ex)
            {
                return Json(new { state = false, Msg = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}