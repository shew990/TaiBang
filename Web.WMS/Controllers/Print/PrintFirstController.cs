using BILWeb.Login.User;
using BILWeb.Material;
using BILWeb.OutBarCode;
using BILWeb.Print;
using BILWeb.T00L;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using SqlSugarDAL.barcode;
using SqlSugarDAL.Until;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Report.Print;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Print
{
    [RoleActionFilter(Message = "Print/PrintFirst")]
    public class PrintFirstController : Controller
    {
        protected UserInfo currentUser = Common.Commom.ReadUserInfo();

        /// <summary>
        /// 返回主视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<Barcode_Model> list = TempData["list"] as List<Barcode_Model>;
            string Path = TempData["Path"] as string;
            ViewData["Data"] = list;
            ViewData["Path"] = Path;
            return View();
        }

        /// <summary>
        /// 获取表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                    return Json(new { Result = -1, ResultValue = "获取不到数据！" }, JsonRequestBehavior.AllowGet);
                List<Barcode_Model> barcodelist = new List<Barcode_Model>();
                DataTable dt = ImportExcelFilexlsx(path);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Barcode_Model barcode = new Barcode_Model();
                    barcode.MaterialNo = dt.Rows[i][0].ToString();
                    barcode.MaterialDesc = dt.Rows[i][1].ToString();
                    barcode.spec = dt.Rows[i][2].ToString();
                    barcode.StoreCondition = dt.Rows[i][3].ToString();//客户型号
                    barcode.BatchNo = dt.Rows[i][4].ToString();
                    barcode.ProductBatch = dt.Rows[i][5].ToString();
                    barcode.StrongHoldCode = dt.Rows[i][6].ToString();
                    barcode.warehouseno = dt.Rows[i][7].ToString();
                    barcode.warehousename = dt.Rows[i][8].ToString();
                    barcode.department = dt.Rows[i][9].ToString();//部门编码
                    barcode.departmentname = dt.Rows[i][10].ToString();
                    barcode.CusCode = dt.Rows[i][11].ToString();
                    barcode.CusName = dt.Rows[i][12].ToString();
                    barcode.ProtectWay = dt.Rows[i][13].ToString();//单据类型
                    barcode.LABELMARK = dt.Rows[i][14].ToString();//客户订单号
                    barcode.Qty = Convert.ToDecimal(dt.Rows[i][15]);
                    barcode.Unit = dt.Rows[i][16].ToString();
                    barcode.InnerPackQty = Convert.ToDecimal(dt.Rows[i][17]);

                    barcodelist.Add(barcode);
                }
                if (barcodelist == null || barcodelist.Count == 0)
                    return Json(new { Result = -1, ResultValue = "获取不到数据！" }, JsonRequestBehavior.AllowGet);
                return Json(new { Result = 1, Data = barcodelist }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = -1, ResultValue = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 导入excel
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportExcel()
        {
            List<Barcode_Model> barcodelist = new List<Barcode_Model>();
            HttpPostedFileBase File = Request.Files["file"];
            string path = "";
            if (File.ContentLength > 0)
            {
                var Isxls = Path.GetExtension(File.FileName).ToString().ToLower();
                if (Isxls != ".xls" && Isxls != ".xlsx")
                    Content("请上传Excel文件");
                var FileName = File.FileName;//获取文件夹名称
                path = Server.MapPath("~/Upload/" + DateTime.Now.ToString("yyyyMMddHHmmss")) + FileName;
                File.SaveAs(path);//将文件保存到服务器
                DataTable dt = new DataTable();
                if (Isxls == ".xls")
                    dt = ImportExcelFile(path);
                if (Isxls == ".xlsx")
                    dt = ImportExcelFilexlsx(path);
                barcodelist = Print_DB.ConvertToModel<Barcode_Model>(dt);
            }
            TempData["list"] = barcodelist;
            TempData["Path"] = path;
            return RedirectToAction("Index", "PrintFirst");
        }

        //期初打印方法
        [ValidateInput(false)]
        public ActionResult Print(string data)
        {
            SuccessResult successResult = new SuccessResult();
            successResult.Success = false;
            try
            {
                var barcodes = JsonConvert.DeserializeObject<List<Barcode_Model>>(data);
                List<T_OutBarcode> outBarcodes = new List<T_OutBarcode>();
                DateTime time = Convert.ToDateTime(DateTime.Now.ToString());
                foreach (var item in barcodes)
                {
                    int count = item.Qty % item.InnerPackQty == 0
                        ? Convert.ToInt32(Math.Floor(item.Qty / item.InnerPackQty))
                        : Convert.ToInt32(Math.Floor(item.Qty / item.InnerPackQty)) + 1;
                    var serialnos = SerialnoHelp.GetSerialnos(count);//多条序列号
                    var material = new MaterialService()
                        .GetList(x => x.MATERIALNO == item.MaterialNo && x.STRONGHOLDCODE == item.StrongHoldCode)
                        .FirstOrDefault();
                    if (material == null)
                        throw new Exception("没有物料编号为" + item.MaterialNo + "的物料!");

                    for (int i = 0; i < count; i++)
                    {
                        T_OutBarcode barcode = new T_OutBarcode();
                        barcode.materialno = item.MaterialNo;
                        barcode.materialdesc = item.MaterialDesc;
                        barcode.spec = item.spec;
                        barcode.batchno = item.BatchNo;
                        barcode.productbatch = item.ProductBatch;
                        barcode.strongholdcode = item.StrongHoldCode;
                        barcode.erpwarehouseno = item.warehouseno;
                        barcode.erpwarehousename = item.warehousename;
                        barcode.storecondition = item.StoreCondition;
                        //barcode.ware = item.warehousename;//仓库名称
                        barcode.department = item.department;
                        barcode.departmentname = item.departmentname;
                        barcode.barcodetype = 1;
                        barcode.cuscode = item.CusCode == null ? "" : item.CusCode;
                        barcode.cusname = item.CusName == null ? "" : item.CusName;
                        barcode.protectway = item.ProtectWay;
                        barcode.labelmark = item.LABELMARK == null ? "" : item.LABELMARK;
                        barcode.qty = item.Qty <= item.InnerPackQty ? item.Qty : ((i == count - 1)
                                    ? (item.Qty % item.InnerPackQty == 0 ? item.InnerPackQty
                                    : item.Qty % item.InnerPackQty) : item.InnerPackQty);
                        barcode.unit = item.Unit;
                        barcode.ReceiveTime = time;
                        barcode.serialno = serialnos[i];
                        barcode.barcode = "2@" + item.MaterialNo + "@" + barcode.qty + "@" + serialnos[i];
                        barcode.materialnoid = material.ID;
                        barcode.creater = currentUser.UserNo;

                        outBarcodes.Add(barcode);
                    }
                }
                new OutBarcodeService().Insert(outBarcodes);
                successResult.Data = time.ToString();
                successResult.Success = true;
            }
            catch (Exception ex)
            {
                successResult.Msg = ex.Message;
            }
            return Json(successResult, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 获取序列号
        ///// </summary>
        ///// <param name="count"></param>
        ///// <returns></returns>
        //public List<string> GetSerialnos(int count)
        //{
        //    List<string> serialnos = new List<string>();
        //    for (int i = 0; i < count; i++)
        //    {
        //        string code = "1" + DateTime.Now.ToString("MMdd") + getSqu(Guid.NewGuid().ToString("N"));
        //        if (serialnos.Find(x => x == code) != null)
        //        {
        //            i--;
        //            continue;
        //        }
        //        serialnos.Add(code);
        //    }
        //    return serialnos;
        //}

        //public string getSqu(string ss)
        //{
        //    if (ss.Length >= 8)
        //    {
        //        ss = ss.Substring(ss.Length - 8, 8);
        //    }
        //    else
        //    {
        //        ss = "00000000" + ss;
        //        ss = ss.Substring(ss.Length - 8, 8);
        //    }
        //    return ss;
        //}

        //public JsonResult Print(string IDs, string Path)
        //{
        //    try
        //    {
        //        List<Barcode_Model> barcodelist = new List<Barcode_Model>();
        //        var Isxls = System.IO.Path.GetExtension(Path).ToString().ToLower();
        //        DataTable dt = new DataTable();
        //        if (Isxls == ".xls")
        //        {
        //            dt = ImportExcelFile(Path);
        //        }
        //        if (Isxls == ".xlsx")
        //        {
        //            dt = ImportExcelFilexlsx(Path);
        //        }
        //        barcodelist = Print_DB.ConvertToModel<Barcode_Model>(dt);
        //        List<Barcode_Model> barcodelistnew = new List<Barcode_Model>();
        //        List<Barcode_Model> barcodelistnewsub = new List<Barcode_Model>();
        //        List<Barcode_Model> barcodelistnewAll = new List<Barcode_Model>();
        //        string[] ids = IDs.Split(',');
        //        for (int i = 0; i < ids.Length; i++)
        //        {
        //            if (ids[i] != "")
        //            {
        //                List<Barcode_Model> barcodes = new List<Barcode_Model>();
        //                barcodes.AddRange(barcodelist.Where(P => P.RowNo == ids[i]));
        //                //单张条码
        //                //Barcode_Model model = (Barcode_Model)DeepCopy(barcodes[0]);
        //                //barcodelistnew.Add(model);
        //                //多张条码
        //                if (barcodes.Count == 1 && barcodes[0].BoxCount >= 1)
        //                {
        //                    for (int j = 0; j < barcodes[0].BoxCount; j++)
        //                    {
        //                        Barcode_Model model = (Barcode_Model)DeepCopy(barcodes[0]);
        //                        barcodelistnew.Add(model);
        //                    }
        //                }

        //            }
        //        }

        //        if (barcodelistnew != null && barcodelistnew.Count > 0)
        //        {
        //            string strMsg = "";
        //            PrintInController printIn = new PrintInController();
        //            List<string> squence = printIn.GetSerialnos(barcodelistnew.Count.ToString(), "外", ref strMsg);
        //            int k = 0;
        //            DateTime time = DateTime.Now;
        //            for (int i = 0; i < barcodelistnew.Count; i++)
        //            {
        //                barcodelistnew[i].CompanyCode = "SHJC";
        //                barcodelistnew[i].BarcodeType = 1;
        //                barcodelistnew[i].SerialNo = squence[k++];
        //                barcodelistnew[i].Creater = currentUser.UserNo;
        //                barcodelistnew[i].ReceiveTime = time;
        //                barcodelistnew[i].BarCode = "1@" + barcodelistnew[i].StrongHoldCode + "@" + barcodelistnew[i].MaterialNo + "@" + barcodelistnew[i].BatchNo + "@" + barcodelistnew[i].Qty + "@" + barcodelistnew[i].SerialNo;

        //                //查物料
        //                T_Material_Func funM = new T_Material_Func();
        //                string strErrMsg = "";
        //                List<T_MaterialInfo> modelList = funM.GetMaterialModelBySql(barcodelistnew[i].MaterialNo, ref strErrMsg);
        //                if (modelList == null || modelList.Count == 0)
        //                {
        //                    //失败
        //                    return Json(new { state = false, obj = "没有该物料号" + barcodelistnew[i].MaterialNo }, JsonRequestBehavior.AllowGet);
        //                }
        //                if (modelList[0].sku == "是")
        //                {
        //                    for (int kk = 0; kk < barcodelistnew[i].Qty; kk++)
        //                    {
        //                        Barcode_Model modelsub = (Barcode_Model)DeepCopy(barcodelistnew[i]);
        //                        modelsub.fserialno = barcodelistnew[i].SerialNo;
        //                        barcodelistnewsub.Add(modelsub);
        //                    }
        //                }
        //            }

        //            //本体打印
        //            DateTime timesub = DateTime.Now.AddSeconds(5);
        //            if (barcodelistnewsub != null && barcodelistnewsub.Count > 0)
        //            {
        //                List<string> squencesub = printIn.GetSerialnos(barcodelistnewsub.Count.ToString(), "内", ref strMsg);
        //                int ksub = 0;

        //                for (int isub = 0; isub < barcodelistnewsub.Count; isub++)
        //                {
        //                    barcodelistnewsub[isub].CompanyCode = "SHJC";
        //                    barcodelistnewsub[isub].BarcodeType = 2;
        //                    barcodelistnewsub[isub].SerialNo = squencesub[ksub++];
        //                    barcodelistnewsub[isub].Creater = currentUser.UserNo;
        //                    barcodelistnewsub[isub].ReceiveTime = timesub;
        //                    barcodelistnewsub[isub].Qty = 1;

        //                    barcodelistnewsub[isub].BarCode = "2@" + barcodelistnewsub[isub].StrongHoldCode + "@" + barcodelistnewsub[isub].MaterialNo + "@" + barcodelistnewsub[isub].BatchNo + "@1@" + barcodelistnewsub[isub].SerialNo;

        //                }
        //            }
        //            barcodelistnewAll.AddRange(barcodelistnew);
        //            barcodelistnewAll.AddRange(barcodelistnewsub);
        //            Print_DB func = new Print_DB();
        //            if (func.SubBarcodesNoPrint(barcodelistnewAll, ref strMsg))
        //            {
        //                if (barcodelistnewsub != null && barcodelistnewsub.Count > 0)
        //                {
        //                    return Json(new { state = true, obj = time.ToString("yyyy/MM/dd HH:mm:ss"), obj1 = timesub.ToString("yyyy/MM/dd HH:mm:ss") }, JsonRequestBehavior.AllowGet);
        //                }
        //                else
        //                {
        //                    return Json(new { state = true, obj = time.ToString("yyyy/MM/dd HH:mm:ss"), obj1 = "" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //            else
        //            {
        //                //失败
        //                return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            //失败
        //            return Json(new { state = false, obj = "数据为空" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
        //    }

        //}

        public JsonResult CheckSKU(string serialno)
        {
            try
            {
                T_OutBarcode_DB funb = new T_OutBarcode_DB();
                string sku = funb.GetMaterialno(serialno);
                if (sku == "是")
                {
                    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { state = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { state = false }, JsonRequestBehavior.AllowGet);
            }
        }




        public object DeepCopy(object _object)
        {
            Type T = _object.GetType();
            object o = Activator.CreateInstance(T);
            PropertyInfo[] PI = T.GetProperties();
            for (int i = 0; i < PI.Length; i++)
            {
                PropertyInfo P = PI[i];
                P.SetValue(o, P.GetValue(_object));
            }
            return o;
        }


        public static DataTable ImportExcelFile(string filePath)

        {

            HSSFWorkbook hssfworkbook;
            #region//初始化信息

            try

            {

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))

                {

                    hssfworkbook = new HSSFWorkbook(file);

                }

            }

            catch (Exception e)

            {

                throw e;

            }

            #endregion
            NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(sheet.GetRow(0).GetCell(j).ToString());
            }
            //sheet.cu
            while (rows.MoveNext())
            {
                HSSFRow row = (HSSFRow)rows.Current;
                if (row.RowNum != 0)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataTable ImportExcelFilexlsx(string filePath)
        {
            XSSFWorkbook hssfworkbook;
            #region//初始化信息

            try

            {

                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))

                {

                    hssfworkbook = new XSSFWorkbook(file);

                }

            }

            catch (Exception e)

            {

                throw e;

            }

            #endregion
            NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable();
            for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
            {
                dt.Columns.Add(sheet.GetRow(0).GetCell(j).ToString());
            }
            //sheet.cu
            while (rows.MoveNext())
            {
                XSSFRow row = (XSSFRow)rows.Current;
                if (row.RowNum != 0)
                {
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(i);
                        if (cell == null)
                        {
                            dr[i] = null;
                        }
                        else
                        {
                            dr[i] = cell.ToString();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }
            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {  //You can log something here     
                       //throw;    
                    }
                }
            }

            return obj;
        }




        //string sq = "";
        //[HttpPost]
        //public void SaveBarcode(string erpvoucherno, string materialno, string materialdesc, string ean, string batch, string edate, string num, string everynum, string receivetime, string RowNO, string RowNODel)
        //{
        //    Print_DB func = new Print_DB();
        //    func.FirstPrint()


        //    try
        //    {
        //        string err = "";
        //        //计算外箱数量,和尾箱数量,和尾箱里面的个数
        //        int outboxnum = 0;
        //        int inboxnum = 0;
        //        decimal tailnum = 0;
        //        GetBoxInfo(ref outboxnum, ref tailnum, ref inboxnum, num, everynum);
        //        if (outboxnum == 0)
        //            return Json(new { state = false, obj = "打印数量为0" }, JsonRequestBehavior.AllowGet);

        //        Print_DB print_DB = new Print_DB();
        //        List<string> squence = GetSerialnos(outboxnum + inboxnum, ref err);

        //        //int matenoid = selectItem.MaterialNoID;
        //        sq = "";
        //        //存放打印条码内容
        //        List<Barcode_Model> listbarcode = new List<Barcode_Model>();
        //        int k = 0;
        //        //执行打印外箱命令
        //        for (int i = 0; i < outboxnum; i++)
        //        {
        //            Barcode_Model model = new Barcode_Model();
        //            model.MaterialNo = materialno;
        //            model.MaterialDesc = materialdesc;
        //            model.BatchNo = batch;
        //            model.ErpVoucherNo = erpvoucherno;
        //            model.EDate = Convert.ToDateTime(edate);
        //            model.Qty = Convert.ToDecimal(everynum);
        //            model.SerialNo = squence[k++];
        //            model.Creater = Common.Commom.currentUser.UserNo;
        //            model.EAN = ean;
        //            model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
        //            model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
        //            model.RowNo = RowNO;
        //            model.RowNoDel = RowNODel;
        //            listbarcode.Add(model);
        //        }
        //        if (inboxnum == 1)
        //        {
        //            Barcode_Model model = new Barcode_Model();
        //            model.MaterialNo = materialno;
        //            model.MaterialDesc = materialdesc;
        //            model.BatchNo = batch;
        //            model.ErpVoucherNo = erpvoucherno;
        //            model.EDate = Convert.ToDateTime(edate);
        //            model.Qty = Convert.ToDecimal(tailnum);
        //            model.SerialNo = squence[k++];
        //            model.Creater = Common.Commom.currentUser.UserNo;
        //            model.EAN = ean;
        //            model.ReceiveTime = string.IsNullOrEmpty(receivetime) ? DateTime.Now : Convert.ToDateTime(receivetime);
        //            model.BarCode = "1@" + model.StrongHoldCode + "@" + model.MaterialNo + "@" + model.EAN + "@" + model.EDate.ToString("yyyy-MM-dd") + "@" + model.BatchNo + "@" + model.Qty + "@" + model.SerialNo;
        //            model.RowNo = RowNO;
        //            model.RowNoDel = RowNODel;
        //            listbarcode.Add(model);
        //        }

        //        if (print_DB.SubBarcodes(listbarcode, "sup", 1, ref err))
        //        {
        //            string serialnos = "";
        //            for (int i = 0; i < listbarcode.Count; i++)
        //            {
        //                serialnos += listbarcode[i].SerialNo + ",";
        //            }
        //            return Json(new { state = true, obj = serialnos }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { state = false, obj = err }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { state = false, obj = ex.ToString() }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        private List<string> GetSerialnos(int v, ref string err)
        {
            List<string> serialnos = new List<string>();
            for (int i = 0; i < v; i++)
            {
                var seed = Guid.NewGuid().GetHashCode();
                //string code = DateTime.Now.ToString("yyMMddHHmm") + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');
                string code = DateTime.Now.ToString("yyMMdd") + "77" + new Random(seed).Next(0, 999999).ToString().PadLeft(6, '0');//奥碧虹
                if (serialnos.Find(t => t == code) == null)
                {
                    serialnos.Add(code);
                }
                else
                {
                    i--;
                }
            }
            return serialnos;
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





    }
}