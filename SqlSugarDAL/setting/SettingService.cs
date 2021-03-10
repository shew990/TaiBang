using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.setting
{
    public class SettingService : DbContext<T_Setting>
    {
        public object GetSettingsPage(int limit, int page)
        {
            var settings = GetSugarQueryable();
            return new
            {
                Result = 1,
                ResultValue = (settings == null || settings.Count() == 0) ? "没有符合条件的数据" : "",
                Data = settings.Skip(limit * (page - 1)).Take(limit).ToList(),
                PageData = new
                {
                    totalCount = settings.Count(),
                    pageSize = limit,
                    currentPage = page,
                    totalPages = settings.Count() % limit > 0
                    ? (Math.Floor(Convert.ToDouble(settings.Count() / limit)) + 1)
                    : (settings.Count() / limit)
                }
            };
        }

        public SuccessResult DeleteSave(T_Setting setting)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                Delete(setting);

                successResult.Msg = "删除成功";
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return successResult;
        }

        public SuccessResult Submit(string userNo, T_Setting setting)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                if (setting.Id == null)
                {
                    setting.Id = Guid.NewGuid().ToString();
                    setting.Creater = userNo;
                    setting.CreateTime = DateTime.Now;
                    Insert(setting);//新增
                }
                else
                {
                    setting.Updater = userNo;
                    setting.UpdateTime = DateTime.Now;
                    Update(setting);//修改
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
