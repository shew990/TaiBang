﻿using Newtonsoft.Json;
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

        public SuccessResult Submit(string formJson, string orderId, string qualityQty, string remark)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                ProductService productService = new ProductService();
                var checkRecord = JsonConvert.DeserializeObject<T_CheckRecord>(formJson);

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
                queryData.QualityQty = checkRecord.QualityQty;

                var result = Db.Ado.UseTran(() =>
                {
                    Insert(queryData);

                    var product = productService.GetById(orderId);
                    product.QulityQty += Convert.ToDecimal(qualityQty);
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
