using System.Linq;
using System.Threading.Tasks;
using Vehicle.Model.Common.Models;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseVehicle
    {
        Task<IQueryable<TEntity>> GetAsync();
        Task<int> InsertAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }    
}
