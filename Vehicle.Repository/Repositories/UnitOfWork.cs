using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Vehicle.DAL.Contexts;
using Vehicle.DAL.Intefaces;
using Vehicle.Model.Models;
using Vehicle.Repository.Common.Interfaces;

namespace Vehicle.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public IRepository<VehicleMake> MakeRepository { get; set; }
        public IRepository<VehicleModel> ModelRepository { get; set; }

        public async Task<int> CommitAsync()
        {
            int result = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await _context.SaveChangesAsync();
                scope.Complete();
            }
            
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
