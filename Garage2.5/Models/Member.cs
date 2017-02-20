using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2._5.Models
{
    public class Member
    {

        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        // Navigation Properties
        [Display(Name = "Parked Vehicles")]
        public virtual ICollection<Vehicle> ParkedVehicles { get; set; }
    }
}