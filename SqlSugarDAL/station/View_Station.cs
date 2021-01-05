using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.station
{
    public class View_Station
    {
        public Int32? Id { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public String LineName { get; set; }

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

        [SugarColumn(IsIgnore = true)]
        public string CreateTimeString
        {
            get
            {
                return this.CreateTime == null ? "" : ((DateTime)this.CreateTime).ToString("yyyy/MM/dd");
            }
        }

        public DateTime? UpdateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string UpdateTimeString
        {
            get
            {
                return this.UpdateTime == null ? "" : ((DateTime)this.UpdateTime).ToString("yyyy/MM/dd");
            }
        }

        /// <summary>
        /// 产线id
        /// </summary>
        public int LineId { get; set; }
    }
}
