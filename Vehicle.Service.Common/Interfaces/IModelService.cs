using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Model.Models;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IModelService
    {
        Task<List<VehicleModel>> GetModelsAsync();
        Task<VehicleModel> GetModelAsync(int id);
        void InsertModelAsync(VehicleModel model);
        void UpdateModelAsync(VehicleModel model);
        void DeleteModelAsync(VehicleModel model);
    }
}
