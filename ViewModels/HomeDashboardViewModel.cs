using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.ViewModels
{
    public class HomeDashboardViewModel
    {
        // Keep the assessment requirement visible on the homepage
        public List<GroupMemberViewModel> GroupMembers { get; set; } = new List<GroupMemberViewModel>();

        // Dashboard sections
        public List<FeaturedPackageCardVm> FeaturedPackages { get; set; } = new List<FeaturedPackageCardVm>();
        public List<UpcomingScheduleRowVm> UpcomingSchedules { get; set; } = new List<UpcomingScheduleRowVm>();

        // Optional: so the view can show "no upcoming tours" nicely with today's context
        public DateTime Today { get; set; } = DateTime.Today;
    }

    public class FeaturedPackageCardVm
    {
        public int TravelPackageId { get; set; }
        public string Name { get; set; }
        public string Destination { get; set; }
        public decimal Price { get; set; }
    }

    public class UpcomingScheduleRowVm
    {
        public int TourScheduleId { get; set; }
        public DateTime AvailableDate { get; set; }
        public int DurationDays { get; set; }
        public int GroupSizeLimit { get; set; }
        public decimal Price { get; set; }

        public int TravelPackageId { get; set; }
        public string PackageName { get; set; }
        public string Destination { get; set; }
    }
}