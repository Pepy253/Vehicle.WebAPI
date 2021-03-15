using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using Vehicle.Model.Common.Models;

namespace Vehicle.DAL.Intefaces
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseVehicle;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        Task<int> SaveChangesAsync();
        void Dispose();
    }
}
