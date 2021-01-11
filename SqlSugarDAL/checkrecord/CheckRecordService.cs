using Newtonsoft.Json;
using SqlSugarDAL.product;
using SqlSugarDAL.Until;
using SqlSugarDAL.view_checkrecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.checkrecord
{
    public class CheckRecordService : DbContext<T_CheckRecord>
    {
        public SuccessResult DeleteById(int checkRecordId)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                var objCheckRecord = new View_CheckRecordService().GetSugarQueryable(x => x.Id == checkRecordId).First();
                var result = Db.Ado.UseTran(() =>
                {
                    Delete(checkRecordId);
                    ProductService productService = new ProductService();
                    var objRecord = productService.GetSugarQueryable(x => x.ErpVoucherNo == objCheckRecord.ErpVoucherNo).First();
                    objRecord.QulityQty -= objCheckRecord.RecordQualityQty;
                    productService.Update(objRecord);
                });
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                successResult.Msg = "删除成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }

        public SuccessResult Submit(string formJson, string orderId, string remark, string userNo)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                ProductService productService = new ProductService();
                var checkRecord = JsonConvert.DeserializeObject<T_CheckRecord>(formJson);

                var product = productService.GetById(Convert.ToInt32(orderId));
                var records = new View_CheckRecordService()
                    .GetSugarQueryable(x => x.ErpVoucherNo == product.ErpVoucherNo);
                var sumNoQualituQty = records.Sum(x => x.RecordNoQualityQty);
                var sumQualityQty = records.Sum(x => x.RecordQualityQty);
                var sumBackQualityQty = records.Sum(x => x.BackQualityQty);
                if (product.ProductQty == sumQualityQty && sumNoQualituQty - sumBackQualityQty == 0)
                {
                    successResult.Msg = "该订单已检验完成,不能再检验!";
                    return successResult;
                }
                if (checkRecord.BackQualityQty + sumBackQualityQty > sumNoQualituQty)
                {
                    var shenyu = sumNoQualituQty - sumBackQualityQty;
                    successResult.Msg = "复检合格数量 不能大于 未复检不合格数量" + shenyu + "";
                    return successResult;
                }
                var shengyu = product.ProductQty - (sumQualityQty + sumNoQualituQty);
                if (checkRecord.QualityQty + checkRecord.NoQualityQty > shengyu)
                {
                    successResult.Msg = "合格数量+不合格数量不能大于 订单剩余数量" + shengyu + "!";
                    return successResult;
                }

                T_CheckRecord queryData = new T_CheckRecord();
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
                queryData.Checker = userNo;

                queryData.BackQualityQty = checkRecord.BackQualityQty;
                queryData.QualityQty = checkRecord.QualityQty;
                queryData.NoQualityQty = checkRecord.NoQualityQty;

                var result = Db.Ado.UseTran(() =>
                {
                    Insert(queryData);

                    product.QulityQty += Convert.ToDecimal(checkRecord.QualityQty);
                    product.Remark = remark;
                    productService.Update(product);
                });
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
                successResult.Msg = "保存成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }

    }
}
