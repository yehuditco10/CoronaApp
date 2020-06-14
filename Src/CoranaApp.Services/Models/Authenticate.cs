using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaApp.Services.Models
{
  public  class Authenticate
    {
       [Required]
        public string name { get; set; }

        [Required]
        public string password { get; set; }
    }
}
