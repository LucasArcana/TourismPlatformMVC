using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TourismPlatformMVC.Models;

namespace TourismPlatformMVC.Controllers
{
    public class BookingsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bookings
        public ActionResult Index()
        {
            return View(db.Bookings.ToList());
        }

        // GET: Bookings/MyBookings
        public ActionResult MyBookings()
        {
            // prototype: just reuse Index for now
            return RedirectToAction("Index");
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create(int? tourScheduleId)
        {
            ViewBag.TouristId = new SelectList(db.TouristProfiles, "TouristId", "FullName");
            ViewBag.TourScheduleId = new SelectList(db.TourSchedules.OrderBy(s => s.AvailableDate),
                                                    "TourScheduleId", "TourScheduleId",
                                                    tourScheduleId);

            var booking = new Booking
            {
                TourScheduleId = tourScheduleId ?? 0,
                ParticipantsCount = 1,
                BookingStatus = BookingStatusEnum.Pending,
                PaymentStatus = PaymentStatusEnum.Unpaid,
                CreatedAt = DateTime.Now
            };

            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TouristId,TourScheduleId,ParticipantsCount,BookingStatus,PaymentStatus")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                booking.CreatedAt = DateTime.Now;
                db.Bookings.Add(booking);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TouristId = new SelectList(db.TouristProfiles, "TouristId", "FullName", booking.TouristId);
            ViewBag.TourScheduleId = new SelectList(db.TourSchedules, "TourScheduleId", "TourScheduleId", booking.TourScheduleId);

            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
