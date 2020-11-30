using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.seepalletinfo
{
    public class SeePalletInfo
    {
        public int Id { get; set; }

        /// <summary>
        /// 产线
        /// </summary>
        public String Line { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public String Station { get; set; }

        /// <summary>
        /// pdf文件地址
        /// </summary>
        public String PdfUrl { get; set; }

        /// <summary>
        /// mac地址
        /// </summary>
        public String MacAddress { get; set; }
    }
}
