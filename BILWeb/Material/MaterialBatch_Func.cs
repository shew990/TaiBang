﻿using BILBasic.Basing.Factory;
using BILBasic.JSONUtil;
using BILBasic.User;
using BILWeb.OutBarCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        


        public string GetZhList(string ErpVoucherNo)
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
                BaseInfo = _db.GetZhList(ErpVoucherNo);
                BaseInfo.ForEach(item => item.VoucherType = 52);
                LogNet.LogInfo("----------------------------------------------------转换单获取单据信息" + BaseInfo);
                if (BaseInfo == null|| BaseInfo.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "获取失败";
                    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<U9Zh>>>(messageModel);
                }
                else
                {
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

                modelList.ForEach(item=> {
                    item.PostUser = user.UserNo;
                    item.GUID = Guid;

                    if (item.Type == 0&& item.barcodeList.Count>0)
                    {
                        item.BatchNo = item.barcodeList[0].BatchNo;
                    }
                    if (item.Type == 1 && item.barcodeList.Count ==item.detail.Count )
                    {
                        //扫描的条码给明细行赋值
                        for (int i = 0; i < item.detail.Count; i++)
                        {
                            for (int j = 0; j < item.barcodeList.Count; j++)
                            {
                                if (item.detail[i].MaterialNo== item.barcodeList[j].MaterialNo)
                                {
                                    item.detail[i].BatchNo = item.barcodeList[j].BatchNo;
                                }
                            }
                        }
                    }

                });


                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<U9Zh>>(modelList);

                BILBasic.Interface.T_Interface_Func tfunc = new BILBasic.Interface.T_Interface_Func();
                LogNet.LogInfo("ERPJsonBefore:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                messageModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutBarCodeInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + messageModel);

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





    }

}




