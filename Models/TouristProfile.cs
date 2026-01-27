using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.Models
{
    public class TouristProfile
    {
        public int TouristId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}