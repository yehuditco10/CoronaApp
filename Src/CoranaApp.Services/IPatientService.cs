using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
   public interface IPatientService
    {
        Patient Get(string id);

        void Save(Patient patient);
        Task<Patient> Authenticate(string userName, string password);

        Task<Patient> Register(Patient newPatient);
    }
}
