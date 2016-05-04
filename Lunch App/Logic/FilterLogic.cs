using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunch_App.Models;

namespace Lunch_App.Logic
{
    public static class FilterLogic
    {
        public static List<int> Filter(List<ResturantFilterModel> resturants, List<SurveyFilterModel> surveys)
        {
            var results = new List<int>();
            return results;
        }


        private static SurveyTotal CombineSurveys(List<SurveyFilterModel> surveys)
        {
            var result = new SurveyTotal();

            foreach (var s in surveys)
            {
                result.ZipCodes.AddRange(FindZipCodes(s.ZipCode, s.ZipCodeRadius));
                result.NotWantedCuisines.Add(s.CuisineNotWanted);
                result.WantedCuisines.Add(s.CuisineWanted);
                result.SuggestedResturantIds.Add(s.SuggestedResturantId);
            }

            //TODO: result.LunchTime

            //TODO: result.DiataryIssues



            return result;
        }

        private static IEnumerable<int> FindZipCodes(int zipCode, int zipCodeRadius)
        {
            var zipList = new List<int>() {zipCode};

            //TODO: use api to find list of zipcodes in range

            return zipList;
        }
    }

}