using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vehicle.WebAPI.Models
{
    public class VehicleMakeDTO
    {
        public int Id { get; set; }
        [Display(Name = "Make Name")]
        public string Name { get; set; }
        [Display(Name = "Abbreviation")]
        public string Abrv { get; set; }
        public IList<VehicleModelDTO> VehicleModels { get; set; }
    }
}