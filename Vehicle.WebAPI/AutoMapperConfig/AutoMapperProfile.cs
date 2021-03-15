using AutoMapper;
using Vehicle.Model.Common.Interfaces;
using Vehicle.Model.Entities;

namespace Vehicle.WebAPI.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VehicleMake, IVehicleMakeDTO>().ReverseMap();
            CreateMap<VehicleModel, IVehicleModelDTO>().ReverseMap();            
        }
    }
}