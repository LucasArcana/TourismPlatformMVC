using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.Models
{
    public class TravelPackage
    { 
        public int TravelPackageId { get; set; }
        public string Name { get; set; }

        public string Destination { get; set; }
        public decimal Price { get; set; }

        public int AgencyId { get; set; }

    }
}