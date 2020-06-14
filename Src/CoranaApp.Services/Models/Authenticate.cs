using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoronaApp.Services.Models
{
    /// <summary>
    /// Authenticate user
    /// </summary>
    public class Authenticate
    {
        /// <summary>
        /// The name of the patient
        /// </summary>
        [Required]
        [MinLength(2)]
        public string name { get; set; }

        /// <summary>
        /// The password of the patient
        /// </summary>
        [Required]
        public string password { get; set; }
    }
}
