
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoronaApp.Services;
using CoronaApp.Dal;
using Microsoft.EntityFrameworkCore;
using CoronaApp.Services.Models;
using Serilog;

namespace CoronaApp.Services
{
    public class PatientRepository : IPatientRepository
    {
        private readonly CoronaContext _context;
        public PatientRepository(CoronaContext coronaContext)
        {
            _context = coronaContext;
        }
        public Patient Get(string id)
        {
            
            Patient patient = _context.Patients.Include(p => p.locations)
                 .FirstOrDefault(pa => pa.id == id);
            if (patient == null)
            {
                Log.Error("Patient {pateient} didn't find", id);
                throw new Exception("didn't find");
            }
            if (patient.locations.Count() > 0)
            {
                Log.Information("Get locations for patient {pateient}", id);
                return patient;
            }
           
            throw new Exception("no location");
        }

        public void Save(Patient patient)
        {

            try
            {
                List<Location> locationsToUpdate = _context.Locations.Where(l => l.patientId == patient.id).ToList();
                _context.Locations.RemoveRange(locationsToUpdate);
                _context.Locations.AddRange(patient.locations);
                _context.SaveChanges();
                Log.Information("saved locations for patient {pateient}", patient.id);
            }
            catch (Exception)
            {
                throw new Exception("adding failed");
            }


        }





        //public static async Task<List<Location>> GetLocationsByPatientAsync(String id)
        //{
        //    try
        //    {
        //        if (LocationRepository.locations.FirstOrDefault(l => l.patientId == id) != null)
        //        {
        //            return LocationRepository.locations.Where(i => i.patientId == id).ToList();
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw new Exception("we didn't find");
        //    }

        //    return null;
        //}
        //public async Task<List<Location>> Get(string patienId)
        //{
        //    try
        //    {
        //        var results = await GetLocationsByPatientAsync(patienId);
        //        if (results == null || results.Count() <= 0)
        //        {
        //            throw new Exception("we didn't find");

        //        }
        //        return results;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.ToString());
        //        //return this.StatusCode(StatusCodes.Status500InternalServerError, "data failed");
        //    }
        //}
    }
}
