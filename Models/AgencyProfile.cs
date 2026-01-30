using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

    namespace TourismPlatformMVC.Models
    {


    public class AgencyProfile
    {
        [Key]
        public int AgencyId { get; set; }         

        public string AgencyName { get; set; }
        public string ServicesOffered { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }        
    }

}