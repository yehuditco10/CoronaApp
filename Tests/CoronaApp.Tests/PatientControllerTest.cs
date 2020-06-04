using CoronaApp.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
//using Xunit;

namespace CoronaApp.Tests
{
    public class PatientControllerTest:IClassFixture<WebApplicationFactory<Api.Startup>>
    {
        private readonly WebApplicationFactory<Api.Startup> _factory;

        public PatientControllerTest(WebApplicationFactory<Api.Startup> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async void GetPatient_ById_ReturnPatient()
        {
            //Arrange-The test app prepares a request.
            var client = _factory.CreateClient();
            //Act-The client submits the request and receives the response.
            var response = await client.GetAsync("/api/patient/111");
            //Assert-The actual response is validated as a pass or fail based on an expected response.
            response.EnsureSuccessStatusCode();
            var content = response.Content;
        }

        [Fact]
        public async Task Get_PatientService_ProvidesQuoteInPage()
        {
            // Arrange
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IPatientService, TestPatientService>();
                });
            })
                .CreateClient();

            //Act
            var response = await client.GetAsync("/api/patient/111");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
