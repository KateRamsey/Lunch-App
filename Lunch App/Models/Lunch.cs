using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lunch_App.Models
{
    public class Lunch
    {
        public int Id { get; set; }
        public virtual List<LunchMembers> Members { get; set; } = new List<LunchMembers>();
        public virtual List<Survey> Surveys { get; set; } = new List<Survey>();
        public DateTime MeetingDateTime { get; set; }
        public virtual LunchUser Creator { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual List<RestaurantOptions> Options { get; set; } = new List<RestaurantOptions>();
    }

    public class RestaurantOptions
    {
        public int Id { get; set; }
        public Lunch Lunch { get; set; }
        public Restaurant Restaurant { get; set; }
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
        [Display(Name="Lunch Time:")]
        public DateTime MeetingTime { get; set; }

    }

    public class LunchVM
    {
        public int Id { get; set; }
        public List<UserVM> Members { get; set; } = new List<UserVM>();
        public DateTime MeetingDateTime { get; set; }
        public UserVM Creator { get; set; }
        public RestaurantVM Restaurant { get; set; }
    }

    public class LunchIndexVM
    {
        public int Id { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public string CreatorHandle { get; set; }
        public bool RestaurantSelected { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantLocation { get; set; }
    }

    public class LunchPickVM
    {
        public int Id { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public List<RestaurantPickVM> Picks { get; set; } = new List<RestaurantPickVM>();
        public int SelectedId { get; set; }
    }
}