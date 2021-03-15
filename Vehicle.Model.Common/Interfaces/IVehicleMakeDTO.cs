using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle.Model.Common.Interfaces
{
    public interface IVehicleMakeDTO
    {
        [Key]
        int Id { get; set; }
        [Index(IsUnique = true)]
        [Required]
        [Display(Name = "Make Name")]
        string Name { get; set; }
        [Index(IsUnique = true)]
        [Required]
        [Display(Name = "Abbreviation")]
        string Abrv { get; set; }
        [JsonIgnore]
        List<IVehicleModelDTO> VehicleModels { get; set; }
    }
}
