using SqlSugarDAL.product;
using SqlSugarDAL.Until;
using SqlSugarDAL.view_checkrecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.checkrecord
{
    public class CheckRecordService : DbContext<T_CheckRecord>
    {
        public SuccessResult DeleteById(int checkRecordId)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                var objCheckRecord = new View_CheckRecordService().GetSugarQueryable(x => x.Id == checkRecordId).First();
                var result = Db.Ado.UseTran(() =>
                {
                    Delete(checkRecordId);
                    ProductService productService = new ProductService();
                    var objRecord = productService.GetSugarQueryable(x => x.ErpVoucherNo == objCheckRecord.ErpVoucherNo).First();
                    objRecord.QulityQty -= objCheckRecord.RecordQualityQty;
                    productService.Update(objRecord);
                });
                if (!result.IsSuccess)
                    throw new Exception(result.ErrorMessage);
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
