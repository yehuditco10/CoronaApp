using CoronaApp.Services.Models;
using System.Collections.Generic;

namespace CoronaApp.Services
{
    public interface IUserService
    {
        User Authenticate(string userName, string password);
        ICollection<Location> GetAll();
    }
}