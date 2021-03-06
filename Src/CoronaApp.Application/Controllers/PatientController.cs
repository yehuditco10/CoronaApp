﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoronaApp.Services;
using CoronaApp.Services.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoronaApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        // GET api/<PatientController>/5
        [HttpGet]
        public async Task<ActionResult<Patient>> Get()
        {
            var id = User.Claims.SingleOrDefault(p => p.Type.Contains("userId")).Value;
            try
            {
                return await _patientService.GetAsync(id);
            }
            catch (Exception e)
            {
                return BadRequest("no loactions");
            }

        }
        // POST api/<PatientController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Patient patient)
        {
            if (patient.locations == null)
                return new UnsupportedMediaTypeResult();
            try
            {
                await _patientService.SaveAsync(patient);
                return Ok(true);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]Patient newPatient)
        {
            if (String.IsNullOrEmpty(newPatient.password) || String.IsNullOrEmpty(newPatient.name))
                return new UnsupportedMediaTypeResult();
            var message = await _patientService.RegisterAsync(newPatient);
            if (message.Contains("succeded"))
                return Ok(newPatient);
            return BadRequest(message);
        }

        //Async?
        [HttpGet("username")]
        public ActionResult GetUserNameByJWT()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userNameClaim = claimsIdentity.FindFirst("userName");
            return Ok(new { name = userNameClaim.Value });
        }
        /// <summary>
        /// authenticate patient
        /// </summary>
        /// <param name="authModel"></param>
        /// <returns>An actionResult of type patient - with his JWT</returns>
        /// <response code="200">Returns the JWT</response>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Patient))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody]Authenticate authModel)
        {
            var user = await _patientService.AuthenticateAsync(authModel.name, authModel.password);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }
        [HttpPost("add")]
        public async Task post(Patient patient)
        {
            bool success = _patientService.post(patient);
            if (success == true)
            {
                _patientService.sendMessage("patient " + patient.id + " added to the DB");
                await _patientService.InvokeCommandCreateUser(patient.id);
            }

        }
    }
}
