using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.View_Product
{
    public partial class View_Product_Func : TBase_Func<View_Product_DB, View_Product_Model>, IView_ProductService
    {
        protected override bool CheckModelBeforeSave(View_Product_Model model, ref string strError)
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
            return "生产工单";
        }
    }
}
