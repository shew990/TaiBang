using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.line
{
    public class LineService : DbContext<T_Line>
    {
        public object GetLinesPage(int limit, int page, string lineName)
        {
            var records = GetSugarQueryable(x => x.LineName.Contains(lineName) && x.IsDel != 1);
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

        public SuccessResult DeleteSave(T_Line line)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                var lineQ = GetById((int)line.Id);
                lineQ.IsDel = 1;
                Update(lineQ);

                successResult.Msg = "删除成功!";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }

        public SuccessResult Submit(T_Line line)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                if (line.Id == null)
                {
                    line.IsDel = 0;
                    line.CreateTime = DateTime.Now;
                    Insert(line);//新增
                }
                else
                {
                    Update(line);//修改
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
