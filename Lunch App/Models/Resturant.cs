using System;
using System.Collections.Generic;

namespace Lunch_App.Models
{
    public class Resturant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string LocationZip { get; set; }
        public string HoursOfOperation { get; set; }
        public int PriceRange { get; set; }
        public string Website { get; set; }
        public Cuisine CuisineType { get; set; }
        public int DietaryOptions { get; set; }

        public virtual List<LunchUser> Fans { get; set; } = new List<LunchUser>();
        public virtual List<Lunch> Lunches { get; set; } = new List<Lunch>();
    }

    public class ResturantVM
    {
        public ResturantVM(Resturant r)
        {
            Id = r.Id;
            Name = r.Name;
            LocationZip = r.LocationZip;
            Location = r.Location;
            HoursOfOperation = r.HoursOfOperation;
            PriceRange = r.PriceRange;
            Website = r.Website;
            CuisineType = r.CuisineType;
            DietaryOptions = r.DietaryOptions;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string LocationZip { get; set; }
        public string HoursOfOperation { get; set; }
        public int PriceRange { get; set; }
        public string Website { get; set; }
        public Cuisine CuisineType { get; set; }
        public int DietaryOptions { get; set; }
    }

    public class ResturantPickVM
    {
        public ResturantPickVM(Resturant r)
        {
            Id = r.Id;
            Name = r.Name;
            LocationZip = r.LocationZip;
            Location = r.Location;
            HoursOfOperation = r.HoursOfOperation;
            PriceRange = r.PriceRange;
            Website = r.Website;
            CuisineType = r.CuisineType;

            DietaryOptions = r.DietaryOptions;
            SetDietaryBools();
        }

        private void SetDietaryBools()
        {
            if ((DietaryOptions & (int)DietaryIssues.Vegan) == DietaryOptions)
            {
                Vegan = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Vegetarian) == DietaryOptions)
            {
                Vegetarian = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.GlutenFree) == DietaryOptions)
            {
                GlutenFree = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.NutAllergy) == DietaryOptions)
            {
                NutAllergy = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.ShellFishAllergy) == DietaryOptions)
            {
                ShellFishAllergy = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Kosher) == DietaryOptions)
            {
                Kosher = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Halaal) == DietaryOptions)
            {
                Halaal = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.LactoseIntolerant) == DietaryOptions)
            {
                LactoseIntolerant = true;
            }
        }


        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string LocationZip { get; set; }
        public string HoursOfOperation { get; set; }
        public int PriceRange { get; set; }
        public string Website { get; set; }
        public Cuisine CuisineType { get; set; }
        public int DietaryOptions { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool GlutenFree { get; set; }
        public bool NutAllergy { get; set; }
        public bool ShellFishAllergy { get; set; }
        public bool Kosher { get; set; }
        public bool Halaal { get; set; }
        public bool LactoseIntolerant { get; set; }
        public bool Selected { get; set; }
    }


    public class ResturantFilterModel
    {
        public ResturantFilterModel(Resturant r)
        {
            Id = r.Id;
            LocationZip = r.LocationZip;
            HoursOfOperation = r.HoursOfOperation;
            PriceRange = r.PriceRange;
            CuisineType = r.CuisineType;
            DietaryOptions = r.DietaryOptions;
        }

        public int Id { get; set; }
        public string LocationZip { get; set; }
        public string HoursOfOperation { get; set; }
        public int PriceRange { get; set; }
        public Cuisine CuisineType { get; set; }
        public int DietaryOptions { get; set; }
        public int Score { get; set; }
    }
}


public enum Cuisine
{
    Mexican,
    Sushi,
    Pizza,
    Cajun,
    American,
    Chinese,
    Italian,
    Sandwitch,
    Japanese,
    Seafood,
    Thai,
    BBQ,
    Steak,
    Buffet,
    Cuban,
    Southern,
    MiddleEastern,
    Mediterranean,
    Breakfast
}