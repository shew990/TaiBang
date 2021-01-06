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
        /// 电脑ip地址
        /// </summary>
        public String IpAddress { get; set; }

        /// <summary>
        /// 是否删除(0：未删除，1：已删除)
        /// </summary>
        public Int32 IsDel { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 产线id
        /// </summary>
        public int LineId { get; set; }
    }
}
