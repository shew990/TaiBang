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
    public partial class T_ProductDetail_DB : BILBasic.Basing.Factory.Base_DB<T_ProductDetail>
    {

        /// <summary>
        /// 添加t_Productwithtask
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_ProductDetail t_ProductDetail)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_ProductDetail t_ProductDetail)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_ProductDetail ToModel(IDataReader reader)
        {
            T_ProductDetail t_ProductDetail = new T_ProductDetail();

            t_ProductDetail.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_ProductDetail.HeaderID = dbFactory.ToModelValue(reader, "headerid").ToInt32(); 
            t_ProductDetail.MaterialNo = (string)dbFactory.ToModelValue(reader, "MaterialNo");
            t_ProductDetail.MaterialName = (string)dbFactory.ToModelValue(reader, "MaterialName");
            t_ProductDetail.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_ProductDetail.Spec = (string)dbFactory.ToModelValue(reader, "Spec");
            t_ProductDetail.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MaterialDesc");
            t_ProductDetail.Qty = (decimal)dbFactory.ToModelValue(reader, "Qty").ToDecimal();

            t_ProductDetail.ProMaterialNo = (string)dbFactory.ToModelValue(reader, "ProMaterialNo");
            t_ProductDetail.ProMaterialName = (string)dbFactory.ToModelValue(reader, "ProMaterialName");
            t_ProductDetail.ProSpec = (string)dbFactory.ToModelValue(reader, "ProSpec");
            t_ProductDetail.ProMaterialDesc = (string)dbFactory.ToModelValue(reader, "ProMaterialDesc");
            return t_ProductDetail;
        }

        protected override string GetViewName()
        {
            return "v_ProductDetail";
        }

        protected override string GetTableName()
        {
            return "T_ProductDetail";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_ProductDetail model)
        {
            string strSql = " where 1=1 ";
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }
            return strSql;
        }

        

    }
}
