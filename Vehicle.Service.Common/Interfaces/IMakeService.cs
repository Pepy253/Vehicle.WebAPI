using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Model.Models;

namespace Vehicle.Service.Common.Interfaces
{
    public interface IMakeService
    {
        Task<List<VehicleMake>> GetMakesAsync();
        Task<VehicleMake> GetMakeAsync(int id);
        void InsertMakeAsync(VehicleMake make);
        void UpdateMakeAsync(VehicleMake make);
        void DeleteMakeAsync(VehicleMake make);
    }
}
