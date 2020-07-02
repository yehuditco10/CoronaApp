using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaApp.Services
{
   public interface ILocationService
    {

        public Task<ICollection<Location>> GetAsync(LocationSearch locationSearch);
        public Task<ICollection<Location>> GetByPagingAsync(int pageIndex, int numForPage);

    }
}
