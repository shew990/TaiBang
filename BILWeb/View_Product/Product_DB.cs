using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using System.Data;

namespace BILWeb.Product
{
    public partial class T_Product_DB : BILBasic.Basing.Factory.Base_DB<T_Product>
    {

        /// <summary>
        /// 添加t_Productwithtask
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Product T_Product)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_Product T_Product)
        {
            throw new NotImplementedException();
        }




        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Product ToModel(IDataReader reader)
        {
            T_Product T_Product = new T_Product();

            T_Product.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            T_Product.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            T_Product.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            T_Product.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DepartmentCode");
            T_Product.DepartmentName = (string)dbFactory.ToModelValue(reader, "DepartmentName");

            T_Product.ErpVoucherTypeCode = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeCode");
            T_Product.ErpVoucherTypeName = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeName");
            T_Product.BatchNo = (string)dbFactory.ToModelValue(reader, "BatchNo");
            T_Product.Unit = (string)dbFactory.ToModelValue(reader, "Unit");
            T_Product.PubDescSeg10_Code = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Code");

            T_Product.PubDescSeg10_Name = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Name");
            T_Product.PubDescSeg5 = (string)dbFactory.ToModelValue(reader, "PubDescSeg5");
            T_Product.PubDescSeg4 = (string)dbFactory.ToModelValue(reader, "PubDescSeg4");
            T_Product.PubDescSeg7 = (string)dbFactory.ToModelValue(reader, "PubDescSeg7");

            T_Product.LineCode = (string)dbFactory.ToModelValue(reader, "LineCode");
            T_Product.LineName = (string)dbFactory.ToModelValue(reader, "LineName");
            T_Product.ErpWarehouseNo = (string)dbFactory.ToModelValue(reader, "ErpWarehouseNo");
            T_Product.ErpWarehouseName = (string)dbFactory.ToModelValue(reader, "ErpWarehouseName");

            T_Product.MaterialNo = (string)dbFactory.ToModelValue(reader, "MaterialNo");
            T_Product.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MaterialDesc");
            T_Product.spec = (string)dbFactory.ToModelValue(reader, "spec");
            T_Product.MaterialName = (string)dbFactory.ToModelValue(reader, "MaterialName");

            T_Product.CustomerCode = (string)dbFactory.ToModelValue(reader, "CustomerCode");
            T_Product.CustomerName = (string)dbFactory.ToModelValue(reader, "CustomerName");
            T_Product.CustomerShortName = (string)dbFactory.ToModelValue(reader, "CustomerShortName");
            T_Product.QulityQty = (decimal)dbFactory.ToModelValue(reader, "QulityQty");
            T_Product.LinkQty = (decimal)dbFactory.ToModelValue(reader, "LinkQty");
            T_Product.ProductQty = (decimal)dbFactory.ToModelValue(reader, "ProductQty");
            
            return T_Product;
        }

        protected override string GetViewName()
        {
            return "v_Product";
        }

        protected override string GetTableName()
        {
            return "T_Product";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_Product model)
        {
            string strSql = base.GetFilterSql(user, model);
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }
            return strSql + " order by id desc";
        }


    }
}
