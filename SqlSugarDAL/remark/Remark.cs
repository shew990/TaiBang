using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.remark
{
    public class Remark
    {
        /// <summary>
        /// 表头id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 备注描述
        /// </summary>
        public string RemarkDesc { get; set; }
    }
}
