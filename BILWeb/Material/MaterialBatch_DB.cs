using BILBasic.DBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BILBasic.Common;
using BILBasic.User;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using BILBasic.JSONUtil;
using BILWeb.Stock;
using SqlSugarDAL.barcode;
using BILWeb.T00L;
using System.IO;
using System.Xml.Serialization;
using BILWeb.Warehouse;

namespace BILWeb.Material
{
    public partial class T_Material_Batch_DB : BILBasic.Basing.Factory.Base_DB<T_Material_BatchInfo>
    {

        /// <summary>
        /// 添加t_material_batch
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Material_BatchInfo t_material_batch)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_Material_BatchInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            if (model.ID <= 0)
            {
                int voucherID = base.GetTableID("seq_matarial_batch");

                model.ID = voucherID.ToInt32();

                strSql = "insert into T_MATERIAL_BATCH (id, headerid, brand, batch, departmentcode, departmentname, pquantily, dquantily, rquantily, edate, wbs, projectno, customerno, customername, iescrow, factorycode, factoryname, version, rohs, customernote, exceptionnote,  creater, createtime)" +
                        "values ('" + voucherID + "', '" + model.HeaderID + "','" + model.Brand + "','" + model.BatchNo + "','" + model.DepartmentCode + "','" + model.DepartmentName + "','" + model.PQuantily + "','" + model.DQuantily + "'," +
                        "'" + model.RQuantily + "','" + model.Edate + "','" + model.WBS + "','" + model.ProjectNo + "','" + model.CustomerNo + "','" + model.CustomerName + "','" + model.IEscrow + "','" + model.FactoryCode + "','" + model.FactoryName + "'," +
                        "'" + model.Version + "','" + model.ROHS + "','" + model.CustomerNote + "','" + model.ExceptionNote + "','" + user.UserNo + "',sysdate)";

                lstSql.Add(strSql);
            }
            else
            {
                strSql = "update t_Material_Batch a set a.Batch='" + model.BatchNo + "',a.Brand='" + model.Brand + "',a.Customername='" + model.CustomerName + "',a.Customerno='" + model.CustomerNo + "',a.Customernote='" + model.CustomerNote + "',a.Departmentcode='" + model.DepartmentCode + "'," +
                        "a.Departmentname='" + model.DepartmentName + "',a.Dquantily='" + model.DQuantily + "',a.Edate='" + model.Edate + "',a.Exceptionnote='" + model.ExceptionNote + "',a.Factorycode='" + model.FactoryCode + "',a.Factoryname='" + model.FactoryName + "',a.Iescrow='" + model.IEscrow + "'," +
                        "a.Modifyer='" + user.Modifyer + "',a.Modifytime=Sysdate,a.Pquantily='" + model.PQuantily + "',a.Projectno='" + model.ProjectNo + "',a.Rohs='" + model.ROHS + "',a.Rquantily='" + model.RQuantily + "',a.Version='" + model.Version + "',a.Wbs='" + model.WBS + "' where a.Id = '" + model.ID + "'";
                lstSql.Add(strSql);
            }

