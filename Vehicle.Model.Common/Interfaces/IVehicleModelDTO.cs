using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle.Model.Common.Interfaces
{
    public interface IVehicleModelDTO
    {
        [Key]
        int Id { get; set; }
        
        [Required]
        [Display(Name = "Vehicle Make")]
        int MakeId { get; set; }
        
        [Index(IsUnique = true)]
        [Required]
        [Display(Name = "Vehicle Model")]
        string Name { get; set; }

        [Index(IsUnique = true)]
        [Required]
        string Abrv { get; set; }
        [JsonIgnore]
        IVehicleMakeDTO VehicleMake { get; set; }
    }
}
