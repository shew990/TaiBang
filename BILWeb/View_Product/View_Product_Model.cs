using BILBasic.Basing.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.View_Product
{
    public class View_Product_Model : Base_Model
    {
        //无参构造函数
        public View_Product_Model() : base() { }

        public Int32? id { get; set; }

        public Int32? headerid { get; set; }

        public String StrongHoldCode { get; set; }

        public String StrongHoldName { get; set; }

        public String ErpVoucherTypeCode { get; set; }

        public String HeadErpVoucherNo { get; set; }

        public String ErpVoucherTypeName { get; set; }

        public Decimal? ProductQty { get; set; }

        public String BatchNo { get; set; }

        public String Unit { get; set; }

        public String DepartmentCode { get; set; }

        public String DepartmentName { get; set; }

        public String PubDescSeg10_Code { get; set; }

        public String PubDescSeg5 { get; set; }

        public String PubDescSeg10_Name { get; set; }

        public String PubDescSeg4 { get; set; }

        public String PubDescSeg7 { get; set; }

        public String LineCode { get; set; }

        public String LineName { get; set; }

        public String ErpWarehouseNo { get; set; }

        public String ErpWarehouseName { get; set; }

        public String HeadMaterialNo { get; set; }

        public String HeadMaterialDesc { get; set; }

        public String HeadMaterialName { get; set; }

        public String HeadSpec { get; set; }

        public String CustomerCode { get; set; }

        public String CustomerName { get; set; }

        public String CustomerShortName { get; set; }

        public String MaterialDesc { get; set; }

        public String Spec { get; set; }

        public Decimal? QulityQty { get; set; }

        public String MaterialName { get; set; }

        public String MaterialNo { get; set; }

        public String ErpVoucherNo { get; set; }
    }
}
