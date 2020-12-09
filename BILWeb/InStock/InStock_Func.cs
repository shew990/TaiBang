using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.JSONUtil;
using BILBasic.User;
using BILBasic.Interface;
using BILWeb.OutStock;
using BILWeb.SyncService;

namespace BILWeb.InStock
{
    public partial class T_InStock_Func : TBase_Func<T_InStock_DB, T_InStockInfo>, IInStockService
    {

        protected override bool CheckModelBeforeSave(T_InStockInfo model, ref string strError)
        {
            if (model == null)
            {
                strError = "客户端传来的实体类不能为空！";
                return false;
            }

            return true;
        }

        protected override string GetModelChineseName()
        {
            return "收货";
        }

        //protected override T_InStockInfo GetModelByJson(string ModelJson)
        //{
        //    return JSONHelper.JsonToObject<T_InStockInfo>(ModelJson);
        //}

        protected override T_InStockInfo GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;
            T_InStockDetailInfo model = JSONHelper.JsonToObject<T_InStockDetailInfo>(strJson);

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                //BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
                //PFunc.Sync(10, string.Empty, model.ErpVoucherNo, -1, ref errorMsg, "ERP", -1, null);
                ParamaterFiled_DB PDB = new ParamaterFiled_DB();
                PDB.GetVoucherNo(model.ErpVoucherNo, ref errorMsg);
               
            }
            return JSONHelper.JsonToObject<T_InStockInfo>(strJson);
        }

        public string GetInStockStatusByTaskNo(string strTaskNo)
        {
            T_InStock_DB _db = new T_InStock_DB();
            return _db.GetInStockStatusByTaskNo(strTaskNo);
        }

        public int GetInStockVoucherType(string strTaskNo)
        {
            T_InStock_DB _db = new T_InStock_DB();
            return _db.GetInStockVoucherType(strTaskNo);
        }

        
            public bool OpenInStockVoucherNo(int ID, UserModel user, ref string strError)
        {
            try
            {
                bool bSucc = false;
                if (ID == 0)
                {
                    strError = "客户端传入关闭ID为0！";
                    return false;
                }
                T_InStockInfo model = new T_InStockInfo();
                model.ID = ID;
                bSucc = base.GetModelByID(ref model, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                if (model.Status != 5)
                {
                    strError = "单据没有关闭，不能打开！";
                    return false;
                }
                
                string strError1 = string.Empty;
                model.Status = 1;
                //打开WMS单据状态
                bSucc = base.UpadteModelByModelSql(user, model, ref strError1);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError1;
                }
                else
                {
                    strError = strError + "\r\n" + "WMS单据打开成功！";
                }

                return bSucc;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }


        #region 关闭入库单据

        public bool CloseInStockVoucherNo(int ID, UserModel user, ref string strError)
        {
            try
            {
                bool bSucc = false;
                if (ID == 0)
                {
                    strError = "客户端传入关闭ID为0！";
                    return false;
                }
                T_InStockInfo model = new T_InStockInfo();
                model.ID = ID;
                bSucc = base.GetModelByID(ref model, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                if (model.Status != 1)
                {
                    strError = "单据已经开始操作不能关闭！";
                    return false;
                }


                List<T_OutStockInfo> modelpost = new List<T_OutStockInfo>();
                T_OutStockInfo TOutStockInfo = new T_OutStockInfo();
                TOutStockInfo.VoucherType = 50;
                TOutStockInfo.ErpVoucherNo = model.ErpVoucherNo;
                TOutStockInfo.CompanyCode = model.StrongHoldCode;
                TOutStockInfo.StrongHoldCode = model.StrongHoldCode;
                TOutStockInfo.ERPVoucherType = model.ERPVoucherType;
                modelpost.Add(TOutStockInfo);
                bSucc = PostCloseOutStockVoucherNo(modelpost, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                string strError1 = string.Empty;
                model.Status = 5;//5是关闭状态
                //关闭WMS单据状态
                bSucc = base.UpadteModelByModelSql(user, model, ref strError1);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError1;
                }
                else
                {
                    strError = strError + "\r\n" + "WMS单据关闭成功！";
                }

                return bSucc;

            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }
        }

        public bool PostCloseOutStockVoucherNo(List<T_OutStockInfo> modelList, ref string strError)
        {
            try
            {
                BaseMessage_Model<List<T_OutStockInfo>> model = new BaseMessage_Model<List<T_OutStockInfo>>();
                bool bSucc = false;
                //string strUserNo = string.Empty;
                //string strPostUser = string.Empty;
                //string StrongHoldCode = string.Empty;

                //StrongHoldCode = modelList[0].ErpVoucherNo.Substring(0, 3);
                //modelList.ForEach(t => t.VoucherType = 50);
                //modelList.ForEach(t => t.WmsStatus = "E");
                //modelList.ForEach(t => t.StrongHoldCode = StrongHoldCode);

                T_Interface_Func tfunc = new T_Interface_Func();
                string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_OutStockInfo>>(modelList);
                LogNet.LogInfo("closeJSON:" + ERPJson);
                string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

                model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_OutStockInfo>>>(interfaceJson);

                LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_OutStockInfo>>>(model));

                //过账失败直接返回
                if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
                {
                    strError = "回传T100关闭状态失败！" + model.Message;
                    bSucc = false;
                }
                else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
                {
                    strError = "回传T100关闭状态成功！凭证号：" + model.MaterialDoc;
                    bSucc = true;
                }

                return bSucc;
            }
            catch (Exception ex)
            {
                strError = ex.Message;
                return false;
            }


        }

        //public bool PostCloseInStockVoucherNo(List<T_InStockInfo> modelList, ref string strError)
        //{
        //    try
        //    {
        //        BaseMessage_Model<List<T_InStockInfo>> model = new BaseMessage_Model<List<T_InStockInfo>>();
        //        bool bSucc = false;
        //        //string strUserNo = string.Empty;
        //        //string strPostUser = string.Empty;
        //        //string StrongHoldCode = string.Empty;

        //        //StrongHoldCode = modelList[0].ErpVoucherNo.Substring(0, 3);
        //        //modelList.ForEach(t => t.VoucherType = 9993);
        //        //modelList.ForEach(t => t.VoucherType = 50);
        //        //modelList.ForEach(t => t.WmsStatus = "E");
        //        //modelList.ForEach(t => t.StrongHoldCode = StrongHoldCode);

        //        T_Interface_Func tfunc = new T_Interface_Func();
        //        string ERPJson = BILBasic.JSONUtil.JSONHelper.ObjectToJson<List<T_InStockInfo>>(modelList);
        //        string interfaceJson = tfunc.PostModelListToInterface(ERPJson);

        //        model = BILBasic.JSONUtil.JSONHelper.JsonToObject<BaseMessage_Model<List<T_InStockInfo>>>(interfaceJson);

        //        LogNet.LogInfo("ERPJsonAfter:" + BILBasic.JSONUtil.JSONHelper.ObjectToJson<BaseMessage_Model<List<T_InStockInfo>>>(model));

        //        //过账失败直接返回
        //        if (model.HeaderStatus == "E" && !string.IsNullOrEmpty(model.Message))
        //        {
        //            strError = "回传T100关闭状态失败！" + model.Message;
        //            bSucc = false;
        //        }
        //        else if (model.HeaderStatus == "S" && !string.IsNullOrEmpty(model.MaterialDoc)) //过账成功，并且生成了凭证要记录数据库
        //        {
        //            strError = "回传T100关闭状态成功！凭证号：" + model.MaterialDoc;
        //            bSucc = true;
        //        }

        //        return bSucc;
        //    }
        //    catch (Exception ex)
        //    {
        //        strError = ex.Message;
        //        return false;
        //    }


        //}


        #endregion

    }
}

