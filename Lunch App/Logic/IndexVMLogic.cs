using System;
using System.Collections.Generic;
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


            var lunches = db.LunchMembers.Where(x => x.Member.Id == userId).Select(x => x.Lunch.Id);

            foreach (var l in lunches)
            {
                var newLunch = new LunchVM();
                //TODO: build up newLunch
                newLunch.Id = l;





                indexView.Lunches.Add(newLunch);
            }

            if (indexView.Lunches != null)
            {
                //TODO: next lunch datetime
            }


            var listOfLunches = db.Lunches.Where(l => l.Creator.Id == userId).ToList();

            foreach (var l in listOfLunches)
            {
                foreach (var s in l.Surveys.Where(s => !s.IsFinished))
                {
                    indexView.WaitingOnSurveys = true;
                    break;
                }
            }




            //TODO: lunches ready to pick

            //Buddies <--- add later

            //Favorite Resturants <--- add later

            return indexView;
        }
    }
}