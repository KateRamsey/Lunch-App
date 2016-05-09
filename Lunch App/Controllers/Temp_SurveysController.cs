using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lunch_App.Models;
using Microsoft.Ajax.Utilities;

namespace Lunch_App.Controllers
{
    public class Temp_SurveysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Temp_Surveys
        public ActionResult Index()
        {
            return View(db.Surveys.ToList());
        }

        // GET: Temp_Surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: Temp_Surveys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Temp_Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IsFinished,TimeAvailable,MinutesAvailiable,ZipCode,ZipCodeRadius,CuisineWanted,CuisineNotWanted,DiataryIssues")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(survey);
        }

        // GET: Temp_Surveys/Edit/5
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

        // POST: Temp_Surveys/Edit/5
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

            survey.DietaryIssues = SurveyDietaryIssues(surveyVM);


            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private int SurveyDietaryIssues(SurveyEditVM survey)
        {
            int issues = 0;

            if (survey.Vegan)
            {
                issues += (int)DietaryIssues.Vegan;
            }
            if (survey.Vegetarian)
            {
                issues += (int)DietaryIssues.Vegetarian;
            }
            if (survey.GlutenFree)
            {
                issues += (int)DietaryIssues.GlutenFree;
            }
            if (survey.NutAllergy)
            {
                issues += (int)DietaryIssues.NutAllergy;
            }
            if (survey.ShellFishAllergy)
            {
                issues += (int)DietaryIssues.ShellFishAllergy;
            }
            if (survey.Kosher)
            {
                issues += (int)DietaryIssues.Kosher;
            }
            if (survey.Halaal)
            {
                issues += (int)DietaryIssues.Halaal;
            }
            if (survey.LactoseIntolerant)
            {
                issues += (int)DietaryIssues.LactoseIntolerant;
            }

            return issues;
        }

        // GET: Temp_Surveys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // POST: Temp_Surveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Survey survey = db.Surveys.Find(id);
            db.Surveys.Remove(survey);
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
