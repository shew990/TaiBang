using SqlSugar;
using System;
using System.Linq;
using System.Text;

namespace SqlSugarDAL.view_checkrecord
{
    ///<summary>
    ///
    ///</summary>
    public partial class View_CheckRecord
    {
        public View_CheckRecord()
        {
        }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Id { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Sensei { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Scratch { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Bruise { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Speckle { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DownEdge { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Rust { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string MissedProcess { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Decibel { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Dot { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Disaccord { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Noise { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CardPoint { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ShaftTight { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Shake { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OutputSize { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InPutSize { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string NeglectedLoading { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string NotInPlace { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Others { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Minute { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Burning { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string WrongLabe { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Resistance { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string WrongNumberControl { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string DielectricStrength { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string GearBump { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string AbnormalNoise { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BearingFailure { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string OutPort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InputPort { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string MountingFlange { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InstallKeyway { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InstallationShaftDiameter { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string InstallTheStop { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string BoltCenter { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string NoLoad { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int ProductOrderId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string TotalUnqualifiedNumber { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string RawMaterial { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Contour { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public DateTime SaveTime { get; set; }

        /// <summary>
        /// 保存日期显示
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public String SaveTimeString { get { return this.SaveTime == null ? "" : this.SaveTime.ToString("yyyy-MM-dd"); } }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ErpVoucherNo { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string CustomerName { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string spec { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:True
        /// </summary>           
        public Decimal QulityQty { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public Decimal ProductQty { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Decription { get; set; }

        /// <summary>
        /// 番号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public String MaterialNo { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public String LineName { get; set; }

        /// <summary>
        /// 未质检数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Decimal NoQualityQty { get { return this.ProductQty - this.QulityQty; } }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }

        /// <summary>
        /// 质检记录合格数量
        /// </summary>
        public Decimal RecordQualityQty { get; set; }

        /// <summary>
        /// 质检记录不合格数量
        /// </summary>
        public Decimal RecordNoQualityQty { get; set; }

        /// <summary>
        /// 首检送检数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public Decimal CheckQty { get { return this.RecordQualityQty + this.RecordNoQualityQty; } }

        /// <summary>
        /// 合格率
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public String PassRate
        {
            get
            {
                return this.CheckQty == 0 ?
                    "100.00" : (this.RecordQualityQty / this.CheckQty * 100).ToString("0.00");
            }
        }

        /// <summary>
        /// 总合格率
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public String PassRateAll { get; set; }

        /// <summary>
        /// 据点
        /// </summary>
        public String StrongHoldCode { get; set; }

        /// <summary>
        /// 检验人
        /// </summary>
        public String Checker { get; set; }

        /// <summary>
        /// 复检数量
        /// </summary>
        public Decimal BackQualityQty { get; set; }

        /// <summary>
        /// 检验班组
        /// </summary>
        public String Teams { get; set; }

        /// <summary>
        /// 转速
        /// </summary>
        public String Speed { get; set; }

    }
}
