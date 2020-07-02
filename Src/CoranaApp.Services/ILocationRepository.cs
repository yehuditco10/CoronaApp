using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
    public interface ILocationRepository
    {
        Task<ICollection<Location>> GetAsync(LocationSearch locationSearch);
        Task<ICollection<Location>> GetByPagingAsync(int pageIndex, int numForPage);
    }
}
