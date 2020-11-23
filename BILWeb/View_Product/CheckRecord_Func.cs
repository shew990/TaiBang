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

    public partial class T_CheckRecord_Func : TBase_Func<T_CheckRecord_DB, T_CheckRecord>, ICheckRecord
    {

        protected override bool CheckModelBeforeSave(T_CheckRecord model, ref string strError)
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
            return "记录";
        }

        protected override T_CheckRecord GetModelByJson(string strJson)
        {
            throw new NotImplementedException();
        }

    
    }
}