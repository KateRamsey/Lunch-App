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
            var results = new List<int>();
            var surveyTotal = CombineSurveys(surveys);


            foreach (var r in resturants)
            {
                if (((surveyTotal.DiataryIssues & r.DietaryOptions) == surveyTotal.DiataryIssues) 
                    && !surveyTotal.NotWantedCuisines.Contains(r.CuisineType)
                    //&& ResturantOpen(r.HoursOfOperation, surveyTotal.LunchTime)
                    && surveyTotal.ZipCodes.Contains(r.LocationZip))
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

        private static HoursOfOperations BreakHoursToRanges(string hoursOfOperation)
        {
            var hours = new HoursOfOperations();

            var parsed = hoursOfOperation.Split(',');

            var splitOn = new char[] {' ','-'};

    foreach (var s in parsed)
            {
                if (s.StartsWith("M-F"))
                {
                    //set Mon-Fri start and end date
                }
                else if (s.StartsWith("Sa"))
                {
                    //set Sat start and end date
                }
                else if (s.StartsWith("Su"))
                {
                    //set Sun start and end date
                    var sunSplit = s.Split(splitOn);
                    var sunOpen = sunSplit[1];
                    var sunClose = sunSplit[2];
                    var nextSunday = DateTime.Now;

                    while(nextSunday.DayOfWeek != DayOfWeek.Sunday)
                    {
                        nextSunday = nextSunday.AddDays(1);
                    }
                    hours.SunOpen = TimeToDateTime(sunOpen, nextSunday);
                    hours.SunClose = TimeToDateTime(sunClose, nextSunday);
                }
            }



            return hours;
        }

        private static DateTime TimeToDateTime(string time, DateTime date)
        {
            var hour = int.Parse(time.Substring(0, time.Length - 1));
            var aOrP = time.Substring(time.Length - 1);

            if (aOrP == "p" || aOrP == "P")
            {
                hour += 12;
            }

            return date.AddHours(hour);

        }


        private static SurveyTotal CombineSurveys(List<SurveyFilterModel> surveys)
        {
            var result = new SurveyTotal {DiataryIssues = 0};
            foreach (var s in surveys)
            {
                result.PossibleZips.AddRange(FindZipCodes(s.ZipCode, s.ZipCodeRadius));
                result.NotWantedCuisines.Add(s.CuisineNotWanted);
                result.WantedCuisines.Add(s.CuisineWanted);
                result.SuggestedResturantIds.Add(s.SuggestedResturantId);
                result.DiataryIssues = result.DiataryIssues | s.DiataryIssues;
            }

            result.ZipCodes = result.PossibleZips.GroupBy(z => z, z => z)
                .Where(g => g.Count() == surveys.Count()).Select(g => g.Key).ToList();

           
            
            
            //TODO: result.LunchTime complicated logic

            result.LunchTime = surveys.First().TimeAvailable;


            return result;
        }

        public static IEnumerable<string> FindZipCodes(string zipCode, int zipCodeRadius)
        {

            var client = new RestClient("http://www.zipcodeapi.com/rest/ye29DnmW3cFSb9nmn3EEm7qiaj1E2M6A18KEw7AzjgsupPQNTvsfCSyNbzsemzPn");

            var request = new RestRequest(
                $"/radius.json/{zipCode}/{zipCodeRadius}/mile", Method.GET);

            var response = client.Execute(request);

            var content = (JObject)JsonConvert.DeserializeObject(response.Content);


            var zips = content["zip_codes"].ToObject<List<ZipsFromAPI>>().Select(x=>x.zip_code);

            return zips;
        }

        public class ZipsFromAPI
        {
            public string zip_code { get; set; }
        }
    }

}