using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.view_product
{
    public class View_Product
    {
        public Int32 headerid { get; set; }

        public String ErpVoucherTypeCode { get; set; }

        public String HeadErpVoucherNo { get; set; }

        public String ErpVoucherTypeName { get; set; }

        /// <summary>
        /// 生产订单数量
        /// </summary>
        public Decimal ProductQty { get; set; }

        public String BatchNo { get; set; }

        public String Unit { get; set; }

        public String PubDescSeg10_Code { get; set; }

        public String PubDescSeg5 { get; set; }

        public String PubDescSeg10_Name { get; set; }

        public String PubDescSeg4 { get; set; }

        public String PubDescSeg7 { get; set; }

        public String LineCode { get; set; }

        public String LineName { get; set; }

        public String ErpWarehouseNo { get; set; }

        public String ErpWarehouseName { get; set; }

        /// <summary>
        /// 外箱物料编号
        /// </summary>
        public String HeadMaterialNo { get; set; }

        /// <summary>
        /// 外箱物料说明
        /// </summary>
        public String HeadMaterialDesc { get; set; }

        /// <summary>
        /// 外箱物料名称
        /// </summary>
        public String HeadMaterialName { get; set; }

        /// <summary>
        /// 外箱规格
        /// </summary>
        public String HeadSpec { get; set; }

        public String CustomerCode { get; set; }

        public String CustomerName { get; set; }

        public String CustomerShortName { get; set; }

        public String MaterialDesc { get; set; }

        public String Spec { get; set; }

        /// <summary>
        /// 质检合格数量
        /// </summary>
        public Decimal QulityQty { get; set; }

        public String MaterialName { get; set; }

        public String MaterialNo { get; set; }

        /// <summary>
        /// 未质检数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Decimal NoQualityQty { get { return this.ProductQty - this.QulityQty; } }
    }
}
