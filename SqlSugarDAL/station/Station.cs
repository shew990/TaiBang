using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.station
{
    /// <summary>
    /// 工位表
    /// </summary>
    public class T_Station
    {
        public int Id { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public String StationName { get; set; }

        /// <summary>
        /// pdf文件地址
        /// </summary>
        public String PDFAddress { get; set; }

        /// <summary>
        /// 产线id
        /// </summary>
        public int LineId { get; set; }
    }
}
