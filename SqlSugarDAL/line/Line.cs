using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.line
{
    public class T_Line
    {
        public Int32? Id { get; set; }

        public String LineName { get; set; }

        public int IsDel { get; set; }

        public String Creater { get; set; }

        public DateTime? CreateTime { get; set; }

        [SugarColumn(IsIgnore = true)]
        public String CreateTimeString
        {
            get
            {
                return this.CreateTime == null
                    ? "" : ((DateTime)this.CreateTime).ToString("yyyy-MM-dd");
            }
        }
    }
}
