﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.JSONUtil;
using BILBasic.User;
using Newtonsoft.Json;
using BILWeb.Product;
using BILWeb.Print;
using BILWeb.T00L;
using BILWeb.SyncService;

namespace BILWeb.Product
{

    public partial class T_Product_Func : TBase_Func<T_Product_DB, T_Product>, IProductService
    {

        protected override bool CheckModelBeforeSave(T_Product model, ref string strError)
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
            return "生产订单表头";
        }

        protected override T_Product GetModelByJson(string strJson)
        {
            string errorMsg = string.Empty;
            T_Product model = JSONHelper.JsonToObject<T_Product>(strJson);

            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                //BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();
                //PFunc.Sync(10, string.Empty, model.ErpVoucherNo, -1, ref errorMsg, "ERP", -1, null);
                ParamaterFiled_DB PDB = new ParamaterFiled_DB();
                PDB.GetVoucherNo(model.ErpVoucherNo, ref errorMsg,"1");

            }
            return JSONHelper.JsonToObject<T_Product>(strJson);
        }


        protected override List<T_Product> GetModelListByJson(string UserJson, string ModelListJson)
        {
            List<T_Product> modelList = new List<T_Product>();
            modelList = JSONHelper.JsonToObject<List<T_Product>>(ModelListJson);
            modelList = modelList.Where(t => t.ScanQty > 0).ToList();
            return modelList;
        }
        
        
        public string GetModelList(string UserJson, string ModelJson)
        {

            BaseMessage_Model<List<T_Product>> messageModel = new BaseMessage_Model<List<T_Product>>();
            try
            {
                string strError = string.Empty;
                if (string.IsNullOrEmpty(UserJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "用户JSON为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }
                if (string.IsNullOrEmpty(ModelJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "查询数据为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }
                UserModel user = JSONHelper.JsonToObject<UserModel>(UserJson);
                T_Product productmodel = JSONHelper.JsonToObject<T_Product>(ModelJson);
                List<T_Product> modellist = new List<T_Product>();
                T_Product_DB db = new T_Product_DB();
                modellist = db.GetModelListADF(user, productmodel);
                modellist.ForEach(item =>
                {
                    T_ProductDetail_DB dbdetail = new T_ProductDetail_DB();
                    List<T_ProductDetail> modellistdetail = new List<T_ProductDetail>();
                    T_ProductDetail detail = new T_ProductDetail() { HeaderID = item.ID };
                    modellistdetail = dbdetail.GetModelListADF(user, detail);
                    item.Detail = modellistdetail;
                });
                if (modellist == null || modellist.Count == 0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "生产订单查询为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                if (modellist[0].StrongHoldCode!=user.StrongHoldCode)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = " 该生产订单【"+ modellist[0].StrongHoldCode + "】不属于该当前用户所属的事业部【"+ user.StrongHoldCode + "】！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //获取成功
                messageModel.ModelJson = modellist;
                messageModel.HeaderStatus = "S";
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.ToString();
                return JsonConvert.SerializeObject(messageModel);
            }

        }

        public string SaveT_ProDuctBarcodeADF(string user, string modelList,string PrintIP, string OutBarcode)
        {

            BaseMessage_Model<List<T_Product>> messageModel = new BaseMessage_Model<List<T_Product>>();
            try
            {
                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(modelList))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入参数不能为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                UserModel userModel = JSONHelper.JsonToObject<UserModel>(user);
                T_Product product = JSONHelper.JsonToObject<T_Product>(modelList);

                //string ipport = "";
                string ErrMsg = "";
                T_Product_DB ProductDB = new T_Product_DB();
                T_Product Newproduct = ProductDB.GetModelListADF(userModel, product)[0];
                Newproduct.ScanQty = product.ScanQty;
                Newproduct.Detail = product.Detail;
                
                if (Newproduct.ProductQty< Newproduct.LinkQty+ product.ScanQty)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入的关联数量超过未关联数量，不合法！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                Barcode_Model BarcodeModel = new Barcode_Model();
                if (!string.IsNullOrEmpty(OutBarcode))
                {
                    BarcodeModel.BarCode = OutBarcode;
                }
               
                if (ProductDB.SaveProDuctBarcode(userModel, Newproduct, ref BarcodeModel))
                {
                    string date = DateTime.Now.ToString("yyyyMMddHHmmss");
                    BarcodeModel.PrintName = PrintIP;
                    string data = JsonConvert.SerializeObject(BarcodeModel);
                    //HttpTool.Post("http://192.168.250.37:8022/api/print/PrintOuterBox", data, date);
                    if (true)
                    {
                        messageModel.HeaderStatus = "S";
                        messageModel.Message = "打印成功";
                        return JsonConvert.SerializeObject(messageModel);
                    }
                    else
                    {
                        messageModel.HeaderStatus = "E";
                        messageModel.Message = ErrMsg;
                        return JsonConvert.SerializeObject(messageModel);
                    }
                }
                else
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "数据插入失败！";
                    return JsonConvert.SerializeObject(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.ToString();
                return JsonConvert.SerializeObject(messageModel);
            }

        }

        protected override string GetModelListByJsonToERP(UserModel user, List<T_Product> modelList, string strPost = "")
        {
            modelList.ForEach(item=>item.PostQty=item.ScanQty);
            modelList.ForEach(item => item.GUID = user.GUID);
            //modelList.ForEach(item => item.PostUser = user.UserNo);
            modelList.ForEach(item => item.PostUser = user.UserName);
            return JSONHelper.ObjectToJson<List<T_Product>>(modelList);

        }

        public string SaveT_BarcodeADF(string BarcodeJson)
        {
            LogNet.LogInfo("U9生成条码类BarcodeJson:" + BarcodeJson);
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();
            try
            {
                if (string.IsNullOrEmpty(BarcodeJson))
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "传入参数不能为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                List<Barcode_Model> BarcodeModel = JSONHelper.JsonToObject<List<Barcode_Model>>(BarcodeJson);
                T_Product_DB ProductDB = new T_Product_DB();
                string strMsg = "";
                if (ProductDB.SaveBarcode(BarcodeModel,ref strMsg))
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "打印成功";
                    return JsonConvert.SerializeObject(messageModel);
                }
                else
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "数据插入失败！"+ strMsg;
                    return JsonConvert.SerializeObject(messageModel);
                }
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.ToString();
                return JsonConvert.SerializeObject(messageModel);
            }

        }
        


        public string CloseProduct(string ErpVoucherno) {
            BaseMessage_Model<string> messageModel = new BaseMessage_Model<string>();
            try
            {
                T_Product_DB ProD = new T_Product_DB();
                if (ProD.CloseProduct(ErpVoucherno))
                {
                    messageModel.HeaderStatus = "S";
                    messageModel.Message = "成功！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                messageModel.HeaderStatus = "E";
                messageModel.Message = "传入参数不能为空！";
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex)
            {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.ToString();
                return JsonConvert.SerializeObject(messageModel);
            }
        }
    }
}