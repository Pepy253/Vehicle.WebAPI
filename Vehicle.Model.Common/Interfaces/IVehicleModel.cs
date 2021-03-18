using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.Model.Common.Interfaces
{
    public interface IVehicleModel
    {
        int Id { get; set; }
        string Name { get; set; }
        string Abrv { get; set; }
        IVehicleMake VehicleMake { get; set; }
    }
}