            return lstSql;
        }

        protected override List<string> GetDeleteSql(UserModel user, T_Material_BatchInfo model)
        {
            List<string> lstSql = new List<string>();
            string strSql = string.Empty;

            strSql = "delete t_Material_Batch where id = '" + model.ID + "'";

            lstSql.Add(strSql);

            return lstSql;
        }


        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Material_BatchInfo ToModel(IDataReader reader)
        {
            T_Material_BatchInfo t_material_batch = new T_Material_BatchInfo();
            //读取的是库存的数据
            t_material_batch.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            t_material_batch.BatchNo = dbFactory.ToModelValue(reader, "BatchNo").ToDBString();
            t_material_batch.Edate = dbFactory.ToModelValue(reader, "Edate").ToDateTime();
            t_material_batch.StockQty = dbFactory.ToModelValue(reader, "Qty").ToDecimal();
            t_material_batch.CreateTime = dbFactory.ToModelValue(reader, "CreateTime").ToDateTime();
            t_material_batch.WareHouseNo = dbFactory.ToModelValue(reader, "WareHouseNo").ToDBString();
            t_material_batch.HouseNo = dbFactory.ToModelValue(reader, "HouseNo").ToDBString();
            t_material_batch.AreaNo = dbFactory.ToModelValue(reader, "AreaNo").ToDBString();
            t_material_batch.SupCode = dbFactory.ToModelValue(reader, "SupCode").ToDBString();
            t_material_batch.SupName = dbFactory.ToModelValue(reader, "SupName").ToDBString();
            t_material_batch.Status = dbFactory.ToModelValue(reader, "Status").ToInt32();
            t_material_batch.SupPrdDate = dbFactory.ToModelValue(reader, "SupPrdDate").ToDateTime();
            t_material_batch.SupPrdBatch = dbFactory.ToModelValue(reader, "SupPrdBatch").ToDBString();
            t_material_batch.ProductDate = dbFactory.ToModelValue(reader, "ProductDate").ToDateTime();
            t_material_batch.ProductBatch = dbFactory.ToModelValue(reader, "ProductBatch").ToDBString();

            return t_material_batch;
            //t_material_batch.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            //t_material_batch.HeaderID = dbFactory.ToModelValue(reader, "HeaderID").ToInt32();
            //t_material_batch.Brand = (string)dbFactory.ToModelValue(reader, "BRAND");
            //t_material_batch.Batch = (string)dbFactory.ToModelValue(reader, "BATCH");
            //t_material_batch.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DEPARTMENTCODE");
            //t_material_batch.DepartmentName = (string)dbFactory.ToModelValue(reader, "DEPARTMENTNAME");
            //t_material_batch.PQuantily = (decimal?)dbFactory.ToModelValue(reader, "PQUANTILY");
            //t_material_batch.DQuantily = (decimal?)dbFactory.ToModelValue(reader, "DQUANTILY");
            //t_material_batch.RQuantily = (decimal?)dbFactory.ToModelValue(reader, "RQUANTILY");
            //t_material_batch.Edate = (DateTime?)dbFactory.ToModelValue(reader, "EDATE");
            //t_material_batch.WBS = (string)dbFactory.ToModelValue(reader, "WBS");
            //t_material_batch.ProjectNo = (string)dbFactory.ToModelValue(reader, "PROJECTNO");
            //t_material_batch.CustomerNo = (string)dbFactory.ToModelValue(reader, "CUSTOMERNO");
            //t_material_batch.CustomerName = (string)dbFactory.ToModelValue(reader, "CUSTOMERNAME");
            //t_material_batch.IEscrow = (decimal?)dbFactory.ToModelValue(reader, "IESCROW");
            //t_material_batch.FactoryCode = (string)dbFactory.ToModelValue(reader, "FACTORYCODE");
            //t_material_batch.FactoryName = (string)dbFactory.ToModelValue(reader, "FACTORYNAME");
            //t_material_batch.Version = (string)dbFactory.ToModelValue(reader, "VERSION");
            //t_material_batch.ROHS = (string)dbFactory.ToModelValue(reader, "ROHS");
            //t_material_batch.CustomerNote = (string)dbFactory.ToModelValue(reader, "CUSTOMERNOTE");
            //t_material_batch.ExceptionNote = (string)dbFactory.ToModelValue(reader, "EXCEPTIONNOTE");

            //t_material_batch.Creater = (string)dbFactory.ToModelValue(reader, "CREATER");
            //t_material_batch.CreateTime = (DateTime?)dbFactory.ToModelValue(reader, "CREATETIME");
            //t_material_batch.Modifyer = (string)dbFactory.ToModelValue(reader, "MODIFYER");
            //t_material_batch.ModifyTime = (DateTime?)dbFactory.ToModelValue(reader, "MODIFYTIME");


        }

        protected override string GetViewName()
        {
            return "V_MATERIAL_BATCH";
        }

        protected override string GetTableName()
        {
            return "T_MATERIAL_BATCH";
        }

        protected override string GetHeaderIDFieldName()
        {
            return "MaterialNoID";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        public int CheckMaterialExist(T_Material_BatchInfo model)
        {
            string strSql = string.Format("SELECT COUNT(1) FROM T_MATERIAL_BATCH WHERE BATCH = '{0}' and id <> '{1}'", model.BatchNo, model.ID);
            return GetScalarBySql(strSql).ToInt32();
        }


        public override List<T_Material_BatchInfo> GetModelListByHeaderID(int headerID)
        {
            List<T_Material_BatchInfo> list = base.GetModelListByHeaderID(headerID);
            List<T_Material_BatchInfo> groupList = null;
            if (list.Count > 0)
            {
                groupList = list
                .GroupBy(x => new { x.BatchNo, x.AreaNo })
                .Select(group => new T_Material_BatchInfo
                {
                    BatchNo = group.Key.BatchNo,
                    StockQty = group.Sum(p => p.StockQty),
                    Edate = group.FirstOrDefault().Edate,
                    CreateTime = group.FirstOrDefault().CreateTime,
                    WareHouseNo = group.FirstOrDefault().WareHouseNo,
                    HouseNo = group.FirstOrDefault().HouseNo,
                    AreaNo = group.FirstOrDefault().AreaNo,
                    SupCode = group.FirstOrDefault().SupCode,
                    SupName = group.FirstOrDefault().SupName,
                    SupPrdBatch = group.FirstOrDefault().SupPrdBatch,
                    SupPrdDate = group.FirstOrDefault().SupPrdDate,
                    ProductBatch = group.FirstOrDefault().ProductBatch,
                    ProductDate = group.FirstOrDefault().ProductDate
                }).ToList();
            }

            return groupList;
        }


        //获取据点列表
        public U9BaseInfo GetStrongholdList()
        {
            try
            {
                U9BaseInfo baseInfo = new U9BaseInfo();
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string json = "{\"VoucherType\":\"9998\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9BaseInfo>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //获取基础列表
        public U9BaseInfo GetInfoList(string id, string StrongHoldCode)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                //string json = "{\"company_no\":\"" + 1001909046618667 + "\",\"data_no\":\"0300\",\"VoucherType\":\"9997\"}";
                string json = "{\"company_no\":\"" + id + "\",\"data_no\":\"" + StrongHoldCode + "\",\"VoucherType\":\"9997\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("转换单接口返回的JSON:" + ERPJson);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9BaseInfo>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }


        }

        public U9BaseInfo GetInfoListThree(string id, string StrongHoldCode, string DeparMentNo)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                //string json = "{\"company_no\":\"" + 1001909046618667 + "\",\"max_code\":\"" + DeparMentNo + "\",\"data_no\":\"0300\",\"VoucherType\":\"9997\"}";

                string json = "{\"company_no\":\"" + id + "\",\"max_code\":\"" + DeparMentNo + "\",\"data_no\":\"" + StrongHoldCode + "\",\"VoucherType\":\"9997\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("转换单接口返回的JSON:" + ERPJson);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9BaseInfo>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool SaveCheckToU9(List<T_StockInfoEX> stocks, string CheckNo, string UserNo, ref string strMsg)
        {
            try
            {
                T_WareHouse_DB TWareHouseDB = new T_WareHouse_DB();
                string StrongHoldCode = "";
                string StrongHoldCodeName = "";
                TWareHouseDB.GetStrongholdcode(stocks[0].WarehouseNo, ref StrongHoldCode, ref StrongHoldCodeName);

                List<U9StockTo> U9StockTos = new List<U9StockTo>();
                //简化实体类
                stocks.ForEach(item =>
                {
                    U9StockTos.Add(new U9StockTo()
                    {
                        MaterialNo = item.MaterialNo,
                        BatchNo = item.BatchNo,
                        ErpWarehouseNo = item.WarehouseNo,
                        FromErpWarehouse = item.WarehouseNo,
                        PostQty = item.Qty,
                        ScanQty = item.ScanQty,
                        PostUser = UserNo,
                        StrongHoldCode = StrongHoldCode,
                        GUID = CheckNo,
                        StrVoucherType = "InvSheet001",
                        Unit = "S001"
                    });
                });


                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string JSONU9 = JSONHelper.ObjectToJson<List<U9StockTo>>(U9StockTos);
                LogNet.LogInfo("提交U9盘点单json:" + JSONU9);
                string ERPJson = TIF.PostCheck(JSONU9);
                LogNet.LogInfo("提交U9盘点单返回json:" + ERPJson);
                if (ERPJson.Substring(0, 1) == "1")
                {
                    return true;
                }
                else
                {
                    strMsg = ERPJson.Substring(1, ERPJson.Length - 1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                strMsg = ex.ToString();
                return false;
            }
        }

        public List<U9Stock> GetStockInfo(string WareHouseNo, string StrongHoldCode, string MaterialNo)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();

                string json = "{\"company_no\":\"" + StrongHoldCode + "\",\"edit_time\":\"" + WareHouseNo + "\",\"data_no\":\"" + MaterialNo + "\",\"VoucherType\":\"8888\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("盘点单接口返回的JSON:" + ERPJson);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<List<U9Stock>>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //SOP列表
        public List<MoReport> GetSopList(string ErpVoucherNo)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string json = "{\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"9996\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("SOP列表:" + ERPJson);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<List<MoReport>>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //获取转换单
        public List<U9Zh> GetZhList(string ErpVoucherNo)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string json = "{\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"52\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("-------------------------------------------转换单ERP返回：" + ERPJson);
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<List<U9Zh>>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        public string Post(string ErpVoucherNo, string Remark, string Guid, string Creater)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string json = "{\"company_no\":\"" + ErpVoucherNo + "\",\"max_code\":\"" + Remark + "\",\"erp_vourcher_type\":\"" + Creater + "\",\"data_no\":\"" + Guid + "\",\"VoucherType\":\"9992\"}";
                string ERPJson = TIF.GetModelListByInterface(json);

                LogNet.LogInfo("-------------------------------------------直发公司ERP返回：" + ERPJson);
                return ERPJson;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //获取成品入库单是成品还是原料
        public bool isChengpin(string ErpVoucherNo, ref string msg)
        {
            try
            {
                msg = "";
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                //string json = "{\"company_no\":\"" + 1001909046618667 + "\",\"data_no\":\"0300\",\"VoucherType\":\"9997\"}";
                string json = "{\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"7777\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
                LogNet.LogInfo("成品入库单是成品还是原料:" + ERPJson);
                if (ERPJson.Substring(0, 1) == "1")
                {
                    return true;
                }
                if (ERPJson.Substring(0, 1) == "0")
                {
                    return false;
                }
                return true;
                msg = ERPJson;
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                return true;
            }


        }

        ////提交转换单
        //public bool PostZh(U9Zh model, UserModel user, string Guid, ref string receive,ref string Message)
        //{

        //    try
        //    {




        //        BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
        //        string json = "\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"52\"}";
        //        string ERPJson = TIF.GetModelListByInterface(json);
        //        return BILBasic.JSONUtil.JSONHelper.JsonToObject<U9Zh>(ERPJson);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public bool PostZh(UserModel user, List<U9Zh> list, ref string Msg, ref string SerialNos)
        {
            try
            {
                SerialNos = string.Empty;
                if (list.Count == 0)
                {
                    Msg = "提交的参数不能为空！";
                    return false;
                }

                List<string> sqls = new List<string>();

                //删除所有扫描到的条码
                List<T_StockInfo> StockInfoDel = new List<T_StockInfo>();
                list[0].detail.ForEach(item =>
                {
                    foreach (var barcode in item.barcodeList)
                    {
                        if (StockInfoDel.FindAll(Stock => Stock.Barcode == barcode.Barcode).Count < 1) {
                            string sql = string.Format("delete  T_STOCK where barcode='{0}'", barcode.Barcode);
                            sqls.Add(sql);
                            sqls.AddRange(GetTaskTransSqlList(user, barcode, list[0], 210));

                            StockInfoDel.Add(barcode);
                        }
                    }
                });

                //新增条码
                foreach (U9ZhDetail U9ZhDetail in list[0].detailBehind)
                {
                    //寻找对应的行
                    U9ZhDetail findDetail = list[0].detail.FindLast(it => it.Row == U9ZhDetail.Row);
                    
                    T_Material_DB MDB = new T_Material_DB();
                    T_StockInfo t_Stockd = DeepCopyByXml<T_StockInfo>(findDetail.barcodeList[0]);
                    t_Stockd.ErpVoucherNo = list[0].ErpVoucherNo;
                    t_Stockd.MaterialNo = U9ZhDetail.MaterialNo;
                    t_Stockd.BatchNo = U9ZhDetail.BatchNo;
                    t_Stockd.MaterialDesc = U9ZhDetail.MaterialDesc;
                    t_Stockd.Qty = U9ZhDetail.Qty;
                    t_Stockd.Spec = U9ZhDetail.Spec;
                    t_Stockd.SerialNo = SerialnoHelp.GetSerialnos(1)[0];
                    t_Stockd.Barcode = "2@" + t_Stockd.MaterialNo + "@" + t_Stockd.Qty + "@" + t_Stockd.SerialNo;


                    int Materialnoid1 = MDB.GetMaterialNoid(U9ZhDetail.MaterialNo, list[0].StrongHoldCode);
                    if (Materialnoid1 == 0)
                    {
                        Msg = "据点【" + list[0].StrongHoldCode + "】物料主数据没有物料【" + U9ZhDetail.MaterialNo + "】信息！";
                        return false;
                    }
                    else
                    {
                        t_Stockd.MaterialNoID = Materialnoid1;
                    }

                    string strSql8 = "insert into t_stock(serialno,Materialno,materialdesc,qty,status,isdel,Creater,Createtime,batchno,unit,unitname,Palletno," +
                                  "islimitstock,materialnoid,warehouseid,houseid,areaid,Receivestatus,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,EDATE,SUPCODE,SUPNAME," +
                                 "SUPPRDBATCH,Isquality,Stocktype,ean,BARCODETYPE,projectNo,TracNo)" +
                                 "values ('" + t_Stockd.SerialNo + "','" + t_Stockd.MaterialNo + "','" + t_Stockd.MaterialDesc + "','" + t_Stockd.Qty + "','" + t_Stockd.IsQuality + "','1'" +
                                 ",'" + user.UserNo + "',getdate(),'" + t_Stockd.BatchNo + "','" + t_Stockd.Unit + "','" + t_Stockd.UnitName + "'" +
                                 ",(select palletno from t_Palletdetail where serialno = '" + t_Stockd.SerialNo + "'),'1','" + t_Stockd.MaterialNoID + "'" +
                                 ", '" + t_Stockd.WareHouseID + "','" + t_Stockd.HouseID + "','" + t_Stockd.AreaID + "','1','" + t_Stockd.Barcode + "','" + t_Stockd.StrongHoldCode + "', " +
                                 "  '" + t_Stockd.StrongHoldName + "','" + t_Stockd.CompanyCode + "',null,'" + t_Stockd.SupCode + "','" + t_Stockd.SupName + "'," +
                                 "'" + t_Stockd.SupPrdBatch + "','3' ,'1','" + t_Stockd.EAN + "','" + t_Stockd.BarCodeType + "','" + (t_Stockd.ProjectNo == null ? "" : t_Stockd.ProjectNo) + "','" + (t_Stockd.TracNo == null ? "" : t_Stockd.TracNo) + "' )";
                    sqls.Add(strSql8);

                    string strSql9 = "INSERT INTO T_OUTBARCODE(voucherno,rowno,erpvoucherno,vouchertype,materialno,materialdesc,spec,cuscode,cusname,supcode,supname,outpackqty,innerpackqty," +
                        "voucherqty,qty,nopack,printqty,barcode,barcodetype,serialno,barcodeno,outcount,innercount,mantissaqty,isrohs,outbox_id,abatchqty,isdel,creater,createtime,modifyer," +
                        "modifytime, materialnoid,strongholdcode,strongholdname,companycode,productdate,supprdbatch,supprddate,productbatch,edate,storecondition,specialrequire,batchno,barcodemtype," +
                        "rownodel, protectway,boxweight,unit,labelmark,boxdetail,matebatch,mixdate,relaweight,productclass,itemqty,workno,mtypef,prorowno,prorownodel,boxcount,dimension,ean,fserialno," +
                        "standard, erpmateid,subiarrsid,originalCode,status,ReceiveTime,Inner_Id,ProjectNo,TracNo,department,erpwarehouseno,departmentname,erpwarehousename) " +
                        "select  voucherno,rowno,'" + t_Stockd.ErpVoucherNo + "',vouchertype,'" + t_Stockd.MaterialNo + "','" + t_Stockd.MaterialDesc + "','" + t_Stockd.Spec + "',cuscode,cusname,supcode,supname,outpackqty,innerpackqty,voucherqty,'" + t_Stockd.Qty + "',nopack,printqty,'" + t_Stockd.Barcode + "',barcodetype," +
                        "'" + t_Stockd.SerialNo + "',barcodeno,outcount,innercount,mantissaqty,isrohs,outbox_id,abatchqty,isdel,creater,createtime,modifyer,modifytime," + t_Stockd.MaterialNoID + ",strongholdcode,strongholdname,companycode,productdate,supprdbatch," +
                        "supprddate, '" + U9ZhDetail.RandomCode + "',edate,storecondition,specialrequire,'" + t_Stockd.BatchNo + "',barcodemtype,rownodel,protectway,boxweight,unit,labelmark,boxdetail,matebatch,mixdate,relaweight,productclass,itemqty," +
                        "workno, mtypef,prorowno,prorownodel,boxcount,dimension,ean,fserialno,standard,erpmateid,subiarrsid,originalCode,status,ReceiveTime,Inner_Id,ProjectNo,TracNo,department,erpwarehouseno,departmentname," +
                        "erpwarehousename from t_outbarcode where serialno= '" + findDetail.barcodeList[0].SerialNo + "'";
                    sqls.Add(strSql9);

                    SerialNos = SerialNos + "'" + t_Stockd.SerialNo + "',";
                    sqls.AddRange(GetTaskTransSqlList(user, t_Stockd, list[0], 209));

                }
                bool istrue = UpdateModelListStatusBySql(sqls, ref Msg);
                return istrue;





                //SerialNos = string.Empty;

                //if (list.Count==0)
                //{
                //    Msg = "提交的参数不能为空！";
                //    return false;
                //}

                //List<T_StockInfo> t_Stocks = new List<T_StockInfo>();
                ////得到数据修改条码信息
                //List<string> sqls = new List<string>();
                //List<string> jilv = new List<string>();

                //for (int i = 0; i < list.Count; i++)
                //{
                //    if (list[i].Type == 0)
                //    {
                //        for (int b = 0; b < list[i].barcodeList.Count; b++)
                //        {  //可理解为都是删除
                //            jilv.AddRange(GetTaskTransSqlList(user, list[i].barcodeList[b], list[i], 210));
                //        }

                //        U9ZhDetail model = list[i].detail[0];
                //        //新条码
                //        T_StockInfo t_Stock = DeepCopyByXml<T_StockInfo>(list[i].barcodeList[0]);
                //        t_Stock.MaterialNo = model.MaterialNo;
                //        t_Stock.MaterialDesc = model.MaterialDesc;
                //        t_Stock.Qty = model.Qty;
                //        t_Stock.SerialNo = SerialnoHelp.GetSerialnos(1)[0];
                //        t_Stock.Barcode = "2@" + t_Stock.MaterialNo + "@" + t_Stock.Qty + "@" + t_Stock.SerialNo;

                //        T_Material_DB MDB = new T_Material_DB();
                //        int Materialnoid = MDB.GetMaterialNoid(t_Stock.MaterialNo, list[i].StrongHoldCode);
                //        if (Materialnoid==0)
                //        {
                //            Msg = "据点【" + list[i].StrongHoldCode + "】物料主数据没有物料【" + t_Stock.MaterialNo + "】信息！";
                //            return false;
                //        }
                //        else
                //        {
                //            t_Stock.MaterialNoID = Materialnoid;
                //        }

                //        //需要转换的类型
                //        string update = string.Format("update T_STOCK set MATERIALNO='{0}',materialdesc='{1}',qty={2},barcode='{3}',serialno='{4}',MaterialNoID={5},batchno='{6}'  where barcode='{7}' ",
                //            model.MaterialNo, model.MaterialDesc, model.Qty, t_Stock.Barcode, t_Stock.SerialNo, t_Stock.MaterialNoID, model.BatchNo, list[i].barcodeList[0].Barcode);//两种模式都只会扫描一次所以条码集合肯定只能是一个
                //        sqls.Add(update);

                //        string update1 = string.Format("update T_outbarcode set MATERIALNO='{0}',materialdesc='{1}',qty={2},barcode='{3}',serialno='{4}',MaterialNoID={5},batchno='{6}',ProductBatch='{7}'  where barcode='{8}' ",
                //       model.MaterialNo, model.MaterialDesc, model.Qty, t_Stock.Barcode, t_Stock.SerialNo, t_Stock.MaterialNoID, model.BatchNo, model.RandomCode, list[i].barcodeList[0].Barcode);//两种模式都只会扫描一次所以条码集合肯定只能是一个
                //        sqls.Add(update1);

                //        SerialNos = SerialNos + "'" + t_Stock.SerialNo + "',";
                //        //生成一个条码加进去
                //        jilv.AddRange(GetTaskTransSqlList(user, t_Stock, list[i], 209));
                //        t_Stocks.Add(t_Stock);
                //        if (list[i].detail.Count > 1)
                //        {
                //            for (int d = 1; d < list[i].detail.Count; d++)
                //            {
                //                U9ZhDetail modeld = list[i].detail[d];
                //                T_StockInfo t_Stockd = DeepCopyByXml<T_StockInfo>(list[i].barcodeList[0]);
                //                t_Stockd.MaterialNo = modeld.MaterialNo;
                //                t_Stockd.BatchNo = modeld.BatchNo;
                //                t_Stockd.MaterialDesc = modeld.MaterialDesc;
                //                t_Stockd.Qty = modeld.Qty;
                //                t_Stockd.SerialNo = SerialnoHelp.GetSerialnos(1)[0];
                //                t_Stockd.Barcode = "2@" + t_Stockd.MaterialNo + "@" + t_Stockd.Qty + "@" + t_Stockd.SerialNo;


                //                int Materialnoid1 = MDB.GetMaterialNoid(modeld.MaterialNo, list[i].StrongHoldCode);
                //                if (Materialnoid1 == 0)
                //                {
                //                    Msg = "据点【" + list[i].StrongHoldCode + "】物料主数据没有物料【" + t_Stock.MaterialNo + "】信息！";
                //                    return false;
                //                }
                //                else
                //                {
                //                    t_Stock.MaterialNoID = Materialnoid1;
                //                }

                //                string strSql8 = "insert into t_stock(serialno,Materialno,materialdesc,qty,status,isdel,Creater,Createtime,batchno,unit,unitname,Palletno," +
                //                              "islimitstock,materialnoid,warehouseid,houseid,areaid,Receivestatus,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,EDATE,SUPCODE,SUPNAME," +
                //                             "SUPPRDBATCH,Isquality,Stocktype,ean,BARCODETYPE,projectNo,TracNo)" +
                //                             "values ('" + t_Stockd.SerialNo + "','" + t_Stockd.MaterialNo + "','" + t_Stockd.MaterialDesc + "','" + t_Stockd.Qty + "','" + t_Stockd.IsQuality + "','1'" +
                //                             ",'" + user.UserNo + "',getdate(),'" + t_Stockd.BatchNo + "','" + t_Stockd.Unit + "','" + t_Stockd.UnitName + "'" +
                //                             ",(select palletno from t_Palletdetail where serialno = '" + t_Stockd.SerialNo + "'),'1','" + t_Stockd.MaterialNoID + "'" +
                //                             ", '" + user.WarehouseID + "','" + user.ReceiveHouseID + "','" + user.ReceiveAreaID + "','1','" + t_Stockd.Barcode + "','" + t_Stockd.StrongHoldCode + "', " +
                //                             "  '" + t_Stockd.StrongHoldName + "','" + t_Stockd.CompanyCode + "',null,'" + t_Stockd.SupCode + "','" + t_Stockd.SupName + "'," +
                //                             "'" + t_Stockd.SupPrdBatch + "','3' ,'1','" + t_Stockd.EAN + "','" + t_Stockd.BarCodeType + "','" + (t_Stockd.ProjectNo == null ? "" : t_Stockd.ProjectNo) + "','" + (t_Stockd.TracNo == null ? "" : t_Stockd.TracNo) + "' )";
                //                sqls.Add(strSql8);

                //                string strSql9 = "INSERT INTO T_OUTBARCODE(voucherno,rowno,erpvoucherno,vouchertype,materialno,materialdesc,spec,cuscode,cusname,supcode,supname,outpackqty,innerpackqty,"+
                //                    "voucherqty,qty,nopack,printqty,barcode,barcodetype,serialno,barcodeno,outcount,innercount,mantissaqty,isrohs,outbox_id,abatchqty,isdel,creater,createtime,modifyer," +
                //                    "modifytime, materialnoid,strongholdcode,strongholdname,companycode,productdate,supprdbatch,supprddate,productbatch,edate,storecondition,specialrequire,batchno,barcodemtype," +
                //                    "rownodel, protectway,boxweight,unit,labelmark,boxdetail,matebatch,mixdate,relaweight,productclass,itemqty,workno,mtypef,prorowno,prorownodel,boxcount,dimension,ean,fserialno," + 
                //                    "standard, erpmateid,subiarrsid,originalCode,status,ReceiveTime,Inner_Id,ProjectNo,TracNo,department,erpwarehouseno,departmentname,erpwarehousename) "+
                //                    "select  voucherno,rowno,erpvoucherno,vouchertype,'" + t_Stockd.MaterialNo + "','" + t_Stockd.MaterialDesc + "',spec,cuscode,cusname,supcode,supname,outpackqty,innerpackqty,voucherqty,'" + t_Stockd.Qty + "',nopack,printqty,'" + t_Stockd.Barcode + "',barcodetype," +
                //                    "'" + t_Stockd.SerialNo + "',barcodeno,outcount,innercount,mantissaqty,isrohs,outbox_id,abatchqty,isdel,creater,createtime,modifyer,modifytime," + t_Stockd.MaterialNoID + ",strongholdcode,strongholdname,companycode,productdate,supprdbatch," +
                //                    "supprddate, '"+ modeld.RandomCode + "',edate,storecondition,specialrequire,'"+ t_Stockd.BatchNo + "',barcodemtype,rownodel,protectway,boxweight,unit,labelmark,boxdetail,matebatch,mixdate,relaweight,productclass,itemqty," +
                //                    "workno, mtypef,prorowno,prorownodel,boxcount,dimension,ean,fserialno,standard,erpmateid,subiarrsid,originalCode,status,ReceiveTime,Inner_Id,ProjectNo,TracNo,department,erpwarehouseno,departmentname," + 
                //                    "erpwarehousename from t_outbarcode where serialno= '"+ t_Stock.SerialNo + "'";
                //                sqls.Add(strSql9);

                //                SerialNos = SerialNos + "'"+ t_Stockd.SerialNo + "',";
                //                jilv.AddRange(GetTaskTransSqlList(user, t_Stockd, list[i], 209));
                //                t_Stocks.Add(t_Stockd);
                //            }
                //        }

                //    }
                //    else
                //    {
                //        for (int b = 0; b < list[i].barcodeList.Count; b++)
                //        {  //可理解为都是删除
                //            jilv.AddRange(GetTaskTransSqlList(user, list[i].barcodeList[b], list[i], 210));
                //        }
                //        //多对一是删除  反正只保留一个
                //        for (int j = 1; j < list[i].barcodeList.Count; j++)
                //        {
                //            string sql = string.Format("delete  T_STOCK where barcode='{0}'", list[i].barcodeList[j].Barcode);
                //            sqls.Add(sql);
                //        }
                //        //新条码
                //        T_StockInfo t_Stock = DeepCopyByXml<T_StockInfo>(list[i].barcodeList[0]);

                //        t_Stock.MaterialNo = list[i].MaterialNo;
                //        t_Stock.BatchNo = list[i].BatchNo;
                //        t_Stock.MaterialDesc = list[i].MaterialDesc;
                //        t_Stock.Qty = list[i].Qty;
                //        t_Stock.SerialNo = SerialnoHelp.GetSerialnos(1)[0];
                //        t_Stock.Barcode = "2@" + t_Stock.MaterialNo + "@" + t_Stock.Qty + "@" + t_Stock.SerialNo;

                //        T_Material_DB MDB = new T_Material_DB();
                //        int Materialnoid1 = MDB.GetMaterialNoid(list[i].MaterialNo, list[i].StrongHoldCode);
                //        if (Materialnoid1 == 0)
                //        {
                //            Msg = "据点【" + list[i].StrongHoldCode + "】物料主数据没有物料【" + t_Stock.MaterialNo + "】信息！";
                //            return false;
                //        }
                //        else
                //        {
                //            t_Stock.MaterialNoID = Materialnoid1;
                //        }


                //        string onesql = string.Format("update T_STOCK set MATERIALNO='{0}',materialdesc='{1}',qty={2},barcode='{3}',serialno='{4}',MaterialNoID={5},batchno='{6}'   where barcode='{7}' "
                //        , list[i].MaterialNo, list[i].MaterialDesc, list[i].Qty, t_Stock.Barcode, t_Stock.SerialNo, t_Stock.MaterialNoID, t_Stock.BatchNo, list[i].barcodeList[0].Barcode);
                //        sqls.Add(onesql);

                //        string onesql1 = string.Format("update T_outbarcode set MATERIALNO='{0}',materialdesc='{1}',qty={2},barcode='{3}',serialno='{4}',MaterialNoID={5},batchno='{6}',ProductBatch='{7}'   where barcode='{8}' ",
                //        list[i].MaterialNo, list[i].MaterialDesc, list[i].Qty, t_Stock.Barcode, t_Stock.SerialNo, t_Stock.MaterialNoID, t_Stock.BatchNo, list[i].RandomCode, list[i].barcodeList[0].Barcode);//两种模式都只会扫描一次所以条码集合肯定只能是一个
                //        sqls.Add(onesql1);
                //        SerialNos = SerialNos + "'" + t_Stock.SerialNo + "',";
                //        //生成一个条码加进去
                //        jilv.AddRange(GetTaskTransSqlList(user, t_Stock, list[i], 209));
                //        t_Stocks.Add(t_Stock);
                //    }

                //}
                //sqls.AddRange(jilv);
                //sqls.ForEach(item=> { LogNet.LogInfo(item);});
                //bool istrue = UpdateModelListStatusBySql(sqls, ref Msg);
                //return istrue;

            }
            catch (Exception ex)
            {
                Msg = ex.ToString();
                return false;
            }

        }

        public static T DeepCopyByXml<T>(T obj)
        {
            object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                retval = xml.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }




        // Tasktype=209转换入  Tasktype=210转换出
        private List<string> GetTaskTransSqlList(UserModel user, T_StockInfo model, U9Zh detailModel, int Tasktype)
        {
            //int id = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
            int id = 999999;

            List<string> lstSql = new List<string>();
            string strSql = "insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,taskno,batchno,barcode,status,materialdoc,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
            " values ('" + id + "' , '" + model.SerialNo + "'," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','','','" + model.Qty + "','" + Tasktype + "'," +
            " 52,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
            "'" + detailModel.Unit + "','" + detailModel.Unit + "','','','" + detailModel.ErpVoucherNo + "'," +
            "  '','" + detailModel.StrongHoldCode + "','" + detailModel.StrongHoldName + "',''," +
            "  '" + model.SupPrdBatch + "',''," +
            " '" + model.BatchNo + "' ,'" + model.Barcode + "','" + model.Status + "','','','" + model.EAN + "'," +
            "  (select WAREHOUSENO from T_WAREHOUSE where id ='" + model.WareHouseID + "')," +
            " (select WAREHOUSENAME from T_WAREHOUSE where id ='" + model.WareHouseID + "'), " +
            " (select HOUSENO from T_HOUSE where id='" + model.HouseID + "')," +
            " (select AREANO from T_AREA where id ='" + model.AreaID + "')," +
            " '',''," +
            " ''," +
            " '','" + model.PalletNo + "','" + model.IsPalletOrBox + "' )  ";//,(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),'" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "'

            lstSql.Add(strSql);

            return lstSql;
        }




    }

    public class CommonInfo
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class U9BaseInfo
    {
        public List<CommonInfo> Orgs { get; set; }
        public List<CommonInfo> Sites { get; set; }
        public List<CommonInfo> Departments { get; set; }
        public List<CommonInfo> Persons { get; set; }
        public List<CommonInfo> DocTypes { get; set; }

    }

    public class U9Stock
    {
        public string StrongHoldCode { get; set; }
        public string StrongHoldName { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDesc { get; set; }
        public string Spec { get; set; }
        public string BatchNo { get; set; }
        public string ErpWareHouseNo { get; set; }
        public string ErpWareHouseName { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public int IsAmount { get; set; }
    }

    public class U9StockTo
    {
        public string StrongHoldCode { get; set; }
        public string MaterialNo { get; set; }
        public string BatchNo { get; set; }
        public string ErpWarehouseNo { get; set; }
        public decimal PostQty { get; set; }
        public decimal ScanQty { get; set; }

        public string PostUser { get; set; }
        public string StrVoucherType { get; set; }
        public string GUID { get; set; }
        public string FromErpWarehouse { get; set; }
        public string Unit { get; set; }

    }

    public class U9Zh
    {
        public string MaterialDoc { get; set; }//凭证
        public string PostUser { get; set; }
        public string GUID { get; set; }

        public string ErpVoucherNo { get; set; }
        public long ErpVoucherType { get; set; }
        public string ErpVoucherTypeName { get; set; }
        public string CreateTime { get; set; }
        public string StrongHoldCode { get; set; }
        public string StrongHoldName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerName { get; set; }
        //二层
        public int RowNo { get; set; }
        public int Type { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDesc { get; set; }
        public string Spec { get; set; }
        public string ErpWareHouseNo { get; set; }
        public string ErpWareHouseName { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public List<U9ZhDetail> detail { get; set; }
        public List<U9ZhDetail> detailBehind { get; set; }

        public List<T_StockInfo> barcodeList { get; set; }

        public int VoucherType { get; set; }

        public string BatchNo { get; set; }

        public string RandomCode { get; set; }
    }
    public class U9ZhDetail
    {
        public int subFlag { get; set; }
        public int Row { get; set; }
        public int RowNo { get; set; }
        public int Type { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string MaterialDesc { get; set; }
        public string Spec { get; set; }
        public string ErpWareHouseNo { get; set; }
        public string ErpWareHouseName { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }

        public string BatchNo { get; set; }

        public string RandomCode { get; set; }

        public List<T_StockInfo> barcodeList { get; set; }

    }
    public class MoReport
    {
        /// <summary>
        /// 生产订单号
        /// </summary>
        public string ErpVoucherNo { get; set; }
        /// <summary>
        /// SOP文件地址1
        /// </summary>
        public string Sop1 { get; set; }
        /// <summary>
        /// SOP文件地址2
        /// </summary>
        public string Sop2 { get; set; }
        /// <summary>
        /// SOP文件地址3
        /// </summary>
        public string Sop3 { get; set; }
        /// <summary>
        /// SOP文件地址4
        /// </summary>
        public string Sop4 { get; set; }
        /// <summary>
        /// SOP文件地址5
        /// </summary>
        public string Sop5 { get; set; }
        /// <summary>
        /// SOP文件地址6
        /// </summary>
        public string Sop6 { get; set; }
    }
}
