using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.Until
{
    public class SuccessResult
    {
        public Object Data { get; set; }

        public String Msg { get; set; }

        public Boolean Success { get; set; }

        public Int32 ErrorCode { get; set; }
    }
}
