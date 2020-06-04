using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
    public interface ILocationSearchRepository
    {
        ICollection<Location> Get(LocationSearch locationSearch);
        ICollection<Location> GetByDate(LocationSearch criteria);

    }
}
