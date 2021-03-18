using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;


namespace Vehicle.Repository.Common.Interfaces
{
    public interface IModelRepository : IRepository<VehicleModelEntity>
    {
        Task<PagedList<VehicleModelEntity>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<VehicleModelEntity> GetModelByIdAsync(int id);
        void CreateModelAsync(VehicleModelEntity model);
        void UpdateModelAsync(VehicleModelEntity model);
        void DeleteModelAsync(VehicleModelEntity model);
    }
}
