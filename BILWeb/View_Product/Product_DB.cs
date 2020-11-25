using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BILBasic.Basing.Factory;
using BILBasic.DBA;
using Oracle.ManagedDataAccess.Client;
using BILBasic.Common;
using BILBasic.User;
using System.Data;
using BILWeb.Print;

namespace BILWeb.Product
{
    public partial class T_Product_DB : BILBasic.Basing.Factory.Base_DB<T_Product>
    {

        /// <summary>
        /// 添加t_Productwithtask
        /// </summary>
        protected override IDataParameter[] GetSaveModelIDataParameter(T_Product T_Product)
        {
            //注意!head表ID要填basemodel的headerID new SqlParameter("@CustomerID", DbHelperSQL.ToDBValue(model.HeaderID)),
            throw new NotImplementedException();


        }

        protected override List<string> GetSaveSql(UserModel user, ref T_Product T_Product)
        {
            throw new NotImplementedException();
        }

        protected override List<string> GetSaveModelListSql(UserModel user, List<T_Product> modelList)
        {

            if (modelList == null || modelList.Count == 0)
            {
                return null;
            }
            List<string> lstSql = new List<string>();

            //获取托盘码
            int ID = base.GetTableIDBySqlServer("t_Pallet");
            string PalletNo = "P" + System.DateTime.Now.ToString("yyMMdd") + ID.ToString().PadLeft(6, '0');


            foreach (var item in modelList)
            {
                string strSql1 = "update T_Product  set   Receiveqty = (isnull( Receiveqty,0) + '" + item.ScanQty + "')  where ErpVoucherNo  ='" + item.ErpVoucherNo + "'";
                lstSql.Add(strSql1);// remainqty = (case when (isnull( remainqty,0) - '" + item.ScanQty + "') <= 0 then 0 else isnull( remainqty,0) - '" + item.ScanQty + "' end),

                //插托盘表头
                string strSql4 = string.Format("SET IDENTITY_INSERT t_Pallet on ;insert into t_Pallet(Id, Palletno, Creater, Createtime,Strongholdcode,Strongholdname,Companycode,Pallettype,ERPVOUCHERNO)" +
                     " values ('{0}','{1}','{2}',GETDATE(),'{3}','{4}','{5}','{6}','{7}');SET IDENTITY_INSERT t_Pallet off ;", ID, PalletNo, user.UserNo, modelList[0].StrongHoldCode, modelList[0].StrongHoldName, modelList[0].CompanyCode, '1', modelList[0].ErpVoucherNo);

                lstSql.Add(strSql4);

                foreach (var itemBarCode in item.lstBarCode)
                {
                    string strSql2 = "insert into t_stock(serialno,Materialno,materialdesc,qty,status,isdel,Creater,Createtime,batchno,unit,unitname,Palletno," +
                             "islimitstock,materialnoid,warehouseid,houseid,areaid,Receivestatus,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,EDATE,SUPCODE,SUPNAME," +
                            "SUPPRDBATCH,Isquality,Stocktype,ean,BARCODETYPE,projectNo,TracNo)" +
                            "values ('" + itemBarCode.SerialNo + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','" + itemBarCode.Qty + "','3','1'" +
                            ",'" + user.UserNo + "',getdate(),'" + itemBarCode.BatchNo + "','" + item.Unit + "','" + item.Unit + "'" +
                            ",'" + PalletNo + "','1','" + itemBarCode.MaterialNoID + "'" +
                            ", '" + user.WarehouseID + "','" + user.ReceiveHouseID + "','" + user.ReceiveAreaID + "','1','" + itemBarCode.BarCode + "','" + item.StrongHoldCode + "', " +
                            "  '" + itemBarCode.StrongHoldName + "','" + itemBarCode.CompanyCode + "','" + GetStrDateTime(itemBarCode.EDate) + "','',''," +
                            "'" + itemBarCode.SupPrdBatch + "','3' ,'1','" + itemBarCode.EAN + "','" + itemBarCode.BarcodeType + "','" + (itemBarCode.ProjectNo == null ? "" : itemBarCode.ProjectNo) + "','" + (itemBarCode.TracNo == null ? "" : itemBarCode.TracNo) + "' )";

                    lstSql.Add(strSql2);

                    var TaskTransID = base.GetTableIDBySqlServerTaskTrans("t_tasktrans");
                    string strSql3 = "SET IDENTITY_INSERT t_tasktrans on ;insert into t_tasktrans(id, Serialno,towarehouseid,Tohouseid, Toareaid, Materialno, Materialdesc, Supcuscode, " +
                                "Supcusname, Qty, Tasktype, Vouchertype, Creater, Createtime,TaskdetailsId, Unit, Unitname,materialnoid," +
                                "erpvoucherno,voucherno,barcode,STRONGHOLDCODE,STRONGHOLDNAME,COMPANYCODE,SUPPRDBATCH,EDATE,TASKNO,batchno,ToWarehouseNo,ToHouseNo,ToAreaNo,ToWarehouseName)" +
                            " values ('" + TaskTransID + "','" + itemBarCode.SerialNo + "','" + user.WarehouseID + "','" + user.ReceiveHouseID + "'," +
                            "'" + user.ReceiveAreaID + "','" + itemBarCode.MaterialNo + "','" + itemBarCode.MaterialDesc + "','',''," +
                            " '" + itemBarCode.Qty + "','5','','" + user.UserName + "',getdate(),'" + item.ID + "'," +
                            "'" + item.Unit + "','" + item.Unit + "','" + itemBarCode.MaterialNoID + "','" + item.ErpVoucherNo + "','','" + itemBarCode.BarCode + "'," +
                            "'" + item.StrongHoldCode + "','" + item.StrongHoldName + "','" + item.CompanyCode + "','" + itemBarCode.SupPrdBatch + "'" +
                            " ,'" + GetStrDateTime(itemBarCode.EDate) + "','','" + itemBarCode.BatchNo + "','" + user.ReceiveWareHouseNo + "','" + user.ReceiveHouseNo + "','" + user.ReceiveAreaNo + "','" + user.ReceiveWareHouseName + "') SET IDENTITY_INSERT t_tasktrans off";
                    lstSql.Add(strSql3);

                    //插入托盘表
                    var detailID = base.GetTableIDBySqlServerTaskTrans("t_Palletdetail");
                    string strSql5 = string.Format("SET IDENTITY_INSERT t_Palletdetail on ;insert into t_Palletdetail(Id, Headerid, Palletno, Materialno, Materialdesc, Serialno,Creater," +
                    "Createtime,RowNo,VOUCHERNO,ERPVOUCHERNO,materialnoid,qty,BARCODE,StrongHoldCode,StrongHoldName,CompanyCode,pallettype," +
                    "batchno,rownodel,PRODUCTDATE,SUPPRDBATCH,SUPPRDDATE,PRODUCTBATCH,EDATE,Supplierno,Suppliername,Unit)" +
                    "values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',GETDATE(),'{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','1'," +
                    "'{16}','{17}',null,'{19}',null,'null','{22}','{23}','{24}','{25}');SET IDENTITY_INSERT t_Palletdetail off ;", detailID, ID, PalletNo, itemBarCode.MaterialNo, itemBarCode.MaterialDesc, itemBarCode.SerialNo, user.UserNo,
                    itemBarCode.RowNo, itemBarCode.VoucherNo, itemBarCode.ErpVoucherNo, itemBarCode.MaterialNoID, itemBarCode.Qty, itemBarCode.BarCode,
                    itemBarCode.StrongHoldCode, itemBarCode.StrongHoldName, itemBarCode.CompanyCode, itemBarCode.BatchNo, itemBarCode.RowNoDel,
                    itemBarCode.ProductDate, itemBarCode.SupPrdBatch, itemBarCode.SupPrdDate, itemBarCode.ProductBatch, GetStrDateTime(itemBarCode.EDate), itemBarCode.SupCode, itemBarCode.SupName, itemBarCode.Unit);

                    lstSql.Add(strSql5);

                }

            }

            return lstSql;
        }

