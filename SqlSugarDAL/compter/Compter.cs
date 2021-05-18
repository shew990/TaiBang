using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.compter
{
    public class T_Compter
    {
        public int Id { get; set; }

        /// <summary>
        /// 电脑ip地址
        /// </summary>
        public String IpAddress { get; set; }

        /// <summary>
        /// 工位id
        /// </summary>
        public int StationId { get; set; }
    }
}
