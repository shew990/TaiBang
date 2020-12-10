using BILBasic.Basing.Factory;
using BILBasic.Common;
using BILBasic.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BILWeb.View_Product
{
    public partial class View_Product_DB : Base_DB<View_Product_Model>
    {
        protected override IDataParameter[] GetSaveModelIDataParameter(View_Product_Model model)
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref View_Product_Model model)
        {
            throw new NotImplementedException();
        }

        protected override string GetViewName()
        {
            return "View_Product";
        
        }

        protected override string GetTableName()
        {
            return "t_palletdetail";
        }

        protected override string GetFilterSql(UserModel user, View_Product_Model model)
        {
            //string strSql = base.GetFilterSql(user, model);
            string strSql = "where 1=1 ";
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }
            return strSql;
        }

        protected override View_Product_Model ToModel(IDataReader reader)
        {
            View_Product_Model model = new View_Product_Model();
            model.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            model.headerid = dbFactory.ToModelValue(reader, "headerid").ToInt32();
            model.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            model.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            model.ErpVoucherTypeCode = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeCode");
            model.HeadErpVoucherNo = (string)dbFactory.ToModelValue(reader, "HeadErpVoucherNo");
            model.ErpVoucherTypeName = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeName");
            model.ProductQty = (decimal)dbFactory.ToModelValue(reader, "ProductQty");
            model.BatchNo = (string)dbFactory.ToModelValue(reader, "BatchNo");
            model.Unit = (string)dbFactory.ToModelValue(reader, "Unit");
            model.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DepartmentCode");
            model.DepartmentName = (string)dbFactory.ToModelValue(reader, "DepartmentName");
            model.PubDescSeg10_Code = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Code");
            model.PubDescSeg5 = (string)dbFactory.ToModelValue(reader, "PubDescSeg5");
            model.PubDescSeg10_Name = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Name");
            model.PubDescSeg4 = (string)dbFactory.ToModelValue(reader, "PubDescSeg4");
            model.PubDescSeg7 = (string)dbFactory.ToModelValue(reader, "PubDescSeg7");
            model.LineCode = (string)dbFactory.ToModelValue(reader, "LineCode");
            model.LineName = (string)dbFactory.ToModelValue(reader, "LineName");
            model.ErpWarehouseNo = (string)dbFactory.ToModelValue(reader, "ErpWarehouseNo");
            model.ErpWarehouseName = (string)dbFactory.ToModelValue(reader, "ErpWarehouseName");
            model.HeadMaterialNo = (string)dbFactory.ToModelValue(reader, "HeadMaterialNo");
            model.HeadMaterialDesc = (string)dbFactory.ToModelValue(reader, "HeadMaterialDesc");
            model.HeadMaterialName = (string)dbFactory.ToModelValue(reader, "HeadMaterialName");
            model.HeadSpec = (string)dbFactory.ToModelValue(reader, "HeadSpec");
            model.CustomerCode = (string)dbFactory.ToModelValue(reader, "CustomerCode");
            model.CustomerName = (string)dbFactory.ToModelValue(reader, "CustomerName");
            model.CustomerShortName = (string)dbFactory.ToModelValue(reader, "CustomerShortName");
            model.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MaterialDesc");
            model.Spec = (string)dbFactory.ToModelValue(reader, "Spec");
            model.QulityQty = (decimal)dbFactory.ToModelValue(reader, "QulityQty");
            model.MaterialName = (string)dbFactory.ToModelValue(reader, "MaterialName");
            model.MaterialNo = (string)dbFactory.ToModelValue(reader, "MaterialNo");
            model.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            model.ProductBatch = (string)dbFactory.ToModelValue(reader, "ProductBatch");

            return model;
        }
    }
}
