using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BILWeb.Product
{
    /// <summary>
    /// t_Productwithtask的实体类
    /// 作者:方颖
    /// 日期：2019/9/5 16:37:49
    /// </summary>

    public class T_ProductDetail : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_ProductDetail() : base() { }

        public int headerid { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string Spec { get; set; }
        public string MaterialDesc { get; set; }
        public decimal Qty { get; set; }

        public string ProMaterialNo { get; set; }
        public string ProMaterialName { get; set; }
        public string ProSpec { get; set; }
        public string ProMaterialDesc { get; set; }
    }
}

