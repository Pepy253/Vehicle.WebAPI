using System.Data.Entity;
using Vehicle.Repository.Common.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Vehicle.DAL.Intefaces;

namespace Vehicle.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class 
    {
        protected readonly IDbContext _context;

        public Repository(IDbContext context)
        {
            _context = context;
        }


        public Task<IQueryable<TEntity>> GetAsync()
        {
            var query = _context.Set<TEntity>().AsQueryable();

            return Task.FromResult(query);
        }


        public Task<int> InsertAsync(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            return Task.FromResult(1);            
        }

        public Task<int> UpdateAsync(TEntity entity)
        {

            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Set<TEntity>().Remove(entity);

            return Task.FromResult(1);
        }
    }
}
