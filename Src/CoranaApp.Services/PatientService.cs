using CoronaApp.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
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
        public async Task<Patient> GetAsync(string id)
        {
            return await _patientRepository.GetAsync(id);
        }

        public async Task SaveAsync(Patient patient)
        {
           bool success= await _patientRepository.SaveAsync(patient);
        }

        public async Task<Patient> AuthenticateAsync(string userName, string password)
        {
            Patient user = await _patientRepository.IsValidAsync(userName, password);
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
                    new Claim(ClaimTypes.Name, user.name),
                     new Claim("userName", user.name),
                     new Claim("userId", user.id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.token = tokenHandler.WriteToken(token);
            return user;
        }

        public async Task<Patient> RegisterAsync(Patient newPatient)
        {
            await _patientRepository.AddAsync(newPatient);
            var patient = await AuthenticateAsync(newPatient.name, newPatient.password);
            return patient;
        }

        public bool post(Patient patient)
        {
            return  true;
        }

        public void sendMessage(string id)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var message = "patient "+id+" added to the DB";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);
              
            }
        }
    }
}
