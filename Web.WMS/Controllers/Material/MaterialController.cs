using BILBasic.Common;
using BILWeb.BaseInfo;
using BILWeb.Material;
using BILWeb.SyncService;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SqlSugarDAL.barcode;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Web.WMS.Common;
using WMS.Factory;
using WMS.Web.Filter;

namespace Web.WMS.Controllers.Material
{
    [RoleActionFilter(Message = "Basic/Material")]
    public class MaterialController : BaseController<T_MaterialInfo>
    {
        private IMaterialService materialService;
        public MaterialController()
        {
            materialService = (IMaterialService)ServiceFactory.CreateObject("Material.T_Material_Func");
            baseservice = materialService;
        }



        public JsonResult Sync(string MaterialNo)
        {
            //string ErrorMsg = ""; int WmsVoucherType = -1; string syncType = "ERP"; int syncExcelVouType = -1; DataSet excelds = null;
            //BILWeb.SyncService.ParamaterField_Func PFunc = new BILWeb.SyncService.ParamaterField_Func();

            //if (PFunc.Sync(99, string.Empty, MaterialNo, WmsVoucherType, ref ErrorMsg, syncType, syncExcelVouType, excelds))
            //{

            //    return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(new { state = false, obj = ErrorMsg }, JsonRequestBehavior.AllowGet);
            //}

            string strMsg = "";
            ParamaterFiled_DB PDB = new ParamaterFiled_DB();
            if (PDB.GetVoucherNo(MaterialNo, ref strMsg,"0"))
            {
                return Json(new { state = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { state = false, obj = strMsg }, JsonRequestBehavior.AllowGet);
            }



        }


        //T_Material_Func tfunc = new T_Material_Func();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        //public ActionResult GetModelList(DividPage page, T_MaterialInfo model)
        //{
        //    string strMsg = "";
        //    List<T_MaterialInfo> gmodelList = new List<T_MaterialInfo>();
        //    tfunc.GetModelListByPage(ref gmodelList, Commom.currentUser, model, ref page, ref strMsg);
        //    ViewData["PageData"] = new PageData<T_MaterialInfo> { data = gmodelList, dividPage = page, link = Common.PageTag.ModelToUriParam(model, "/Material/GetModelList") };
        //    return View("MaterialList");
        //}

        //[HttpGet]
        //public ActionResult GetModel(T_MaterialInfo model)
        //{
        //    string strMsg = "";
        //    tfunc.GetModelByID(ref model, ref strMsg);
        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Add(T_MaterialInfo model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string strMsg = "";
        //        if (tfunc.SaveModelBySqlToDB(Common.Commom.currentUser, ref model, ref strMsg))
        //        {
        //            return RedirectToAction("GetModelList");
        //        }
        //    }
        //    return RedirectToAction("GetModel", model);

        //}

        //[HttpPost]
        //public JsonResult Delect(string ID)
        //{
        //    string strMsg = "";
        //    string[] Ids = ID.TrimEnd(',').Split(',');
        //    foreach (string id in Ids)
        //    {
        //        tfunc.DeleteModelByModelSql(Common.Commom.currentUser, new T_MaterialInfo { ID = Convert.ToInt32(id) }, ref strMsg);
        //    }
        //    return Json(strMsg, JsonRequestBehavior.AllowGet);
        //}
        #region 物料导入
        public string ImportMaterial()
        {
            //  string returnstr = "";
            List<T_Material> detailsList = new List<T_Material>(); ;
            try
            {
                HttpPostedFileBase file = Request.Files["file"];
                //     HttpPostedFile file = System.Web.HttpContext.Current.Request.Files[0];
                string FileName;
                string savePath;
                if (file == null || file.ContentLength <= 0)
                {
                    return "文件为空";
                }
                else
                {
                    try
                    {
                        #region 新建文件存到服务器
                        string filename = Path.GetFileName(file.FileName);
                        int filesize = file.ContentLength;//获取上传文件的大小单位为字节byte
                        string fileEx = System.IO.Path.GetExtension(filename);//获取上传文件的扩展名
                        string NoFileName = System.IO.Path.GetFileNameWithoutExtension(filename);//获取无扩展名的文件名
                        int Maxsize = 100000 * 1024;//定义上传文件的最大空间大小为4M
                        string FileType = ".xls,.xlsx";//定义上传文件的类型字符串

                        FileName = DateTime.Now.ToString("yyyyMMddhhmmss") + fileEx;
                        if (!FileType.Contains(fileEx))
                        {
                            //result.success = false;
                            return "文件类型不对，只能导入xls和xlsx格式的文件！";
                            //return result;
                        }
                        if (filesize >= Maxsize)
                        {

                            //result.data = "上传文件超过4M，不能上传！";
                            //return result;
                        }
                        string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/excel/";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        savePath = Path.Combine(path, FileName);
                        file.SaveAs(savePath);

                        #endregion
                        #region 读取文件添加到数据库
                        IWorkbook wk = null;
                        int errorIndex = 0;
                        try
                        {
                            FileStream fs = System.IO.File.OpenRead(savePath);
                            if (fileEx.Equals(".xls"))
                            {
                                //xls
                                wk = new HSSFWorkbook(fs);
                            }
                            else
                            {
                                //xlsx
                                wk = new XSSFWorkbook(fs);
                            }
                            fs.Close();
                            ISheet sheet = wk.GetSheetAt(0);
                            IRow row = sheet.GetRow(0);  //读取当前行数据
                                                         //获取wms单据号
                                                         // string wmsNo = purchaseOrderManage.GetTreeNo();
                                                         //定义一个字典datatable 
                            Dictionary<DataTable, string> keyValuePairs = new Dictionary<DataTable, string>();
                            DataTable detailTable = new DataTable();
                            //  [ID] [int] IDENTITY(1, 1) NOT NULL,
                            detailTable.Columns.Add("ISSERIAL", typeof(int));
                            detailTable.Columns.Add("UNIT", typeof(string));
                            detailTable.Columns.Add("MATERIALNO", typeof(string));
                            detailTable.Columns.Add("MATERIALDESC", typeof(string));
                            detailTable.Columns.Add("MATERIALDESCEN", typeof(string));
                            detailTable.Columns.Add("STACKWAREHOUSE", typeof(int));
                            detailTable.Columns.Add("STACKHOUSE", typeof(int));
                            detailTable.Columns.Add("STACKAREA", typeof(int));
                            detailTable.Columns.Add("LENGTH", typeof(decimal));
                            detailTable.Columns.Add("WIDE", typeof(decimal));
                            detailTable.Columns.Add("HIGHT", typeof(decimal));
                            detailTable.Columns.Add("VOLUME", typeof(decimal));
                            detailTable.Columns.Add("WEIGHT", typeof(decimal));
                            detailTable.Columns.Add("NETWEIGHT", typeof(decimal));
                            detailTable.Columns.Add("PACKRULE", typeof(decimal));
                            detailTable.Columns.Add("STACKRULE", typeof(decimal));
                            detailTable.Columns.Add("DISRULE", typeof(decimal));
                            detailTable.Columns.Add("SUPPLIERNO", typeof(string));
                            detailTable.Columns.Add("SUPPLIERNAME", typeof(string));
                            detailTable.Columns.Add("UNITNAME", typeof(string));
                            detailTable.Columns.Add("KEEPERNO", typeof(string));
                            detailTable.Columns.Add("KEEPERNAME", typeof(string));
                            detailTable.Columns.Add("ISDANGEROUS", typeof(int));
                            detailTable.Columns.Add("ISACTIVATE", typeof(int));
                            detailTable.Columns.Add("ISBOND", typeof(int));
                            detailTable.Columns.Add("ISQUALITY", typeof(int));
                            detailTable.Columns.Add("CREATER", typeof(string));
                            detailTable.Columns.Add("CREATETIME", typeof(DateTime));
                            detailTable.Columns.Add("MODIFYER", typeof(DateTime));
                            detailTable.Columns.Add("MODIFYTIME", typeof(DateTime));
                            detailTable.Columns.Add("ISDEL", typeof(int));
                            detailTable.Columns.Add("PARTNO", typeof(string));
                            detailTable.Columns.Add("STRONGHOLDCODE", typeof(string));
                            detailTable.Columns.Add("STRONGHOLDNAME", typeof(string));
                            detailTable.Columns.Add("COMPANYCODE", typeof(string));
                            detailTable.Columns.Add("MAINTYPECODE", typeof(string));
                            detailTable.Columns.Add("MAINTYPENAME", typeof(string));
                            detailTable.Columns.Add("PURCHASETYPECODE", typeof(string));
                            detailTable.Columns.Add("PURCHASETYPENAME", typeof(string));
                            detailTable.Columns.Add("PRODUCTTYPECODE", typeof(string));
                            detailTable.Columns.Add("PRODUCTTYPENAME", typeof(string));
                            detailTable.Columns.Add("QUALITYDAY", typeof(decimal));
                            detailTable.Columns.Add("QUALITYMON", typeof(decimal));
                            detailTable.Columns.Add("BRAND", typeof(string));
                            detailTable.Columns.Add("PLACEAREA", typeof(string));
                            detailTable.Columns.Add("LIFECYCLE", typeof(string));
                            detailTable.Columns.Add("PACKQTY", typeof(decimal));
                            detailTable.Columns.Add("PALLETVOLUME", typeof(decimal));
                            detailTable.Columns.Add("PALLETPACKQTY", typeof(decimal));
                            detailTable.Columns.Add("PACKVOLUME", typeof(decimal));
                            detailTable.Columns.Add("STATUS", typeof(int));
                            detailTable.Columns.Add("SPEC", typeof(string));
                            detailTable.Columns.Add("STORECONDITION", typeof(string));
                            detailTable.Columns.Add("SPECIALREQUIRE", typeof(string));
                            detailTable.Columns.Add("PROTECTWAY", typeof(string));
                            detailTable.Columns.Add("VOUCHERNO", typeof(string));
                            detailTable.Columns.Add("VOUCHERTYPE", typeof(int));
                            detailTable.Columns.Add("ERPSTATUS", typeof(string));
                            detailTable.Columns.Add("LASTSYNCTIME", typeof(DateTime));
                            detailTable.Columns.Add("ERPBARCODE", typeof(string));
                            detailTable.Columns.Add("COLORCODE", typeof(string));
                            detailTable.Columns.Add("COLORNAME", typeof(string));
                            detailTable.Columns.Add("SIZECODE", typeof(string));
                            detailTable.Columns.Add("SIZENAME", typeof(string));
                            detailTable.Columns.Add("SAMPLECODE", typeof(string));
                            detailTable.Columns.Add("SKU", typeof(string));
                            detailTable.Columns.Add("ENACODE", typeof(string));
                            detailTable.Columns.Add("ERPMATERIALID", typeof(string));
                            detailTable.Columns.Add("OUTPACKAGE", typeof(decimal));
                            detailTable.Columns.Add("INPACKAGE", typeof(decimal));
                            detailTable.Columns.Add("materialnotemp", typeof(string));
                            detailTable.Columns.Add("materialdesctemp", typeof(string));
                            detailTable.Columns.Add("materialidtemp", typeof(int));
                            detailTable.Columns.Add("STANDARDBOX", typeof(string));
                            detailTable.Columns.Add("STANDARDBOX1", typeof(string));
                            detailTable.Columns.Add("STANDARDBOX2", typeof(string));
                            detailTable.Columns.Add("STANDARDBOX3", typeof(string));
                            detailTable.Columns.Add("CUSTOMERNO", typeof(string));
                            detailTable.Columns.Add("BRANDINTRO", typeof(string));
                            detailTable.Columns.Add("ERPVOUCHERTYPE", typeof(string));
                            detailTable.Columns.Add("dEDate", typeof(DateTime));
                            List<String> updateList = new List<string>();
                            T_System_DB t_System_DB = new T_System_DB();
                            try
                            {

                                for (int i = 1; i <= sheet.LastRowNum; i++)
                                {

                                    errorIndex = i;
                                    T_Material material = new T_Material();
                                    row = sheet.GetRow(i);
                                    if (row != null)
                                    {

                                        //开始判断数据库存在不存在
                                        string materialno = "";
                                        string strongholcode = "";
                                        string packqty = "";
                                        string PALLETPACKQTY = "";
                                        string spec = "";
                                        string materialdesc = "";
                                        formaterCell(row.GetCell(0), ref materialno);
                                        formaterCell(row.GetCell(3), ref strongholcode);
                                        formaterCell(row.GetCell(4), ref packqty);
                                        formaterCell(row.GetCell(5), ref PALLETPACKQTY);
                                        formaterCell(row.GetCell(2), ref spec);
                                        formaterCell(row.GetCell(1), ref materialdesc);
                                        formaterCell(row.GetCell(4), ref packqty);
                                        if (materialno == "error" || strongholcode == "error" || packqty == "error" || PALLETPACKQTY == "error" || spec == "error" || materialdesc == "error" || packqty == "error")
                                        {
                                            return String.Format("Excel{0}行格式解析失败", i);
                                        }
                                        string sql = string.Format("select id from T_MATERIAL where materialno='{0}'  and STRONGHOLDCODE='{1}'", materialno, strongholcode);
                                        object o = t_System_DB.dbFactory.ExecuteScalar(CommandType.Text, sql);
                                        if (o != null)
                                        { //找到了 跳出当前循环
                                            string updatesql = string.Format(@"update T_MATERIAL set materialno = '{0}' , PACKQTY ={1}
                                        , PALLETPACKQTY ={2} , SPEC = '{3}'  , materialdesc='{6}'  where materialno = '{4}'  and STRONGHOLDCODE = '{5}' ", materialno, packqty, PALLETPACKQTY, spec, materialno, strongholcode, materialdesc);
                                            updateList.Add(updatesql);
                                            continue;
                                        }
                                        DataRow dataRow = detailTable.NewRow();
                                        dataRow["ISSERIAL"] = 0;
                                        dataRow["UNIT"] = "";
                                        dataRow["MATERIALNO"] = materialno;
                                        dataRow["MATERIALDESC"] = materialdesc;
                                        dataRow["MATERIALDESCEN"] = "";
                                        dataRow["STACKWAREHOUSE"] = 0;
                                        dataRow["STACKHOUSE"] = 0;
                                        dataRow["STACKAREA"] = 0;
                                        dataRow["LENGTH"] = 0;
                                        dataRow["WIDE"] = 0;
                                        dataRow["HIGHT"] = 0;
                                        dataRow["VOLUME"] = 0;
                                        dataRow["WEIGHT"] = 0;
                                        dataRow["NETWEIGHT"] = 0;
                                        dataRow["PACKRULE"] = 0;
                                        dataRow["STACKRULE"] = 0;
                                        dataRow["DISRULE"] = 0;
                                        dataRow["SUPPLIERNO"] = "";
                                        dataRow["SUPPLIERNAME"] = "";
                                        dataRow["UNITNAME"] = "";
                                        dataRow["KEEPERNO"] = "";
                                        dataRow["KEEPERNAME"] = "";
                                        dataRow["ISDANGEROUS"] = 0;
                                        dataRow["ISACTIVATE"] = 0;
                                        dataRow["ISBOND"] = 0;
                                        dataRow["ISQUALITY"] = 0;
                                        dataRow["CREATER"] = "";
                                        dataRow["CREATETIME"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                        //dataRow["MODIFYER"] = typeof(DateTime));
                                        //dataRow["MODIFYTIME"] = typeof(DateTime));
                                        dataRow["ISDEL"] = 1;
                                        dataRow["PARTNO"] = "";
                                        dataRow["STRONGHOLDCODE"] = strongholcode;
                                        dataRow["STRONGHOLDNAME"] = "";
                                        dataRow["COMPANYCODE"] = "";
                                        dataRow["MAINTYPECODE"] = "";
                                        dataRow["MAINTYPENAME"] = "";
                                        dataRow["PURCHASETYPECODE"] = "";
                                        dataRow["PURCHASETYPENAME"] = "";
                                        dataRow["PRODUCTTYPECODE"] = "";
                                        dataRow["PRODUCTTYPENAME"] = "";
                                        dataRow["QUALITYDAY"] = 0;
                                        dataRow["QUALITYMON"] = 0;
                                        dataRow["BRAND"] = "";
                                        dataRow["PLACEAREA"] = "";
                                        dataRow["LIFECYCLE"] = "";
                                        dataRow["PACKQTY"] = Convert.ToDecimal(packqty);
                                        dataRow["PALLETVOLUME"] = 0;
                                        dataRow["PALLETPACKQTY"] = Convert.ToDecimal(PALLETPACKQTY);
                                        dataRow["PACKVOLUME"] = 0;
                                        dataRow["STATUS"] = 0;
                                        dataRow["SPEC"] = spec;
                                        dataRow["STORECONDITION"] = "";
                                        dataRow["SPECIALREQUIRE"] = "";
                                        dataRow["PROTECTWAY"] = "";
                                        dataRow["VOUCHERNO"] = "";
                                        dataRow["VOUCHERTYPE"] = 0;
                                        dataRow["ERPSTATUS"] = "";
                                        //  dataRow["LASTSYNCTIME"] = typeof(DateTime));
                                        dataRow["ERPBARCODE"] = "";
                                        dataRow["COLORCODE"] = "";
                                        dataRow["COLORNAME"] = "";
                                        dataRow["SIZECODE"] = "";
                                        dataRow["SIZENAME"] = "";
                                        dataRow["SAMPLECODE"] = "";
                                        dataRow["SKU"] = "";
                                        dataRow["ENACODE"] = "";
                                        dataRow["ERPMATERIALID"] = "";
                                        dataRow["OUTPACKAGE"] = 0;
                                        dataRow["INPACKAGE"] = 0;
                                        dataRow["materialnotemp"] = "";
                                        dataRow["materialdesctemp"] = "";
                                        dataRow["materialidtemp"] = 0;
                                        dataRow["STANDARDBOX"] = "";
                                        dataRow["STANDARDBOX1"] = "";
                                        dataRow["STANDARDBOX2"] = "";
                                        dataRow["STANDARDBOX3"] = "";
                                        dataRow["CUSTOMERNO"] = "";
                                        dataRow["BRANDINTRO"] = "";
                                        dataRow["ERPVOUCHERTYPE"] = "";
                                        //  dataRow["dEDate"] = typeof(DateTime));
                                        detailTable.Rows.Add(dataRow);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                            keyValuePairs.Add(detailTable, "T_MATERIAL");
                            string strError = "";
                            t_System_DB.dbFactory.SqlBatchCopy(keyValuePairs, ref strError);
                            int arr = t_System_DB.dbFactory.ExecuteNonQueryList(updateList);
                            if (strError != "")
                            {
                                return strError;
                            }
                            if (arr == -2 && updateList.Count > 0)
                            {
                                return "修改失败";
                            }
                        }
                        catch (Exception ex)
                        {
                            int arrrr = errorIndex;
                            return string.Format("第{0}行解析文件报错", arrrr);
                            //只在Debug模式下才输出
                            //   Console.WriteLine(e.Message);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {

                        return "解析文件报错";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
                //  LoggingFactory.GetLogger().Log(ex.ToString());
                //result.success = false;
                //result.data = "导入失败！";
                //return result;
            }
            return "导入成功";
        }

        public void formaterCell(ICell cell, ref string value)
        {
            try
            {
                //        Unknown = -1,
                //Numeric = 0,
                //String = 1,
                //Formula = 2,
                //Blank = 3,
                //Boolean = 4,
                //Error = 5
                switch (cell.CellType)
                {
                    case (CellType)1:
                        value = cell.StringCellValue.ToString().Trim();
                        break;
                    case (CellType)4:
                        value = cell.BooleanCellValue.ToString();
                        break;
                    case (CellType)0:
                        value = cell.NumericCellValue.ToString();
                        break;
                    default:
                        value = "error";
                        return;

                }
            }
            catch (Exception ex)
            {
                value = "error";
                throw new Exception("解析异常");
            }
        }



        #endregion
    }
}