using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoronaApp.Services;
using CoronaApp.Services.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoronaApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {

            _patientService = patientService;
        }
        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public object Get(string id)
        {
            try
            {
                return _patientService.Get(id);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody]Patient patient)
        {
            try
            {
                _patientService.Save(patient);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _patientService.Authenticate(model.UserName, model.Password.ToString());
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }

    }
}
