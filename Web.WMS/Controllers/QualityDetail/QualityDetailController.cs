using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.Product;
using BILWeb.Quality;
using BILWeb.View_Product;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using SqlSugarDAL.checkrecord;
using SqlSugarDAL.product;
using SqlSugarDAL.remark;
using SqlSugarDAL.Until;
using SqlSugarDAL.view_checkrecord;
using SqlSugarDAL.view_product;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers
{
    [RoleActionFilter(Message = "QualityDetail/QualityDetail")]
    public class QualityDetailController : Controller
    {
        View_CheckRecordService view_CheckRecordService = new View_CheckRecordService();
        CheckRecordService checkRecordService = new CheckRecordService();

        string strongHoldCode = Commom.ReadUserInfo().StrongHoldCode;

        /// <summary>
        /// 跳转主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckRecordOut()
        {
            ViewBag.strongHoldCode = strongHoldCode;
            return View();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <param name="OrderNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public ActionResult GetOrderList(int limit, int page, string OrderNo, string StartDate, string EndDate)
        {
            var ordersObj = view_CheckRecordService.GetOrderList(limit, page, OrderNo, StartDate, EndDate);
            return Json(ordersObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public ActionResult OutExcel(string orderNo, string startDate, string endDate)
        {
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet();
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("日期");
            if (strongHoldCode == "0403")
            {
                row1.CreateCell(1).SetCellValue("制令号");
                row1.CreateCell(2).SetCellValue("客户名称");
                row1.CreateCell(3).SetCellValue("型号规格");
                row1.CreateCell(4).SetCellValue("送检数");
                row1.CreateCell(5).SetCellValue("外观掉漆");
                row1.CreateCell(6).SetCellValue("外观划伤");
                row1.CreateCell(7).SetCellValue("外观磕碰伤");
                row1.CreateCell(8).SetCellValue("外观污迹");
                row1.CreateCell(9).SetCellValue("外观塌边");
                row1.CreateCell(10).SetCellValue("外观锈迹");
                row1.CreateCell(11).SetCellValue("外观漏工序");
                row1.CreateCell(12).SetCellValue("噪音分贝");
                row1.CreateCell(13).SetCellValue("噪音打点");
                row1.CreateCell(14).SetCellValue("噪音不一致");
                row1.CreateCell(15).SetCellValue("噪音杂音");
                row1.CreateCell(16).SetCellValue("卡点卡顿");
                row1.CreateCell(17).SetCellValue("运转轴紧");
                row1.CreateCell(18).SetCellValue("运转抖动");
                row1.CreateCell(19).SetCellValue("输出尺寸");
                row1.CreateCell(20).SetCellValue("输入尺寸");
                row1.CreateCell(21).SetCellValue("装配漏装");
                row1.CreateCell(22).SetCellValue("装配不到位");
                row1.CreateCell(23).SetCellValue("其他原因");
                row1.CreateCell(24).SetCellValue("精度弧分");
                row1.CreateCell(25).SetCellValue("不合格数");
                row1.CreateCell(26).SetCellValue("备注");
            }
            else
            {
                row1.CreateCell(1).SetCellValue("流水线班组");
                row1.CreateCell(2).SetCellValue("单据编号");
                row1.CreateCell(3).SetCellValue("客户名称");
                row1.CreateCell(4).SetCellValue("电机型号/规格");
                row1.CreateCell(5).SetCellValue("交检数");
                row1.CreateCell(6).SetCellValue("外观掉漆");
                row1.CreateCell(7).SetCellValue("外观碰划伤");
                row1.CreateCell(8).SetCellValue("外观氧化");
                row1.CreateCell(9).SetCellValue("外观生锈");
                row1.CreateCell(10).SetCellValue("标签贴错");
                row1.CreateCell(11).SetCellValue("电机阻值");
                row1.CreateCell(12).SetCellValue("电流不一致");
                row1.CreateCell(13).SetCellValue("号码管有误");
                row1.CreateCell(14).SetCellValue("介电强度");
                row1.CreateCell(15).SetCellValue("齿轮磕碰");
                row1.CreateCell(16).SetCellValue("减速箱异响");
                row1.CreateCell(17).SetCellValue("轴承损坏");
                row1.CreateCell(18).SetCellValue("输入孔");
                row1.CreateCell(19).SetCellValue("输出孔");
                row1.CreateCell(20).SetCellValue("安装法兰");
                row1.CreateCell(21).SetCellValue("安装键槽");
                row1.CreateCell(22).SetCellValue("安装轴径");
                row1.CreateCell(23).SetCellValue("安装止口");
                row1.CreateCell(24).SetCellValue("安装孔距");
                row1.CreateCell(25).SetCellValue("空载电流");
                row1.CreateCell(26).SetCellValue("总不合格数");
                row1.CreateCell(27).SetCellValue("原材料");
                row1.CreateCell(28).SetCellValue("外型");
                row1.CreateCell(29).SetCellValue("处置");
                row1.CreateCell(30).SetCellValue("备注");
            }
            var orders = view_CheckRecordService.GetRecords(orderNo, startDate, endDate);
            for (int i = 0; i < orders.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);//给sheet添加一行
                row.CreateCell(0).SetCellValue(orders[i].SaveTimeString);
                if (strongHoldCode == "0403")
                {
                    row.CreateCell(1).SetCellValue(orders[i].ErpVoucherNo);
                    row.CreateCell(2).SetCellValue(orders[i].CustomerName);
                    row.CreateCell(3).SetCellValue(orders[i].spec);
                    row.CreateCell(4).SetCellValue(orders[i].QulityQty.ToString());
                    row.CreateCell(5).SetCellValue(orders[i].Sensei);
                    row.CreateCell(6).SetCellValue(orders[i].Scratch);
                    row.CreateCell(7).SetCellValue(orders[i].Bruise);
                    row.CreateCell(8).SetCellValue(orders[i].Speckle);
                    row.CreateCell(9).SetCellValue(orders[i].DownEdge);
                    row.CreateCell(10).SetCellValue(orders[i].Rust);
                    row.CreateCell(11).SetCellValue(orders[i].MissedProcess);
                    row.CreateCell(12).SetCellValue(orders[i].Decibel);
                    row.CreateCell(13).SetCellValue(orders[i].Dot);
                    row.CreateCell(14).SetCellValue(orders[i].Disaccord);
                    row.CreateCell(15).SetCellValue(orders[i].Noise);
                    row.CreateCell(16).SetCellValue(orders[i].CardPoint);
                    row.CreateCell(17).SetCellValue(orders[i].ShaftTight);
                    row.CreateCell(18).SetCellValue(orders[i].Shake);
                    row.CreateCell(19).SetCellValue(orders[i].OutputSize);
                    row.CreateCell(20).SetCellValue(orders[i].InPutSize);
                    row.CreateCell(21).SetCellValue(orders[i].NeglectedLoading);
                    row.CreateCell(22).SetCellValue(orders[i].NotInPlace);
                    row.CreateCell(23).SetCellValue(orders[i].Others);
                    row.CreateCell(24).SetCellValue(orders[i].Minute);
                    row.CreateCell(25).SetCellValue(orders[i].TotalUnqualifiedNumber);
                    row.CreateCell(26).SetCellValue(orders[i].Decription);
                }
                else
                {
                    row.CreateCell(1).SetCellValue("");//流水线班组
                    row.CreateCell(2).SetCellValue(orders[i].ErpVoucherNo);
                    row.CreateCell(3).SetCellValue(orders[i].CustomerName);
                    row.CreateCell(4).SetCellValue(orders[i].spec);
                    row.CreateCell(5).SetCellValue(orders[i].QulityQty.ToString());
                    row.CreateCell(6).SetCellValue(orders[i].Sensei);
                    row.CreateCell(7).SetCellValue(orders[i].Scratch);
                    row.CreateCell(8).SetCellValue(orders[i].Burning);
                    row.CreateCell(9).SetCellValue(orders[i].Rust);
                    row.CreateCell(10).SetCellValue(orders[i].WrongLabe);
                    row.CreateCell(11).SetCellValue(orders[i].Resistance);
                    row.CreateCell(12).SetCellValue(orders[i].Disaccord);
                    row.CreateCell(13).SetCellValue(orders[i].WrongNumberControl);
                    row.CreateCell(14).SetCellValue(orders[i].DielectricStrength);
                    row.CreateCell(15).SetCellValue(orders[i].GearBump);
                    row.CreateCell(16).SetCellValue(orders[i].AbnormalNoise);
                    row.CreateCell(17).SetCellValue(orders[i].BearingFailure);
                    row.CreateCell(18).SetCellValue(orders[i].InputPort);
                    row.CreateCell(19).SetCellValue(orders[i].OutPort);
                    row.CreateCell(20).SetCellValue(orders[i].MountingFlange);
                    row.CreateCell(21).SetCellValue(orders[i].InstallKeyway);
                    row.CreateCell(22).SetCellValue(orders[i].InstallationShaftDiameter);
                    row.CreateCell(23).SetCellValue(orders[i].InstallTheStop);
                    row.CreateCell(24).SetCellValue(orders[i].BoltCenter);
                    row.CreateCell(25).SetCellValue(orders[i].NoLoad);
                    row.CreateCell(26).SetCellValue(orders[i].TotalUnqualifiedNumber);
                    row.CreateCell(27).SetCellValue(orders[i].RawMaterial);
                    row.CreateCell(28).SetCellValue(orders[i].Contour);
                    row.CreateCell(29).SetCellValue("");//处置
                    row.CreateCell(30).SetCellValue(orders[i].Decription);
                }
            }
            string fileName = "质检明细列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            MemoryStream memoryStream = new MemoryStream();
            book.Write(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return File(memoryStream, "application/vnd.ms-excel", fileName);
        }

        /// <summary>
        /// 跳转质检页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModelList(string checkRecordId = null)
        {
            TempData["checkRecordId"] = checkRecordId;

            ViewBag.checkRecordId = checkRecordId;
            ViewBag.StrongHoldCode = strongHoldCode;
            return View();
        }

        /// <summary>
        /// 获取初始化数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AsyncInit()
        {
            if (TempData["checkRecordId"] != null)
            {
                var record = view_CheckRecordService.GetRecord(Convert.ToInt32(TempData["checkRecordId"]));
                return Json(record, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取备注数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRemarks()
        {
            var remarks = new RemarkService().GetList().Select(x => x.RemarkDesc);
            return Json(remarks, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除质检记录
        /// </summary>
        /// <param name="checkRecordId"></param>
        /// <returns></returns>
        public ActionResult Delete(string checkRecordId)
        {
            var successResult = checkRecordService.DeleteById(Convert.ToInt32(checkRecordId));
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据生产单号获取订单数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModel(string orderNo)
        {
            var model = new ProductService().GetList(x => x.ErpVoucherNo == orderNo).FirstOrDefault();
            T_CheckRecord checkRecord = new T_CheckRecord();
            if (model != null)
                checkRecord = checkRecordService.GetList(x => x.ProductOrderId == model.id).FirstOrDefault();
            return Json(new { product = model, record = checkRecord }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit(string formJson, string orderId, string qualityQty, string remark)
        {
            var successResult = checkRecordService.Submit(formJson, orderId, qualityQty, remark);
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

    }
}