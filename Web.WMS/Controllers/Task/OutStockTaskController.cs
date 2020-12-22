using BILWeb.Login.User;
using BILWeb.OutStock;
using BILWeb.OutStockTask;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Task
{
    [RoleActionFilter(Message = "Task/OutStockTask")]
    public class OutStockTaskController : BaseController<T_OutStockTaskInfo>
    {
        protected UserInfo currentUser = Common.Commom.ReadUserInfo();

        private IOutStockTaskService outStockTaskService;
        public OutStockTaskController()
        {
            outStockTaskService = (IOutStockTaskService)ServiceFactory.CreateObject("OutStockTask.T_OutStockTask_Func");
            baseservice = outStockTaskService;
        }

        T_OutStockTask_Func tfunc_task = new T_OutStockTask_Func();
        T_OutTaskDetails_Func tfunc_detail = new T_OutTaskDetails_Func();
        public JsonResult GetDetail(Int32 ID)
        {
            List<T_OutStockTaskDetailsInfo> modelList = new List<T_OutStockTaskDetailsInfo>();
            string strError = "";
            tfunc_detail.GetModelListByHeaderID(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }

        public FileResult Excel(T_OutStockTaskInfo model)
        {
            string strErrMsg = string.Empty;
            List<T_OutStockTaskDetailsInfo> lstExport = new List<T_OutStockTaskDetailsInfo>();
            tfunc_detail.GetExportTaskDetail(model, ref lstExport, ref strErrMsg);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("任务号");
            row1.CreateCell(1).SetCellValue("单据类型");
            row1.CreateCell(2).SetCellValue("ERP单号");
            row1.CreateCell(4).SetCellValue("物料名称");
            row1.CreateCell(3).SetCellValue("物料号");
            row1.CreateCell(4).SetCellValue("物料名称");
            row1.CreateCell(5).SetCellValue("任务数");
            row1.CreateCell(6).SetCellValue("剩余数");
            row1.CreateCell(9).SetCellValue("下架数");
            row1.CreateCell(7).SetCellValue("操作人");
            row1.CreateCell(8).SetCellValue("操作时间");
            row1.CreateCell(9).SetCellValue("创建人");
            row1.CreateCell(10).SetCellValue("创建时间");
            row1.CreateCell(11).SetCellValue("供应商编号");
            row1.CreateCell(12).SetCellValue("供应商");
            row1.CreateCell(13).SetCellValue("状态");
            //row1.CreateCell(14).SetCellValue("拣货车");
            //row1.CreateCell(15).SetCellValue("地标");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(lstExport[i].TaskNo);
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].StrVoucherType);
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].ErpVoucherNo);
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].MaterialNo);
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].MaterialDesc);
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].TaskQty.ToString());
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].RemainQty.ToString());
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].UnShelveQty.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].OperatorUserName.ToString());
                rowtemp.CreateCell(9).SetCellValue(lstExport[i].OperatorDateTime.ToString());
                rowtemp.CreateCell(10).SetCellValue(lstExport[i].Creater.ToString());
                rowtemp.CreateCell(11).SetCellValue(lstExport[i].CreateTime.ToString());
                rowtemp.CreateCell(12).SetCellValue(lstExport[i].SupCusCode);
                rowtemp.CreateCell(13).SetCellValue(lstExport[i].StrStatus);
                //rowtemp.CreateCell(14).SetCellValue(lstExport[i].CarNo);
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }


        ////关闭任务
        //[HttpPost]
        //public JsonResult CloseTask(string ID)
        //{
        //    try
        //    {
        //        //判断当前用户，只有admin，能操作
        //        //if (currentUser.UserNo !="admin" && currentUser.UserNo != "2008050001" && currentUser.UserNo != "2012020004")
        //        //{
        //        //    return Json(new { state = false, obj = "任务关闭权限只有admin，（叶盼盼）2008050001，（游亚勇）2012020004才能操作！" }, JsonRequestBehavior.AllowGet);
        //        //}

        //        T_OutStockTaskInfo model = new T_OutStockTaskInfo();
        //        model.ID =Convert.ToInt32(ID);
        //        string strError = "";
        //        if (!tfunc_task.GetModelByID(ref model, ref strError)) {
        //            return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
        //        }
        //        if (model.Status == 5)
        //        {
        //            return Json(new { state = false, obj = "当前任务已关闭,请不要重复关闭!" }, JsonRequestBehavior.AllowGet);
        //        }

        //        string strmsg = "";
        //        List<T_OutStockTaskInfo> model1 = new List<T_OutStockTaskInfo>();

        //        tfunc_task.GetModelListByFilter(ref model1, ref strmsg,"", "erpvoucherno='"+ model.ErpVoucherNo + "'");
        //        List<T_OutStockInfo> modelpost = new List<T_OutStockInfo>();
        //        if (model1 != null && model1.Count > 0) {
        //            for (int i = 0; i < model1.Count; i++)
        //            {
        //                T_OutStockInfo TOutStockInfo = new T_OutStockInfo();
        //                TOutStockInfo.VoucherType = 50;
        //                TOutStockInfo.ErpVoucherNo = model1[i].ErpVoucherNo;
        //                TOutStockInfo.CompanyCode = model1[i].StrongHoldCode;
        //                TOutStockInfo.StrongHoldCode = model1[i].StrongHoldCode;
        //                TOutStockInfo.ERPVoucherType = model1[i].ERPVoucherType;
        //                modelpost.Add(TOutStockInfo);
        //            }

        //        }

        //        if (!tfunc_task.PostCloseOutStockVoucherNo(modelpost, ref strmsg))
        //        {
        //            return Json(new { state = false, obj = "ERP端关闭失败!"+ strmsg }, JsonRequestBehavior.AllowGet);
        //        }

        //        //if (model.Status == 3)
        //        //{
        //        //    return Json(new { state = false, obj = "当前任务完成,不能关闭!" }, JsonRequestBehavior.AllowGet);
        //        //}
        //        model.Status = 5;
        //        if (tfunc_task.UpadteModelByModelSql(currentUser,model, ref strError))
        //        {
        //            return Json(new { state = true, obj = "当前任务关闭成功!" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { state = false, obj = "关闭失败!" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //关闭任务
        [HttpPost]
        public JsonResult CloseTask(string ID)
        {
            try
            {
                T_OutStockTaskInfo model = new T_OutStockTaskInfo();
                model.ID = Convert.ToInt32(ID);
                string strError = "";
                if (!tfunc_task.GetModelByID(ref model, ref strError))
                {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                if (model.Status == 6)
                {
                    return Json(new { state = false, obj = "当前单据已复核,不能撤销扫描!" }, JsonRequestBehavior.AllowGet);
                }
                string strmsg = ""; 
                if (!tfunc_task.BackOutTask(currentUser, model, ref strmsg))
                {
                    return Json(new { state = true, obj = "当前任务撤销成功!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = "撤销失败!"+ strmsg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


        //打开任务
        [HttpPost]
        public JsonResult OpenTask(string ID)
        {
            try
            {
                T_OutStockTaskInfo model = new T_OutStockTaskInfo();
                model.ID = Convert.ToInt32(ID);
                string strError = "";
                if (!tfunc_task.GetModelByID(ref model, ref strError))
                {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                if (model.Status != 5)
                {
                    return Json(new { state = false, obj = "当前任务未关闭,请不要重复打开!" }, JsonRequestBehavior.AllowGet);
                }

                string strmsg = "";
                List<T_OutStockTaskInfo> model1 = new List<T_OutStockTaskInfo>();

                tfunc_task.GetModelListByFilter(ref model1, ref strmsg, "", "erpvoucherno='" + model.ErpVoucherNo + "'");
                model.Status = 1;
                if (tfunc_task.UpadteModelByModelSql(currentUser, model, ref strError))
                {
                    return Json(new { state = true, obj = "当前任务打开成功!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = "打开失败!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}