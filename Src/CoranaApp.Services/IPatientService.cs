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

        Task<string> RegisterAsync(Patient newPatient);
        bool post(Patient patient);
        void sendMessage(string id);
        Task InvokeCommandCreateUser(string id);
    }
}
