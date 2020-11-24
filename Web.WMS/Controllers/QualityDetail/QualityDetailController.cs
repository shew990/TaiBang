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
    public class QualityDetailController : Controller
    {
        /// <summary>
        /// 跳转主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModelList()
        {
            return View();
        }

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
                var checkRecord = JsonConvert.DeserializeObject<T_CheckRecord>(formJson);
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

    }
}