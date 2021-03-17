using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Configuration;
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
                setting.PalletUrl = ConfigurationManager.AppSettings["PalletUrl"]
                                    + setting.HouseNo + "&OrderType=" + setting.OrderType;
                var settings = GetList();
                if (settings.Find(x => x.PalletUrl == setting.PalletUrl) != null)
                {
                    successResult.Msg = "该仓库该看板类型地址已存在!";
                    return successResult;
                }

                setting.Id = Guid.NewGuid().ToString();
                setting.Creater = userNo;
                setting.CreateTime = DateTime.Now;
                Insert(setting);//新增

                successResult.Msg = "生成成功!";
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
