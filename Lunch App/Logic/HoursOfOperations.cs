using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lunch_App.Logic
{
    public class HoursOfOperations
    {
        public static IEnumerable<HoursOfOperations> Parse(string s)
        {
            //Examples 
            //Monday 8am-2pm
            //Tuesday-Saturday 10pm-11pm
            //Friday-Saturday 11am-3pm, Sunday 11am-3pm
            var split = s.Split(' ');

            var times = split[1].Split('-');
            var formats = new[] { "htt", "h:mmtt" };
            var begin = DateTime.ParseExact(times[0], formats, CultureInfo.InvariantCulture,
                DateTimeStyles.NoCurrentDateDefault);
            var end = DateTime.ParseExact(times[1], formats, CultureInfo.InvariantCulture,
                DateTimeStyles.NoCurrentDateDefault);


            var results = new List<HoursOfOperations>();
            if (split[0].Contains("-"))
            {
                var dowrange = split[0].Split('-');
                DayOfWeek startdow;
                Enum.TryParse(dowrange[0], out startdow);
                DayOfWeek enddow;
                Enum.TryParse(dowrange[1], out enddow);

                for (DayOfWeek i = startdow; i <= enddow; i++)
                {
                    results.Add(new HoursOfOperations() { Close = end, Open = begin, DayOfWeek = i });
                }
            }
            else
            {

                DayOfWeek dow;
                Enum.TryParse(split[0], out dow);
                results.Add(new HoursOfOperations() {Close = end, Open= begin, DayOfWeek = dow});
            }

            return results;
        }

        public DayOfWeek DayOfWeek { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }

        
    }
}