using System;
using System.Data.Entity;
using Vehicle.Model.Common.Models;
using Vehicle.Repository.Common.Interfaces;
using System.Data.Entity.Validation;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vehicle.DAL.Intefaces;
using System.Data.Entity.Infrastructure;

namespace Vehicle.Repository.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IDbContext Context;
        private IDbSet<TEntity> _entites;

        public Repository(IDbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {    
            var result = await Entites.FirstOrDefaultAsync(i => i.Id == id);
            return result;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Entites.ToListAsync();
        }

        public Task<int> InsertAsync(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                Entites.Add(entity);
            }

            return Task.FromResult(1);
            
        }

        public Task<int> UpdateAsync(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Entites.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;

            return Task.FromResult(1);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            DbEntityEntry dbEntityEntry = Context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            else
            {
                Entites.Attach(entity);
                Entites.Remove(entity);
            }

            return Task.FromResult(1);
        }

        private IDbSet<TEntity> Entites
        {
            get
            {
                if (_entites == null)
                {
                    _entites = Context.Set<TEntity>();
                }

                return _entites;
            }
        }
    }
}
