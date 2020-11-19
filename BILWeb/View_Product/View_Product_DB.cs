using BILBasic.Basing.Factory;
using BILBasic.User;
using DAL;
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

        protected override string GetTableName()
        {
            throw new NotImplementedException();
        }

        protected override string GetViewName()
        {
            throw new NotImplementedException();
        }

        protected override View_Product_Model ToModel(IDataReader reader)
        {
            throw new NotImplementedException();
        }
    }
}
