using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public interface IPatientService
    {
        Task<Patient> GetAsync(string id);

        Task SaveAsync(Patient patient);
        Task<Patient> AuthenticateAsync(string userName, string password);

        Task<Patient> RegisterAsync(Patient newPatient);

    }
}
