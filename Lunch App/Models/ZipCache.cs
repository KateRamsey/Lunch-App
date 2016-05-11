using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Lunch_App.Models
{
    public class ZipCache
    {
        public int Id { get; set; }
        public string Zip { get; set; }
        public int Radius { get; set; }
        public string ZipsInRadius { get; set; }
    }
}