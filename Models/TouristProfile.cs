using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TourismPlatformMVC.Models
{
    public class TouristProfile
    {
        [Key]
        public int TouristId { get; set; }

        public string FullName { get; set; }
        public string ContactNumber { get; set; }

        public string UserId { get; set; }
    }

}