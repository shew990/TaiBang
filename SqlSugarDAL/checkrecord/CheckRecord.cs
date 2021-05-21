using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugarDAL.checkrecord
{
    public class T_CheckRecord
    {
        /// <summary>
        /// 表头id
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// 生产订单id
        /// </summary>
        public Int32 ProductOrderId { get; set; }

        /// <summary>
        /// 掉漆
        /// </summary>
        public String Sensei { get; set; }

        /// <summary>
        /// 划伤
        /// </summary>
        public String Scratch { get; set; }

        /// <summary>
        /// 磕碰伤
        /// </summary>
        public String Bruise { get; set; }

        /// <summary>
        /// 污迹
        /// </summary>
        public String Speckle { get; set; }

        /// <summary>
        /// 塌边
        /// </summary>
        public String DownEdge { get; set; }

        /// <summary>
        /// 锈迹
        /// </summary>
        public String Rust { get; set; }

        /// <summary>
        /// 漏工序
        /// </summary>
        public String MissedProcess { get; set; }

        /// <summary>
        /// 分贝
        /// </summary>
        public String Decibel { get; set; }

        /// <summary>
        /// 打点
        /// </summary>
        public String Dot { get; set; }

        /// <summary>
        /// 不一致
        /// </summary>
        public String Disaccord { get; set; }

        /// <summary>
        /// 杂音
        /// </summary>
        public String Noise { get; set; }

        /// <summary>
        /// 卡点卡顿
        /// </summary>
        public String CardPoint { get; set; }

        /// <summary>
        /// 轴紧
        /// </summary>
        public String ShaftTight { get; set; }

        /// <summary>
        /// 抖动
        /// </summary>
        public String Shake { get; set; }

        /// <summary>
        /// 输出尺寸
        /// </summary>
        public String OutputSize { get; set; }

        /// <summary>
        /// 输入尺寸
        /// </summary>
        public String InPutSize { get; set; }

        /// <summary>
        /// 漏装
        /// </summary>
        public String NeglectedLoading { get; set; }

        /// <summary>
        /// 装不到位
        /// </summary>
        public String NotInPlace { get; set; }

        /// <summary>
        /// 其他原因
        /// </summary>
        public String Others { get; set; }

        /// <summary>
        /// 弧分
        /// </summary>
        public String Minute { get; set; }

        /// <summary>
        /// 氧化
        /// </summary>
        public String Burning { get; set; }

        /// <summary>
        /// 标签贴错
        /// </summary>
        public String WrongLabe { get; set; }

        /// <summary>
        /// 电机阻值
        /// </summary>
        public String Resistance { get; set; }

        /// <summary>
        /// 号码管有误
        /// </summary>
        public String WrongNumberControl { get; set; }

        /// <summary>
        /// 介电强度
        /// </summary>
        public String DielectricStrength { get; set; }

        /// <summary>
        /// 齿轮磕碰
        /// </summary>
        public String GearBump { get; set; }

        /// <summary>
        /// 减速箱异响
        /// </summary>
        public String AbnormalNoise { get; set; }

        /// <summary>
        /// 轴承损坏
        /// </summary>
        public String BearingFailure { get; set; }

        /// <summary>
        /// 输入孔
        /// </summary>
        public String InputPort { get; set; }

        /// <summary>
        /// 输出孔
        /// </summary>
        public String OutPort { get; set; }

        /// <summary>
        /// 安装法兰
        /// </summary>
        public String MountingFlange { get; set; }

        /// <summary>
        /// 安装键槽
        /// </summary>
        public String InstallKeyway { get; set; }

        /// <summary>
        /// 安装轴径
        /// </summary>
        public String InstallationShaftDiameter { get; set; }

        /// <summary>
        /// 安装止口
        /// </summary>
        public String InstallTheStop { get; set; }

        /// <summary>
        /// 安装孔距
        /// </summary>
        public String BoltCenter { get; set; }

        /// <summary>
        /// 空载电流范围
        /// </summary>
        public String NoLoad { get; set; }

        /// <summary>
        /// 总不合格数
        /// </summary>
        public Int32 TotalUnqualifiedNumber { get; set; }

        /// <summary>
        /// 原材料
        /// </summary>
        public String RawMaterial { get; set; }

        /// <summary>
        /// 外形
        /// </summary>
        public String Contour { get; set; }

        /// <summary>
        /// 保存时间
        /// </summary>
        public DateTime SaveTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Decription { get; set; }

        /// <summary>
        /// 合格数量
        /// </summary>
        public Decimal QualityQty { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        public Decimal NoQualityQty { get; set; }

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
