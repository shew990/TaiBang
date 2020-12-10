using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.view_checkrecord
{
    public class View_CheckRecordService : DbContext<View_CheckRecord>
    {
        public object GetOrderList(int limit, int page)
        {
            var records = GetSugarQueryable().ToList();
            var data = records.Skip(limit * (page - 1)).Take(limit);
            return new
            {
                Result = 1,
                ResultValue = (records == null || records.Count() == 0) ? "没有数据" : "",
                Data = data,
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

    }
}
