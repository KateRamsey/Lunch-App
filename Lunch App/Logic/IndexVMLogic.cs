using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Lunch_App.Models;

namespace Lunch_App.Logic
{
    public static class IndexVMLogic
    {
        public static IndexVM BuildIndexVM(string userId, ApplicationDbContext db)
        {
            var indexView = new IndexVM();

            indexView.OutstandingSurveys = 
                db.Surveys.Where(s => s.User.Id == userId && !s.IsFinished).Select(x => x.Id).ToList();

            var lunches = db.LunchMembers.Where(x => x.Member.Id == userId).Include("Lunch").Include("Lunch.Restaurant").ToList();

            foreach (var l in lunches)
            {
                var newLunch = new LunchIndexVM
                {
                    Id = l.Lunch.Id,
                    MeetingDateTime = l.Lunch.MeetingDateTime,
                    CreatorHandle = l.Lunch.Creator.Handle
                };

                if (l.Lunch.Restaurant != null)
                {
                    newLunch.RestaurantName = l.Lunch.Restaurant.Name;
                    newLunch.RestaurantLocation = l.Lunch.Restaurant.Location;
                    newLunch.RestaurantId = l.Lunch.Restaurant.Id;
                    newLunch.RestaurantSelected = true;
                }
                
                

                indexView.Lunches.Add(newLunch);
            }


            if (indexView.Lunches != null)
            {
                //indexView.Lunches = indexView.Lunches.OrderBy(x => x.MeetingDateTime).ToList();
                //foreach (var l in indexView.Lunches.Where(l => l.MeetingDateTime == DateTime.Today))
                //{
                //    indexView.NextLunch = l.MeetingDateTime;
                //}

                indexView.Lunches = indexView.Lunches.OrderByDescending(x => x.MeetingDateTime).ToList();
            }


            var listOfLunchesCreated = db.Lunches.Where(l => l.Creator.Id == userId).Include(l=>l.Restaurant)
                .Include(l=>l.Surveys).ToList();

            foreach (var l in listOfLunchesCreated)
            {
                indexView.WaitingOnSurveys = l.Surveys.Any(x => !x.IsFinished);
                if (indexView.WaitingOnSurveys)
                {
                    break;
                }
            }


            foreach (var lunch in listOfLunchesCreated.Where(lunch => lunch.Restaurant == null).
                Where(lunch => lunch.Surveys.All(s => s.IsFinished)))
            {
                indexView.LunchReadyToPick.Add(lunch.Id);
            }

            //Buddies <--- add later

            //Favorite Restaurants <--- add later

            return indexView;
        }
    }
}