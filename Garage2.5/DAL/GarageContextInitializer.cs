using Garage2._5.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Garage2._5.Helper;

namespace Garage2._5.DAL
{
    internal class GarageContextInitializer : DropCreateDatabaseAlways<GarageContext>
    {
        protected override void Seed(GarageContext context)
        {
            var vehicleTypes = new[]
            {
                new VehicleType { Type = "Car" },
                new VehicleType { Type = "Motorcycle" },
                new VehicleType { Type = "Bus"  },
                new VehicleType { Type = "Boat"  },
                new VehicleType { Type = "Airplane" }
            };
            context.VehicleTypes.AddRange(vehicleTypes);
            context.SaveChanges();

            var members = new[]
            {
                new Member { Username="olle", Name = "Olle Olsson", Phone = "08-6582345" },
                new Member { Username="kalo", Name = "Kalle Olsson", Phone = "08-1212345"  },
                new Member { Username="olka", Name = "Olle Karlsson", Phone = "08-6758345"  },
                new Member { Username="larol", Name = "Olle Larsson", Phone = "08-8362345"  },
                new Member { Username="perol", Name = "Olle Persson", Phone = "08-8932345"  }
            };
            context.Members.AddRange(members);
            context.SaveChanges();

            var gen = new RandomRegistration();
            var rand = new Random();

            var vehicles = new List<Vehicle>(vehicleTypes.Length * 3);
            while (vehicles.Capacity != vehicles.Count)
            {
                var vehicle = new Vehicle {
                    VehicleTypeId = vehicleTypes[rand.Next(vehicleTypes.Length)].Id,
                    CheckinTime = DateTime.Now.AddMinutes(-rand.Next(24 * 60)),
                    MemberId = members[rand.Next(members.Length)].Id
                };
                do
                {
                    vehicle.Registration = gen.Next();
                } while (vehicles.Any(v => v.Registration == vehicle.Registration));
                vehicles.Add(vehicle);
                
            }
            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }
    }
}