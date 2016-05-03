﻿using System;
using System.Collections.Generic;

namespace Lunch_App.Models
{
    public class Lunch
    {
        public int Id { get; set; }
        public virtual List<LunchMembers> Members { get; set; }
        public virtual List<Survey> Surveys { get; set; }
        public DateTime MeetingDateTime { get; set; }
        public LunchUser Creator { get; set; }
        public virtual Resturant Resturant { get; set; }
        public virtual List<ResturantOptions> Options { get; set; }
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
        public LunchUser Member { get; set; }
        public Lunch Lunch { get; set; }
        public DateTime InvitedTime { get; set; }
    }
}