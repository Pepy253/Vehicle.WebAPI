using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vehicle.Model.Models;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<VehicleMake> MakeRepository { get; set; }
        IRepository<VehicleModel> ModelRepository { get; set; }
        Task<int>CommitAsync();
        void Dispose();
    }
}
