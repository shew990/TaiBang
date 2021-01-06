using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.view_product
{
    public class View_ProductService:DbContext<View_Product>
    {
        public View_Product GetProduct(string erpOrderNo)
        {
            var product = GetSugarQueryable(x => x.HeadErpVoucherNo == erpOrderNo).First();
            return product;
        }
    }
}
