using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService()
        {
        }

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }
        public async Task<ICollection<Location>> GetAsync(LocationSearch locationSearch = null)
        {
            var res = await _locationRepository.GetAsync(locationSearch);
            return res;
        }

        public async Task<ICollection<Location>> GetByPagingAsync(int pageIndex, int numForPage)
        {
            var res = await _locationRepository.GetByPagingAsync(pageIndex, numForPage);
            return res;
        }
    }
}
