using BILWeb.OutBarCode;
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

    public class T_Product : BILBasic.Basing.Factory.Base_Model
    {
        //无参构造函数
        public T_Product() : base() { }

        public string ErpVoucherTypeCode { get; set; }
        public string ErpVoucherTypeName { get; set; }
        public string BatchNo { get; set; }
        public string Unit { get; set; }

        public string PubDescSeg10_Code { get; set; }
        public string PubDescSeg10_Name { get; set; }
        public string PubDescSeg5 { get; set; }
        public string PubDescSeg4 { get; set; }
        public string PubDescSeg7 { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }


        public string ErpWarehouseNo { get; set; }
        public string ErpWarehouseName { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialDesc { get; set; }
        public string spec { get; set; }
        public string MaterialName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerShortName { get; set; }
        public decimal QulityQty { get; set; }//质检合格数量
        public decimal LinkQty { get; set; }//关联数量
        public decimal PostQty { get; set; }//提交数量
        //public decimal RemainQty { get; set; }//剩余数量
        public decimal ProductQty { get; set; }//生产订单数量
        public decimal ScanQty { get; set; }//扫描数量
        public decimal Receiveqty { get; set; }//完工数量

        public List<T_ProductDetail> Detail { get; set; }
        public List<T_OutBarCodeInfo> lstBarCode { get; set; }
    }
}

