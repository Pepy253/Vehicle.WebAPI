using System.Collections.Generic;
using Vehicle.Model.Common.Models;

namespace Vehicle.Model.Models
{
    public class VehicleMake : BaseEntity
    {
        public List<VehicleModel> VehicleModels { get; set; }
    }
}
