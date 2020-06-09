using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoronaApp.Services;
using CoronaApp.Services.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
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
        [HttpGet]
        public object Get()
        {
            var id = User.Claims.SingleOrDefault(p=>p.Type.Contains("name")).Value;
            try
            {
                return _patientService.Get(id);
            }
            catch (Exception e)
            {
                throw e;
            }


        // POST api/<PatientController>
        [HttpPost]
        public void Post([FromBody]Patient patient)
        {
          var x=  User.Claims;
            try
            {
                _patientService.Save(patient);
            }
            catch (Exception e)
            {

                throw e;
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Patient newPatient)
        {
            var patient = await _patientService.Register(newPatient);
            if (patient == null)
                return BadRequest(new { message = "Register Failed" });
            return Ok(patient);
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
        [HttpGet("username")]
        public async Task<string> GetUserNameByJWT(string jwt)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userNameClaim = claimsIdentity.FindFirst("userName");
            return userNameClaim.Value;
        }
        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]Authenticate authModel)
        {
            var user = await _patientService.Authenticate(authModel.name, authModel.password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }
       
        public class AuthenticateModel
        {
            public int Id { get; set; }
            public string Password { get; set; }
            public string UserName { get; set; }
            public string Token { get; set; }
        }
    }
}
