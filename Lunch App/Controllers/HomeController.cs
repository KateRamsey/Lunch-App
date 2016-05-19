using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Lunch_App.Models;
using Microsoft.AspNet.Identity;
using Lunch_App.Logic;
using Microsoft.AspNet.Identity.Owin;

namespace Lunch_App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {

            var userId = User.Identity.GetUserId();


            var model = db.Lunches.Include("Surveys").Include("Restaurant").Where(x => x.Creator.Id == userId || x.Members.Any(m => m.Member.Id == userId)).ToList()
                .Select(l => new LunchDashboardVM(l, userId));

            //IndexVM indexView = IndexVMLogic.BuildIndexVM(userId, db);

            return View(model);
        }




        // GET: Home/CreateLunch
        public ActionResult CreateLunch()
        {
            var newLunch = new LunchCreationVM() {MeetingTime = DateTime.Today.AddHours(12)};
            var user = new List<LunchUser>();
            user.Add(db.Users.Find(User.Identity.GetUserId()));
            
            var potentialMembers = db.Users.Select(x => x).ToList().Except(user)
                .Select(u => new UserVM(u)).ToList();
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
            var userId = User.Identity.GetUserId();
            var newLunch = new Lunch
            {
                Creator = db.Users.Find(userId),
                MeetingDateTime = lunch.MeetingTime
            };

            var memberListIds = (from m in lunch.Members where m.IsChecked select m.Id).ToList();
            memberListIds.Add(userId);
            var attendingUsers = db.Users.Where(x => memberListIds.Contains(x.Id)).ToList();
            

            var members = attendingUsers.Select(user => new LunchMembers()
            {InvitedTime = DateTime.Now, Lunch = newLunch, Member = user}).ToList();
            newLunch.Members.AddRange(members);

            foreach (var member in members)
            {
                db.Surveys.Add(new Survey() {Lunch = newLunch, User = db.Users.Find(member.Member.Id)});  
            }
            SendSurveyEmails(attendingUsers);

            db.Lunches.Add(newLunch);
            db.SaveChanges();
            TempData["Message"] = "Lunch group created and notices sent... time to fill out your survey";
            return RedirectToAction("Index", "Home");
        }

        private async Task SendSurveyEmails(List<LunchUser> attendingUsers)
        {
            const string message = "You have been invited to lunch, please login to http://lunchconnect.azurewebsites.net/ to fill out a short survey";
            foreach (var user in attendingUsers)
            {
                await UserManager.SendEmailAsync(user.Id, "New Lunch Survey", message);
            }
        }


        // GET: Home/EditSurvey/5
        public ActionResult EditSurvey(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            var surveyVM = new SurveyEditVM()
            {
                Id = survey.Id,
                LunchId = survey.Lunch.Id,
                UserId = survey.User.Id,
                MeetingTime = survey.Lunch.MeetingDateTime,
                CreatorHandle = survey.Lunch.Creator.Handle
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
            survey.SuggestedRestaurant = db.Restaurants.Find(surveyVM.SuggestedRestaurantId);
            survey.TimeAvailable = surveyVM.TimeAvailable;
            survey.ZipCode = surveyVM.ZipCode;
            survey.ZipCodeRadius = surveyVM.ZipCodeRadius;

            survey.DietaryIssues = DietartyIssueLogic.SurveyDietaryIssues(surveyVM);


            db.SaveChanges();
            TempData["Message"] = "Survey submitted";
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

            var restaurants = new List<RestaurantFilterModel>();
            foreach (var r in db.Restaurants)
            {
                restaurants.Add(new RestaurantFilterModel(r));
            }

            var options = FilterLogic.Filter(restaurants, surveys, db);


            int rank = 1;
            foreach (var o in options)
            {
                lunch.Options.Add(new RestaurantOptions() {Lunch = lunch, Restaurant = db.Restaurants.Find(o), Rank = rank});
                rank++;
            }



            var lunchPick = new LunchPickVM()
            {
                Id = lunch.Id,
                MeetingDateTime = lunch.MeetingDateTime
            };
            foreach (var o in lunch.Options)
            {
                lunchPick.Picks.Add(new RestaurantPickVM(o.Restaurant));
            }

            return View(lunchPick);
        }


        // POST: Home/PickLunch/5
        [HttpPost]
        public ActionResult PickLunch(LunchPickVM pick)
        {
            var lunch = db.Lunches.Find(pick.Id);
            var pickedRestaurant = db.Restaurants.Find(pick.SelectedId);

            if (lunch == null || pickedRestaurant == null)
            {
                return View(pick);
            }
            var restaurant = db.Restaurants.Find(pickedRestaurant.Id);
            if(restaurant == null)
            {
                return View(pick);
            }

            lunch.Restaurant = restaurant;
            db.SaveChanges();


            var attendingUsers = db.LunchMembers.Where(x => x.Lunch == lunch).Select(y=>y.Member.Id);
            
            SendLunchPickEmails(attendingUsers, restaurant, pick);

            TempData["Message"] = "Restaurant picked and notices sent, have a great lunch!";
            return RedirectToAction("Index", "Home");
        }

        private async Task SendLunchPickEmails(IQueryable<string> attendingUserIds, Restaurant restaurant, LunchPickVM lunch)
        {
            var message = $"It's a plan! Join your group for lunch at {restaurant.Name}, " +
                          $"located at {restaurant.Location}, {restaurant.LocationZip}. " +
                          $"Lunch starts at {lunch.MeetingDateTime.ToShortTimeString()} on {lunch.MeetingDateTime.ToShortDateString()}. " +
                          "Thank you for using LunchConnect, and have a great lunch! -Kate";
            foreach (var userId in attendingUserIds)
            {
                await UserManager.SendEmailAsync(userId, "Lunch Plan", message);
            }
        }

        // GET: Home/CreateRestaurant
        public ActionResult CreateRestaurant()
        {
            return View();
        }


        // POST: Home/CreateRestaurant
        [HttpPost]
        public ActionResult CreateRestaurant(RestaurantCreateVM newRestaurant)
        {
            if (!ModelState.IsValid)
            {
                return View(newRestaurant);
            }

            var restaurant = newRestaurant.CreateRestaurant();

            db.Restaurants.Add(restaurant);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        // GET: Home/RestaurantDetails/5
        public ActionResult RestaurantDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var restaurant = db.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }
            var restaurantView = new RestaurantDetailVM(restaurant);

            return View(restaurantView);
        }

    }
}