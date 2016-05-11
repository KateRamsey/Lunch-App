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
            foreach (var s in db.Surveys.Where(s => s.User.Id == userId && !s.IsFinished))
            {
                indexView.OutstandingSurveys.Add(s.Id);
            }


            var lunches = db.LunchMembers.Where(x => x.Member.Id == userId).Include("Lunch").ToList();

            foreach (var l in lunches)
            {
                var newLunch = new LunchIndexVM
                {
                    Id = l.Lunch.Id,
                    MeetingDateTime = l.Lunch.MeetingDateTime,
                    CreatorHandle = l.Lunch.Creator.Handle
                };

                if (l.Lunch.Resturant != null)
                {
                    newLunch.ResturantName = l.Lunch.Resturant.Name;
                    newLunch.ResturantLocation = l.Lunch.Resturant.Location;
                }
                

                indexView.Lunches.Add(newLunch);
            }


            if (indexView.Lunches != null)
            {
                indexView.Lunches = indexView.Lunches.OrderByDescending(x => x.MeetingDateTime).ToList();
                indexView.NextLunch = indexView.Lunches.First().MeetingDateTime;
            }


            var listOfLunchesCreated = db.Lunches.Where(l => l.Creator.Id == userId).ToList();

            foreach (var l in listOfLunchesCreated)
            {
                foreach (var s in l.Surveys.Where(s => !s.IsFinished))
                {
                    indexView.WaitingOnSurveys = true;
                    break;
                }
            }


            var ready = true;
            foreach (var lunch in listOfLunchesCreated)
            {
                if (lunch.Resturant == null)
                {
                    if (lunch.Surveys.Any(s => !s.IsFinished))
                    {
                        ready = false;
                    }

                    if (ready)
                    {
                        indexView.LunchReadyToPick.Add(lunch.Id);
                    }
                }

                ready = true;
            }


            //Buddies <--- add later

            //Favorite Resturants <--- add later

            return indexView;
        }
    }
}