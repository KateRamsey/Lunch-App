using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunch_App.Models;
using RestSharp;

namespace Lunch_App.Logic
{
    public static class FilterLogic
    {
        public static List<int> Filter(List<ResturantFilterModel> resturants, List<SurveyFilterModel> surveys)
        {
            var results = new List<int>();
            var surveyTotal = CombineSurveys(surveys);

            foreach (var r in resturants)
            {
                if (((surveyTotal.DiataryIssues & r.DietaryOptions) == surveyTotal.DiataryIssues) 
                    && !surveyTotal.NotWantedCuisines.Contains(r.CuisineType)
                    && ResturantOpen(r.HoursOfOperation, surveyTotal.LunchTime))
                {
                    results.Add(r.Id);
                }
            }

            

            //TODO: Call Rank()



            return results;
        }

        private static List<int> Rank(List<ResturantFilterModel> resturants, SurveyTotal surveyTotal)
        {

            //TODO: sort on suggested resturants and cuisine types wanted
            var result = new List<int>();


            return result;
        } 

        private static bool ResturantOpen(string hoursOfOperation, DateTime lunchTime)
        {
            var dateRanges = BreakHoursToRanges(hoursOfOperation);
            //TODO: check ranges against lunchTime
            return true;
        }

        private static object BreakHoursToRanges(string hoursOfOperation)
        {
            //TODO: parse string to ranges
            throw new NotImplementedException();
        }


        private static SurveyTotal CombineSurveys(List<SurveyFilterModel> surveys)
        {
            var result = new SurveyTotal {DiataryIssues = 0};
            foreach (var s in surveys)
            {
                result.ZipCodes.AddRange(FindZipCodes(s.ZipCode, s.ZipCodeRadius));
                result.NotWantedCuisines.Add(s.CuisineNotWanted);
                result.WantedCuisines.Add(s.CuisineWanted);
                result.SuggestedResturantIds.Add(s.SuggestedResturantId);
                result.DiataryIssues = result.DiataryIssues | s.DiataryIssues;
            }

            //TODO: result.LunchTime complicated logic

            result.LunchTime = surveys.First().TimeAvailable;


            return result;
        }

        public static IEnumerable<int> FindZipCodes(int zipCode, int zipCodeRadius)
        {
            var zipList = new List<int>() {zipCode};


            var client = new RestClient("http://www.zipcodeapi.com/rest/ye29DnmW3cFSb9nmn3EEm7qiaj1E2M6A18KEw7AzjgsupPQNTvsfCSyNbzsemzPn");

            var request = new RestRequest(
                $"/radius.json/{zipCode}/{zipCodeRadius}/mile", Method.GET);

            var response = client.Execute(request);

            var content = response.Content;

            List<ZipsFromAPI> myDeserializedObjList =
                (List<ZipsFromAPI>) Newtonsoft.Json.JsonConvert.DeserializeObject(content);

            zipList.AddRange(myDeserializedObjList.Select(z => z.zip_code));

            return zipList;
        }

        public class ZipsFromAPI
        {
            public int zip_code { get; set; }
        }
    }

}