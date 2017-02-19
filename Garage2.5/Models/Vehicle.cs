﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Garage2._5.Models {
    public class Vehicle {
        public int Id { get; set; }

        public string Registration { get; set; }

        [Display(Name = "Checkin time")]
        [DisplayFormat(DataFormatString = "{0:dd-MM HH\\:mm}")]
        public DateTime CheckinTime { get; set; }

        [Display(Name = "Parked Time")]
        [DisplayFormat(DataFormatString = "{0:h\\:mm}")]
        public TimeSpan ParkedTime => DateTime.Now - CheckinTime;

        public int VehicleTypeId { get; set; }

        public int MemberId { get; set; }

        public long ParkingUnit { get; set; }

        // Navigation Properties
        [Display(Name = "Vehicle type")]
        public virtual VehicleType Type { get; set; }

        [Display(Name = "Owner")]
        public virtual Member Owner { get; set; }

        public int Units => Type?.Size ?? 0;

        [Display(Name = "Parking Space")]
        public string ParkingSpot {
            get {
                if (Type.Size <= 3)
                    return (ParkingUnit / 3 + 1).ToString();
                return ParkingUnit / 3 + 1 + "-" + ((ParkingUnit + (Units - 3)) / 3 + 1);
            }
        }
    }
}
