using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public interface IPatientRepository
    {
        Task<Patient> GetAsync(string id);

        Task SaveAsync(Patient patient);
        Task<Patient> IsValidAsync(string userName, string password);
        Task AddAsync(Patient newPatient);

    }
}
