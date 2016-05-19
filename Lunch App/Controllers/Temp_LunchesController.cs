using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lunch_App.Models;
using Microsoft.AspNet.Identity;

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
            var lunch = db.Lunches.Find(id);
            if (lunch == null)
            {
                return HttpNotFound();
            }

            var lunchView = new LunchVM()
            {
                Id = lunch.Id,
                Creator = new UserVM(lunch.Creator),
                MeetingDateTime = lunch.MeetingDateTime
            };

            if (lunch.Restaurant != null)
            {
                lunchView.Restaurant = new RestaurantVM(lunch.Restaurant);
            }

            foreach (var m in lunch.Members)
            {
                lunchView.Members.Add(new UserVM(db.Users.Find(m)));
            }

            return View(lunchView);
        }

        // GET: Temp_Lunches/Create
        public ActionResult Create()
        {
            var newLunch = new LunchCreationVM() { MeetingTime = DateTime.Now };
            var potentialMembers = db.Users.Select(x => x).ToList()
                .Select(u => new UserVM(u));
            newLunch.Members.AddRange(potentialMembers);

            return View(newLunch);
        }

        // POST: Temp_Lunches/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(LunchCreationVM lunch)
        {
            if (!ModelState.IsValid)
            {
                return View(lunch);
            }

            var newLunch = new Lunch
            {
                Creator = db.Users.Find(User.Identity.GetUserId()),
                MeetingDateTime = lunch.MeetingTime
            };

            var memberListIds = (from m in lunch.Members where m.IsChecked select m.Id).ToList();
            var attendingUsers = db.Users.Where(x => memberListIds.Contains(x.Id));

            var members = new List<LunchMembers>();
            foreach (var user in attendingUsers)
            {
                members.Add(new LunchMembers() { InvitedTime = DateTime.Now, Lunch = newLunch, Member = user });
            }
            newLunch.Members.AddRange(members);

            foreach (var member in members)
            {
                db.Surveys.Add(new Survey() {Lunch = newLunch, User = db.Users.Find(member.Member.Id)});
            }


            db.Lunches.Add(newLunch);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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
