using CoronaApp.Services.Models;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NServiceBus;
using NServiceBus.Logging;
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
        static ILog log = LogManager.GetLogger<PatientService>();
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
            bool success = await _patientRepository.SaveAsync(patient);
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

            //IServiceBus
           
        }



        public bool post(Patient patient)
        {

            return true;
        }

        public void sendMessage(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);


                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);

            }
        }

        public async Task InvokeCommandCreateUser(string patientId)
        {
            Console.WriteLine("Hello World!");


            var endpointConfiguration = new EndpointConfiguration("createUser");

            var transport = endpointConfiguration.UseTransport<LearningTransport>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                  .ConfigureAwait(false);

            var command = new CreateUser()
            {
                UserId = patientId
            };

            // Send the command to the local endpoint
            log.Info($"Sending CreateUser command, UserId = {command.UserId}");
            await endpointInstance.SendLocal(command)
                .ConfigureAwait(false);

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

    }
}
