using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CoronaApp.Dal;
using System.Linq;

namespace CoronaApp.Services
{
    public class LocationSearchRepository : ILocationSearchRepository
    {
        private readonly CoronaContext _coronaContext;
        public LocationSearchRepository(CoronaContext coronaContext)
        {
            _coronaContext = coronaContext;
        }
        public ICollection<Location> Get(LocationSearch locationSearchModel)
        {
            try
            {
                using (CoronaContext coronaContext = new CoronaContext())
                {
                    if (locationSearchModel.age != 0)
                    {
                        List<Patient> patients = coronaContext.Patients.Where(p => p.age == locationSearchModel.age).ToList();
                        if (patients.Count() > 0)
                        {
                            List<Location> locationsByAge = new List<Location>();
                            foreach (var patient in patients)
                            {
                                List<Location> locations = coronaContext.Locations.Where(l => l.patientId == patient.id).ToList();
                                if (locations.Count() > 0)
                                {
                                    Location locationModel = new Location();
                                    locationsByAge.AddRange(locations);
                                }
                            }
                            return locationsByAge;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            throw new Exception("No locations");
        }
        public ICollection<Models.Location> GetByDate(Models.LocationSearch criteria)
        {
            if (criteria.startDate < DateTime.MinValue && criteria.endDate >= DateTime.Now)//valid dates.
            { throw new Exception("One or more of the dates are not valid."); }

            try
            {
                using (CoronaContext coronaContext = new CoronaContext())
                {
                    List<Location> returnObj = new List<Models.Location>();
                    List<Location> matchLocations = coronaContext.Locations.Where
                        (location => !(criteria.endDate <= location.startDate && criteria.startDate <= location.endDate)).ToList();//-------------------not perfect
                    if (matchLocations.Count() > 0)
                    {
                        Models.Location locationModel = new Models.Location();
                        returnObj.AddRange(locationModel.ToLocationModel(matchLocations));
                    }
                    return returnObj;
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
    }
}
