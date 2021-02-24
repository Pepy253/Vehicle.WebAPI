using Vehicle.Model.Common.Models;

namespace Vehicle.Model.Models
{
    public class VehicleModel : BaseEntity
    {
        public int MakeId { get; set; }
        public VehicleMake VehicleMake { get; set; }
    }
}
