using System.Collections.Generic;
using Vehicle.Model.Common.Models;

namespace Vehicle.Model.Entities
{
    public class VehicleMake : BaseVehicle
    {
        public IList<VehicleModel> VehicleModels { get; set; }
    }
}
