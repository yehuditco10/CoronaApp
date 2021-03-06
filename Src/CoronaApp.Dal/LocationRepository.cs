﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoronaApp.Dal;
using CoronaApp.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace CoronaApp.Services
{
    public class LocationRepository : ILocationRepository
    {
        private readonly CoronaContext _coronaContext;
        public LocationRepository(CoronaContext _coronaContext)
        {
            this._coronaContext = _coronaContext;
        }
        public async Task<ICollection<Location>> GetAsync(LocationSearch locationSearch = null)
        {
            try
            {
                var list = await _coronaContext.Locations.ToListAsync();
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
                throw new Exception("exeption in search function");
            }

        }

        public async Task<ICollection<Location>> GetByPagingAsync(int pageIndex, int numForPage)
        {
            try
            {
                var list = (from t in _coronaContext.Locations
                            orderby t.id
                            select t).Take(numForPage);
                var list2 = _coronaContext.Locations
                           .OrderBy(t => t.id)
                           .Take(5);
                return await _coronaContext.Locations.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}