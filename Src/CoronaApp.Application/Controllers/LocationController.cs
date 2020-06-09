using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoronaApp.Services.Models;
using Microsoft.AspNetCore.Mvc;
using CoronaApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace CoronaApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        //GET: api/<LocationController>
        [HttpGet]
        public async Task< IEnumerable<Location>> Get([FromQuery] LocationSearch locationSearch = null)
        {
            return await _locationService.GetAsync(locationSearch);
        }
        [HttpPost]
        public async Task<IEnumerable<Location>> Post(LocationSearch locationSearch)
        {
            return await _locationService.GetAsync(locationSearch);
        }
    }
}
