using BILBasic.Basing.Factory;
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


        public string GetInfoList(string id)
        {
            BaseMessage_Model<U9BaseInfo> messageModel = new BaseMessage_Model<U9BaseInfo>();

            try
            {
                T_Material_Batch_DB _db = new T_Material_Batch_DB();
                U9BaseInfo BaseInfo = new U9BaseInfo();
                BaseInfo=_db.GetInfoList(id);
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


        public U9BaseInfo GetStrongholdList()
        {
            U9BaseInfo baseInfo = new U9BaseInfo();
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string json = "{\"VoucherType\":\"9998\"}";
            string ERPJson = TIF.GetModelListByInterface(json);
            return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9BaseInfo>(ERPJson);
        }
        //获取基础列表
        public U9BaseInfo GetInfoList(string id)
        {
            BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
            string json = "{\"data_no\":\"" + id + "\",\"VoucherType\":\"9997\"}";
            string ERPJson = TIF.GetModelListByInterface(json);
            return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9BaseInfo>(ERPJson);

        }


    }

}
