


using CoronaApp.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoronaApp.Dal
{
    public class CoronaContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationSearch> LocationSearches { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source = DESKTOP-1HT6NS2; Initial Catalog = Corona_DB; Integrated Security = True");
            base.OnConfiguring(optionsBuilder);
        }
        public CoronaContext(DbContextOptions<CoronaContext> options)
     : base(options)
        { }

        public CoronaContext()
        {
        }
    }

}
