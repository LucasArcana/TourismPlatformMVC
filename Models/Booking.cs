using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int TouristId { get; set; }
        public int TourScheduleId { get; set; }

        public int ParticipantsCount { get; set; }
        public int BookingStatus { get; set; }
        public int PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}