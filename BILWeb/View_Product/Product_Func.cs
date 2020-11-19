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


    }
}