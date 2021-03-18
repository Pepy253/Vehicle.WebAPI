using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<IVehicleModel>> FindModelsAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<IVehicleModel> GetModelAsync(IVehicleModel model);
        Task<int> InsertModelAsync(IVehicleModel model);
        Task<int> UpdateModelAsync(IVehicleModel model);
        Task<int> DeleteModelAsync(IVehicleModel model);
    }
}
