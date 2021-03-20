using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;
using Vehicle.Model.Common.Interfaces;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IMakeRepository : IRepository<VehicleMakeEntity>
    {
        Task<PagedList<IVehicleMake>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<IVehicleMake> GetMakeByIdAsync(int id);
        void CreateMakeAsync(IVehicleMake make);
        void UpdateMakeAsync(IVehicleMake make);
        void DeleteMakeAsync(IVehicleMake make);

    }
}
