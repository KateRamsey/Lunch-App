using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    public class ResturantCreateVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [Display(Name = "Zip Code")]
        public string LocationZip { get; set; }
        [Required]
        [Display(Name = "Hours of Operation")]
        public string HoursOfOperation { get; set; }
        [Display(Name = "Price Range")]
        public int PriceRange { get; set; }
        public string Website { get; set; }
        [Required]
        public Cuisine CuisineType { get; set; }
        public bool Vegan { get; set; }
        public bool Vegetarian { get; set; }
        public bool GlutenFree { get; set; }
        public bool NutAllergy { get; set; }
        public bool ShellFishAllergy { get; set; }
        public bool Kosher { get; set; }
        public bool Halaal { get; set; }
        public bool LactoseIntolerant { get; set; }

        public Resturant CreateResturant()
        {
            var resturant = new Resturant()
            {
                Name = this.Name,
                Location = this.Location,
                LocationZip = this.LocationZip,
                HoursOfOperation = this.HoursOfOperation,
                PriceRange = this.PriceRange,
                Website = this.Website,
                CuisineType = this.CuisineType,
            };
            resturant.DietaryOptions = GetDietaryOptions(this);

            return resturant;
        }

        private int GetDietaryOptions(ResturantCreateVM resturantCreateVm)
        {
            int dietaryOptions = 0;

            if (Vegan)
            {
                dietaryOptions += (int)DietaryIssues.Vegan;
            }
            if (Vegetarian)
            {
                dietaryOptions += (int)DietaryIssues.Vegetarian;
            }
            if (GlutenFree)
            {
                dietaryOptions += (int)DietaryIssues.GlutenFree;
            }
            if (NutAllergy)
            {
                dietaryOptions += (int)DietaryIssues.NutAllergy;
            }
            if (ShellFishAllergy)
            {
                dietaryOptions += (int)DietaryIssues.ShellFishAllergy;
            }
            if (Kosher)
            {
                dietaryOptions += (int)DietaryIssues.Kosher;
            }
            if (Halaal)
            {
                dietaryOptions += (int)DietaryIssues.Halaal;
            }
            if (LactoseIntolerant)
            {
                dietaryOptions += (int)DietaryIssues.LactoseIntolerant;
            }

            return dietaryOptions;
        }
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
        public ResturantPickVM()
        {
            
        }
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
            if ((DietaryOptions & (int)DietaryIssues.Vegan) == (int)DietaryIssues.Vegan)
            {
                Vegan = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Vegetarian) == (int)DietaryIssues.Vegetarian)
            {
                Vegetarian = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.GlutenFree) == (int)DietaryIssues.GlutenFree)
            {
                GlutenFree = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.NutAllergy) == (int)DietaryIssues.NutAllergy)
            {
                NutAllergy = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.ShellFishAllergy) == (int)DietaryIssues.ShellFishAllergy)
            {
                ShellFishAllergy = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Kosher) == (int)DietaryIssues.Kosher)
            {
                Kosher = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.Halaal) == (int)DietaryIssues.Halaal)
            {
                Halaal = true;
            }
            if ((DietaryOptions & (int)DietaryIssues.LactoseIntolerant) == (int)DietaryIssues.LactoseIntolerant)
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

    public class ResturantDetailVM
    {
        public ResturantDetailVM()
        {

        }

        public ResturantDetailVM(Resturant r)
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

            this.SetDietaryBools();
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

        private void SetDietaryBools()
        {
            if ((DietaryOptions & (int) DietaryIssues.Vegan) == (int)DietaryIssues.Vegan)
            {
                Vegan = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.Vegetarian) == (int)DietaryIssues.Vegetarian)
            {
                Vegetarian = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.GlutenFree) == (int)DietaryIssues.GlutenFree)
            {
                GlutenFree = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.NutAllergy) == (int)DietaryIssues.NutAllergy)
            {
                NutAllergy = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.ShellFishAllergy) == (int)DietaryIssues.ShellFishAllergy)
            {
                ShellFishAllergy = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.Kosher) == (int)DietaryIssues.Kosher)
            {
                Kosher = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.Halaal) == (int)DietaryIssues.Halaal)
            {
                Halaal = true;
            }
            if ((DietaryOptions & (int) DietaryIssues.LactoseIntolerant) == (int)DietaryIssues.LactoseIntolerant)
            {
                LactoseIntolerant = true;
            }
        }
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
    None,
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