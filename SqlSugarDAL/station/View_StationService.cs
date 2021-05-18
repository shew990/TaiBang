using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.station
{
    public class View_StationService : DbContext<View_Station>
    {
        public object GetStationsPage(int limit, int page, string lineName, string stationName, string ipAddress)
        {
            var records = GetStationsByWhere(lineName, stationName, ipAddress);
            return new
            {
                Result = 1,
                ResultValue = (records == null || records.Count() == 0) ? "没有符合条件的数据" : "",
                Data = records.Skip(limit * (page - 1)).Take(limit).ToList(),
                PageData = new
                {
                    totalCount = records.Count(),
                    pageSize = limit,
                    currentPage = page,
                    totalPages = records.Count() % limit > 0
                    ? (Math.Floor(Convert.ToDouble(records.Count() / limit)) + 1)
                    : (records.Count() / limit)
                }
            };
        }

        public List<View_Station> GetStationsByWhere(string lineName, string stationName, string ipAddress)
        {
            var records = GetSugarQueryable(x => x.IsDel == 0);
            if (!string.IsNullOrEmpty(lineName))
                records = records.Where(x => x.LineName.Contains(lineName));
            if (!string.IsNullOrEmpty(stationName))
                records = records.Where(x => x.StationName.Contains(stationName));
            //if (!string.IsNullOrEmpty(ipAddress))
            //    records = records.Where(x => x.IpAddress.Contains(ipAddress));
            return records.ToList();
        }

        public SuccessResult DeleteSave(View_Station view_Station)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                StationService stationService = new StationService();
                var station = stationService.GetById((int)view_Station.Id);
                station.IsDel = 1;
                stationService.Update(station);

                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }

        public SuccessResult Submit(View_Station view_Station)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                StationService stationService = new StationService();
                if (view_Station.Id == null)
                {
                    T_Station station = new T_Station();
                    station.LineId = view_Station.LineId;
                    station.StationName = view_Station.StationName;
                    station.CreateTime = DateTime.Now;
                    stationService.Insert(station);//新增
                }
                else
                {
                    var station = stationService.GetById((int)view_Station.Id);
                    station.LineId = view_Station.LineId;
                    station.StationName = view_Station.StationName;
                    station.UpdateTime = DateTime.Now;
                    stationService.Update(station);//修改
                }
                successResult.Msg = "保存成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }
    }
}
