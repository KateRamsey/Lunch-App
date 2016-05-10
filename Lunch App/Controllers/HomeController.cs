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
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new {returnUrl = "/home/index"});
            }

            var userId = User.Identity.GetUserId();
            IndexVM indexView = IndexVMLogic.BuildIndexVM(userId, db);

            return View(indexView);
        }




        // GET: Home/CreateLunch
        public ActionResult CreateLunch()
        {
            var newLunch = new LunchCreationVM() {MeetingTime = DateTime.Now};
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
            {InvitedTime = DateTime.Now, Lunch = newLunch, Member = user}).ToList();
            newLunch.Members.AddRange(members);

            foreach (var member in members)
            {
                db.Surveys.Add(new Survey() {Lunch = newLunch, User = db.Users.Find(member.Member.Id)});
                //TODO: Send notice of invite/survey to group
            }


            db.Lunches.Add(newLunch);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }


        // GET: Home/EditSurvey/5
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

        // POST: Home/EditSurvey/5
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

        // GET: Home/PickLunch/5
        public ActionResult PickLunch(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var lunch = db.Lunches.Find(id);
            if (lunch == null)
            {
                return RedirectToAction("Index");
            }

           
            var surveys = new List<SurveyFilterModel>();
            foreach (var s in lunch.Surveys)
            {
                surveys.Add(new SurveyFilterModel(s));
            }

            var resturants = new List<ResturantFilterModel>();
            foreach (var r in db.Resturants)
            {
                resturants.Add(new ResturantFilterModel(r));
            }

            var options = FilterLogic.Filter(resturants, surveys);


            int rank = 1;
            foreach (var o in options)
            {
                lunch.Options.Add(new ResturantOptions() {Lunch = lunch, Resturant = db.Resturants.Find(o), Rank = rank});
                rank++;
            }



            var lunchPick = new LunchPickVM()
            {
                Id = lunch.Id,
                MeetingDateTime = lunch.MeetingDateTime
            };
            foreach (var o in lunch.Options)
            {
                lunchPick.Picks.Add(new ResturantPickVM(o.Resturant));
            }

            return View(lunchPick);
        }


        // POST: Home/PickLunch/5
        [HttpPost]
        public ActionResult PickLunch(LunchPickVM pick)
        {
            var lunch = db.Lunches.Find(pick.Id);
            var pickedResturant = db.Resturants.Find(pick.SelectedId);

            if (lunch == null || pickedResturant == null)
            {
                return View(pick);
            }
            var resturant = db.Resturants.Find(pickedResturant.Id);
            if(resturant == null)
            {
                return View(pick);
            }

            lunch.Resturant = resturant;
            db.SaveChanges();

            //TODO: Send notice of pick to group
            return RedirectToAction("Index", "Home");
        }

        // GET: Home/CreateResturant
        public ActionResult CreateResturant()
        {
            return View();
        }


        // POST: Home/CreateResturant
        [HttpPost]
        public ActionResult CreateResturant(ResturantCreateVM newResturant)
        {
            if (!ModelState.IsValid)
            {
                return View(newResturant);
            }

            var resturant = newResturant.CreateResturant();

            db.Resturants.Add(resturant);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}