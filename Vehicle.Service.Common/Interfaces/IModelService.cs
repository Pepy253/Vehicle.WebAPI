using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<IVehicleModelDTO>> FindModelsAsync(QueryStringParameters qSParameters);
        Task<IVehicleModelDTO> GetModelAsync(IVehicleModelDTO modelDTO);
        Task<int> InsertModelAsync(IVehicleModelDTO modelDTO);
        Task<int> UpdateModelAsync(IVehicleModelDTO modelDTO);
        Task<int> DeleteModelAsync(IVehicleModelDTO modelDTO);
    }
}
