using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Garage2._5.Helper;
using Garage2._5.Models;

namespace Garage2._5.DAL
{
    internal class GarageContextInitializer : DropCreateDatabaseAlways<GarageContext>
    {
        private static string MakeUsername(string name)
        {
            var names = name.Split(' ');
            return (names[0].Substring(startIndex: 0, length: 1) + names[1]).RemoveDiacritics().ToLowerInvariant();
        }

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

            var nameGen = new RandomName();
            var numGen = new RandomPhone();

            var members = new List<Member>(vehicleTypes.Length * 3);
            for (var i = 0; i < members.Capacity; i++)
            {
                var member = new Member
                {
                    Name = nameGen.Next(),
                    Phone = numGen.Next()
                };
                member.Username = MakeUsername(member.Name);
                if (!members.Any(m => m.Name == member.Name || m.Username == member.Username))
                    members.Add(member);
            }

            context.Members.AddRange(members);
            context.SaveChanges();

            var gen = new RandomRegistration();
            var rand = new Random();

            var vehicles = new List<Vehicle>(vehicleTypes.Length * 3);
            while (vehicles.Capacity != vehicles.Count)
            {
                var vehicle = new Vehicle
                {
                    VehicleTypeId = vehicleTypes[rand.Next(vehicleTypes.Length)].Id,
                    CheckinTime = DateTime.Now.AddMinutes(-rand.Next(24 * 60)),
                    MemberId = members[rand.Next(members.Count)].Id
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
