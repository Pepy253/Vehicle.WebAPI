using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Entities;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IMakeReposiotry : IRepository<VehicleMake>
    {
        Task<PagedList<VehicleMake>> FindMakesAsync(QueryStringParameters qSParameters);
        Task<VehicleMake> GetMakeByIdAsync(int id);
        void CreateMakeAsync(VehicleMake make);
        void UpdateMakeAsync(VehicleMake make);
        void DeleteMakeAsync(VehicleMake make);

    }
}
