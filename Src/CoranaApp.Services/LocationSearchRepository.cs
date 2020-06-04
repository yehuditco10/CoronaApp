using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

using CoronaApp.Entities;
using System.Linq;

namespace CoronaApp.Services
{
    public class LocationSearchRepository : ILocationSearchRepository
    {
        //public ICollection<LocationModel> Get(LocationSearchModel locationSearchModel)
        //{
        //    try
        //    {
        //        using (CoronaContext coronaContext = new CoronaContext())
        //        {
        //            if (locationSearchModel.age != 0)
        //            {
        //                List<Patient> patients = coronaContext.Patients.Where(p => p.age == locationSearchModel.age).ToList();
        //                if (patients.Count() > 0)
        //                {
        //                    List<LocationModel> locationsByAge = new List<LocationModel>();
        //                    foreach (var patient in patients)
        //                    {
        //                        List<Location> locations = coronaContext.Locations.Where(l => l.patientId == patient.id).ToList();
        //                        if (locations.Count() > 0)
        //                        {
        //                            LocationModel locationModel = new LocationModel();
        //                            locationsByAge.AddRange(locationModel.ToLocationModel(locations));
        //                        }
        //                    }
        //                    return locationsByAge;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.ToString());
        //    }
        //    throw new Exception("No locations");
        //}
        //public ICollection<LocationModel> GetByDate(LocationSearchModel criteria)
        //{
        //    if (criteria.startDate < DateTime.MinValue && criteria.endDate >= DateTime.Now)//valid dates.
        //    { throw new Exception("One or more of the dates are not valid."); }

        //    try
        //    {
        //        using (CoronaContext coronaContext = new CoronaContext())
        //        {
        //            List<LocationModel> returnObj = new List<LocationModel>();
        //            List<Location> matchLocations = coronaContext.Locations.Where
        //                (location => !(criteria.endDate <= location.startDate && criteria.startDate <= location.endDate)).ToList();//-------------------not perfect
        //            if (matchLocations.Count() > 0)
        //            {
        //                LocationModel locationModel = new LocationModel();
        //                returnObj.AddRange(locationModel.ToLocationModel(matchLocations));
        //            }
        //            return returnObj;
        //        }


        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.ToString());
        //    }
        //}
        public ICollection<Models.Location> Get(Models.LocationSearch locationSearch)
        {
            throw new NotImplementedException();
        }

        public ICollection<Models.Location> GetByDate(Models.LocationSearch criteria)
        {
            throw new NotImplementedException();
        }
    }
}
