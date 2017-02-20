using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Garage2._5.DAL;
using Garage2._5.Models;

namespace Garage2._5.ViewModels
{
    public class Statistics
    {
        public Statistics(GarageContext ctx)
        {
            ColorStatistics = new Dictionary<int, int>();
            TypeStatistics = new Dictionary<int, int>();
            TypeColorStatistics = new Dictionary<int, Dictionary<int, int>>();
            TypeNames = new Dictionary<int, string>();
            ColorNames = new Dictionary<int, string>();
            //MaximumUnits = ctx.GarageConfiguration.MaxUnits;
        }
        public Dictionary<int, string> TypeNames { get; set; }
        public Dictionary<int, string> ColorNames { get; set; }

        [Display(Name = "Statistics by Vehicle Color")]
        public Dictionary<int, int> ColorStatistics { get; set; }

        [Display(Name = "Statistics by Vehicle Type & Color")]
        public Dictionary<int, Dictionary<int, int>> TypeColorStatistics { get; set; }

        [Display(Name = "Statistics by Vehicle Type")]
        public Dictionary<int, int> TypeStatistics { get; set; }

        [Display(Name = "Total Wheel Count")]
        public int TotalWheels { get; set; }
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Display(Name = "Total Costs")]
        public decimal TotalCost { get; set; }
        [Display(Name = "Total Vehicles")]
        public int TotalVehicles { get; set; }
        private long TotalUnitsUsed { get; set; }
        private long MaximumUnits { get; }
        [Display(Name = "Total Spots Used")]
        public string TotalSpaceUsed => $"{TotalUnitsUsed / 3.0:F1} / {MaximumUnits / 3}";

        [Display(Name = "Total Members")]
        public int TotalMembers { get; set; }

        public void Update(Vehicle vehicle, DateTime now, decimal pricePerMinute)
        {
            TotalVehicles += 1;
            TotalMembers += 1;
            TotalWheels += vehicle.NumberOfWheels;
            TotalCost += (decimal)Math.Round((now - vehicle.CheckinTime).TotalMinutes) * pricePerMinute;
            TotalUnitsUsed += vehicle.Units;

            if (!ColorNames.ContainsKey(vehicle.VehicleColorId))
                ColorNames[vehicle.VehicleColorId] = vehicle.Color.Name;
            if (!TypeNames.ContainsKey(vehicle.VehicleTypeId))
                TypeNames[vehicle.VehicleTypeId] = vehicle.Type.Type;

            // Vehicle Color statistics
            if (!ColorStatistics.ContainsKey(vehicle.VehicleColorId))
                ColorStatistics[vehicle.VehicleColorId] = 1;
            else
                ColorStatistics[vehicle.VehicleColorId] += 1;

            // Vehicle Type statistics
            if (!TypeStatistics.ContainsKey(vehicle.VehicleTypeId))
                TypeStatistics[vehicle.VehicleTypeId] = 1;
            else
                TypeStatistics[vehicle.VehicleTypeId] += 1;

            // Vehicle Type & Color statistics
            if (!TypeColorStatistics.ContainsKey(vehicle.VehicleTypeId))
                TypeColorStatistics[vehicle.VehicleTypeId] = new Dictionary<int, int>();
            var tcs = TypeColorStatistics[vehicle.VehicleTypeId];
            if (!tcs.ContainsKey(vehicle.VehicleColorId))
                tcs[vehicle.VehicleColorId] = 1;
            else
                tcs[vehicle.VehicleColorId] += 1;
        }
    }
}
