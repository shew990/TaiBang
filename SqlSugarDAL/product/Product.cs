using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.product
{
    public class T_Product
    {
        public Int32 id { get; set; }

        /// <summary>
        /// 组织编码
        /// </summary>
        public String StrongHoldCode { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public String StrongHoldName { get; set; }

        /// <summary>
        /// 生产单号
        /// </summary>
        public String ErpVoucherNo { get; set; }

        /// <summary>
        /// 单据类型code
        /// </summary>
        public String ErpVoucherTypeCode { get; set; }

        /// <summary>
        /// 单据类型名称
        /// </summary>
        public String ErpVoucherTypeName { get; set; }

        /// <summary>
        /// 生产订单数量
        /// </summary>
        public Decimal ProductQty { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        public String Unit { get; set; }

        public String DepartmentCode { get; set; }

        public String DepartmentName { get; set; }

        /// <summary>
        /// 该生产订单对应的销售订单上的销售部门编码
        /// </summary>
        public String PubDescSeg10_Code { get; set; }
        public String PubDescSeg10_Name { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public String PubDescSeg5 { get; set; }

        /// <summary>
        /// 生产要求
        /// </summary>
        public String PubDescSeg4 { get; set; }

        /// <summary>
        /// 客户型号
        /// </summary>
        public String PubDescSeg7 { get; set; }

        /// <summary>
        /// 产线编码
        /// </summary>
        public String LineCode { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public String LineName { get; set; }
        public String ErpWarehouseNo { get; set; }
        public String ErpWarehouseName { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public String MaterialNo { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public String MaterialDesc { get; set; }

        /// <summary>
        /// 物料规格
        /// </summary>
        public String spec { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public String MaterialName { get; set; }

        public String CustomerCode { get; set; }

        public String CustomerName { get; set; }

        public String CustomerShortName { get; set; }

        /// <summary>
        /// 质检合格数量
        /// </summary>
        public Decimal QulityQty { get; set; }

        /// <summary>
        /// 关联数量
        /// </summary>
        public Decimal LinkQty { get; set; }

        /// <summary>
        /// 提交数量
        /// </summary>
        public Decimal PostQty { get; set; }

        /// <summary>
        /// 完工数量
        /// </summary>
        public Decimal Receiveqty { get; set; }

        /// <summary>
        /// 标准装箱量
        /// </summary>
        public Decimal PackQty { get; set; }

        /// <summary>
        /// 生产订单状态
        /// </summary>
        public String Strstatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 未质检数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Decimal NoQualityQty { get { return this.ProductQty - this.QulityQty; } }
    }
}
