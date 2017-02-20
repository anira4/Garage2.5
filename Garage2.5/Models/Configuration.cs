using System.ComponentModel.DataAnnotations;

namespace Garage2._5.Models
{
    public class Configuration
    {
        public int Id { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        [Display(Name = "Parking Spaces")]
        public int ParkingSpaces { get; set; } = 100;

        [Required]
        [Range(minimum: 0, maximum: int.MaxValue)]
        [Display(Name = "Price Per Minute")]
        public int PricePerMinute { get; set; } = 1;

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        [Display(Name = "Members Per Page")]
        public int MembersPerPage { get; set; } = 10;

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue)]
        [Display(Name = "Vehicles Per Page")]
        public int VehiclesPerPage { get; set; } = 10;

        public int ParkingUnits => ParkingSpaces * 3;
    }
}
