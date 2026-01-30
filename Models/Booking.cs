using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TourismPlatformMVC.Models
{
    public enum BookingStatusEnum { Pending = 0, Confirmed = 1, Completed = 2, Cancelled = 3 }
    public enum PaymentStatusEnum { Unpaid = 0, Paid = 1, Refunded = 2 }

    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        public int TouristId { get; set; }

        [Required]
        public int TourScheduleId { get; set; }

        [Range(1, 100)]
        public int ParticipantsCount { get; set; }

        [Required]
        public BookingStatusEnum BookingStatus { get; set; }

        [Required]
        public PaymentStatusEnum PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}