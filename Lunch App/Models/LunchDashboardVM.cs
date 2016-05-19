using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch_App.Models
{
    public class LunchDashboardVM
    {
        public LunchDashboardVM(Lunch lunch, string myId)
        {
            LunchId = lunch.Id;
            MeetingTime = lunch.MeetingDateTime;
            if (lunch.Restaurant != null)
            {
                RestaurantId = lunch.Restaurant.Id;
                RestaurantName = lunch.Restaurant.Name;
            }
            NumberOfSurveys = lunch.Surveys.Count();
            NumberOfSurveyFinished = lunch.Surveys.Count(x => x.IsFinished);
            IsCreatedByMe = lunch.Creator.Id == myId;
            var mysurvey = lunch.Surveys.FirstOrDefault(s => s.User.Id == myId);
            if (mysurvey != null)
            {
                NeedsResponse = !mysurvey.IsFinished;
                SurveyId = mysurvey.Id;
            }
        }

        public int LunchId { get; set; }
        public DateTime MeetingTime { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }
        public int NumberOfSurveys { get; set; }
        public int NumberOfSurveyFinished { get; set; }
        public bool SurveysFinished => NumberOfSurveys == NumberOfSurveyFinished;
        public bool IsCreatedByMe { get; set; }
        public bool NeedsResponse { get; set; }
        public int SurveyId { get; set; }

        public bool LunchToday => MeetingTime.Date == DateTime.Today;

        public bool LunchPast => MeetingTime < DateTime.Today;
    }
}