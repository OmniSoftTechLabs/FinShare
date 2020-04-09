using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using Excel.Helper;
using DAL;
using System.Data;
using System.IO;

namespace FinShare.Controllers
{
    public class ImportExcelController : Controller
    {
        //
        // GET: /ImportExcel/
        [HttpGet]
        [ActionName("Create")]
        public ActionResult Create_Get(Int32? calltype)
        {
            List<impyears> years = new List<impyears>();
            List<AMC> amcs = new List<AMC>();
            ImportExcelDAL objdal = new ImportExcelDAL();
            amcs = objdal.GetAMCList();
            int currentyear = DateTime.Now.Year;
            impyears year1;
            int j = 0;
            for (int i = 10; i >= 0; i--)
            {
                year1 = new impyears();
                year1.Id = currentyear - i;
                year1.Name = currentyear - i;
                years.Insert(j, year1);
                j++;
            }

            var model = new ImportExcel()
            {
                years = years,
                AMCS = amcs,
                SuccessMsg = calltype == 0 ? "" : "File Imported Successfully."
            };
            return View(model);


        }


        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create_Post(ImportExcel entobj, string AMCCode)
        {
            int uploadcnt = 0;
            if (ModelState.IsValid)
            {

                ImportExcelDAL objdal = new ImportExcelDAL();
                uploadcnt = objdal.ConfirmUpload(entobj);
                if (uploadcnt == 0)
                    return RedirectToAction("Import", "ImportExcel", new { Month = entobj.Month.ToString(), year = entobj.Year.ToString(), AmcCode = AMCCode, AmcID = entobj.AMCID.ToString() });
                else
                    return RedirectToAction("ReImport", "ImportExcel", new { Month = entobj.Month.ToString(), year = entobj.Year.ToString(), AmcCode = AMCCode, AmcID = entobj.AMCID.ToString() });
            }
            else
            {
                return View("Create", new { calltype = 0 });
            }
        }

        [HttpGet]
        public ActionResult Import(string Month, string year, string AMCCode, string AMCID)
        {
            var model = new ImportExcel()
            {
                Month = Convert.ToInt32(Month),
                Year = Convert.ToInt32(year),
                AMCID = AMCID,
                AMCCode = AMCCode
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("Import")]
        [ValidateAntiForgeryToken]
        public ActionResult Import_Post(HttpPostedFileBase upload, ImportExcel entobj, string AMCCode)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;

                    string tempamccode = Convert.ToString(entobj.AMCID.Split('$')[1]);
                    string invalidfilemsg = "Invalid file formate for";
                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View("Import", entobj);
                    }

                    reader.IsFirstRowAsColumnNames = true;


                    DataSet result = reader.AsDataSet();
                    reader.Close();

                    if (tempamccode.ToUpper() == "CAMS") //CAMS
                    {
                        if (result.Tables[0].Columns[2].ColumnName.ToLower() != "folio_no" || result.Tables[0].Columns[38].ColumnName.ToLower() != "brkage_amt")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " CAMS!!");
                            return View("Import", entobj);
                        }
                    }
                    else if (tempamccode.ToUpper() == "KARVY")//Karvy
                    {
                        //if (result.Tables[0].Columns[2].ColumnName.ToLower() != "investor name" || result.Tables[0].Columns[16].ColumnName.ToLower() != "gross brokerage")
                        if (result.Tables[0].Columns[1].ColumnName.ToLower() != "account number" || result.Tables[0].Columns[16].ColumnName.ToLower() != "gross brokerage")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " KARVY!!");
                            return View("Import", entobj);
                        }
                    }
                    else if (tempamccode.ToUpper() == "FRANKLINE")//Frankline
                    {
                        if (result.Tables[0].Rows[1][3].ToString().ToLower() != "accountno" || result.Tables[0].Rows[1][18].ToString().ToLower() != "brokerage")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " FRANKLINE!!");
                            return View("Import", entobj);
                        }
                    }
                    ImportExcelDAL objdal = new ImportExcelDAL();
                    objdal.InsertUpdate(result.Tables[0], tempamccode, entobj, 0);

                    return RedirectToAction("Create", "ImportExcel", new { calltype = 1 });

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                    return View("Import", entobj);
                }
            }
            return View("Create", new { calltype = 0 });
        }

        [HttpGet]
        public ActionResult ReImport(string Month, string year, string AMCCode, string AMCID)
        {
            var model = new ImportExcel()
            {
                Month = Convert.ToInt32(Month),
                Year = Convert.ToInt32(year),
                AMCID = AMCID,
                AMCCode = AMCCode
            };
            return View(model);
        }

        [HttpPost]
        [ActionName("ReImport")]
        [ValidateAntiForgeryToken]
        public ActionResult ReImport_Post(HttpPostedFileBase upload, ImportExcel entobj, string AMCCode)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;
                    string invalidfilemsg = "Invalid file formate for";
                    // We return the interface, so that
                    IExcelDataReader reader = null;

                    string tempamccode = Convert.ToString(entobj.AMCID.Split('$')[1]);
                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View("ReImport", entobj);
                    }

                    reader.IsFirstRowAsColumnNames = true;


                    DataSet result = reader.AsDataSet();
                    reader.Close();

                    if (tempamccode.ToUpper() == "CAMS") //CAMS
                    {
                        //if (result.Tables[0].Columns[84].ColumnName != "Inv_Name")
                        //{
                        //    ModelState.AddModelError("File", "Please, Select file for CAMS");
                        //    return View("ReImport", entobj);
                        //}

                        /////////////////////////
                        if (result.Tables[0].Columns[84].ColumnName.ToLower() != "inv_name" || result.Tables[0].Columns[38].ColumnName.ToLower() != "brkage_amt")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " CAMS!!");
                            return View("ReImport", entobj);
                        }
                        ////////////////////
                    }
                    else if (tempamccode.ToUpper() == "KARVY")//Karvy
                    {
                        //if (result.Tables[0].Columns[2].ColumnName != "Investor Name")
                        //{
                        //    ModelState.AddModelError("File", "Please, Select file for KARVY");
                        //    return View("ReImport", entobj);
                        //}
                        if (result.Tables[0].Columns[2].ColumnName.ToLower() != "investor name" || result.Tables[0].Columns[16].ColumnName.ToLower() != "gross brokerage")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " KARVY!!");
                            return View("ReImport", entobj);
                        }
                    }
                    else if (tempamccode.ToUpper() == "FRANKLINE")//Frankline
                    {
                        //if (result.Tables[0].Rows[1][4].ToString().ToUpper() != "INVNAME")
                        //{
                        //    ModelState.AddModelError("File", "Please, Select file for FRANKLINE");
                        //    return View("ReImport", entobj);
                        //}
                        if (result.Tables[0].Rows[1][4].ToString().ToLower() != "invname" || result.Tables[0].Rows[1][18].ToString().ToLower() != "brokerage")
                        {
                            ModelState.AddModelError("File", invalidfilemsg + " FRANKLINE!!");
                            return View("ReImport", entobj);
                        }
                    }
                    ImportExcelDAL objdal = new ImportExcelDAL();
                    objdal.InsertUpdate(result.Tables[0], tempamccode, entobj, 1);

                    return RedirectToAction("Create", "ImportExcel", new { calltype = 1 });

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                    return View("ReImport", entobj);
                }
            }
            return View("Create", new { calltype = 0 });
        }
    }
}