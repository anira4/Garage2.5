using Garage2._5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Garage2._5.DAL
{
    public class GarageContext: DbContext 
    {
        public GarageContext() : base("Garage2.5") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new GarageContextInitializer());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Member> Members { get; set;  }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }

    }
}