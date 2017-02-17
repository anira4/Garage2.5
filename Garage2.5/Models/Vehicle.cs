using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._5.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Registration { get; set; }
        public DateTime CheckinTime { get; set; }
        public int VehicleTypeId { get; set; }
        public int MemberId { get; set; }

        // Navigation Properties
        public virtual VehicleType Type { get; set; }
        public virtual Member Owner { get; set; }
    }
}