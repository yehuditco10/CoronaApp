using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaApp.Entities
{
   public class Patient
    {

        public string id { get; set; }
        public int age { get; set; }
        public List<Location> locations { get; set; }
        public Patient()
        {

        }
        public Patient(string id,int age=0)
        {
            this.id = id;
            this.age = age;
          
        }
    }
}
