using AspnetCore.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Excel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace AspnetCore.Controllers
{
    public class ResultsController : Controller
    {
        private RunTestContext db = new RunTestContext();
        private DataTable dt = new DataTable();
        // GET: Results
        public ActionResult ReliabilityTest()
        {
            return View(db.TestRuns.ToList());
            //return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReliabilityTest(HttpPostedFileBase uploadfile)
        {
            if (ModelState.IsValid)
            {
                if (uploadfile != null && uploadfile.ContentLength > 0)
                {
                    Stream stream = uploadfile.InputStream;
                    IExcelDataReader reader = null;
                    if (uploadfile.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }
                    reader.IsFirstRowAsColumnNames = true;
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    return View(result.Tables[0]);
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }
            return View();
            
        }

        public ActionResult PerformanceTest()
        {
            return View();
        }

        public ActionResult DateFilter()
        {
            
            return PartialView();
        }
        public ActionResult UploadReliabilityResults()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadReliabilityResults(HttpPostedFileBase uploadfile)
        {
            if(ModelState.IsValid)
            {
                if(uploadfile != null && uploadfile.ContentLength > 0)
                {
                    Stream stream = uploadfile.InputStream;
                    IExcelDataReader reader = null;
                    if(uploadfile.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if(uploadfile.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return PartialView();
                    }
                    reader.IsFirstRowAsColumnNames = true;
                    DataSet result = reader.AsDataSet();
                    reader.Close();
                    return PartialView(result.Tables[0]);
                }
            }
            else
            {
                ModelState.AddModelError("File", "Please upload your file");
            }
            return PartialView();
        }
    }
}