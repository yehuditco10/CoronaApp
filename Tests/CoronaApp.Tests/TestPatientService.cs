using CoronaApp.Services;
using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoronaApp.Tests
{
    //mock
    public class TestPatientService : IPatientService
    {
        public Patient Get(string id)
        {
            return new Patient();
        }

        public void Save(Patient patient)
        {
           
        }
    }
}
