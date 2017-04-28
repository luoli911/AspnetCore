using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using System.Dynamic;
using AspnetCore.Models;

namespace AspnetCore.Controllers
{
    public class StepController : Controller
    {
        private RunTestContext db = new RunTestContext();
        // GET: Step
        public ActionResult Index(int? id)
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
            string json = "";
            if (System.IO.File.Exists(stepfile.FilePath))
            {
                json = System.IO.File.ReadAllText(stepfile.FilePath);
            }
            
            if(db.Steps.FirstOrDefault(p => p.StepFileId == stepfile.Id)==null)
            {
                Step stepjson = JsonConvert.DeserializeObject<Step>(json);
                stepjson.StepFileId = stepfile.Id;
                db.Steps.Add(stepjson);
                db.SaveChanges();
            }
            Step step = db.Steps.Single(p => p.StepFileId == stepfile.Id);
            if (db.StepQueues.Where(p => p.StepFile.Id == stepfile.Id).Count() == 0)
            {
                SaveQueue(step, stepfile);
            }
            dynamic model = new ExpandoObject();
            model.Step = step;
            model.StepFile = stepfile;
            model.StepQueues = db.StepQueues.Where(p => p.StepFile.Id == stepfile.Id).OrderBy(s => s.StepId);
            return View(model);
        }
        public void SaveQueue(Step step, StepFile stepfile)
        {
            if(step == null)
            {
                return;
            }
            int i = 1;
            foreach (var server in step.Server)
            {
                db.StepQueues.Add(new StepQueue { StepId = 1000 * i, StepName = server.Name, StepValue = server.Value, StepFile = stepfile, State = "Server" });
                i = i + 1;
            }
            foreach (var client in step.Client)
            {
                db.StepQueues.Add(new StepQueue { StepId = 1000 * i, StepName = client.Name, StepValue = client.Value, StepFile = stepfile, State = "Client" });
                i = i + 1;
            }
            db.SaveChanges();
        }
        public ActionResult Create(int? step)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "QueueId,StepId,StepFileId,StepName,StepValue,State")]StepQueue stepqueue)
        {
            if(ModelState.IsValid)
            {
                db.StepQueues.Add(stepqueue);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = stepqueue.StepFileId });
            }
            return View(stepqueue);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepQueue stepqueue = db.StepQueues.Find(id);
            if (stepqueue == null)
            {
                return HttpNotFound();
            }

            return View(stepqueue);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "QueueId,StepId,StepFileId,StepName,StepValue,State")] StepQueue stepqueue)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(stepqueue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {id= stepqueue.StepFileId });
            }
            return View(stepqueue);
        }

        [HttpPost]
        public ActionResult EditAttributes([Bind(Include = "Id,FileName,FilePath,IsTemplate")] StepFile stepfile)
        {

            if (ModelState.IsValid)
            {
                db.Entry(stepfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = stepfile.Id });
            }
            return View(stepfile);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StepQueue stepqueue = db.StepQueues.Find(id);
            if (stepqueue == null)
            {
                return HttpNotFound();
            }
            return View(stepqueue);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            StepQueue stepqueue = db.StepQueues.Find(id);
            db.StepQueues.Remove(stepqueue);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = stepqueue.StepFileId });
        }

        [HttpPost]
        public ActionResult Save([Bind(Include = "StepId,StepFileId,ServerName,ClientName,DotnetVersion,TestappName,WebServer,ServerOS")] Step step)
        {
            if (ModelState.IsValid)
            {
                db.Entry(step).State = EntityState.Modified;
                db.SaveChanges();
                SaveStepFile(step);
                return RedirectToAction("Index", "Home");
            }
            return View(step);

            
        }
        public void SaveStepFile(Step step)
        {
            StepFile stepfile = db.StepFiles.Find(step.StepFileId);
            //Step step = db.Steps.Find(id);
            int snum = 0, cnum = 0;

            List<StepQueue> stepqueues = db.StepQueues.Where(p => p.StepFile.Id == stepfile.Id).OrderBy(s => s.StepId).ToList();
            for (int i = 0; i < stepqueues.Count; i++)
            {
                if (stepqueues[i].State == "Server") snum++;
                if (stepqueues[i].State == "Client") cnum++;
            }
            step.Server = new Server[snum];
            step.Client = new Client[cnum];
            for (int k = 0, i = 0, j = 0; k < stepqueues.Count; k++)
            {
                if (stepqueues[k].State == "Server")
                {
                    step.Server[i] = new Server();
                    step.Server[i].Name = stepqueues[k].StepName;
                    step.Server[i].Value = stepqueues[k].StepValue;
                    i++;
                }
                if (stepqueues[k].State == "Client")
                {
                    step.Client[j] = new Client();
                    step.Client[j].Name = stepqueues[k].StepName;
                    step.Client[j].Value = stepqueues[k].StepValue;
                    j++;
                }
            }

            var json = JsonConvert.SerializeObject(step);
            if (System.IO.File.Exists(stepfile.FilePath))
            {
                System.IO.File.WriteAllText(stepfile.FilePath, json);
               
            }
        }
        public JsonResult Add(StepQueue stepqueue)
        {
            return Json(db.StepQueues.Add(stepqueue), JsonRequestBehavior.AllowGet);
        }
        
    }
}