using CoronaApp.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IConfiguration _configuration;

        public PatientService()
        {
        }

        public PatientService(IPatientRepository patientRepository, IConfiguration configuration)
        {
            _patientRepository = patientRepository;
            _configuration = configuration;
        }
        public Patient Get(string id)
        {
            return _patientRepository.Get(id);
        }

        public void Save(Patient patient)
        {
             _patientRepository.Save(patient);
        }
       
        public async Task<Patient> Authenticate(string userName, string password)
        {
           Patient user =await _patientRepository.IsValid(userName, password);
            if (user == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings").GetSection("Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, user.password)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);
            return user;
        }

        public async Task<Patient> Register(Patient newPatient)
        {
            _patientRepository.Add(newPatient);
            var patient =await Authenticate(newPatient.name, newPatient.password);
            return patient;
        }
    }
}
