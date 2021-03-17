using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.setting
{
    public class T_Setting
    {
        public String Id { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        public String HouseNo { get; set; }

        /// <summary>
        /// 单据类型：(0:出货单看板，1:形体转换看板，2:待定，3：待定)
        /// </summary>
        public Int32? OrderType { get; set; }

        [SugarColumn(IsIgnore = true)]
        public String OrderTypeShow
        {
            get
            {
                return this.OrderType == 0 ? "出货单看板" :
                    this.OrderType == 1 ? "形态转换看板" : this.OrderType == 2 ? "待定" : "待定";
            }
        }

        /// <summary>
        /// 看板地址
        /// </summary>
        public String PalletUrl { get; set; }

        public String Creater { get; set; }

        public DateTime? CreateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public String CreateTimeString
        {
            get
            {
                return this.CreateTime == null ? "" : ((DateTime)this.CreateTime).ToString("yyyy-MM-dd");
            }
        }

        public String Updater { get; set; }

        public DateTime? UpdateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public String UpdateTimeString
        {
            get
            {
                return this.UpdateTime == null ? "" : ((DateTime)this.UpdateTime).ToString("yyyy-MM-dd");
            }
        }
    }
}
