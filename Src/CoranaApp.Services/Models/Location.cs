using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoronaApp.Services.Models
{
    public class Location
    {
        [Required]
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string city { get; set; }
        public string location { get; set; }
      [Required]
        public string patientId { get; set; }

        public Location(string city, DateTime startDate, DateTime endDate, string location, string patientId)
        {
            this.city = city;
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
            this.patientId = patientId;
        }
        public Location()
        {

        }
    }
}