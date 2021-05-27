using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILBasic.User;
using BILWeb.OutBarCode;
using BILWeb.OutStockTask;
using BILWeb.Stock;
using System;
using System.Collections.Generic;
using BILWeb.Query;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BILWeb.Material
{
    public partial class T_Material_Batch_Func : TBase_Func<T_Material_Batch_DB, T_Material_BatchInfo>
    {

        protected override bool CheckModelBeforeSave(T_Material_BatchInfo model, ref string strError)
        {
            T_Material_Batch_DB mdb = new T_Material_Batch_DB();
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            if (mdb.CheckMaterialExist(model) > 0)
            {
                strError = "批次号已经存在！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "物料批次";
        }

        protected override T_Material_BatchInfo GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }


        public string GetStrongholdList()
        {
            BaseMessage_Model<U9BaseInfo> messageModel = new BaseMessage_Model<U9BaseInfo>();

            try
            {
                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                U9BaseInfo BaseInfo = new U9BaseInfo();
                BaseInfo = _db.GetStrongholdList();
                if (BaseInfo == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = BaseInfo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
            }
        }


        public string GetInfoList(string id, string StrongHoldCode)
        {
            BaseMessage_Model<U9BaseInfo> messageModel = new BaseMessage_Model<U9BaseInfo>();

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }
                if (string.IsNullOrEmpty(StrongHoldCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = " 参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }

                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                U9BaseInfo BaseInfo = new U9BaseInfo();
                BaseInfo=_db.GetInfoList(id, StrongHoldCode);
                if (BaseInfo == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = BaseInfo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
            }
        }

        public string GetInfoListThree(string id, string StrongHoldCode, string DeparMentNo)
        {
            BaseMessage_Model<U9BaseInfo> messageModel = new BaseMessage_Model<U9BaseInfo>();

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }
                if (string.IsNullOrEmpty(StrongHoldCode))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = " 参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }

                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                U9BaseInfo BaseInfo = new U9BaseInfo();
                BaseInfo = _db.GetInfoListThree(id, StrongHoldCode, DeparMentNo);
                if (BaseInfo == null)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = BaseInfo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
                }

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<U9BaseInfo>>(messageModel);
            }
        }
        
        public string GetZhList(string ErpVoucherNo, string Type)
        {
            BaseMessage_Model<List<U9Zh>> messageModel = new BaseMessage_Model<List<U9Zh>>();
            try
            {
                if (string.IsNullOrEmpty(ErpVoucherNo))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = " 参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<U9Zh>>>(messageModel);
                }

                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                List<U9Zh> BaseInfo = new List<U9Zh>();
                BaseInfo = _db.GetZhList(ErpVoucherNo, Type);
                BaseInfo.ForEach(item => item.VoucherType = 52);
                LogNet.LogInfo("----------------------------------------------------转换单获取单据信息" + BaseInfo);


                //string aaa = "[{\"detailBehind\":[{\"RowNo\":10,\"Type\":1,\"MaterialNo\":\"MR03070F0001-372\",\"MaterialName\":\"交流小电机\",\"MaterialDesc\":\"交流小电机\",\"Spec\":\"3RK15CC\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":\"TraY-21030078\",\"FanHao\":null,\"RandomCode\":\"5WM3GACM\",\"Row\":10,\"subFlag\":0},{\"RowNo\":20,\"Type\":1,\"MaterialNo\":\"JR20003070F0001-413\",\"MaterialName\":\"小电机减速箱\",\"MaterialDesc\":\"小电机减速箱\",\"Spec\":\"3GN20K-B·10-78\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":\"TraY-21030078\",\"FanHao\":null,\"RandomCode\":\"5WM3GACM\",\"Row\":10,\"subFlag\":0},{\"RowNo\":10,\"Type\":1,\"MaterialNo\":\"MR04022-0020\",\"MaterialName\":\"交流大电机\",\"MaterialDesc\":\"交流大电机\",\"Spec\":\"Y22-100S2-TG-C\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":\"TraY-21030078\",\"FanHao\":null,\"RandomCode\":\"5WM3GACM\",\"Row\":20,\"subFlag\":0},{\"RowNo\":30,\"Type\":1,\"MaterialNo\":\"JR21002022F0001-003\",\"MaterialName\":\"大电机减速箱\",\"MaterialDesc\":\"大电机减速箱\",\"Spec\":\"CV22-200B·T1\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":\"TraY-21030078\",\"FanHao\":null,\"RandomCode\":\"5WM3GACM\",\"Row\":20,\"subFlag\":0}],\"ErpVoucherNo\":\"TraY-21030078\",\"ErpVoucherType\":1001910300976711,\"ErpVoucherTypeName\":\"料品形态转换\",\"CreateTime\":\"2021-03-26\",\"StrongHoldCode\":\"0300\",\"StrongHoldName\":\"营销中心\",\"CustomerNo\":\"\",\"CustomerName\":\"\",\"RowNo\":0,\"Type\":0,\"MaterialNo\":null,\"MaterialName\":null,\"MaterialDesc\":null,\"Spec\":null,\"ErpWareHouseNo\":null,\"ErpWareHouseName\":null,\"Qty\":0.0,\"Unit\":null,\"detail\":[{\"RowNo\":10,\"Type\":0,\"MaterialNo\":\"JMF1902-8684\",\"MaterialName\":\"小电机减速电机\",\"MaterialDesc\":\"小电机减速电机\",\"Spec\":\"3RK15CC/3GN20K-B·10-78\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":null,\"FanHao\":null,\"RandomCode\":null,\"Row\":10,\"subFlag\":0},{\"RowNo\":20,\"Type\":0,\"MaterialNo\":\"JMF1902-1560\",\"MaterialName\":\"大电机减速电机\",\"MaterialDesc\":\"大电机减速电机\",\"Spec\":\"Y22-100S2-TG-C/CV22-200B·T1\",\"ErpWareHouseNo\":\"G060\",\"ErpWareHouseName\":\"东莞成品仓库\",\"Qty\":1.0,\"Unit\":\"Pcs\",\"BatchNo\":null,\"FanHao\":null,\"RandomCode\":null,\"Row\":20,\"subFlag\":0}],\"guid\":null,\"PostUser\":null,\"batchno\":null,\"FanHao\":null,\"RandomCode\":null,\"Row\":0}]";
                //BaseInfo = JsonConvert.DeserializeObject<List<U9Zh>>(aaa);

                if (BaseInfo == null|| BaseInfo.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<U9Zh>>>(messageModel);
                }
                else
                {
                    //获取生单物料库存
                    Stock.T_Stock_DB stockDB = new Stock.T_Stock_DB();
                    List<Stock.T_StockInfo> stockList = new List<Stock.T_StockInfo>();
                    //汇总本次生单的物料
                    var newModelList = from t in BaseInfo[0].detail
                                       group t by new { t1 = t.MaterialNo } into m
                                       select new T_OutStockTaskDetailsInfo
                                       {
                                           MaterialNo = m.Key.t1
                                       };
                    List<T_OutStockTaskDetailsInfo> newModelListret = newModelList.ToList();
                    newModelListret.ForEach(item => { item.StrongHoldCode = BaseInfo[0].StrongHoldCode; });
                    stockList = stockDB.GetCanStockListByMaterialNoIDToSql1(newModelListret);
                    //所有物料都没有库存，不拆分，直接返回
                    if (stockList != null && stockList.Count > 0)
                    {
                        List<T_OutStockTaskDetailsInfo> NewModelList = new List<T_OutStockTaskDetailsInfo>();
                        
                        List<T_StockInfo> stockModelListSum = new List<T_StockInfo>();
                        string strAreaNo = string.Empty;

                        foreach (var item in BaseInfo[0].detail)
                        {
                            List<T_StockInfo> stockModelList = new List<T_StockInfo>();
                            //查找物料可分配库存
                            if (stockModelList.Count == 0 || stockModelList == null)
                            {
                                stockModelList = stockList.FindAll(t => t.MaterialNo == item.MaterialNo && t.WarehouseNo == item.ErpWareHouseNo && t.Qty > 0).OrderBy(t => t.BatchNo).OrderBy(t => t.SortArea).ToList();
                            }
                            var ModelListSum = from t in stockModelList
                                               group t by new
                                               {
                                                   t1 = t.MaterialNo,
                                                   t2 = t.AreaNo,
                                                   t3 = t.StrongHoldCode
                                                   //t4 = t.StrongHoldName,
                                                   //t5 = t.CompanyCode,
                                                   //t6 = t.BatchNo,
                                                   //t7 = t.EDate
                                               } into m
                                               select new T_StockInfo
                                               {
                                                   MaterialNo = m.Key.t1,
                                                   AreaNo = m.Key.t2,
                                                   StrongHoldCode = m.Key.t3,
                                                   //StrongHoldName = m.Key.t4,
                                                   //CompanyCode = m.Key.t5,
                                                   //BatchNo = m.Key.t6,
                                                   //EDate = m.Key.t7,
                                                   Qty = m.Sum(p => p.Qty),
                                                   //FloorType = m.FirstOrDefault().FloorType,
                                                   SortArea = m.FirstOrDefault().SortArea
                                               };
                            stockModelListSum = ModelListSum.ToList();
                            if (stockModelListSum != null && stockModelListSum.Count > 0)
                            {
                                strAreaNo = string.Empty;
                                foreach (var itemArea in stockModelListSum)
                                {
                                    strAreaNo += itemArea.AreaNo + "|";
                                }
                                item.AreaNo = strAreaNo.TrimEnd('|');
                            }
                        }
                    }


                    messageModel.HeaderStatus = "S";
                    messageModel.ModelJson = BaseInfo;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<U9Zh>>>(messageModel);
                }

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<U9Zh>>>(messageModel);
            }
        }

        public string PostZh(string UserJson, string ModelJson, string Guid)
        {
            BaseMessage_Model<List<T_OutBarCodeInfo>> messageModel = new BaseMessage_Model<List<T_OutBarCodeInfo>>();
            try
            {
                string strError = "";
                if (Guid != "")
                {
                    if (!CheckGuid(Guid, ref strError))
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "GUID已经存在，不能重复提交-" + strError;
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
                    }
                }

                if (string.IsNullOrEmpty(UserJson) || string.IsNullOrEmpty(ModelJson) || string.IsNullOrEmpty(Guid))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
                }

                List<U9Zh> modelList = JSONHelper.JsonToObject<List<U9Zh>>(ModelJson);
                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

                modelList.ForEach(t => t.PostUser = user.UserNo);
                modelList.ForEach(t => t.GUID = Guid);

                modelList.ForEach(item =>
                {
                    item.PostUser = user.UserName;
                    item.GUID = Guid;

                    List<U9ZhDetail> detail = new List<U9ZhDetail>();
                    //检验明细里面有没有重复的条码
                    item.detail.ForEach(itemdetail=> {
                        foreach (var barcode in itemdetail.barcodeList)
                        {
                            U9ZhDetail itemdetailLimk = T_Material_Batch_DB.DeepCopyByXml<U9ZhDetail>(itemdetail);
                            itemdetailLimk.Qty = barcode.Qty;
                            itemdetailLimk.BatchNo = barcode.BatchNo;
                            detail.Add(itemdetailLimk);
                        }
                    });
                    item.detail = detail;
                });


                //modelList.ForEach(item=> {
                //    item.PostUser = user.UserNo;
                //    item.GUID = Guid;

                //    if (item.Type == 0 && item.barcodeList.Count > 0)
                //    {
                //        item.BatchNo = item.barcodeList[0].BatchNo;
                //    }
                //    if (item.Type == 1 && item.barcodeList.Count == item.detail.Count)
                //    {
                //        //扫描的条码给明细行赋值
                //        for (int i = 0; i < item.detail.Count; i++)
                //        {
                //            for (int j = 0; j < item.barcodeList.Count; j++)
                //            {
                //                if (item.detail[i].MaterialNo == item.barcodeList[j].MaterialNo)
                //                {
                //                    item.detail[i].BatchNo = item.barcodeList[j].BatchNo;
                //                }
                //            }
                //        }
                //    }

                //});


                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<U9Zh>>(modelList);

                BILBasic.Interface.T_Interface_Func tfunc = new BILBasic.Interface.T_Interface_Func();
                LogNet.LogInfo("---------------------------ERPJsonBefore:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                messageModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutBarCodeInfo>>>(interfaceJson);

                LogNet.LogInfo("--------------------------ERPJsonAfter:" + messageModel);

                //过账失败直接返回
                if (messageModel.HeaderStatus == "E" && !string.IsNullOrEmpty(messageModel.Message))
                {
                    return interfaceJson;
                }
                else if (messageModel.HeaderStatus == "S" && !string.IsNullOrEmpty(messageModel.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    modelList.ForEach(t => t.MaterialDoc = messageModel.MaterialDoc);
                }
                 
                LogNet.LogInfo("ymh：ERPtoWMS-" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel));
                string SerialNos = "";
                if (db.PostZh(user,modelList,ref strError,ref SerialNos) == false)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strError;
                    LogNet.LogInfo("ymh：WMS-失败");
                }
                else
                {
                    SerialNos = SerialNos.Substring(0, SerialNos.Length - 1);
                    T_OutBarCode_Func fuc = new T_OutBarCode_Func();
                    List<T_OutBarCodeInfo> models = new List<T_OutBarCodeInfo>();
                    fuc.GetOutBarCodeInfoBySerialNos(SerialNos,ref models,ref strError);
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "转换操作成功！";
                    messageModel.ModelJson = models;
                    LogNet.LogInfo("ymh：WMS-成功");
                }

                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);

            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
            }
        }

        public string Post(string ErpVoucherNo, string Remark,string Guid,string Creater)
        {
            LogNet.LogInfo("-------------------------------------------直发公司ErpVoucherNo:【" + ErpVoucherNo + "】,  Remark:【"+ Remark + "】, Guid:【"+ Guid + "】, Creater::【"+ Creater + "】");
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();

            try
            {
                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                //成品需要检验库存
                string Msg = "";
                bool isOK = _db.isChengpin(ErpVoucherNo, ref Msg);
                if (isOK) {
                    if (!string.IsNullOrEmpty(Msg))
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = Msg;
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    }

                    T_OutBarcode_DB OutBarcodeDB = new T_OutBarcode_DB();
                    List<T_OutBarCodeInfo> OutBarCodeInfos = OutBarcodeDB.GetModelListByFilter("", " dimension='" + ErpVoucherNo + "'", " * ");
                    if (OutBarCodeInfos == null || OutBarCodeInfos.Count == 0)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "WMS不存在属于该成品入库单【" + ErpVoucherNo + "】的条码！";
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    }
                }
                
                string strERP  = _db.Post(ErpVoucherNo, Remark ,Guid, Creater);
                if (strERP.Substring(0,1)=="0")
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strERP;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

                if (isOK)
                {
                    string strError = "";
                    T_OutTaskDetails_DB _dbOutTaskDetails = new T_OutTaskDetails_DB();
                    LogNet.LogInfo("-------------------------------------------直发公司ERP成功凭证号：" + strERP.Substring(1, strERP.Length - 1));
                    if (_dbOutTaskDetails.DelStockForU9(ErpVoucherNo, strERP.Substring(1, strERP.Length - 1), ref strError) == false)
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "ERP操作成功，ERP凭证号：" + strERP.Substring(1, strERP.Length - 1) + "WMS失败：" + strError;
                        LogNet.LogInfo("-------------------------------------------直发公司ERP成功凭证号：" + messageModel.Message);
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    }
                    else
                    {
                        messageModel.HeaderStatus = "S";
                        messageModel.Message = "操作成功！ERP凭证号：" + strERP.Substring(1, strERP.Length - 1);
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    }
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "操作成功！ERP凭证号：" + strERP.Substring(1, strERP.Length - 1);
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }





            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }
        }

        public List<U9Stock> GetStockInfo(string WareHouseNo, string StrongHoldCode,string MaterialNo)
        {
            try
            {
                if(string.IsNullOrEmpty(WareHouseNo)|| string.IsNullOrEmpty(StrongHoldCode)|| string.IsNullOrEmpty(MaterialNo))
                {
                    return null;
                }
                
                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                List<U9Stock> U9Stocks = new List<U9Stock>();
                U9Stocks = _db.GetStockInfo(WareHouseNo,StrongHoldCode,MaterialNo);
                if (U9Stocks == null)
                {
                    return null;
                }
                else
                {
                    return U9Stocks;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SaveCheckToU9(List<T_StockInfoEX> list,string CheckNo,string UserNo, ref string ErrorMsg)
        {
            try
            {
                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                if (_db.SaveCheckToU9(list, CheckNo, UserNo, ref ErrorMsg))
                {
                    //WMS开始调整
                    Check_DB Check_DB = new Check_DB();
                    if (Check_DB.MingTiaozheng(CheckNo, UserNo, ref ErrorMsg)) {
                        return true;
                    }
                    else
                    {
                        ErrorMsg = "U9过账成功，WMS失败：" + ErrorMsg;
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public string PostZhMove(string UserJson, string List, string Order, string Type, string Guid)
        {
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();
            string strMsg = "";
            try
            {
                if (Guid != "")
                {
                    if (!CheckGuid(Guid, ref strMsg))
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = "GUID已经存在，不能重复提交-" + strMsg;
                        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                    }
                }

                if (string.IsNullOrEmpty(UserJson) || string.IsNullOrEmpty(List) || string.IsNullOrEmpty(Order) || string.IsNullOrEmpty(Type))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "参数不能为空";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }

                List<T_StockInfo> modelList = JSONHelper.JsonToObject<List<T_StockInfo>>(List);
                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);


                T_Material_Batch_DB _db = new T_Material_Batch_DB();
               
                if (!_db.PostZhMove(user,modelList,Order,Type,ref strMsg))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = strMsg;
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "操作成功！";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
                }
 
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.Message;
                return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<string>>(messageModel);
            }
        }


    }

}




