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
            var stations = GetList(x => x.IsDel == 0).OrderBy(x => x.Id).ToList();
            return stations;
        }

        public int GetStationIndex(string ipAddress)
        {
            var index = GetList(x => x.IsDel == 0).FindIndex(x => x.IpAddress == ipAddress);
            return index;
        }

        public T_Station GetStation(string ipAddress)
        {
            var station = GetSugarQueryable(x => x.IpAddress == ipAddress && x.IsDel == 0).First();
            return station;
        }
    }
}
