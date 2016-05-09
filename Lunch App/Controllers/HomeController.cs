using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lunch_App.Models;
using Microsoft.AspNet.Identity;
using Lunch_App.Logic;

namespace Lunch_App.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = "/home/index" });
            }

            var userId = User.Identity.GetUserId();
            IndexVM indexView = IndexVMLogic.BuildIndexVM(userId, db);

            return View(indexView);
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
            var attendingUsers = db.Users.Where(x => memberListIds.Contains(x.Id)).ToList();

            var members = attendingUsers.Select(user => new LunchMembers()
            { InvitedTime = DateTime.Now, Lunch = newLunch, Member = user }).ToList();
            newLunch.Members.AddRange(members);

            foreach (var member in members)
            {
                db.Surveys.Add(new Survey() { Lunch = newLunch, User = db.Users.Find(member.Member.Id) });
            }


            db.Lunches.Add(newLunch);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // GET: Home/Edit/5
        public ActionResult EditSurvey(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            var surveyVM = new SurveyEditVM()
            {
                Id = survey.Id,
                LunchId = survey.Lunch.Id,
                UserId = survey.User.Id,
            };
            return View(surveyVM);
        }

        // POST: Home/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSurvey(SurveyEditVM surveyVM)
        {
            if (!ModelState.IsValid)
            {
                return View(surveyVM);
            }

            var survey = db.Surveys.Find(surveyVM.Id);
            if (survey == null)
            {
                return HttpNotFound();
            }

            survey.IsFinished = true;
            survey.IsComing = surveyVM.IsComing;
            survey.CuisineNotWanted = surveyVM.CuisineNotWanted;
            survey.CuisineWanted = surveyVM.CuisineWanted;
            survey.MinutesAvailiable = surveyVM.MinutesAvailiable;
            survey.SuggestedResturant = db.Resturants.Find(surveyVM.SuggestedResturantId);
            survey.TimeAvailable = surveyVM.TimeAvailable;
            survey.ZipCode = surveyVM.ZipCode;
            survey.ZipCodeRadius = surveyVM.ZipCodeRadius;

            survey.DietaryIssues = DietartyIssueLogic.SurveyDietaryIssues(surveyVM);


            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


    }
}