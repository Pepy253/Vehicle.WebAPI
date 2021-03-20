using AutoMapper;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;
using Vehicle.WebAPI.Models;

namespace Vehicle.WebAPI.AutoMapperConfig
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VehicleMakeEntity, IVehicleMake>().ReverseMap();
            CreateMap<VehicleModelEntity, IVehicleModel>().ReverseMap();
            CreateMap<VehicleMakeDTO, IVehicleMake>().ReverseMap();
            CreateMap<VehicleModelDTO, IVehicleModel>().ReverseMap();
            CreateMap<Paging, PagingParams>().ReverseMap();
            CreateMap<Filtering, FilteringParams>().ReverseMap();
            CreateMap<Sorting, SortingParams>().ReverseMap();
        }
    }
}