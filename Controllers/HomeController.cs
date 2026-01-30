using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TourismPlatformMVC.Models;
using TourismPlatformMVC.ViewModels;

namespace TourismPlatformMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var vm = new HomeDashboardViewModel();

            // Assessment requirement: group members table
            vm.GroupMembers = new List<GroupMemberViewModel>
            {
                new GroupMemberViewModel { StudentId = "20021750", FullName = "Marcos Yukihiro Vieira Yamashita" },
                new GroupMemberViewModel { StudentId = "20028065", FullName = "GURJOT SINGH" },
                new GroupMemberViewModel { StudentId = "20032744", FullName = "Aven Matthew MAJELLANO" }
            };

            // Featured packages (simple: latest 6)
            vm.FeaturedPackages = db.TravelPackages
                .OrderByDescending(p => p.TravelPackageId)
                .Take(6)
                .Select(p => new FeaturedPackageCardVm
                {
                    TravelPackageId = p.TravelPackageId,
                    Name = p.Name,
                    Destination = p.Destination,
                    Price = p.Price
                })
                .ToList();

            // Upcoming schedules (join schedules -> packages since you don’t have nav properties yet)
            var today = DateTime.Today;

            vm.UpcomingSchedules = (from s in db.TourSchedules
                                    join p in db.TravelPackages on s.TravelPackageId equals p.TravelPackageId
                                    where s.AvailableDate >= today
                                    orderby s.AvailableDate ascending
                                    select new UpcomingScheduleRowVm
                                    {
                                        TourScheduleId = s.TourScheduleId,
                                        AvailableDate = s.AvailableDate,
                                        DurationDays = s.DurationDays,
                                        GroupSizeLimit = s.GroupSizeLimit,
                                        Price = s.Price,
                                        TravelPackageId = p.TravelPackageId,
                                        PackageName = p.Name,
                                        Destination = p.Destination
                                    })
                                    .Take(10)
                                    .ToList();

            vm.Today = today;

            return View(vm);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LoginSuccess()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
        [Authorize]

        [Authorize]
        public ActionResult SeedDemoData()
        {
#if !DEBUG
    return HttpNotFound();
#endif

            // Ensure we have at least one agency to attach packages to
            var agencyId = db.AgencyProfiles.Select(a => a.AgencyId).FirstOrDefault();

            if (agencyId == 0)
            {
                // Create a simple agency if none exist
                var agency = new AgencyProfile
                {
                    AgencyName = "Demo Agency",
                    ServicesOffered = "Guided tours, day trips",
                    Description = "Demo agency profile for seeding data",
                    UserId = User.Identity.Name // quick + safe for now
                };

                db.AgencyProfiles.Add(agency);
                db.SaveChanges();
                agencyId = agency.AgencyId;
            }

            // 1) Seed travel packages if none exist
            if (!db.TravelPackages.Any())
            {
                var p1 = new TravelPackage
                {
                    Name = "Sydney Harbour Highlights",
                    Destination = "Sydney",
                    Price = 199,
                    AgencyId = agencyId
                };

                var p2 = new TravelPackage
                {
                    Name = "Blue Mountains Day Trip",
                    Destination = "Katoomba",
                    Price = 249,
                    AgencyId = agencyId
                };

                db.TravelPackages.Add(p1);
                db.TravelPackages.Add(p2);
                db.SaveChanges();
            }

            // 2) Seed schedules if none exist (safe even if only 1 package exists)
            if (!db.TourSchedules.Any())
            {
                var packages = db.TravelPackages.Take(2).ToList();

                for (int i = 0; i < packages.Count; i++)
                {
                    db.TourSchedules.Add(new TourSchedule
                    {
                        TravelPackageId = packages[i].TravelPackageId,
                        AvailableDate = DateTime.Today.AddDays(7 + (i * 7)),
                        DurationDays = 1,
                        GroupSizeLimit = (i == 0) ? 20 : 15,
                        Price = packages[i].Price
                    });
                }

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
