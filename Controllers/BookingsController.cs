using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TourismPlatformMVC.Models;

namespace TourismPlatformMVC.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        // (optional: keep public or restrict to admins later)
        public ActionResult Index()
        {
            return View(db.Bookings.OrderByDescending(b => b.CreatedAt).ToList());
        }

        // GET: Bookings/MyBookings
        [Authorize]
        public ActionResult MyBookings()
        {
            var userId = User.Identity.GetUserId();

            var tourist = db.TouristProfiles.FirstOrDefault(t => t.UserId == userId);
            if (tourist == null)
            {
                TempData["Error"] = "Tourist profile not found. Please create your tourist profile first.";
                return RedirectToAction("Index", "Home");
            }

            var myBookings = db.Bookings
                .Where(b => b.TouristId == tourist.TouristId)
                .OrderByDescending(b => b.CreatedAt)
                .ToList();

            return View(myBookings);
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            return View(booking);
        }

        // GET: Bookings/Create?tourScheduleId=2
        [Authorize]
        public ActionResult Create(int? tourScheduleId)
        {
            // Find or create TouristProfile for logged-in user (quick + functional)
            var userId = User.Identity.GetUserId();
            var tourist = db.TouristProfiles.FirstOrDefault(t => t.UserId == userId);

            if (tourist == null)
            {
                tourist = new TouristProfile
                {
                    UserId = userId,
                    FullName = User.Identity.Name,
                    ContactNumber = "" // if your model requires it, put something simple
                };

                db.TouristProfiles.Add(tourist);
                db.SaveChanges();
            }

            // Build schedule dropdown with nice text (package + destination + date)
            var scheduleList = db.TourSchedules
                .Join(db.TravelPackages,
                    s => s.TravelPackageId,
                    p => p.TravelPackageId,
                    (s, p) => new { s. TourScheduleId, p.Name, p.Destination, s.AvailableDate })
                    .OrderBy(x => x.AvailableDate)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.TourScheduleId,
                        Text = $"{x.Name} ({x.Destination}) - {x.AvailableDate.ToString("dd MMM yyyy")}"
                    }).ToList();

            ViewBag.TourScheduleId = new SelectList(scheduleList, "TourScheduleId", "Text", tourScheduleId);

            // Defaults (don’t let user type these)
            var booking = new Booking
            {
                TouristId = tourist.TouristId,
                TourScheduleId = tourScheduleId ?? 0,
                ParticipantsCount = 1,
                BookingStatus = BookingStatusEnum.Pending,
                PaymentStatus = PaymentStatusEnum.Unpaid,
                CreatedAt = DateTime.Now
            };

            return View(booking);
        }

        // POST: Bookings/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourScheduleId,ParticipantsCount")] Booking booking)
        {
            var userId = User.Identity.GetUserId();
            var tourist = db.TouristProfiles.FirstOrDefault(t => t.UserId == userId);

            if (tourist == null)
            {
                TempData["Error"] = "Tourist profile not found. Please create your tourist profile first.";
                return RedirectToAction("Index", "Home");
            }

            // Force safe values (never trust the form for these)
            booking.TouristId = tourist.TouristId;
            booking.BookingStatus = BookingStatusEnum.Pending;
            booking.PaymentStatus = PaymentStatusEnum.Unpaid;
            booking.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                TempData["Success"] = "Booking created successfully!";
                return RedirectToAction("MyBookings");
            }

            // Rebuild dropdown if invalid (same text list)
            var scheduleList = (from s in db.TourSchedules
                                join p in db.TravelPackages on s.TravelPackageId equals p.TravelPackageId
                                orderby s.AvailableDate
                                select new
                                {
                                    s.TourScheduleId,
                                    Text = p.Name + " (" + p.Destination + ") - " + s.AvailableDate.ToString("dd MMM yyyy")
                                }).ToList();

            ViewBag.TourScheduleId = new SelectList(scheduleList, "TourScheduleId", "Text", booking.TourScheduleId);

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            return View(booking);
        }

        // POST: Bookings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookingId,TouristId,TourScheduleId,ParticipantsCount,BookingStatus,PaymentStatus,CreatedAt")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var booking = db.Bookings.Find(id);
            if (booking == null) return HttpNotFound();

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
