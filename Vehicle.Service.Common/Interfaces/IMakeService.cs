using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IMakeService
    {
        Task<PagedList<IVehicleMakeDTO>> FindMakesAsync(QueryStringParameters qSParameters);
        Task<IVehicleMakeDTO> GetMakeAsync(IVehicleMakeDTO makeDTO);
        Task<int> InsertMakeAsync(IVehicleMakeDTO makeDTO);
        Task<int> UpdateMakeAsync(IVehicleMakeDTO makeDTO);
        Task<int> DeleteMakeAsync(IVehicleMakeDTO makeDTO);
    }
}
