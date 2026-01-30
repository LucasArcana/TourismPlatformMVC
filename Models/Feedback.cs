using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TourismPlatformMVC.Models
{
    public class Feedback
    {
        [Key]
        public int BookingId { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}