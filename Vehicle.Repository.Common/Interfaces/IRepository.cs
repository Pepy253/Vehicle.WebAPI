using System.Linq;
using System.Threading.Tasks;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAsync();
        Task<int> InsertAsync(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }    
}
