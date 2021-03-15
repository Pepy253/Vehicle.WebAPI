using System.ComponentModel.DataAnnotations.Schema;
using Vehicle.Model.Common.Models;

namespace Vehicle.Model.Entities
{
    public class VehicleModel : BaseVehicle
    {
        public int MakeId { get; set; }
        [ForeignKey("MakeId")]
        public VehicleMake VehicleMake { get; set; }
    }
}
