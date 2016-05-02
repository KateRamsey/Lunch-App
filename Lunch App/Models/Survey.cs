using System;

namespace Lunch_App.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public virtual LunchUser User { get; set; }
        public virtual Lunch Lunch { get; set; }
        public bool IsFinished { get; set; }
        public DateTime TimeAvailable { get; set; }
        public int MinutesAvailiable { get; set; }
        public int ZipCode { get; set; }
        public int ZipCodeRadius { get; set; }
        //CusineType wanted
        //CusineType not wanted
        public Resturant SuggestedResturant { get; set; }
        public int DiataryIssues { get; set; }
    }
}