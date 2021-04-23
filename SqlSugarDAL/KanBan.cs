using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL
{
    public class ReturnKanban
    {
        public int result = 0;
        public string resultValue = "";
        public List<Kanban> data;
    }

    public class Kanban
    {
        /// <summary>
        /// 日期--
        /// </summary>
        public string BusinessDate { get; set; }

        public string BusinessDateShow
        {
            get
            {
                return (this.BusinessDate == null || this.BusinessDate == "") ? ""
                    : Convert.ToDateTime(this.BusinessDate).ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 单号--
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 客户编码--
        /// </summary>
        public string Customer_Code { get; set; }

        /// <summary>
        /// 发货方式名称--
        /// </summary>
        public string TransportModeName { get; set; }

        /// <summary>
        /// 发货方式编码--
        /// </summary>
        public string TransportModeCode { get; set; }

        /// <summary>
        /// 单据数量--
        /// </summary>
        public decimal Qty { get; set; }



        //形态转换看板

        /// <summary>
        /// 转换类型--
        /// </summary>
        public string TransferType { get; set; }

        /// <summary>
        /// 跟单员--
        /// </summary>
        public string BussinessMan { get; set; }

        /// <summary>
        /// 加急标识--
        /// </summary>
        public string EmergencyFlag { get; set; }

        public String EmergencyFlagShow
        {
            get
            {
                return (this.EmergencyFlag == "" || this.EmergencyFlag == null) ? ""
                    : this.EmergencyFlag == "True" ? "加急" : "";
            }
        }

        /// <summary>
        /// 完成状态名称--
        /// </summary>
        public string Status { get; set; }

        public String StatusShow
        {
            get
            {
                return (this.Status == "" || this.Status == null) ? "" :
                    this.Status == "Open" ? "开立" : this.Status == "Approving" ? "核准中" : "已核准";
            }
        }

        /// <summary>
        /// 预备货员--
        /// </summary>
        public string PreStocker { get; set; }

        /// <summary>
        /// 已备货数量
        /// </summary>
        public Decimal? SHELVEQTY { get; set; }

        /// <summary>
        /// 备货人
        /// </summary>
        public String CREATER { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public String BackColor { get; set; }

        /// <summary>
        /// 备货数量
        /// </summary>
        public Decimal? PrepareQty { get; set; }

        /// <summary>
        /// 实际备货人
        /// </summary>
        public String Wms_user { get; set; }

    }
}
