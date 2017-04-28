using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspnetCore.Models;

namespace AspnetCore.Controllers
{
    public class TestRunsController : Controller
    {
        private RunTestContext db = new RunTestContext();

        // GET: TestRuns
        public async Task<ActionResult> Index()
        {
            return View(await db.TestRuns.ToListAsync());
        }

        // GET: TestRuns/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestRun testRun = await db.TestRuns.FindAsync(id);
            if (testRun == null)
            {
                return HttpNotFound();
            }
            return View(testRun);
        }

        // GET: TestRuns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TestRuns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RunId,RunName,ScenarioId,State,ScriptPath,StartTime,Tester")] TestRun testRun)
        {
            if (ModelState.IsValid)
            {
                db.TestRuns.Add(testRun);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(testRun);
        }

        // GET: TestRuns/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestRun testRun = await db.TestRuns.FindAsync(id);
            if (testRun == null)
            {
                return HttpNotFound();
            }
            return View(testRun);
        }

        // POST: TestRuns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RunId,RunName,ScenarioId,State,ScriptPath,StartTime,Tester")] TestRun testRun)
        {
            if (ModelState.IsValid)
            {
                db.Entry(testRun).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(testRun);
        }

        // GET: TestRuns/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestRun testRun = await db.TestRuns.FindAsync(id);
            if (testRun == null)
            {
                return HttpNotFound();
            }
            return View(testRun);
        }

        // POST: TestRuns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            TestRun testRun = await db.TestRuns.FindAsync(id);
            db.TestRuns.Remove(testRun);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
