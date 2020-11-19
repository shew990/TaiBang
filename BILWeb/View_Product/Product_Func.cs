using System;
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
            throw new NotImplementedException();
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
                    T_ProductDetail detail = new T_ProductDetail() { headerid = item.ID };
                    modellistdetail = dbdetail.GetModelListADF(user, detail);
                    item.Detail = modellistdetail;
                });
                if (modellist == null|| modellist.Count==0)
                {
                    messageModel.HeaderStatus = "E";
                    messageModel.Message = "生产订单查询为空！";
                    return JsonConvert.SerializeObject(messageModel);
                }

                //获取成功
                messageModel.ModelJson = modellist;
                messageModel.HeaderStatus = "S";
                return JsonConvert.SerializeObject(messageModel);
            }
            catch (Exception ex) {
                messageModel.HeaderStatus = "E";
                messageModel.Message = ex.ToString();
                return JsonConvert.SerializeObject(messageModel);
            }
            
        }



    }
}