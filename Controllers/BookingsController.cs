using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TourismPlatformMVC.Controllers
{
    public class BookingsController : Controller
    {
        // GET: Bookings
        public ActionResult MyBookings()
        {
            return View();
        }
    }
}