        public bool SaveProDuctBarcode(UserModel user, T_Product modelList)
        {
            try
            {
                Print_DB print_DB = new Print_DB();
                List<Barcode_Model> listbarcode = new List<Barcode_Model>();
                Barcode_Model model = new Barcode_Model();
                model.CompanyCode = "";
                model.StrongHoldCode = modelList.StrongHoldCode;
                model.MaterialNoID = modelList.MaterialNoID;
                model.MaterialNo = modelList.MaterialNo;
                model.MaterialDesc = modelList.MaterialDesc;
                model.BatchNo = modelList.BatchNo;
                model.ProductBatch = GetBatchno(model.BatchNo);//给批号加密成8位
                model.ErpVoucherNo = modelList.ErpVoucherNo;
                model.Qty = Convert.ToDecimal(modelList.ScanQty);

                var seed = Guid.NewGuid().GetHashCode();
                string code = DateTime.Now.ToString("MMdd") + getSqu(Guid.NewGuid().ToString("N"));

                model.SerialNo = code;
                model.Creater = user.UserNo;
                model.ReceiveTime = DateTime.Now;
                model.BarCode = "2@" + model.MaterialNo + "@" + model.Qty + "@" + model.SerialNo;
                model.RowNo = "1";
                model.RowNoDel = "1";
                model.BarcodeType = 1;
                listbarcode.Add(model);
                string err = "";
                if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
                {
                    //修改关联量，插入记录表
                    if (updateproduct(modelList, model.BarCode))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool updateproduct(T_Product product, string fserialno)
        {
            try
            {
                List<string> sqls = new List<string>();
                string strsql1 = "update T_Product set LinkQty=LinkQty+" + product.ScanQty + " where erpvoucherno='" + product.ErpVoucherNo + "';";
                sqls.Add(strsql1);
                product.Detail.ForEach(item =>
                {
                    string insertsql = "insert into t_linkserialno(fserialno,serialno,CREATETIME) VALUES ('" + fserialno + "','" + item.Barcode + "',getdate()); ";
                    sqls.Add(insertsql);
                });

                int i = dbFactory.ExecuteNonQueryList(sqls);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



        /// <summary>
        /// 将获取的单条数据转封装成对象返回
        /// </summary>
        protected override T_Product ToModel(IDataReader reader)
        {
            T_Product T_Product = new T_Product();

            T_Product.ID = dbFactory.ToModelValue(reader, "ID").ToInt32();
            T_Product.ErpVoucherNo = (string)dbFactory.ToModelValue(reader, "ErpVoucherNo");
            T_Product.StrongHoldCode = (string)dbFactory.ToModelValue(reader, "StrongHoldCode");
            T_Product.StrongHoldName = (string)dbFactory.ToModelValue(reader, "StrongHoldName");
            T_Product.DepartmentCode = (string)dbFactory.ToModelValue(reader, "DepartmentCode");
            T_Product.DepartmentName = (string)dbFactory.ToModelValue(reader, "DepartmentName");

            T_Product.ErpVoucherTypeCode = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeCode");
            T_Product.ErpVoucherTypeName = (string)dbFactory.ToModelValue(reader, "ErpVoucherTypeName");
            T_Product.BatchNo = (string)dbFactory.ToModelValue(reader, "BatchNo");
            T_Product.Unit = (string)dbFactory.ToModelValue(reader, "Unit");
            T_Product.PubDescSeg10_Code = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Code");

            T_Product.PubDescSeg10_Name = (string)dbFactory.ToModelValue(reader, "PubDescSeg10_Name");
            T_Product.PubDescSeg5 = (string)dbFactory.ToModelValue(reader, "PubDescSeg5");
            T_Product.PubDescSeg4 = (string)dbFactory.ToModelValue(reader, "PubDescSeg4");
            T_Product.PubDescSeg7 = (string)dbFactory.ToModelValue(reader, "PubDescSeg7");

            T_Product.LineCode = (string)dbFactory.ToModelValue(reader, "LineCode");
            T_Product.LineName = (string)dbFactory.ToModelValue(reader, "LineName");
            T_Product.ErpWarehouseNo = (string)dbFactory.ToModelValue(reader, "ErpWarehouseNo");
            T_Product.ErpWarehouseName = (string)dbFactory.ToModelValue(reader, "ErpWarehouseName");

            T_Product.MaterialNo = (string)dbFactory.ToModelValue(reader, "MaterialNo");
            T_Product.MaterialDesc = (string)dbFactory.ToModelValue(reader, "MaterialDesc");
            T_Product.spec = (string)dbFactory.ToModelValue(reader, "spec");
            T_Product.MaterialName = (string)dbFactory.ToModelValue(reader, "MaterialName");

            T_Product.CustomerCode = (string)dbFactory.ToModelValue(reader, "CustomerCode");
            T_Product.CustomerName = (string)dbFactory.ToModelValue(reader, "CustomerName");
            T_Product.CustomerShortName = (string)dbFactory.ToModelValue(reader, "CustomerShortName");
            T_Product.QulityQty = (decimal)dbFactory.ToModelValue(reader, "QulityQty");
            T_Product.LinkQty = (decimal)dbFactory.ToModelValue(reader, "LinkQty");
            T_Product.LinkQty = (decimal)dbFactory.ToModelValue(reader, "LinkQty");
            T_Product.ProductQty = (decimal)dbFactory.ToModelValue(reader, "ProductQty");

            T_Product.PackQty = (decimal)dbFactory.ToModelValue(reader, "PackQty");
            T_Product.Strstatus = (string)dbFactory.ToModelValue(reader, "Strstatus");
            T_Product.ProductBatch = (string)dbFactory.ToModelValue(reader, "ProductBatch");

            return T_Product;
        }

        protected override string GetViewName()
        {
            return "v_Product";
        }

        protected override string GetTableName()
        {
            return "T_Product";
        }

        protected override string GetSaveProcedureName()
        {
            return "";
        }

        protected override string GetFilterSql(UserModel user, T_Product model)
        {
            string strSql = " where 1=1 ";
            string strAnd = " and ";
            if (!string.IsNullOrEmpty(model.ErpVoucherNo))
            {
                strSql += strAnd;
                strSql += " erpvoucherno like '%" + model.ErpVoucherNo.Trim() + "%' ";
            }
            return strSql;
        }

        #region 生成加密批次
        public string GetBatchno(string batch)
        {
            string NewProductBatch = getSqu(Guid.NewGuid().ToString("N"));
            try
            {
                //检查批次是否已经存在
                string ProductBatch = GetProductBatch(batch);
                if (string.IsNullOrEmpty(ProductBatch))
                {
                    //插入新的加密批次
                    if (InsertBatch(batch, NewProductBatch))
                    {
                        return NewProductBatch;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return ProductBatch;
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        public string GetProductBatch(string batch)
        {
            string strSql = "select ProductBatch from  t_batch where batchno='" + batch + "'";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        public string GetBatch(string ProductBatch)
        {
            string strSql = "select batchno from  t_batch where ProductBatch='" + ProductBatch + "'";
            return base.GetScalarBySql(strSql).ToDBString();
        }

        public bool InsertBatch(string batch, string ProductBatch)
        {
            try
            {
                List<string> sqls = new List<string>();
                int ID = base.GetTableIDBySqlServer("t_batch");
                string sqlhead = "SET IDENTITY_INSERT t_batch on ;insert into t_batch(id,batchno,ProductBatch,CREATETIME) VALUES (" + ID + ",'" + batch + "','" + ProductBatch + "',getdate()) SET IDENTITY_INSERT t_batch off ";
                sqls.Add(sqlhead);
                int i = dbFactory.ExecuteNonQueryList(sqls);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string getSqu(string ss)
        {
            if (ss.Length >= 8)
                ss = ss.Substring(ss.Length - 8, 8);
            else
            {
                ss = "00000000" + ss;
                ss = ss.Substring(ss.Length - 8, 8);
            }
            return ss;
        }

        //关闭生产订单
        public bool CloseProduct(string ErpVoucherno)
        {
            try
            {
                List<string> sqls = new List<string>();
                string strsql = "update T_Product set Strstatue='关闭' where erpvoucherno='" + ErpVoucherno + "'";
                sqls.Add(strsql);
                int i = dbFactory.ExecuteNonQueryList(sqls);
                if (i == -2)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion

    }
}