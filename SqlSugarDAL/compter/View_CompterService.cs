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
        public int GetStationIndex(string ipAddress)
        {
            var compter = GetList(x => x.IsDel == 0 && x.IpAddress == ipAddress).FirstOrDefault();
            if (compter == null)
                return -1;
            var index = new StationService().GetList(x => x.LineId == compter.LineId)
                .FindIndex(x => x.Id == compter.StationId);
            return index;
        }
    }
}
