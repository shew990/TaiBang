using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.station
{
    public class StationService : DbContext<T_Station>
    {
        public List<T_Station> GetStations()
        {
            var stations = GetSugarQueryable().OrderBy(x => x.Id).ToList();
            return stations;
        }

        public int GetStationIndex(string ipAddress)
        {
            var index = GetList().FindIndex(x => x.IpAddress == ipAddress);
            return index;
        }
    }
}
