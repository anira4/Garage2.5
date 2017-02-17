using Garage2._5.Models;
using System;
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

            var vehicles = new[]
            {
                new Vehicle { VehicleTypeId = vehicleTypes[0].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[0].Id, Registration = "ABC123" },
                new Vehicle { VehicleTypeId = vehicleTypes[0].Id, CheckinTime = DateTime.Parse("2017-02-17 11:35"), MemberId = members[1].Id, Registration = "XCD234"  },
                new Vehicle { VehicleTypeId = vehicleTypes[0].Id, CheckinTime = DateTime.Parse("2017-02-17 10:30"), MemberId = members[2].Id, Registration = "IDW345"  },

                new Vehicle { VehicleTypeId = vehicleTypes[1].Id, CheckinTime = DateTime.Parse("2017-02-17 11:20"), MemberId = members[3].Id, Registration = "PCE456"  },
                new Vehicle { VehicleTypeId = vehicleTypes[1].Id, CheckinTime = DateTime.Parse("2017-02-16 15:40"), MemberId = members[4].Id, Registration = "POX456"  },
                new Vehicle { VehicleTypeId = vehicleTypes[1].Id, CheckinTime = DateTime.Parse("2017-02-16 16:30"), MemberId = members[0].Id, Registration = "PCE567"  },

                new Vehicle { VehicleTypeId = vehicleTypes[2].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[1].Id, Registration = "OLW471"  },
                new Vehicle { VehicleTypeId = vehicleTypes[2].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[2].Id, Registration = "PCY432"  },
                new Vehicle { VehicleTypeId = vehicleTypes[2].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[3].Id, Registration = "OIX654"  },

                new Vehicle { VehicleTypeId = vehicleTypes[3].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[4].Id, Registration = "OXY432"  },
                new Vehicle { VehicleTypeId = vehicleTypes[3].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[1].Id, Registration = "MCR634"  },
                new Vehicle { VehicleTypeId = vehicleTypes[3].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[2].Id, Registration = "OMR543"  },

                new Vehicle { VehicleTypeId = vehicleTypes[4].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[3].Id, Registration = "PXT476"  },
                new Vehicle { VehicleTypeId = vehicleTypes[4].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[4].Id, Registration = "LBN477"  },
                new Vehicle { VehicleTypeId = vehicleTypes[4].Id, CheckinTime = DateTime.Parse("2017-02-17 11:30"), MemberId = members[0].Id, Registration = "MUX634"  }
            };
            context.Vehicles.AddRange(vehicles);
            context.SaveChanges();
        }
    }
}