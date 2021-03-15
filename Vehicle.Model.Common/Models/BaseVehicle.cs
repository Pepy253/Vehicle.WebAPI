using Vehicle.Model.Common.Interfaces;


namespace Vehicle.Model.Common.Models
{
    public class BaseVehicle : IVehicle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abrv { get; set; }
    }
}