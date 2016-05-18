using System;
using System.ComponentModel.DataAnnotations;

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
        public ZipCodeRadiusOption ZipCodeRadius { get; set; }
        public Cuisine CuisineWanted { get; set; }
        public Cuisine CuisineNotWanted { get; set; }
        public virtual Resturant SuggestedResturant { get; set; }
        public int DietaryIssues { get; set; }
    }

    public enum ZipCodeRadiusOption
    {
        Five = 5,
        Fifteen = 15,
        Thirty = 30
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
        public ZipCodeRadiusOption ZipCodeRadius { get; set; }
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
        [Display(Name = "Are you coming to lunch? Leave blank if you can't make it")]
        public bool IsComing { get; set; }
        public DateTime TimeAvailable { get; set; } = DateTime.Now;
        public int MinutesAvailiable { get; set; } = 10000;

        [Display(Name = "What zip code are you coming from?")]
        [Required]
        public string ZipCode { get; set; } = "72120";

        [Display(Name = "How many miles can you travel?")]
        [Required]
        [Range(5, int.MaxValue, ErrorMessage = "Select a range")]
        public ZipCodeRadiusOption ZipCodeRadius { get; set; } = ZipCodeRadiusOption.Thirty;
        [Display(Name = "What type of food do you want to eat?")]
        public Cuisine CuisineWanted { get; set; }
        [Display(Name = "Anything you don't want to eat?")]
        public Cuisine CuisineNotWanted { get; set; }
        [Display(Name = "Is there a particular place that you want to go?")]
        public int SuggestedResturantId { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        [Display(Name = "Gluten Free")]
        public bool GlutenFree { get; set; }
        [Display(Name = "Nut Allergy")]
        public bool NutAllergy { get; set; }
        [Display(Name = "Shellfish Allergy")]
        public bool ShellFishAllergy { get; set; }
        public bool Kosher { get; set; }
        public bool Halaal { get; set; }
        public bool LactoseIntolerant { get; set; }
    }
}