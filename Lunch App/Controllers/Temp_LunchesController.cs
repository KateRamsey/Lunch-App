using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lunch_App.Models;

namespace Lunch_App.Controllers
{
    public class Temp_LunchesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Temp_Lunches
        public ActionResult Index()
        {
            return View(db.Lunches.ToList());
        }

        // GET: Temp_Lunches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = db.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // GET: Temp_Lunches/Create
        public ActionResult Create()
        {
            var newLunch = new Lunch();
            var potentialMembers = db.Users.ToList()
                .Select(u => new LunchMembers() {Member = u, InvitedTime = DateTime.Now});
            newLunch.Members.AddRange(potentialMembers);

            return View(newLunch);
        }

        // POST: Temp_Lunches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MeetingDateTime")] Lunch lunch)
        {
            if (ModelState.IsValid)
            {
                db.Lunches.Add(lunch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lunch);
        }

        // GET: Temp_Lunches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = db.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // POST: Temp_Lunches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MeetingDateTime")] Lunch lunch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lunch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lunch);
        }

        // GET: Temp_Lunches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lunch lunch = db.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }
            return View(lunch);
        }

        // POST: Temp_Lunches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lunch lunch = db.Lunches.Find(id);
            db.Lunches.Remove(lunch);
            db.SaveChanges();
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
