﻿using System;
using System.Collections.Generic;
using BILBasic.User;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using BILBasic.Basing.Factory;
using System.Data;
using BILWeb.Material;

namespace BILWeb.SyncService
{
    public class ParamaterFiled_DB : Base_DB<EmptyModel>
    { 
        /// <summary>
        /// 获取出库入类型
        /// </summary>
        /// <param name="stockType"></param>
        /// <returns></returns>
       internal int GetRealStockType(int stockType)
        {
            using (var db = SqlSugarBase.GetInstance())
            {
                return db.Ado.GetInt("SELECT PARAMETERNAME FROM T_PARAMETER WHERE GROUPNAME = 'Sync_Type' AND PARAMETERID ='" + stockType + "'");
            }
        }

        /// <summary>
        /// 根据wms类型获取同步字段
        /// </summary>
        /// <param name="wmsVourcherType"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal List<ParamaterField_Model> GetPmList(int stockType, int wmsVourcherType, ref string errMsg)
        {
           using (var db = SqlSugarBase.GetInstance())
            {
                List<ParamaterField_Model> pmList = db.Queryable<ParamaterField_Model>()
                    .Where(it => it.InStockType == stockType)
                    .WhereIF(wmsVourcherType != -1, it => it.VoucherType== wmsVourcherType).ToList();
                return pmList;
            }
        }

