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
    public class TourSchedulesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TourSchedules
        public ActionResult Index(int? travelPackageId)
        {
            var query = db.TourSchedules.AsQueryable();

            if (travelPackageId.HasValue)
            {
                query = query.Where(s => s.TravelPackageId == travelPackageId.Value);
            }

            var list = query
                .OrderBy(s => s.AvailableDate)
                .ToList();

            ViewBag.TravelPackageId = travelPackageId;
            return View(list);
        }

        // GET: TourSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourSchedule tourSchedule = db.TourSchedules.Find(id);
            if (tourSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tourSchedule);
        }

        // GET: TourSchedules/Create
        public ActionResult Create()
        {
            ViewBag.TravelPackageId = new SelectList(db.TravelPackages, "TravelPackageId", "Name");
            return View();
        }


        // POST: TourSchedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TourScheduleId,AvailableDate,DurationDays,Price,GroupSizeLimit,TravelPackageId")] TourSchedule tourSchedule)
        {
            if (ModelState.IsValid)
            {
                db.TourSchedules.Add(tourSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tourSchedule);
        }

        // GET: TourSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourSchedule tourSchedule = db.TourSchedules.Find(id);
            if (tourSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tourSchedule);
        }

        // POST: TourSchedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourScheduleId,AvailableDate,DurationDays,Price,GroupSizeLimit,TravelPackageId")] TourSchedule tourSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tourSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tourSchedule);
        }

        // GET: TourSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourSchedule tourSchedule = db.TourSchedules.Find(id);
            if (tourSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tourSchedule);
        }

        // POST: TourSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TourSchedule tourSchedule = db.TourSchedules.Find(id);
            db.TourSchedules.Remove(tourSchedule);
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
