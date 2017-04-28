using AspnetCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AspnetCore.Controllers
{
    public class HomeController : Controller
    {
        private RunTestContext db = new RunTestContext();

        // GET: Home
        public ActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.Scenarios = db.Scenarios.ToList();
            model.StepFiles = db.StepFiles.ToList();
            model.GlobalFiles = db.GlobalFiles.ToList();
            model.TestRuns = db.TestRuns.ToList();
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateScenario()
        {

            PopulateCellsDropDownList();
            return View();
        }
        private void PopulateCellsDropDownList(object selectedCell = null)
        {
            var cellsQuery = from d in db.Cells
                             orderby d.CellName
                             select d;
            ViewBag.CellId = new SelectList(cellsQuery, "CellId", "CellName", selectedCell);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateScenario([Bind(Include = "ScenarioId,ScenarioName,CellId,Testapp,ServerOS,WebServer,DotnetVersion,PerformanceBranch,MusicStoreBranch")]Scenario scenario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    scenario.ScenarioName = scenario.Testapp + "-" + scenario.ServerOS + "-" + scenario.WebServer + "-" + scenario.PerformanceBranch;
                    db.Scenarios.Add(scenario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            PopulateCellsDropDownList(scenario.CellId);
            return View(scenario);
        }
        public ActionResult CreateStepFile()
        {
            PopulateStepFileTemplatesDropDownList();
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateStepFile([Bind(Include = "StepFileId,FileName,FilePath,TemplateId")]StepFile stepfile)
        {
            if (ModelState.IsValid)
            {
                stepfile.FilePath = "C:\\RunRemote\\" + stepfile.FileName;
                db.StepFiles.Add(stepfile);
                db.SaveChanges();
                StepFile steptemplate = db.StepFiles.Find(stepfile.TemplateId);
                FileCopy(steptemplate.FilePath, stepfile.FilePath);
                return RedirectToAction("Index");
            }
            PopulateStepFileTemplatesDropDownList(stepfile.Id);
            return View(stepfile);
        }

        public ActionResult CreateGlobalFile()
        {
            //ViewBag.StepFileId = new SelectList(db.StepFiles.Where(p => p.IsTemplate == true), "StepFileId", "FileName");
            PopulateGlobalFileTemplatesDropDownList();
            //ViewData["StepTemplate"] = new List<SelectListItem> { db.StepFiles.Where(p => p.IsTemplate == true) };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGlobalFile([Bind(Include = "StepFileId,FileName,FilePath,TemplateId")]GlobalFile globalfile)
        {
            if (ModelState.IsValid)
            {
                globalfile.FilePath = "C:\\RunRemote\\" + globalfile.FileName;
                db.GlobalFiles.Add(globalfile);
                db.SaveChanges();
                GlobalFile globaltemplate = db.GlobalFiles.Find(globalfile.TemplateId);
                FileCopy(globaltemplate.FilePath, globalfile.FilePath);
                return RedirectToAction("Index");
            }
            PopulateGlobalFileTemplatesDropDownList(globalfile.Id);
            return View(globalfile);
        }
        private void PopulateStepFileTemplatesDropDownList(object selectedStepFile = null)
        {
            var stepfilesQuery = from d in db.StepFiles
                                 where d.IsTemplate == true
                                 orderby d.FileName
                                 select d;
            ViewBag.StepFileId = new SelectList(stepfilesQuery, "TemplateId", "FileName", selectedStepFile);
        }
        private void PopulateGlobalFileTemplatesDropDownList(object selectedGlobalFile = null)
        {
            var globalfilesQuery = from d in db.GlobalFiles
                                 where d.IsTemplate == true
                                 orderby d.FileName
                                 select d;
            ViewBag.GlobalFileId = new SelectList(globalfilesQuery, "Id", "FileName", selectedGlobalFile);
        }
        public void FileCopy(string path1, string path2)
        {
            int bufferSize = 10240;

            Stream source = new FileStream(path1, FileMode.Open, FileAccess.Read);
            Stream target = new FileStream(path2, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[bufferSize];
            int bytesRead;
            do
            {
                bytesRead = source.Read(buffer, 0, bufferSize);
                target.Write(buffer, 0, bytesRead);
            } while (bytesRead > 0);

            source.Dispose();
            target.Dispose();

        }
        public ActionResult CreateTestRun()
        {
            PopulateScenarioNamesDropDownList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateTestRun([Bind(Include = "RunId,ScenarioId,RunName,ScriptPath,Tester")]TestRun testRun)
        {
            if (ModelState.IsValid)
            {
                testRun.State = State.Available;
                testRun.StartTime = DateTime.Now;
                db.TestRuns.Add(testRun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateScenarioNamesDropDownList(testRun.ScenarioId);
            return View(testRun);
        }
        private void PopulateScenarioNamesDropDownList(object selectedScenario = null)
        {
            var scenariosQuery = from d in db.Scenarios
                             orderby d.ScenarioName
                             select d;
            ViewBag.ScenarioId = new SelectList(scenariosQuery, "ScenarioId", "ScenarioName", selectedScenario);
        }
        // GET: Home/EditScenario/5
        public ActionResult EditScenario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario = db.Scenarios.Find(id);
            if (scenario == null)
            {
                return HttpNotFound();
            }
            PopulateCellsDropDownList(scenario.CellId);
            return View(scenario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditScenario([Bind(Include = "ScenarioId,ScenarioName,CellId,Testapp,ServerOS,WebServer,DotnetVersion,PerformanceBranch,MusicStoreBranch")] Scenario scenario)
        {
            if (ModelState.IsValid)
            {
                scenario.ScenarioName = scenario.Testapp + "-" + scenario.ServerOS + "-" + scenario.WebServer + "-" + scenario.PerformanceBranch;
                db.Entry(scenario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCellsDropDownList(scenario.CellId);
            return View(scenario);
        }
        public ActionResult EditStepFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepFile stepfile = db.StepFiles.Find(id);
            if (stepfile == null)
            {
                return HttpNotFound();
            }
            return View(stepfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStepFile([Bind(Include = "Id,FileName,FilePath,IsTemplate,TemplateId")] StepFile stepfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stepfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(stepfile);
        }

        public ActionResult EditGlobalFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalFile globalfile = db.GlobalFiles.Find(id);
            if (globalfile == null)
            {
                return HttpNotFound();
            }
            return View(globalfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGlobalFile([Bind(Include = "Id,FileName,FilePath,IsTemplate")] GlobalFile globalfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(globalfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(globalfile);
        }
        public ActionResult EditTestRun(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun == null)
            {
                return HttpNotFound();
            }
            PopulateScenarioNamesDropDownList(testrun.ScenarioId);
            //ViewBag.ScenarioId = new SelectList(db.Scenarios, "ScenarioId", "ScenarioName");
            return View(testrun);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTestRun([Bind(Include = "RunId,ScenarioId,RunName,State,ScriptPath,Tester")]TestRun testRun)
        {

            if (ModelState.IsValid)
            {
                testRun.StartTime = DateTime.Now;
                db.Entry(testRun).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateScenarioNamesDropDownList(testRun.ScenarioId);
            return View(testRun);
        }

        public ActionResult DeleteScenario(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenario scenario =db.Scenarios.Find(id);
            if (scenario == null)
            {
                return HttpNotFound();
            }

            return View(scenario);
        }
        [HttpPost]
        public ActionResult DeleteScenario(int id)
        {
            Scenario scenario = db.Scenarios.Find(id);
            db.Scenarios.Remove(scenario);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }
        public ActionResult DeleteStepFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepFile stepfile = db.StepFiles.Find(id);
            if (stepfile == null)
            {
                return HttpNotFound();
            }

            return View(stepfile);
        }

        [HttpPost]
        public ActionResult DeleteStepFile(int id)
        {
            StepFile stepfile = db.StepFiles.Find(id);
            db.StepFiles.Remove(stepfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DeleteGlobalFile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalFile globalfile = db.GlobalFiles.Find(id);
            if (globalfile == null)
            {
                return HttpNotFound();
            }

            return View(globalfile);
        }

        [HttpPost]
        public ActionResult DeleteGlobalFile(int id)
        {
            GlobalFile globalfile = db.GlobalFiles.Find(id);
            db.GlobalFiles.Remove(globalfile);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteTestRun(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun == null)
            {
                return HttpNotFound();
            }
            ViewBag.ScenarioId = new SelectList(db.Scenarios, "ScenarioId", "ScenarioName");
            return View(testrun);
        }
        [HttpPost]
        public ActionResult DeleteTestRun(int id)
        {
            TestRun testrun = db.TestRuns.Find(id);
            db.TestRuns.Remove(testrun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RunRemote(int? id)
        {
            TestRun testrun = db.TestRuns.Find(id);
            if (testrun.State == State.InProcess)
            {
                return RedirectToAction("Index");
            }
            var CellName = db.Cells.Find(testrun.Scenario.CellId).CellName;
            var ServerOS = testrun.Scenario.ServerOS;
            RunRemoteService runservice = new RunRemoteService();
            runservice.RunScript(CellName, ServerOS);
            testrun.State = State.InProcess;
            db.SaveChanges();
            return Json(db.TestRuns.Select(x => new { RunId = x.RunId }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult MonitorLog(int? id)
        {
            TestRun testrun = db.TestRuns.Find(id);
            var CellName = db.Cells.Find(testrun.Scenario.CellId).CellName;
            var ServerOS = testrun.Scenario.ServerOS;
            RunRemoteService runservice = new RunRemoteService();
            runservice.MonitorRemoteLog(CellName, ServerOS);
            testrun.State = State.Fail;
            return Json(db.TestRuns.Select(x => new { RunId = x.RunId }), JsonRequestBehavior.AllowGet);
        }
    }
}