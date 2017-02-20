using System.Collections.Generic;
using Garage2._5.Models;

namespace Garage2._5.ViewModels
{
    public class OverviewViewModel
    {
        public OverviewViewModel(int id)
        {
            Id = id;
        }

        public OverviewViewModel(int id, IEnumerable<Vehicle> parkedVehicles)
        {
            Id = id;
            ParkedVehicles = parkedVehicles;
            IsTaken = true;
        }

        public bool IsTaken { get; }
        public IEnumerable<Vehicle> ParkedVehicles { get; }
        public int Id { get; }
    }
}
