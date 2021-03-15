using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Model.DTOs
{
    public class VehicleModelDTO : IVehicleModelDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vehicle Make")]
        public int MakeId { get; set; }

        [Index(IsUnique = true)]
        [Required]
        [Display(Name = "Vehicle Model")]
        public string Name { get; set; }

        [Index(IsUnique = true)]
        [Required]
        public string Abrv { get; set; }

        public IVehicleMakeDTO VehicleMake { get; set; }
    }
}
