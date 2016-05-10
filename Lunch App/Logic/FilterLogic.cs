using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lunch_App.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Lunch_App.Logic
{
    public static class FilterLogic
    {
        public static List<int> Filter(List<ResturantFilterModel> resturants, List<SurveyFilterModel> surveys)
        {
            var surveyTotal = CombineSurveys(surveys);

            var passingResturants = resturants.Where(r =>
            RestaurantMeetsDietaryNeeds(surveyTotal, r)
            && CuisineWanted(r, surveyTotal)
            && ResturantOpen(r.HoursOfOperation, surveyTotal.LunchTime)
            && AcceptableLocation(r, surveyTotal)).ToList();

            return Rank(passingResturants, surveyTotal);
        }

        private static bool AcceptableLocation(ResturantFilterModel r, SurveyTotal surveyTotal)
        {
            return surveyTotal.ZipCodes.Contains(r.LocationZip);
        }

        private static bool CuisineWanted(ResturantFilterModel r, SurveyTotal surveyTotal)
        {
            return !surveyTotal.NotWantedCuisines.Contains(r.CuisineType);
        }

        private static bool RestaurantMeetsDietaryNeeds(SurveyTotal surveyTotal, ResturantFilterModel r)
        {
            return ((surveyTotal.DietaryIssues & r.DietaryOptions) == surveyTotal.DietaryIssues);
        }

        private static List<int> Rank(List<ResturantFilterModel> resturants, SurveyTotal surveyTotal)
        {
            foreach (var r in resturants)
            {
                if (surveyTotal.SuggestedResturantIds.Contains(r.Id))
                {
                    r.Score += surveyTotal.SuggestedResturantIds.GroupBy(x=>x, v => v).Count(p => p.Key == r.Id) * 50;
                }
                if (surveyTotal.WantedCuisines.Contains(r.CuisineType))
                {
                    r.Score += surveyTotal.WantedCuisines.GroupBy(x => x, v => v).Count(p=>p.Key == r.CuisineType) * 25;
                }
                if (surveyTotal.BaseZips.Contains(r.LocationZip))
                {
                    r.Score += surveyTotal.BaseZips.GroupBy(x => x, v => v).Count(p=>p.Key == r.LocationZip) * 15;
                }
            }

            var result = resturants.OrderByDescending(x => x.Score).Select(v=>v.Id).ToList();

            return result;
        }

        public static bool ResturantOpen(string hoursOfOperation, DateTime lunchTime)
        {
            var dateRanges = BreakHoursToRanges(hoursOfOperation);

            return dateRanges.Where(dr => lunchTime.DayOfWeek == dr.DayOfWeek)
                .Any(dr => lunchTime.Hour >= dr.Open.Hour && lunchTime.AddHours(1).Hour <= dr.Close.Hour);
        }

        public static IEnumerable<HoursOfOperations> BreakHoursToRanges(string hoursOfOperation)
        {
            var parsed = hoursOfOperation.Split(',');

            return parsed.SelectMany(s => HoursOfOperations.Parse(s.TrimStart())).ToList();
        }


        private static SurveyTotal CombineSurveys(List<SurveyFilterModel> surveys)
        {
            surveys.RemoveAll(x => x.IsComing == false);
            var result = new SurveyTotal { DietaryIssues = 0 };
            if (surveys.Count != 0)
            {
                foreach (var s in surveys)
                {
                    result.PossibleZips.AddRange(FindZipCodes(s.ZipCode, s.ZipCodeRadius));
                    result.NotWantedCuisines.Add(s.CuisineNotWanted);
                    result.WantedCuisines.Add(s.CuisineWanted);
                    result.SuggestedResturantIds.Add(s.SuggestedResturantId);
                    result.DietaryIssues = result.DietaryIssues | s.DietaryIssues;
                    result.BaseZips.Add(s.ZipCode);
                }

                result.ZipCodes = result.PossibleZips.GroupBy(z => z, z => z)
                    .Where(g => g.Count() == surveys.Count()).Select(g => g.Key).ToList();




                //TODO: result.LunchTime complicated logic

                result.LunchTime = surveys.First().TimeAvailable;
            }

            return result;
        }

        public static IEnumerable<string> FindZipCodes(string zipCode, int zipCodeRadius)
        {

            var client = new RestClient("http://www.zipcodeapi.com/rest/q6WfHQWcMccemF5D0GWGP9sE87bt1ObrV7GCYDOLP1ZbG5PGMLnBVQf4XrxYgIpb");

            var request = new RestRequest(
                $"/radius.json/{zipCode}/{zipCodeRadius}/mile", Method.GET);

            var response = client.Execute(request);

            var content = (JObject)JsonConvert.DeserializeObject(response.Content);


            var zips = content["zip_codes"].ToObject<List<ZipsFromAPI>>().Select(x => x.zip_code);

            return zips;
        }

        public class ZipsFromAPI
        {
            public string zip_code { get; set; }
        }
    }

}