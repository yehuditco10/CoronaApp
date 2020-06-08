using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
   public  class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
        }

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public ICollection<Location> Get(LocationSearch locationSearch=null)
        {
          var res=  _locationRepository.Get(locationSearch);
            return res;
        }
    }
}
