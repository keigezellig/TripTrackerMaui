using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class VehicleSpeedModel : VehicleModel
    {
        public VehicleSpeedModel(string tripId, string vehicleId, double value) : base(tripId, vehicleId, value)
        {
        }
    }
}
