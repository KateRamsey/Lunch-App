using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch_App.Models
{
    public class IndexVM
    {
        public List<LunchVM> Lunches { get; set; } = new List<LunchVM>();
        public List<int> OutstandingSurveys { get; set; } = new List<int>();
        public DateTime NextLunch { get; set; }
        public bool WaitingOnSurveys { get; set; }
        public List<int> LunchReadyToPick { get; set; }

        public IEnumerable<UserVM> Buddies { get; set; } = new List<UserVM>();
        public IEnumerable<ResturantVM> FavoriteResturants { get; set; } = new List<ResturantVM>();
    }
}