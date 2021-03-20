using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IModelRepository : IRepository<VehicleModelEntity>
    {
        Task<PagedList<IVehicleModel>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<IVehicleModel> GetModelByIdAsync(int id);
        void CreateModelAsync(IVehicleModel model);
        void UpdateModelAsync(IVehicleModel model);
        void DeleteModelAsync(IVehicleModel model);
    }
}
