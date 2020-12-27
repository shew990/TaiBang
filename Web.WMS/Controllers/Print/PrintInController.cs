using BILWeb.AdvInStock;
using BILWeb.InStock;
using BILWeb.Login.User;
using BILWeb.Material;
using BILWeb.OutBarCode;
using BILWeb.Print;
using BILWeb.Product;
using BILWeb.Stock;
using BILWeb.View_Product;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using Web.WMS.Models;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Print
{
    [RoleActionFilter(Message = "Print/PrintIn")]
    public class PrintInController : BaseController<T_AdvInStockDetailInfo>
    {
        UserInfo currentUser = Common.Commom.ReadUserInfo();
        public string Userno = Commom.ReadCookie("userinfo");
        private IAdvInStockDetailService advInStockDetailService;
        public PrintInController()
        {
            advInStockDetailService = (IAdvInStockDetailService)ServiceFactory.CreateObject("AdvInStock.T_AdvInStockDetail_Func");
            baseservice = advInStockDetailService;
        }

        [HttpPost]
        public JsonResult PrintList(string IDs)
        {
            if (string.IsNullOrEmpty(Userno))
            {
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            }
            string strError = string.Empty;
            List<T_AdvInStockDetailInfo> lstAdvInStockDetailInfo = new List<T_AdvInStockDetailInfo>();
            string[] strId = IDs.Split(',');
            for (int i = 0; i < strId.Length; i++)
            {
                if (!string.IsNullOrEmpty(strId[i]))
                {
                    T_AdvInStockDetailInfo model = new T_AdvInStockDetailInfo();
                    model.ID = Convert.ToInt32(strId[i]);
                    if (!advInStockDetailService.GetModelByID(ref model, ref strError))
                    {
                        return Json(new { state = false, obj = strError }, JsonRequestBehavior.AllowGet);
                    }
                    lstAdvInStockDetailInfo.Add(model);
                }
            }

            string err = "";
            Print_DB print_DB = new Print_DB();
            List<Barcode_Model> listbarcode = new List<Barcode_Model>();
            //每行打印
            if (lstAdvInStockDetailInfo != null && lstAdvInStockDetailInfo.Count != 0)
            {
                List<string> squence = GetSerialnos(lstAdvInStockDetailInfo.Count.ToString(), "外", ref err);
                int k = 0;
                for (int i = 0; i < lstAdvInStockDetailInfo.Count; i++)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = lstAdvInStockDetailInfo[i].CompanyCode;
                    model.MaterialNoID = lstAdvInStockDetailInfo[i].MaterialNoID;
                    model.MaterialNo = lstAdvInStockDetailInfo[i].MaterialNo;
                    model.MaterialDesc = lstAdvInStockDetailInfo[i].MaterialDesc;
                    model.BatchNo = lstAdvInStockDetailInfo[i].SupBatch;
                    model.ErpVoucherNo = lstAdvInStockDetailInfo[i].ErpVoucherNo;
                    model.EDate = Convert.ToDateTime(lstAdvInStockDetailInfo[i].EDate);
                    model.Qty = Convert.ToDecimal(lstAdvInStockDetailInfo[i].AdvQty);
                    model.StrongHoldCode = lstAdvInStockDetailInfo[i].StrongHoldCode;
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.EAN = lstAdvInStockDetailInfo[i].EAN;
                    model.ReceiveTime = Convert.ToDateTime(lstAdvInStockDetailInfo[i].CreateTime);
                    model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = lstAdvInStockDetailInfo[i].RowNO;
                    model.RowNoDel = lstAdvInStockDetailInfo[i].RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = lstAdvInStockDetailInfo[i].Createname;
                    model.WorkNo = lstAdvInStockDetailInfo[i].WarehouseName;
                    listbarcode.Add(model);
                }
            }
            if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
            {
                string serialnos = "";
                for (int i = 0; i < listbarcode.Count; i++)
                {
                    serialnos += listbarcode[i].SerialNo + ",";
                }
                return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="num"></param>
        /// <param name="everynum"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveBarcode(string data)
        {
            var objT_InStockDetailInfo = JsonConvert.DeserializeObject<T_InStockDetailInfo>(data);

            string EveryQty = objT_InStockDetailInfo.EveryQty, num = objT_InStockDetailInfo.num,
                Userno = objT_InStockDetailInfo.Userno, erpvoucherno = objT_InStockDetailInfo.ErpVoucherNo,
                materialno = objT_InStockDetailInfo.MaterialNo,
                materialdesc = objT_InStockDetailInfo.MaterialDesc, RowNO = objT_InStockDetailInfo.RowNo,
                RowNODel = objT_InStockDetailInfo.RowNoDel, MaterialNoID = objT_InStockDetailInfo.MaterialNoID.ToString(),
                StrongHoldCode = objT_InStockDetailInfo.StrongHoldCode, CompanyCode = objT_InStockDetailInfo.CompanyCode,
                Createname = objT_InStockDetailInfo.Creater, WarehouseName = objT_InStockDetailInfo.WareHouseNo,
                TracNo = objT_InStockDetailInfo.TracNo, ProjectNo = objT_InStockDetailInfo.ProjectNo,
                BatchNo = objT_InStockDetailInfo.FromBatchNo;

            T_InStock_Func InFunc = new T_InStock_Func();
            string strMsg = "";
            T_InStockInfo InStockInfo = new T_InStockInfo() { ID = objT_InStockDetailInfo.HeaderID };
            InFunc.GetModelByID(ref InStockInfo, ref strMsg);
            //查物料
            T_Material_Func funM = new T_Material_Func();
            string strErrMsg = "";
            List<T_MaterialInfo> modelList = funM.GetMaterialModelBySql(materialno, ref strErrMsg);
            if (modelList == null || modelList.Count == 0)//失败
                return Json(new { state = false, obj = "没有该物料号" + materialno }, JsonRequestBehavior.AllowGet);
            if (string.IsNullOrEmpty(Userno))
                return Json(new { state = false, obj = "Cookie失效，重新登陆！" }, JsonRequestBehavior.AllowGet);
            try
            {
                DateTime time1 = DateTime.Now;
                DateTime time2 = DateTime.Now.AddSeconds(1);
                string err = "";
                //计算外箱数量,和尾箱数量,和尾箱里面的个数
                int outboxnum = 0;
                int inboxnum = 0;
                decimal tailnum = 0;
                GetBoxInfo(ref outboxnum, ref tailnum, ref inboxnum, num, EveryQty);
                if (outboxnum == 0)
                    return Json(new { state = false, obj = "打印数量为0" }, JsonRequestBehavior.AllowGet);

                Print_DB print_DB = new Print_DB();
                List<string> squence = GetSerialnos((outboxnum + inboxnum).ToString(), "外", ref err);//外箱码序列号
                List<string> squenceforin = GetSerialnos(num, "内", ref err);//本体序列号
                List<Barcode_Model> listbarcode = new List<Barcode_Model>();//存放打印条码内容
                T_Product_DB proDB = new T_Product_DB();
                int k = 0;
                for (int i = 0; i < outboxnum; i++)//执行打印外箱命令
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID = Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    //model.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                    model.BatchNo = BatchNo;
                    model.ProductBatch = objT_InStockDetailInfo.ProductBatch;//给批号加密成8位
                    model.ErpVoucherNo = erpvoucherno;
                    model.Qty = Convert.ToDecimal(EveryQty);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.ReceiveTime = time1;
                    model.BarCode = "2@" + model.MaterialNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    model.TracNo = TracNo;
                    model.ProjectNo = ProjectNo;

                    model.SupCode = InStockInfo.SupplierNo;
                    model.SupName = InStockInfo.SupplierName;
                    model.StoreCondition = InStockInfo.StrVoucherType;
                    model.department = InStockInfo.DepartmentCode;
                    model.departmentname = InStockInfo.DepartmentName;

                    model.ProtectWay = objT_InStockDetailInfo.sale_vouchertypename;
                    model.LABELMARK = objT_InStockDetailInfo.Customer_voucherno;


                    model.CusCode = objT_InStockDetailInfo.SupplierNo;
                    model.CusName = objT_InStockDetailInfo.SUPPLIERSHORTNAME;
                    model.erpwarehousename = objT_InStockDetailInfo.ErpWarehouseName;
                    model.StoreCondition = objT_InStockDetailInfo.CustomerItemCode;

                    listbarcode.Add(model);
                }
                if (inboxnum == 1)
                {
                    Barcode_Model model = new Barcode_Model();
                    model.CompanyCode = CompanyCode;
                    model.StrongHoldCode = StrongHoldCode;
                    model.MaterialNoID = Convert.ToInt32(MaterialNoID);
                    model.MaterialNo = materialno;
                    model.MaterialDesc = materialdesc;
                    //model.BatchNo = DateTime.Now.ToString("yyyyMMdd");
                    model.BatchNo = BatchNo;
                    model.ProductBatch = objT_InStockDetailInfo.ProductBatch;//给批号加密成8位
                    model.ErpVoucherNo = erpvoucherno;
                    model.Qty = Convert.ToDecimal(tailnum);
                    model.SerialNo = squence[k++];
                    model.Creater = Userno;
                    model.ReceiveTime = time1;
                    model.BarCode = "2@" + model.MaterialNo + "@" + model.Qty + "@" + model.SerialNo;
                    model.RowNo = RowNO;
                    model.RowNoDel = RowNODel;
                    model.BarcodeType = 1;
                    model.ProductClass = Createname;
                    model.WorkNo = WarehouseName;
                    model.TracNo = TracNo;
                    model.ProjectNo = ProjectNo;

                    model.SupCode = InStockInfo.SupplierNo;
                    model.SupName = InStockInfo.SupplierName;
                    model.StoreCondition = InStockInfo.StrVoucherType;
                    model.department = InStockInfo.DepartmentCode;
                    model.departmentname = InStockInfo.DepartmentName;

                    model.ProtectWay = objT_InStockDetailInfo.sale_vouchertypename;
                    model.LABELMARK = objT_InStockDetailInfo.Customer_voucherno;

                    model.CusCode = objT_InStockDetailInfo.SupplierNo;
                    model.CusName = objT_InStockDetailInfo.SUPPLIERSHORTNAME;
                    model.erpwarehousename = objT_InStockDetailInfo.ErpWarehouseName;
                    model.StoreCondition = objT_InStockDetailInfo.CustomerItemCode;
                    listbarcode.Add(model);
                }
                if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
                {
                    string serialnosS = "";
                    for (int i = 0; i < listbarcode.Count; i++)
                    {
                        if (listbarcode[i].BarcodeType != 1)
                            serialnosS += listbarcode[i].SerialNo + ",";
                    }
                    if (serialnosS == "")
                        return Json(new { state = true, obj = time1.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                    return Json(new { state = true, obj = time1.ToString("yyyy/MM/dd HH:mm:ss"), objS = time2.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        //外箱码打印生产订单
        [HttpPost]
        public ActionResult SaveBarcodePro(string data)
        {
            var objT_InStockDetailInfo = JsonConvert.DeserializeObject<View_Product_Model>(data);
            string err = "";
            T_Product_DB print_DB = new T_Product_DB();
            DateTime time1=DateTime.Now;
            try
            {
                if (print_DB.SaveBarcodeForPro(currentUser, objT_InStockDetailInfo, time1, ref err))
                {
                    return Json(new { state = true, obj = time1.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<string> GetSerialnos(string v, string flag, ref string err)
        {
            decimal Vnum = Convert.ToDecimal(v);
            int VZ = (int)Vnum / 1;
            decimal VL = Vnum % 1;

            List<string> serialnos = new List<string>();
            for (int i = 0; i < VZ; i++)
            {
                //var seed = Guid.NewGuid().GetHashCode();
                //string code = DateTime.Now.ToString("yyMMddHHmmss") + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');
                string code = "1" + DateTime.Now.ToString("MMdd") + getSqu(Guid.NewGuid().ToString("N"));

                if (serialnos.Find(t => t == code) == null)
                {
                    serialnos.Add(code);
                }
                else
                {
                    i--;
                }
            }
            if (VL != 0)
            {
                string code = "1" + DateTime.Now.ToString("MMdd") + getSqu(Guid.NewGuid().ToString("N"));
                if (serialnos.Find(t => t == code) == null)
                {
                    serialnos.Add(code);
                }
                else
                {
                    serialnos.Add(code + "1");
                }
            }
            return serialnos;
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

        private void GetBoxInfo(ref int outboxnum, ref decimal tailnum, ref int inboxnum, string num, string everynum)
        {
            if (decimal.Parse(num) % decimal.Parse(everynum) == 0)
            {
                outboxnum = (int)(decimal.Parse(num) / decimal.Parse(everynum));
                tailnum = decimal.Parse(everynum);
                inboxnum = 0;
            }
            else
            {
                outboxnum = (int)(decimal.Parse(num) / decimal.Parse(everynum));
                tailnum = decimal.Parse(num) % decimal.Parse(everynum);
                inboxnum = 1;
            }

        }


        public JsonResult DeleteForAdv(string ID)
        {
            try
            {
                T_AdvInStockDetailInfo model = new T_AdvInStockDetailInfo { ID = Convert.ToInt32(ID) };
                string strmsg = "";
                advInStockDetailService.GetModelByID(ref model, ref strmsg);
                T_AdvInStockDetail_DB advdb = new T_AdvInStockDetail_DB();
                if (advdb.SaveDeleteAdvDetail(model, out strmsg))
                {
                    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false, obj = strmsg }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }


    }
}