        /// <summary>
        /// 获取最后同步时间
        /// </summary>
        /// <param name="stockType"></param>
        /// <param name="erpVoucherNo"></param>
        /// <returns></returns>
        internal string GetLastSyncTime(int stockType, string erpVoucherNo)
        {
            using (var db = SqlSugarBase.GetInstance())
            {
                string tableName = stockType == 10 ? "T_INSTOCK" : "T_OUTSTOCK";
                string sql = "SELECT LastSyncTIme FROM " + tableName + " WHERE ErpVoucherNo = '" + erpVoucherNo + "'";
                DateTime time = db.Ado.GetDateTime(sql);
                return time.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        /// <summary>
        /// 获取最大单据号
        /// </summary>
        /// <param name="stockType"></param>
        /// <param name="wmsVourcherType"></param>
        /// <returns></returns>
        internal string getLastSyncErpVoucherNo(int stockType, string wmsVourcherType,string erpVoucherType,string erpvouchertype)
        {
            using (var db = SqlSugarBase.GetInstance())
            {
                string tableName = stockType == 10 ?
                (wmsVourcherType == "21" ? "T_QUALITY_SYNC_VIEW" : "T_INTSTOCK_SYNC_VIEW") :
                (wmsVourcherType == "29" || wmsVourcherType == "34" ? "T_OMS_SYNC_VIEW" : "T_OUTTSTOCK_SYNC_VIEW");
                string sql = "SELECT ErpVoucherNo FROM " + tableName + " WHERE VourcherType = '" + erpVoucherType + "' and ErpVourcherType='"+ erpvouchertype + "'";
                return db.Ado.GetString(sql);
            }
        }

        /// <summary>
        /// 获取ID，如果存在，则表示有记录
        /// </summary>
        /// <param name="keys">关键字</param>
        /// <param name="pmListbyType">对照关系</param>
        /// <param name="JToken">JSON</param>
        /// <param name="TableName">表名</param>
        /// <returns></returns>
        internal int checkRecode(string whereString, List<ParamaterField_Model> pmListbyType, JToken JToken,string TableName)
        {
            //获取表头查询记录是否存在条件 配置项：T_PARAMETERTABLE=HEADKEYS
            //判断数据是否存在
            string checkSQL = String.Format(SqlModel.checkSQL, TableName, whereString);
            using (var db = SqlSugarBase.GetInstance())
            {
                return db.Ado.GetInt(checkSQL);
            }
        }

        /// <summary>
        /// 获取新wms单号
        /// </summary>
        /// <param name="strPrex">单据开头</param>
        /// <returns></returns>
        internal string GetWmsWoucherNo(string strPrex,string tableName,string wmsHeadVourcherNo)
        {
            return base.GetNewOrderNo(strPrex, tableName, wmsHeadVourcherNo).ToString();//DateTime.Now.Ticks;// 
        }

        /// <summary>
        /// 查询WMS单号
        /// </summary>
        /// <param name="wMSTableNameH"></param>
        /// <param name="tableID"></param>
        /// <returns></returns>
        internal string GetWmsWoucherNo(string wMSTableNameH, int tableID)
        {
            string checkSQL = String.Format(SqlModel.GetWmsVoucherNo, wMSTableNameH, tableID);
            using (var db = SqlSugarBase.GetInstance())
            {
                return db.Ado.GetString(checkSQL);
            }

        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal int GetHeadID(string key)
        {
            return base.GetTableIDBySqlServer(key);
        }

        /// <summary>
        /// 保存同步数据
        /// </summary>
        /// <param name="SQList"></param>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        internal bool SaveSqlList(List<string> SQList, ref string errMsg)
        {
            using (var db = SqlSugarBase.GetInstance())
            {
                db.Ado.CommandTimeOut = 300000;//设置超时时间
                try
                {
                    //db.BeginTran();//开启事务
                    //特别说明：在事务中，默认情况下是使用锁的，也就是说在当前事务没有结束前，其他的任何查询都需要等待
                    //ReadCommitted：在正在读取数据时保持共享锁，以避免脏读，但是在事务结束之前可以更改数据，从而导致不可重复的读取或幻像数据。
                    db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted); //重载指定事务的级别

                    //特别说明：在事务操作中，对于自增长列的表，插入成功，又回滚的会占据一次自增长值
                    foreach(string sql in SQList)
                    {
                        db.Ado.ExecuteCommand(sql);
                    }
                    //提交事务
                    db.Ado.CommitTran();
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    db.Ado.RollbackTran();//回滚
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取表体ID集合
        /// </summary>
        /// <param name="tableID"></param>
        /// <returns></returns>
        internal List<int> getDetailIDs(string tablenName,int tableID)
        {
            List<int> IDs = new List<int>();
            using (var db = SqlSugarBase.GetInstance())
            {
                string sql = String.Format(SqlModel.GetHeadids, tablenName, tableID);
                SqlDataReader dr= (SqlDataReader)db.Ado.GetDataReader(sql);
                while (dr.Read())
                {
                    IDs.Add(Int32.Parse(dr["ID"].ToString()));
                }
                dr.Close();
            }
            return IDs;
        }




        //protected override SqlParameter[] GetSaveModelSqlParameter(EmptyModel model)
        //{
        //    throw new NotImplementedException();
        //}

        protected override IDataParameter[] GetSaveModelIDataParameter(EmptyModel model)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        //protected override EmptyModel ToModel(SqlDataReader reader)
        //{
        //    EmptyModel emptuModel = new EmptyModel();
        //    return emptuModel;
        //}

        protected override EmptyModel ToModel(IDataReader reader)
        {
            EmptyModel emptuModel = new EmptyModel();
            return emptuModel;
        }

        protected override string GetViewName()
        {
            throw new NotImplementedException();
        }

        protected override string GetTableName()
        {
            throw new NotImplementedException();
        }

        protected override string GetSaveProcedureName()
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveSql(UserModel user, ref EmptyModel model)
        {
            throw new NotImplementedException();
        }
        
        #region 同步单据  
        //物料：flag= "0" 入库单：1  出库单 2
        public bool GetVoucherNo(string Erpvoucherno,ref string ErrorMsg,string flag= "0")
        {
            if (string.IsNullOrEmpty(Erpvoucherno))
            {
                ErrorMsg = "单据号不能为空";
                return false;
            }
            int WmsVoucherType = 9999; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;int StockType = 0;
            string tableName = "t_material";
            if (Erpvoucherno.Contains("Wgr"))//成品入库单
            {
                tableName = " T_instock";
                StockType = 10;
                WmsVoucherType = 50;
            }
            if (Erpvoucherno.Contains("RE"))//退回处理单
            {
                tableName = " T_instock";
                StockType = 10;
                WmsVoucherType = 47;
            }
            if (Erpvoucherno.Contains("MO"))//生产订单
            {
                tableName = " T_Product";
                StockType = 10;
                WmsVoucherType = 51;
            }
            if (Erpvoucherno.Contains("SM"))//销售出货单
            {
                tableName = " T_Task";
                StockType = 20;
                WmsVoucherType = 46;
            }
            if (Erpvoucherno.Contains("Tra"))//形态转换单
            {
                StockType = 10;
                WmsVoucherType = 52;
            }
            if (Erpvoucherno.Contains("DC"))//调拨出库单
            {
                if (flag=="1")
                {
                    tableName = " T_instock";
                    StockType = 10;
                    WmsVoucherType = 30;
                }
                else
                {
                    tableName = " T_Task";
                    StockType = 20;
                    WmsVoucherType = 31;
                }

            }
            if (Erpvoucherno.Contains("DR"))//调拨入库单
            {
                tableName = " T_instock";
                StockType = 10;
                WmsVoucherType = 30;
            }
            if (Erpvoucherno.Contains("ZF"))//杂发单
            {
                StockType = 20;
                WmsVoucherType = 0;
            }
            if (WmsVoucherType ==0|| tableName=="")
            {
                ErrorMsg = "找不到该单据类型！";
                return false;
            }
            if (flag!="0")
            {
                //查看单据是否存在
                T_Material_DB DB = new T_Material_DB();
                if (DB.GetCount("SELECT count(1) FROM " + tableName + " WHERE erpvoucherno ='" + Erpvoucherno + "'") > 0)
                {
                    return true;
                }
               
                //using (var db = SqlSugarBase.GetInstance())
                //{
                //    if (db.Ado.GetInt("SELECT count(1) FROM " + tableName + " WHERE erpvoucherno ='" + Erpvoucherno + "'") > 0)
                //    {
                //        return true;
                //    }
                //}
            }

            //同步单据
            if (SyncErp.SyncJsonFromErp(StockType, string.Empty, Erpvoucherno, WmsVoucherType, ref ErrorMsg))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        #endregion
        
    }
}
