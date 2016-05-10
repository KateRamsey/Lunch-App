using System;

namespace Lunch_App.Models
{
    public class Survey
    {
        public int Id { get; set; }
        public virtual LunchUser User { get; set; }
        public virtual Lunch Lunch { get; set; }
        public bool IsFinished { get; set; }
        public bool IsComing { get; set; }
        public DateTime TimeAvailable { get; set; } = DateTime.Now;
        public int MinutesAvailiable { get; set; }
        public string ZipCode { get; set; }
        public int ZipCodeRadius { get; set; }
        public Cuisine CuisineWanted { get; set; }
        public Cuisine CuisineNotWanted { get; set; }
        public virtual Resturant SuggestedResturant { get; set; }
        public int DietaryIssues { get; set; }
    }

    public class SurveyFilterModel
    {

        public SurveyFilterModel(Survey s)
        {
            TimeAvailable = s.TimeAvailable;
            MinutesAvailiable = s.MinutesAvailiable;
            ZipCode = s.ZipCode;
            ZipCodeRadius = s.ZipCodeRadius;
            CuisineWanted = s.CuisineWanted;
            CuisineNotWanted = s.CuisineNotWanted;
            if (s.SuggestedResturant != null)
            {
                SuggestedResturantId = s.SuggestedResturant.Id;
            }
            DietaryIssues = s.DietaryIssues;
            IsComing = s.IsComing;
        }

        public bool IsComing { get; set; }
        public DateTime TimeAvailable { get; set; }
        public int MinutesAvailiable { get; set; }
        public string ZipCode { get; set; }
        public int ZipCodeRadius { get; set; }
        public Cuisine CuisineWanted { get; set; }
        public Cuisine CuisineNotWanted { get; set; }
        public int SuggestedResturantId { get; set; }
        public int DietaryIssues { get; set; }
    }

    public enum DietaryIssues
    {
        Vegan = 1,
        Vegetarian = 2,
        GlutenFree = 4,
        NutAllergy = 8,
        ShellFishAllergy = 16,
        Kosher = 32,
        Halaal = 64,
        LactoseIntolerant = 128
    }


    public class SurveyEditVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int LunchId { get; set; }
        public bool IsComing { get; set; }
        public DateTime TimeAvailable { get; set; } = DateTime.Now;
        public int MinutesAvailiable { get; set; } = 10000;
        public string ZipCode { get; set; } = "72120";
        public int ZipCodeRadius { get; set; } = 100;
        public Cuisine CuisineWanted { get; set; }
        public Cuisine CuisineNotWanted { get; set; }
        public int SuggestedResturantId { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool GlutenFree { get; set; }
        public bool NutAllergy { get; set; }
        public bool ShellFishAllergy { get; set; }
        public bool Kosher { get; set; }
        public bool Halaal { get; set; }
        public bool LactoseIntolerant { get; set; }
    }
}