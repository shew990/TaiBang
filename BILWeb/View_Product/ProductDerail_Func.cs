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

namespace BILWeb.Product
{

    public partial class T_ProductDetail_Func : TBase_Func<T_ProductDetail_DB, T_ProductDetail>, IProductDerailService
    {

        protected override bool CheckModelBeforeSave(T_ProductDetail model, ref string strError)
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
            return "生产订单表体";
        }

        protected override T_ProductDetail GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    
    }
}