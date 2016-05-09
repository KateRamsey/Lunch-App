using System;
using System.Collections.Generic;

namespace Lunch_App.Models
{
    public class Lunch
    {
        public int Id { get; set; }
        public virtual List<LunchMembers> Members { get; set; } = new List<LunchMembers>();
        public virtual List<Survey> Surveys { get; set; } = new List<Survey>();
        public DateTime MeetingDateTime { get; set; }
        public virtual LunchUser Creator { get; set; }
        public virtual Resturant Resturant { get; set; }
        public virtual List<ResturantOptions> Options { get; set; } = new List<ResturantOptions>();
    }

    public class ResturantOptions
    {
        public int Id { get; set; }
        public Lunch Lunch { get; set; }
        public Resturant Resturant { get; set; }
        public int Rank { get; set; }
    }

    public class LunchMembers
    {
        public int Id { get; set; }
       // public string MemberId { get; set; }
        public virtual LunchUser Member { get; set; }
       // public int LunchId { get; set; }
        public virtual Lunch Lunch { get; set; }
        public DateTime InvitedTime { get; set; }
    }

    public class LunchCreationVM
    {
        public List<UserVM> Members { get; set; } = new List<UserVM>();
        public DateTime MeetingTime { get; set; }

    }

    public class LunchVM
    {
        public int Id { get; set; }
        public List<UserVM> Members { get; set; } = new List<UserVM>();
        public DateTime MeetingDateTime { get; set; }
        public UserVM Creator { get; set; }
        public ResturantVM Resturant { get; set; }
    }
}