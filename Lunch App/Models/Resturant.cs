using System.Collections.Generic;

namespace Lunch_App.Models
{
    public class Resturant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        // Hours of Operation
        public int PriceRange { get; set; }
        public string Website { get; set; }
        // Cusine Type
        public int DietaryOptions { get; set; }

        public virtual List<LunchUser> Fans { get; set; }
        public virtual List<Lunch> Lunches { get; set; }
    }
}