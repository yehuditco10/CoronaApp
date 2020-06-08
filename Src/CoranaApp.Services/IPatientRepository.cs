using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public interface IPatientRepository
    {
        Patient Get(string id);

        void Save(Patient patient);
        Task<Patient> IsValid(string userName, string password);
        void Add(Patient newPatient);
    }
}
