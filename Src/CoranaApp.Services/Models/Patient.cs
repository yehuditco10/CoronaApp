using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace CoronaApp.Services.Models
{
    public class Patient
    {
        public string id { get; set; }
        public int age { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string token { get; set; }
        public List<Location> locations { get; set; }

        public Patient()
        {

        }
    }
}