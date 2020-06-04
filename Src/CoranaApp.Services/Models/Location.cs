using CoronaApp.Entities;
using System;
using System.Collections.Generic;

namespace CoronaApp.Services.Models
{
    public class Location
    {
        public int id { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string city { get; set; }
        public string location { get; set; }
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
        public Location ToLocation(Location locationModel)
        {
            return new Location()
            {
                startDate = locationModel.startDate,
                endDate = locationModel.endDate,
                city = locationModel.city,
                location = locationModel.location,
                patientId = locationModel.patientId
            };
        }
        public Location ToLocationModel(Location location)
        {
            return new Location()
            {
                startDate = location.startDate,
                endDate = location.endDate,
                city = location.city,
                location = location.location,
                patientId = location.patientId
            };
        }

        public List<Location> ToLocation(List<Location> locationsModel)
        {
            List<Location> locations = new List<Location>();
            foreach (var location in locationsModel)
            {
                locations.Add(ToLocation(location));
            }
            return locations;
        }

        public List<Location> ToLocationModel(List<Location> locations)
        {
            List<Location> locationsModel = new List<Location>();
            foreach (var location in locations)
            {
                locationsModel.Add(ToLocationModel(location));
            }
            return locationsModel; 
        }
    }
}