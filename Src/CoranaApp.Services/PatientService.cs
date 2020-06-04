using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService()
        {
        }

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }
        public Patient Get(string id)
        {
            return _patientRepository.Get(id);
        }

        public void Save(Patient patient)
        {
             _patientRepository.Save(patient);
        }
    }
}
