using CoronaApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Services
{
   public interface ILocationService
    {
        public ICollection<Location> Get(LocationSearch locationSearch);
    }
}
