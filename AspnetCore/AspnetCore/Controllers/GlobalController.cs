using AspnetCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AspnetCore.Controllers
{
    public class GlobalController : Controller
    {
        private RunTestContext db = new RunTestContext();
        // GET: Global
        public ActionResult Index(int? id)
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
            string json = "";
            if (System.IO.File.Exists(globalfile.FilePath))
            {
                json = System.IO.File.ReadAllText(globalfile.FilePath);
            }

            if (db.GlobalQueues.FirstOrDefault(p => p.GlobalFileId == globalfile.Id) == null)
            {
                Global globaljson = JsonConvert.DeserializeObject<Global>(json);
                globaljson.GlobalFileId = globalfile.Id;
                db.Globals.Add(globaljson);
                db.SaveChanges();
            }
            Global global = db.Globals.Single(p => p.GlobalFileId == globalfile.Id);
            if (db.GlobalQueues.Where(p => p.GlobalFile.Id == globalfile.Id).Count() == 0)
            {
                SaveQueue(global, globalfile);
            }
            ViewData["GlobalfileId"] = id;
            dynamic model = new ExpandoObject();
            model.Global = global;
            model.GlobalFile = globalfile;
            model.GlobalQueues = db.GlobalQueues.Where(p => p.GlobalFile.Id == globalfile.Id).OrderBy(s => s.QueueId);
            return View(model);
        }

        public void SaveQueue(Global global, GlobalFile globalfile)
        {
            if (global == null)
            {
                return;
            }
            int i = 1;
            foreach (var winglobal in global.Windows)
            {
                db.GlobalQueues.Add(new GlobalQueue { GlobalName = winglobal.Name, GlobalValue = winglobal.Value, GlobalFile = globalfile, State = "Windows" });
                i = i + 1;
            }
            foreach (var linuxglobal in global.Linux)
            {
                db.GlobalQueues.Add(new GlobalQueue { GlobalName = linuxglobal.Name, GlobalValue = linuxglobal.Value, GlobalFile = globalfile, State = "Linux" });
                i = i + 1;
            }
            foreach (var dockerglobal in global.Docker)
            {
                db.GlobalQueues.Add(new GlobalQueue { GlobalName = dockerglobal.Name, GlobalValue = dockerglobal.Value, GlobalFile = globalfile, State = "Docker" });
                i = i + 1;
            }
            db.SaveChanges();
        }

        public ActionResult Create(int? step)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "QueueId,GlobalFileId,GlobalName,GlobalValue,State")]GlobalQueue globalqueue)
        {
            if (ModelState.IsValid)
            {
                db.GlobalQueues.Add(globalqueue);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = globalqueue.GlobalFileId });
            }
            return View(globalqueue);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalQueue globalqueue = db.GlobalQueues.Find(id);
            if (globalqueue == null)
            {
                return HttpNotFound();
            }

            return View(globalqueue);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "QueueId,GlobalFileId,GlobalName,GlobalValue,State")] GlobalQueue globalqueue)
        {

            if (ModelState.IsValid)
            {
                db.Entry(globalqueue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = globalqueue.GlobalFileId });
            }
            return View(globalqueue);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GlobalQueue globalqueue = db.GlobalQueues.Find(id);
            if (globalqueue == null)
            {
                return HttpNotFound();
            }
            return View(globalqueue);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            GlobalQueue globalqueue = db.GlobalQueues.Find(id);
            db.GlobalQueues.Remove(globalqueue);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = globalqueue.GlobalFileId });
        }

        public ActionResult SaveGlobalFile(int? id)
        {
            Global global = db.Globals.SingleOrDefault(p => p.GlobalFileId == id);
            GlobalFile globalfile = db.GlobalFiles.Find(id);
            //Step step = db.Steps.Find(id);
            int wnum = 0, lnum = 0,dnum = 0;
            List<GlobalQueue> globalqueues = db.GlobalQueues.Where(p => p.GlobalFile.Id == globalfile.Id).OrderBy(s => s.QueueId).ToList();
            for (int i = 0; i < globalqueues.Count; i++)
            {
                if (globalqueues[i].State == "Windows") wnum++;
                if (globalqueues[i].State == "Linux") lnum++;
                if (globalqueues[i].State == "Docker") dnum++;
            }
            global.Windows = new Window[wnum];
            global.Linux = new Linux[lnum];
            global.Docker = new Docker[dnum];
            for (int k = 0, i = 0, j = 0; k < globalqueues.Count; k++)
            {
                if (globalqueues[k].State == "Windows")
                {
                    global.Windows[i] = new Window();
                    global.Windows[i].Name = globalqueues[k].GlobalName;
                    global.Windows[i].Value = globalqueues[k].GlobalValue;
                    i++;
                }
                if (globalqueues[k].State == "Linux")
                {
                    global.Linux[j] = new Linux();
                    global.Linux[j].Name = globalqueues[k].GlobalName;
                    global.Linux[j].Value = globalqueues[k].GlobalValue;
                    j++;
                }
            }

            var json = JsonConvert.SerializeObject(global);
            if (System.IO.File.Exists(globalfile.FilePath))
            {
                System.IO.File.WriteAllText(globalfile.FilePath, json);

            }
            return RedirectToAction("Index", "Home");
        }
    }
}