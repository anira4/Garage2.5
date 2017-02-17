﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage2._5.Models
{
    public class Member
    {

        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }

        // Navigation Properties
        public virtual ICollection<Vehicle> ParkedVehicles { get; set; }
    }
}