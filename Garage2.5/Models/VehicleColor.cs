using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage2._5.Models
{
    public class VehicleColor
    {
        public int Id { get; set; }

        [Display(Name = "Color")]
        public string Name { get; set; }
    }
}