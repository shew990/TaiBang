using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.Product;
using BILWeb.Quality;
using BILWeb.View_Product;
using Newtonsoft.Json;
using SqlSugarDAL.checkrecord;
using SqlSugarDAL.product;
using SqlSugarDAL.remark;
using SqlSugarDAL.Until;
using SqlSugarDAL.view_checkrecord;
using SqlSugarDAL.view_product;
using System;
using System.Collections.Generic;
using System.Data;
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
        string strongHoldCode = Commom.ReadUserInfo().StrongHoldCode;

        /// <summary>
        /// 跳转主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModelList()
        {
            ViewBag.StrongHoldCode = strongHoldCode;
            return View();
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
        /// 根据生产单号获取订单数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModel(string orderNo)
        {
            var model = new ProductService().GetList(x => x.ErpVoucherNo == orderNo).FirstOrDefault();
            T_CheckRecord checkRecord = new T_CheckRecord();
            if (model != null)
                checkRecord = new CheckRecordService().GetList(x => x.ProductOrderId == model.id).FirstOrDefault();
            return Json(new { product = model, record = checkRecord }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit(string formJson, string orderId, string qualityQty, string unQualityQty, string remark)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                CheckRecordService checkRecordService = new CheckRecordService();
                ProductService productService = new ProductService();
                var checkRecord = JsonConvert.DeserializeObject<T_CheckRecord>(formJson);
                var queryData = checkRecordService
                    .GetList(x => x.ProductOrderId == Convert.ToInt32(orderId)).FirstOrDefault();
                if (queryData == null)
                {
                    queryData = new T_CheckRecord();
                    queryData.TotalUnqualifiedNumber =
                        unQualityQty == "" ? 0 : Convert.ToInt32(unQualityQty);//总不合格数

                    //一部/二部
                    queryData.Burning = checkRecord.Burning;
                    queryData.WrongLabe = checkRecord.WrongLabe;
                    queryData.Resistance = checkRecord.Resistance;
                    queryData.WrongNumberControl = checkRecord.WrongNumberControl;
                    queryData.DielectricStrength = checkRecord.DielectricStrength;
                    queryData.GearBump = checkRecord.GearBump;
                    queryData.AbnormalNoise = checkRecord.AbnormalNoise;
                    queryData.BearingFailure = checkRecord.BearingFailure;
                    queryData.InputPort = checkRecord.InputPort;
                    queryData.OutPort = checkRecord.OutPort;
                    queryData.MountingFlange = checkRecord.MountingFlange;
                    queryData.InstallKeyway = checkRecord.InstallKeyway;
                    queryData.InstallationShaftDiameter = checkRecord.InstallationShaftDiameter;
                    queryData.InstallTheStop = checkRecord.InstallTheStop;
                    queryData.BoltCenter = checkRecord.BoltCenter;
                    queryData.NoLoad = checkRecord.NoLoad;
                    queryData.RawMaterial = checkRecord.RawMaterial;
                    queryData.Contour = checkRecord.Contour;
                    queryData.SaveTime = DateTime.Now;
                    queryData.ProductOrderId = Convert.ToInt32(orderId);

                    //三部
                    queryData.Sensei = checkRecord.Sensei;
                    queryData.Scratch = checkRecord.Scratch;
                    queryData.Bruise = checkRecord.Bruise;
                    queryData.Speckle = checkRecord.Speckle;
                    queryData.DownEdge = checkRecord.DownEdge;
                    queryData.Rust = checkRecord.Rust;
                    queryData.MissedProcess = checkRecord.MissedProcess;
                    queryData.Decibel = checkRecord.Decibel;
                    queryData.Dot = checkRecord.Dot;
                    queryData.Disaccord = checkRecord.Disaccord;
                    queryData.Noise = checkRecord.Noise;
                    queryData.CardPoint = checkRecord.CardPoint;
                    queryData.ShaftTight = checkRecord.ShaftTight;
                    queryData.Shake = checkRecord.Shake;
                    queryData.OutputSize = checkRecord.OutputSize;
                    queryData.InPutSize = checkRecord.InPutSize;
                    queryData.NeglectedLoading = checkRecord.NeglectedLoading;
                    queryData.NotInPlace = checkRecord.NotInPlace;
                    queryData.Others = checkRecord.Others;
                    queryData.Minute = checkRecord.Minute;
                    checkRecordService.Insert(queryData);
                }
                else
                {
                    queryData.TotalUnqualifiedNumber +=
                        unQualityQty == "" ? 0 : Convert.ToInt32(unQualityQty);//总不合格数

                    queryData.Sensei = checkRecord.Sensei;
                    queryData.Scratch = checkRecord.Scratch;
                    queryData.Bruise = checkRecord.Bruise;
                    queryData.Speckle = checkRecord.Speckle;
                    queryData.DownEdge = checkRecord.DownEdge;
                    queryData.Rust = checkRecord.Rust;
                    queryData.MissedProcess = checkRecord.MissedProcess;
                    queryData.Decibel = checkRecord.Decibel;
                    queryData.Dot = checkRecord.Dot;
                    queryData.Disaccord = checkRecord.Disaccord;
                    queryData.Noise = checkRecord.Noise;
                    queryData.CardPoint = checkRecord.CardPoint;
                    queryData.ShaftTight = checkRecord.ShaftTight;
                    queryData.Shake = checkRecord.Shake;
                    queryData.OutputSize = checkRecord.OutputSize;
                    queryData.InPutSize = checkRecord.InPutSize;
                    queryData.NeglectedLoading = checkRecord.NeglectedLoading;
                    queryData.NotInPlace = checkRecord.NotInPlace;
                    queryData.Others = checkRecord.Others;
                    queryData.Minute = checkRecord.Minute;

                    queryData.Burning = checkRecord.Burning;
                    queryData.WrongLabe = checkRecord.WrongLabe;
                    queryData.Resistance = checkRecord.Resistance;
                    queryData.WrongNumberControl = checkRecord.WrongNumberControl;
                    queryData.DielectricStrength = checkRecord.DielectricStrength;
                    queryData.GearBump = checkRecord.GearBump;
                    queryData.AbnormalNoise = checkRecord.AbnormalNoise;
                    queryData.BearingFailure = checkRecord.BearingFailure;
                    queryData.InputPort = checkRecord.InputPort;
                    queryData.OutPort = checkRecord.OutPort;
                    queryData.MountingFlange = checkRecord.MountingFlange;
                    queryData.InstallKeyway = checkRecord.InstallKeyway;
                    queryData.InstallationShaftDiameter = checkRecord.InstallationShaftDiameter;
                    queryData.InstallTheStop = checkRecord.InstallTheStop;
                    queryData.BoltCenter = checkRecord.BoltCenter;
                    queryData.ProductOrderId = Convert.ToInt32(orderId);
                    queryData.NoLoad = checkRecord.NoLoad;
                    queryData.RawMaterial = checkRecord.RawMaterial;
                    queryData.Contour = checkRecord.Contour;
                    queryData.SaveTime = DateTime.Now;
                    checkRecordService.Update(queryData);
                }
                var product = productService.GetById(orderId);
                product.QulityQty += Convert.ToDecimal(qualityQty);
                product.Remark = remark;
                productService.Update(product);

                successResult.Msg = "保存成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 跳转导出页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckRecordOut()
        {
            ViewBag.strongHoldCode = strongHoldCode;
            return View();
        }

        public ActionResult GetOrderList(int limit, int page)
        {
            var ordersObj = new View_CheckRecordService().GetOrderList(limit, page);
            return Json(ordersObj, JsonRequestBehavior.AllowGet);
        }

    }
}