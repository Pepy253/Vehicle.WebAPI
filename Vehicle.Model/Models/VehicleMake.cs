using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Model.Models
{
    public class VehicleMake : IVehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public IList<IVehicleModel> VehicleModels { get; set; }
    }
}
