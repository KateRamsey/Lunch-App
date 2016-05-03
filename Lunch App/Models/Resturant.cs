using System;
using System.Collections.Generic;

namespace Lunch_App.Models
{
    public class Resturant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string HoursOfOperation { get; set; }
        public int PriceRange { get; set; }
        public string Website { get; set; }
        public Cuisine CuisineType { get; set; }
        public int DietaryOptions { get; set; }

        public virtual List<LunchUser> Fans { get; set; } = new List<LunchUser>();
        public virtual List<Lunch> Lunches { get; set; } = new List<Lunch>();
    }

}


public enum Cuisine
{
    Mexican,
    Sushi,
    Pizza
}