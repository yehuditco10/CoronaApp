using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
   public interface IPatientService
    {
        Patient Get(string id);

        void Save(Patient patient);
    }
}
