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

        ///// <summary>
        ///// 表头id
        ///// </summary>
        //public int headerid { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string MaterialNo { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 物料说明
        /// </summary>
        public string MaterialDesc { get; set; }

        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 外箱物料号
        /// </summary>
        public string ProMaterialNo { get; set; }

        /// <summary>
        /// 外箱物料名称
        /// </summary>
        public string ProMaterialName { get; set; }

        /// <summary>
        /// 外箱物料规格
        /// </summary>
        public string ProSpec { get; set; }

        /// <summary>
        /// 外箱物料说明
        /// </summary>
        public string ProMaterialDesc { get; set; }

        //PDA字段
        public string Barcode { get; set; }//PDA上传的扫描条码

        public bool IsScan { get; set; }

        public bool Isbox { get; set; }

        public decimal PrintQty { get; set; }

        public String ProductBatch { get; set; }

    }
}

