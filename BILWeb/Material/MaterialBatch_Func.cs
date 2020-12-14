using BILBasic.Basing.Factory;
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
            return "";
            //BaseMessage_Model<List<T_OutBarCodeInfo>> messageModel = new BaseMessage_Model<List<T_OutBarCodeInfo>>();
            //try
            //{
            //    string strError = "";
            //    if (Guid != "")
            //    {
            //        if (!CheckGuid(Guid, ref strError))
            //        {
            //            messageModel.HeaderStatus = "E";
            //            messageModel.Message = "GUID已经存在，不能重复提交-" + strError;
            //            return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
            //        }
            //    }

            //    if (string.IsNullOrEmpty(UserJson)||string.IsNullOrEmpty(ModelJson) ||string.IsNullOrEmpty(Guid))
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = "参数不能为空";
            //        return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
            //    }

            //    List<U9Zh> modelList = JSONHelper.JsonToObject<List<U9Zh>>(ModelJson);
            //    UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);

            //    string ERPJson = "";

            //    BILBasic.Interface.T_Interface_Func tfunc = new BILBasic.Interface.T_Interface_Func();
            //    LogNet.LogInfo("ERPJsonBefore:" + ERPJson);
            //    string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

            //    messageModel = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutBarCodeInfo>>>(interfaceJson);

            //    LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BILBasic.Basing.Factory.BaseMessage_Model<List<string>>>(model));

            //    //过账失败直接返回
            //    if (messageModel.HeaderStatus == "E" && !string.IsNullOrEmpty(messageModel.Message))
            //    {
            //        return interfaceJson;
            //    }
            //    else if (messageModel.HeaderStatus == "S" && !string.IsNullOrEmpty(messageModel.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
            //    {
            //        modelList.ForEach(t => t.MaterialDoc = messageModel.MaterialDoc);
            //    }

            //    LogNet.LogInfo("ERPJson:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutBarCodeInfo>>(messageModel));
            //    LogNet.LogInfo("ymh：ERPtoWMS-" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutBarCodeInfo>>(messageModel));

            //    if (db.SaveModelListBySqlToDB(user, ref modelList, ref strError) == false)
            //    {
            //        messageModel.HeaderStatus = "E";
            //        messageModel.Message = strError;
            //        LogNet.LogInfo("ymh：WMS-失败");
            //    }
            //    else
            //    {
            //        messageModel.HeaderStatus = "S";
            //        messageModel.Message = model.MaterialDoc;
            //        LogNet.LogInfo("ymh：WMS-成功");
            //    }

            //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(modelList);

            //}
            //catch (Exception ex)
            //{
            //    messageModel.HeaderStatus = "E";
            //    messageModel.Message = ex.Message;
            //    return BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutBarCodeInfo>>>(messageModel);
            //}
        }
        




    }

}
