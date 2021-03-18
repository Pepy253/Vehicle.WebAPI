using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IMakeService
    {
        Task<PagedList<IVehicleMake>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<IVehicleMake> GetMakeAsync(IVehicleMake makeDTO);
        Task<int> InsertMakeAsync(IVehicleMake makeDTO);
        Task<int> UpdateMakeAsync(IVehicleMake makeDTO);
        Task<int> DeleteMakeAsync(IVehicleMake makeDTO);
    }
}
