using System.Threading.Tasks;
using Vehicle.Common.Helpers;
using Vehicle.Model.Entities;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IModelRepository : IRepository<VehicleModel>
    {
        Task<PagedList<VehicleModel>> FindModelsAsync(QueryStringParameters qSParameters);
        Task<VehicleModel> GetModelByIdAsync(int id);
        void CreateModelAsync(VehicleModel model);
        void UpdateModelAsync(VehicleModel model);
        void DeleteModelAsync(VehicleModel model);
    }
}
