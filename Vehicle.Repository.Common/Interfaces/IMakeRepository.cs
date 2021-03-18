using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.DAL.Entities;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IMakeRepository : IRepository<VehicleMakeEntity>
    {
        Task<PagedList<VehicleMakeEntity>> FindMakesAsync(PagingParams paging, SortingParams sorting, FilteringParams filtering);
        Task<VehicleMakeEntity> GetMakeByIdAsync(int id);
        void CreateMakeAsync(VehicleMakeEntity make);
        void UpdateMakeAsync(VehicleMakeEntity make);
        void DeleteMakeAsync(VehicleMakeEntity make);

    }
}
