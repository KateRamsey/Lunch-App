using System;
using System.Collections.Generic;

namespace Lunch_App.Logic
{
    public class SurveyTotal
    {
        public DateTime LunchTime { get; set; }
        public List<string> ZipCodes { get; set; } = new List<string>();
        public List<Cuisine> WantedCuisines { get; set; } = new List<Cuisine>();
        public List<Cuisine> NotWantedCuisines { get; set; } = new List<Cuisine>();
        public List<int> SuggestedResturantIds { get; set; } = new List<int>();
        public int DiataryIssues { get; set; }

        public List<string> PossibleZips { get; set; } = new List<string>();

    }

    public class HoursOfOperations
    {
        public DateTime MonOpen { get; set; }
        public DateTime MonClose { get; set; }
        public DateTime TueOpen { get; set; }
        public DateTime TueClose { get; set; }
        public DateTime WedOpen { get; set; }
        public DateTime WedClose { get; set; }
        public DateTime ThurOpen { get; set; }
        public DateTime ThurClose { get; set; }
        public DateTime FriOpen { get; set; }
        public DateTime FriClose { get; set; }
        public DateTime SatOpen { get; set; }
        public DateTime SatClose { get; set; }
        public DateTime SunOpen { get; set; }
        public DateTime SunClose { get; set; }
    }
}