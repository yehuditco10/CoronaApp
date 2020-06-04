using CoronaApp.Entities;
using System;
using System.Collections.Generic;

namespace CoronaApp.Services.Models
{
    public class LocationSearch
    {
        public int id { get; set; }
        public string city { get; set; }
        public int age { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public LocationSearch(string city, int age, DateTime startDate, DateTime endDate)
        {
            this.endDate = endDate;
            this.startDate = startDate;
            this.city = city;
            this.age = age;
        }
        public LocationSearch()
        {

        }
        public LocationSearch ToLocationSearch(LocationSearch locationSearchModel)
        {
            return new LocationSearch()
            {
                city = locationSearchModel.city,
                age = locationSearchModel.age,
                startDate = locationSearchModel.startDate,
                endDate = locationSearchModel.endDate
            };
        }
        public LocationSearch ToLocationSearchModel(LocationSearch locationSearch)
        {
            return new LocationSearch()
            {
                city = locationSearch.city,
                age = locationSearch.age,
                endDate = locationSearch.endDate,
                startDate = locationSearch.startDate
            };
        }
        public List<LocationSearch> ToLocationSearch(List<LocationSearch> locationSearchModel)
        {
            List<LocationSearch> returnObj = new List<LocationSearch>();
            LocationSearch searchModel = new LocationSearch();
            locationSearchModel.ForEach(l => returnObj.Add(searchModel.ToLocationSearch(l)));
            return returnObj;
        }
        public List<LocationSearch> ToLocationSearchModel(List<LocationSearch> locationSearch)
        {
            List<LocationSearch> returnObj = new List<LocationSearch>();
            LocationSearch  location = new LocationSearch();
            locationSearch.ForEach(l => returnObj.Add(location.ToLocationSearchModel(l)));
            return returnObj;
        }
    }
}