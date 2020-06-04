
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoronaApp.Dal;
using CoronaApp.Services.Models;
namespace CoronaApp.Services
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CoronaContext _coronaContext;
        public LocationRepository(CoronaContext _coronaContext)
        {
            this._coronaContext = _coronaContext;
        }
        public ICollection<Location> Get(LocationSearch locationSearch = null)
        {
            try
            {
                var list = _coronaContext.Locations.ToList();
                if (locationSearch.city != null && locationSearch.city != "All" && locationSearch.city != "")
                {
                    list = list.Where(c => c.city == locationSearch.city).ToList();
                }
                if (locationSearch.age != 0 && list.Count > 0)
                {
                    List<Patient> patients = _coronaContext.Patients.Where(p => p.age == locationSearch.age).ToList();
                    if (patients.Count() > 0)
                    {
                        List<Location> locationsByAge = new List<Location>();
                        foreach (var patient in patients)
                        {
                            locationsByAge.AddRange(list.Where(l => l.patientId == patient.id).ToList());
                        }
                        list = locationsByAge;
                    }
                }
                if (locationSearch.startDate > DateTime.MinValue)//valid dates.
                {
                    list = list.Where
                   (location => !(locationSearch.endDate <= location.startDate || locationSearch.startDate <= location.endDate)).ToList();//-------------------not perfect                  
                }
                return list;
            }
            catch
            {
                throw new Exception("exeptio in search function");
            }
            
        }
    }
}