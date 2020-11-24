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
    }
}
