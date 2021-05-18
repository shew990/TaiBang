using SqlSugarDAL.compter;
using SqlSugarDAL.station;
using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.compter
{
    public class View_CompterService : DbContext<View_Compter>
    {
        public View_Compter GetCompter(string ipAddress)
        {
            var compter = GetList(x => x.IsDel == 0 && x.IpAddress == ipAddress).FirstOrDefault();
            return compter;
        }
    }
}
