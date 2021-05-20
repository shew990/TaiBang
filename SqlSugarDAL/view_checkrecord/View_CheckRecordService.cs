using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.view_checkrecord
{
    public class View_CheckRecordService : DbContext<View_CheckRecord>
    {
        public object GetOrderList(int limit, int page, string OrderNo, string StartDate, string EndDate, string strongHoldCode)
        {
            var records = GetRecords(OrderNo, StartDate, EndDate, strongHoldCode);
            var recordsItem = new List<View_CheckRecord>();
            records.ForEach(x =>
            {
                recordsItem = records.FindAll(y => y.ErpVoucherNo == y.ErpVoucherNo);
                var sumQualityQty = recordsItem.Sum(z => z.RecordQualityQty);
                var sumCheckQty = recordsItem.Sum(a => a.CheckQty);
                x.PassRateAll = sumCheckQty == 0 ? "0.00" 
                : (sumQualityQty / sumCheckQty * 100).ToString("0.00");
            });
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
        public List<View_CheckRecord> GetRecords(string OrderNo, string StartDate, string EndDate, string strongHoldCode)
        {
            var records = GetSugarQueryable(x => x.StrongHoldCode == strongHoldCode);
            if (!string.IsNullOrEmpty(OrderNo))
                records = records.Where(x => x.ErpVoucherNo.Contains(OrderNo));
            if (!string.IsNullOrEmpty(StartDate))
                records = records.Where(x => x.SaveTime >= Convert.ToDateTime(StartDate));
            if (!string.IsNullOrEmpty(EndDate))
                records = records.Where(x => x.SaveTime <= Convert.ToDateTime(EndDate));
            return records.ToList();
        }

        /// <summary>
        /// 根据id获取视图对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public View_CheckRecord GetRecord(int checkRecordId)
        {
            return GetSugarQueryable(x => x.Id == checkRecordId).First();
        }

        public decimal GetOrderNoQualityQty(string orderNo)
        {
            var records = GetSugarQueryable(x => x.ErpVoucherNo == orderNo);
            if (records.Count() == 0)
                return 0;
            var sumNoQualituQty = records.Sum(x => x.RecordNoQualityQty);
            var sumBackQualityQty = records.Sum(x => x.BackQualityQty);
            return sumNoQualituQty - sumBackQualityQty;
        }

    }
}
