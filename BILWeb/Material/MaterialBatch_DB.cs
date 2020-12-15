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
        public U9BaseInfo GetInfoList(string id,string StrongHoldCode)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
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


        //SOP列表
        public List<MoReport> GetSopList(string ErpVoucherNo)
        {
            try
            {
                BILBasic.Interface.T_Interface_Func TIF = new BILBasic.Interface.T_Interface_Func();
                string json = "{\"data_no\":\"" + ErpVoucherNo + "\",\"VoucherType\":\"9996\"}";
                string ERPJson = TIF.GetModelListByInterface(json);
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
                return BILBasic.JSONUtil.JSONHelper.JsonToObject<List<U9Zh>>(ERPJson);
            }
            catch (Exception ex)
            {
                return null;
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

        // Tasktype=209转换入  Tasktype=210转换出
        private List<string> GetTaskTransSqlList(UserModel user, T_StockInfo model, U9Zh detailModel, int Tasktype)
        {
            int id = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
            List<string> lstSql = new List<string>();
            string strSql = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno, Materialno, Materialdesc, Supcuscode, " +
            "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,partno,materialnoid,erpvoucherno,voucherno," +
            "Strongholdcode,Strongholdname,Companycode,Supprdbatch,taskno,batchno,barcode,status,materialdoc,houseprop,ean,FromWarehouseNo,FromWarehouseName,FromHouseNo,FromAreaNo,ToWarehouseNo,ToWarehouseName,ToHouseNo,ToAreaNo,PalletNo,IsPalletOrBox)" +
            " values ('" + id + "' , '" + model.SerialNo + "'," +
            " '" + model.MaterialNo + "','" + model.MaterialDesc + "','','','" + model.Qty + "','" + Tasktype + "'," +
            " (52,'" + user.UserName + "',getdate(),'" + model.ID + "', " +
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
            " '','" + model.PalletNo + "','" + model.IsPalletOrBox + "' ) SET IDENTITY_INSERT t_tasktrans off ";//,(select  ID from v_Area a where  warehouseno = '" + model.ToErpWarehouse + "' and  AREANO = '" + model.ToErpAreaNo + "'),'" + model.AreaID + "','" + model.WareHouseID + "','" + model.HouseID + "'

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
    public class U9Zh
    {
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
        public string Spec{ get; set; }
        public string ErpWareHouseNo { get; set; }
        public string ErpWareHouseName { get; set; }
        public decimal Qty { get; set; }
        public string Unit { get; set; }
        public List<U9ZhDetail> detail { get; set; }

    }
    public class U9ZhDetail
    {
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
