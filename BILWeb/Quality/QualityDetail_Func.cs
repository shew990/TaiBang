using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using BILBasic.User;
using BILWeb.Login.User;
using BILWeb.OutStock;
using BILBasic.Interface;

namespace BILWeb.Quality
{

    public partial class T_QualityDetail_Func : TBase_Func<T_QualityDetail_DB, T_QualityDetailInfo>, IQualityDetailService
    {
        T_Stock_Func stockFunc = new T_Stock_Func();

        protected override bool CheckModelBeforeSave(T_QualityDetailInfo model, ref string strError)
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
            return "检验单行";
        }


        protected override T_QualityDetailInfo GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;

            T_QualityDetailInfo model = JSONHelper.JsonToObject<T_QualityDetailInfo>(strJson);

            return model;
        }
        

        protected override List<T_QualityDetailInfo> GetModelListByJson(string UserJson, string ModelListJson)
        {
            UserModel userModel = JSONHelper.JsonToObject<UserModel>(UserJson); 
            List<T_QualityDetailInfo> modelList = JSONHelper.JsonToObject<List<T_QualityDetailInfo>>(ModelListJson);

            string strUserNo = string.Empty;
            string strPostUser = string.Empty;

            //if (TOOL.RegexMatch.isExists(userModel.UserNo) == true)
            //{
            //    strUserNo = userModel.UserNo.Substring(0, userModel.UserNo.Length - 1);
            //}
            //else
            //{
            //    strUserNo = userModel.UserNo;
            //}

            //User_DB _db = new User_DB();
            //strPostUser = _db.GetPostAccountByUserNo(strUserNo, modelList[0].StrongHoldCode);

            foreach (var item in modelList)
            {
                item.FromErpWarehouse = item.lstStock.FirstOrDefault().WarehouseNo;
                item.FromErpAreaNo = item.lstStock.FirstOrDefault().AreaNo;
                item.FromBatchNo = item.lstStock.FirstOrDefault().BatchNo;
                item.ToErpWarehouse = userModel.ToSampWareHouseNo;
                item.ToErpAreaNo = userModel.ToSampAreaNo;
                item.PostUser = userModel.UserNo;// strPostUser;
            }

            LogNet.LogInfo("UpadteT_QualityUserADF-----" + JSONHelper.ObjectToJson<List<T_QualityDetailInfo>>(modelList));

            return modelList;  
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErpVoucherNo"></param>
        /// <returns></returns>
        public bool GetQualityUpadteStock(string ErpVoucherNo,ref string strError) 
        {
            T_QualityDetail_DB _db = new T_QualityDetail_DB();
            List<T_QualityDetailInfo> modelList = new List<T_QualityDetailInfo>();
            modelList = _db.GetQualityUpadteStock(ErpVoucherNo);

            if (modelList == null || modelList.Count==0) 
            {
                strError = "未获取到检验单信息！";
                return false;
            }

            return stockFunc.UpdateStockByQuality(modelList,ref  strError);
        }


        #region 删除质检单据

        public bool CloseQualityDetailVoucherNo(int ID, UserModel user, ref string strError)
        {
            try
            {
                bool bSucc = false;
                if (ID == 0)
                {
                    strError = "客户端传入关闭ID为0！";
                    return false;
                }
                T_QualityDetailInfo model = new T_QualityDetailInfo();
                model.ID = ID;
                bSucc = base.GetModelByID(ref model, ref strError);
                if (bSucc == false)
                {
                    return false;
                }
                
                List<T_OutStockInfo> modelpost = new List<T_OutStockInfo>();
                T_OutStockInfo TOutStockInfo = new T_OutStockInfo();
                TOutStockInfo.VoucherType = 50;
                TOutStockInfo.ErpVoucherNo = model.ErpVoucherNo;
                TOutStockInfo.CompanyCode = model.StrongHoldCode;
                TOutStockInfo.StrongHoldCode = model.StrongHoldCode;
                TOutStockInfo.ERPVoucherType = model.ErpVoucherNo.Substring(4,2);
                modelpost.Add(TOutStockInfo);
                bSucc = PostCloseOutStockVoucherNo(modelpost, ref strError);
                if (bSucc == false)
                {
                    return false;
                }

                string strError1 = string.Empty;
                //关闭WMS单据状态
                bSucc = base.DeleteModelByModelSql(user, model, ref strError1);
                //bSucc = base.DeleteModelByID(user, model.ID, ref strError1);

                if (bSucc == false)
                {
                    strError = strError + "\r\n" + strError1;
                }
                else
                {
                    strError = strError + "\r\n" + "WMS单据删除成功！";
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
                else if (model.HeaderStatus == "S") //过账成功，并且生成了凭证要记录数据库//&& !string.IsNullOrEmpty(model.MaterialDoc)
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

        #endregion

    }
}