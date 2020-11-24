using BILBasic.Common;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.Product;
using BILWeb.Quality;
using BILWeb.View_Product;
using Newtonsoft.Json;
using SqlSugarDAL.checkrecord;
using SqlSugarDAL.remark;
using SqlSugarDAL.Until;
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
    public class QualityDetailController : BaseController<T_QualityDetailInfo>
    {
        /// <summary>
        /// 获取备注数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRemarks()
        {
            return Json(new RemarkService().GetList());
        }

        /// <summary>
        /// 根据生产单号获取订单数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModel(string orderNo)
        {
            return Json(new View_ProductService().GetList(x => x.HeadErpVoucherNo == orderNo));
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        public ActionResult Submit(string formJson)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                CheckRecordService checkRecordService = new CheckRecordService();
                var checkRecord = JsonConvert.DeserializeObject<CheckRecord>(formJson);
                var queryData = checkRecordService
                    .GetList(x => x.ProductOrderId == checkRecord.ProductOrderId).FirstOrDefault();
                if (queryData == null)
                    checkRecordService.Insert(checkRecord);
                else
                {
                    queryData.Sensei = checkRecord.Sensei;
                    queryData.Sensei = checkRecord.Scratch;
                    queryData.Bruise = checkRecord.Bruise;
                    queryData.Sensei = checkRecord.Speckle;
                    queryData.Bruise = checkRecord.DownEdge;
                    queryData.Sensei = checkRecord.Rust;
                    queryData.Bruise = checkRecord.MissedProcess;
                    queryData.Sensei = checkRecord.Decibel;
                    queryData.Bruise = checkRecord.Dot;
                    queryData.Sensei = checkRecord.Disaccord;
                    queryData.Bruise = checkRecord.Noise;
                    queryData.Bruise = checkRecord.CardPoint;
                    queryData.Sensei = checkRecord.ShaftTight;
                    queryData.Bruise = checkRecord.Shake;
                    queryData.Sensei = checkRecord.OutputSize;
                    queryData.Bruise = checkRecord.InPutSize;
                    queryData.Sensei = checkRecord.NeglectedLoading;
                    queryData.Bruise = checkRecord.NotInPlace;
                    queryData.Sensei = checkRecord.Others;
                    queryData.Bruise = checkRecord.Minute;
                    checkRecordService.Update(checkRecord);
                }
                successResult.Msg = "保存成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return Json(successResult);
        }




        private IQualityDetailService qualityDetailService;
        public QualityDetailController()
        {
            qualityDetailService = (IQualityDetailService)ServiceFactory.CreateObject("Quality.T_QualityDetail_Func");
            baseservice = qualityDetailService;
        }

        T_InStockDetail_Func tfunc_detail = new T_InStockDetail_Func();

        public JsonResult Sync(string ErpVoucherNo)
        {
            string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
            //10:入库单据
            if (PFunc.Sync(10, string.Empty, ErpVoucherNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            {

                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        //关闭单据
        [HttpPost]
        public JsonResult CloseQuality(string ID)
        {
            try
            {
                string strError = "";
                T_QualityDetail_Func tfunc = new T_QualityDetail_Func();
                if (tfunc.CloseQualityDetailVoucherNo(Convert.ToInt32(ID), currentUser, ref strError))
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

    }
}