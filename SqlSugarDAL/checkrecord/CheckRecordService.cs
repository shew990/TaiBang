using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.checkrecord
{
    public class CheckRecordService : DbContext<T_CheckRecord>
    {
        public SuccessResult DeleteById(string checkRecordId)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                Delete(checkRecordId);

                successResult.Msg = "删除成功!";
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
