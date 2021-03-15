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
