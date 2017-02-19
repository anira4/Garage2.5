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

        private static long FindFirstFreeSpot(IEnumerable<Vehicle> vehicles, bool isMc) {
            vehicles = vehicles.OrderBy(v => v.ParkingUnit);
            Vehicle lastVehicle;
            if (isMc) {
                lastVehicle = vehicles.LastOrDefault(v => v.Type.Size == 1);
                if (lastVehicle != null) {
                    if (lastVehicle.ParkingUnit % 3 == 0 || lastVehicle.ParkingUnit % 3 == 1)
                        return lastVehicle.ParkingUnit + lastVehicle.Units;
                }
            }
            lastVehicle = vehicles.LastOrDefault();
            if (lastVehicle == null)
                return 0;
            var ret = lastVehicle.ParkingUnit + lastVehicle.Units;
            if (ret % 3 != 0) // Is this a MC?
                ret += 3 - ret % 3; // Get next full space available
            return ret;
        }

        protected override void Seed(GarageContext context)
        {
            var vehicleTypes = new[]
            {
                new VehicleType { Type = "Car", Size = 3 },
                new VehicleType { Type = "Motorcycle", Size = 1 },
                new VehicleType { Type = "Bus", Size = 6 },
                new VehicleType { Type = "Boat", Size = 9 },
                new VehicleType { Type = "Airplane", Size = 9 }
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
                    Phone = numGen.Next(),
                    Password = "password"
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
            while (vehicles.Capacity != vehicles.Count) {
                var vehicleType = vehicleTypes[rand.Next(vehicleTypes.Length)];
                var vehicle = new Vehicle
                {
                    VehicleTypeId = vehicleType.Id,
                    Type = vehicleType,
                    CheckinTime = DateTime.Now.AddMinutes(-rand.Next(24 * 60)),
                    MemberId = members[rand.Next(members.Count)].Id,
                    ParkingUnit = FindFirstFreeSpot(vehicles, vehicleType.Size == 1)
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
