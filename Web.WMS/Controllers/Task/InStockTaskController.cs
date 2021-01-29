using BILBasic.Common;
using BILWeb.InStockTask;
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
    [RoleActionFilter(Message = "Task/InStockTask")]
    public class InStockTaskController :  BaseController<T_InStockTaskInfo>
    {

        private IInStockTaskService inStockTaskService;
        public InStockTaskController()
        {
            inStockTaskService = (IInStockTaskService)ServiceFactory.CreateObject("InStockTask.T_InStockTask_Func");
            baseservice = inStockTaskService;
        }
        T_InStockTask_Func tfunc_task = new T_InStockTask_Func();
        T_InTaskDetails_Func tfunc_detail = new T_InTaskDetails_Func();
        public JsonResult GetDetail(Int32 ID)
        {
            List<T_InStockTaskDetailsInfo> modelList = new List<T_InStockTaskDetailsInfo>();
            string strError = "";
            tfunc_detail.GetModelListByHeaderIDForPc(ref modelList, ID, ref strError);
            return Json(modelList, JsonRequestBehavior.AllowGet);
        }


        public FileResult Excel(T_InStockTaskInfo model)
        {
            DividPage page = new DividPage() { CurrentPageShowCounts = 10000000 };
            string strErrMsg = string.Empty;
            List<T_InStockTaskInfo> lstExport = new List<T_InStockTaskInfo>();
            tfunc_task.GetModelListByPage(ref lstExport, currentUser, model, ref page, ref strErrMsg);
            //创建Excel文件的对象
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            //添加一个sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");
            //给sheet1添加第一行的头部标题
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);
            row1.CreateCell(0).SetCellValue("据点名");
            row1.CreateCell(1).SetCellValue("单据类型");
            row1.CreateCell(2).SetCellValue("ERP单号");
            row1.CreateCell(3).SetCellValue("任务号");
            row1.CreateCell(4).SetCellValue("状态");
            row1.CreateCell(5).SetCellValue("任务下发人");
            row1.CreateCell(6).SetCellValue("凭证号");
            row1.CreateCell(7).SetCellValue("创建时间");
            row1.CreateCell(8).SetCellValue("仓库名");
            //将数据逐步写入sheet1各个行
            for (int i = 0; i < lstExport.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(lstExport[i].StrongHoldName);
                rowtemp.CreateCell(1).SetCellValue(lstExport[i].StrVoucherType);
                rowtemp.CreateCell(2).SetCellValue(lstExport[i].ErpVoucherNo);
                rowtemp.CreateCell(3).SetCellValue(lstExport[i].TaskNo);
                rowtemp.CreateCell(4).SetCellValue(lstExport[i].StrStatus);
                rowtemp.CreateCell(5).SetCellValue(lstExport[i].StrTaskIsSuedUser);
                rowtemp.CreateCell(6).SetCellValue(lstExport[i].ErpInVoucherNo);
                rowtemp.CreateCell(7).SetCellValue(lstExport[i].CreateTime.ToString());
                rowtemp.CreateCell(8).SetCellValue(lstExport[i].WareHouseName);
            }
            // 写入到客户端 
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            book.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            return File(ms, "application/vnd.ms-excel", DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
        }

        //关闭任务
        [HttpPost]
        public JsonResult CloseTask(string ID)
        {
            try
            {
                T_InStockTaskInfo model = new T_InStockTaskInfo();
                model.ID = Convert.ToInt32(ID);
                string strError = string.Empty;
                if(!tfunc_task.GetModelByID(ref model, ref strError)) {
                    return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                }
                if (model.Status == 5)
                {
                    return Json(new { state = false, obj = "当前任务已关闭,请不要重复关闭!" }, JsonRequestBehavior.AllowGet);
                }

                if (model.Status == 3)
                {
                    return Json(new { state = false, obj = "当前任务完成,不能关闭!" }, JsonRequestBehavior.AllowGet);
                }

                model.Status = 5;
                if (tfunc_task.UpadteModelByModelSql(currentUser, model, ref strError))
                {
                    return Json(new { state = true, obj = "当前任务关闭成功!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = "关闭失败!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}