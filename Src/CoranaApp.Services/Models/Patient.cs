using System.Collections.Generic;
using System.Linq;
using CoronaApp.Entities;
namespace CoronaApp.Services.Models
{
    public class Patient
    {
        public string id { get; set; }
        public int age { get; set; }
        
        public List<Location> locations { get; set; }
        public Patient()
        {

        }
        //public PatientModel(string id,int age)
        //{
        //    this.id = id;
        //    this.age = age;
        //    //locations=LocationRepository.locations.Where(i => i.patientId == id).ToList();
        //}
        //public Entities.Patient ToPatient()
        //{
        //    List<Location> locations = new List<Location>();
        //    foreach (var item in this.locations)
        //    {
        //        locations.Add(new Location()
        //        {
        //            startDate = item.startDate,
        //            endDate = item.endDate,
        //            city = item.city,
        //            location = item.location,
        //            patientId = item.patientId
        //        }
        //            );
        //    }
        //    return new Entities.Patient()
        //    {
        //        id = this.id,
        //        age=this.age,
        //        locations = locations
        //    };
        //}

        //public Patient ToPatientModel(Entities.Patient patient)
        //{
        //    List<Location> locationsModel = new List<Location>();
        //    foreach (var item in patient.locations)
        //    {
        //        locationsModel.Add(new Location()
        //        {
        //            startDate = item.startDate,
        //            endDate = item.endDate,
        //            city = item.city,
        //            location = item.location,
        //            patientId = item.patientId
        //        }
        //            );
        //    }
        //    return new Patient()
        //    {
        //        id = patient.id,
        //        locations = locationsModel
        //    };
        //}

    }
}