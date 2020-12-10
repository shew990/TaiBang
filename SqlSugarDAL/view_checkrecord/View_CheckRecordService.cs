using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.view_checkrecord
{
    public class View_CheckRecordService : DbContext<View_CheckRecord>
    {
        public object GetOrderList(int limit, int page, string OrderNo, string StartDate, string EndDate)
        {
            var records = GetRecords(OrderNo, StartDate, EndDate);
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

        /// <summary>
        /// 根据条件筛选 质检记录 数据
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <returns></returns>
        public List<View_CheckRecord> GetRecords(string OrderNo, string StartDate, string EndDate)
        {
            var records = GetSugarQueryable();
            if (!string.IsNullOrEmpty(OrderNo))
                records = records.Where(x => x.ErpVoucherNo.Contains(OrderNo));
            if (!string.IsNullOrEmpty(StartDate))
                records = records.Where(x => x.SaveTime >= Convert.ToDateTime(StartDate));
            if (!string.IsNullOrEmpty(EndDate))
                records = records.Where(x => x.SaveTime <= Convert.ToDateTime(EndDate));
            return records.ToList();
        }

    }
}
