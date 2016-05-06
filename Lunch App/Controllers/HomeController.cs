using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunch_App.Models;
using Microsoft.AspNet.Identity;

namespace Lunch_App.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new {returnUrl = "/home/index"});
            }

            return View();
        }

        // GET: Home/CreateLunch
        public ActionResult CreateLunch()
        {
            var newLunch = new LunchCreationVM() { MeetingTime = DateTime.Now };
            var potentialMembers = db.Users.Select(x => x).ToList()
                .Select(u => new UserVM(u));
            newLunch.Members.AddRange(potentialMembers);

            return View(newLunch);
        }

        // POST: Home/CreateLunch
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult CreateLunch(LunchCreationVM lunch)
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
                db.Surveys.Add(new Survey() { Lunch = newLunch, User = db.Users.Find(member.Member.Id) });
            }


            db.Lunches.Add(newLunch);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

    }
}