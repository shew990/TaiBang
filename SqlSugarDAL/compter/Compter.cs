using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.compter
{
    /// <summary>
    /// 电脑表
    /// </summary>
    public class T_Compter
    {
        public int Id { get; set; }

        public String IpAddress { get; set; }

        public int StationId { get; set; }
    }
}
