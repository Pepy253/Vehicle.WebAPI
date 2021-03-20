using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vehicle.WebAPI.Models
{
    public class VehicleModelDTO
    {
        public int Id { get; set; }
        [Required]
        public int MakeId { get; set; }
        [Display(Name = "Model Name")]
        public string Name { get; set; }
        [Display(Name = "Abbreviation")]
        public string Abrv { get; set; }
        public VehicleMakeDTO VehicleMake { get; set; }
    }
}