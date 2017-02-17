using Garage2._5.Models;
using System.Data.Entity;

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
        }
    }
}