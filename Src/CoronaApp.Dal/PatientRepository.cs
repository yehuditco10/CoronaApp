using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<Patient> GetAsync(string id)
        {
            Patient patient = await _context.Patients.Include(p => p.locations)
                 .FirstOrDefaultAsync(pa => pa.id == id);
            if (patient == null)
            {
                Log.Error("Patient {patient} didn't find", id);
                throw new Exception("didn't find");
            }
            if (patient.locations.Count() > 0)
            {
                Log.Information("Get locations for patient {patient}", id);
                return patient;
            }

            throw new Exception("no location");
        }

     
        public async Task AddAsync(Patient newPatient)
        {
            Patient exist = await _context.Patients.FirstOrDefaultAsync(p => p.password == newPatient.password);
            if (exist != null)
            {
                throw new Exception("This password already in use, please choose another");
            }
            newPatient.id = newPatient.password;
             _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();
        }
        public async Task<Patient> IsValidAsync(string userName, string password)
        {
            //  List<Patient> li = _context.Patients.ToList();
            Patient patient = await _context.Patients.Include(l => l.locations)
                .FirstOrDefaultAsync(p => p.name == userName && p.password == password);
            if (patient != null)
                return patient;
            return null;
        }
        public async Task<bool> SaveAsync(Patient patient)
        {
            try
            {
                List<Location> locationsToUpdate = await _context.Locations.Where(l => l.patientId == patient.id).ToListAsync();
                //_context.Locations.RemoveRange(locationsToUpdate);
                 _context.Locations.AddRange(patient.locations);
                var success = await _context.SaveChangesAsync();
                //it was if(success==0), but I think that SaveChangesAsync() return the number of row was effected in db.
                if (success > 0)
                {
                    Log.Information("saved locations for patient {patient}", patient.id);
                    return true;
                }
                return false;
            }

            //?
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Log.Information(e.Message);
                return false;
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
