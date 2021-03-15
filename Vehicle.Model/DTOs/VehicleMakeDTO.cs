using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Model.DTOs
{
    public class VehicleMakeDTO : IVehicleMakeDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [Required]
        [Display(Name ="Make Name")]
        public string Name { get; set; }
        [Index(IsUnique = true)]
        [Required]
        [Display(Name = "Abbreviation")]
        public string Abrv { get; set; }
        
        public List<IVehicleModelDTO> VehicleModels { get; set; }
    }
}
