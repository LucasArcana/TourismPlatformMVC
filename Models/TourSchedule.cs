using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.Models
{
    public class TourSchedule
    {
        public int TourScheduleId { get; set; }

        public DateTime AvailableDate { get; set; }
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public int GroupSizeLimit { get; set; }

        public int TravelPackageId { get; set; }
    }
}
