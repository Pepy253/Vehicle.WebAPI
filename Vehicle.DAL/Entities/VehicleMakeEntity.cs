using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle.DAL.Entities
{
    public class VehicleMakeEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
        public IList<VehicleModelEntity> VehicleModel { get; set; }
    }
}
