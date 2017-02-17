using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2._5.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Registration { get; set; }
        [Display(Name = "Checkin time")]
        public DateTime CheckinTime { get; set; }
        public int VehicleTypeId { get; set; }
        public int MemberId { get; set; }

        // Navigation Properties
        [Display(Name = "Vehicle type")]
        public virtual VehicleType Type { get; set; }
        [Display(Name = "Owner")]
        public virtual Member Owner { get; set; }
    }
}