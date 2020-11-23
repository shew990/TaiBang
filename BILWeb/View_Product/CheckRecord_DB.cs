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
    public partial class T_CheckRecord_DB : BILBasic.Basing.Factory.Base_DB<T_CheckRecord>
    {

        /// <summary>
        /// 添加t_Productwithtask
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_CheckRecord t_CheckRecord)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_CheckRecord t_CheckRecord)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_CheckRecord ToModel(IDataReader reader)
        {
            T_CheckRecord t_CheckRecord = new T_CheckRecord();

            t_CheckRecord.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_CheckRecord.HeaderID = dbFactory.ToModelValue(reader, "headerid").ToInt32(); 
            t_CheckRecord.MaterialNo = (string)dbFactory.ToModelValue(reader, "MaterialNo");
            t_CheckRecord.MaterialName = (string)dbFactory.ToModelValue(reader, "MaterialName");
            t_CheckRecord.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            t_CheckRecord.Spec = (string)dbFactory.ToModelValue(reader, "Spec");
            t_CheckRecord.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MaterialDesc");
            t_CheckRecord.Qty = (decimal)dbFactory.ToModelValue(reader, "Qty").ToDecimal();

            t_CheckRecord.ProMaterialNo = (string)dbFactory.ToModelValue(reader, "ProMaterialNo");
            t_CheckRecord.ProMaterialName = (string)dbFactory.ToModelValue(reader, "ProMaterialName");
            t_CheckRecord.ProSpec = (string)dbFactory.ToModelValue(reader, "ProSpec");
            t_CheckRecord.ProMaterialDesc = (string)dbFactory.ToModelValue(reader, "ProMaterialDesc");
            return t_CheckRecord;
        }

        protected override string GetViewName()
        {
            return "v_CheckRecord";
        }

        protected override string GetTableName()
        {
            return "T_CheckRecord";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_CheckRecord model)
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
