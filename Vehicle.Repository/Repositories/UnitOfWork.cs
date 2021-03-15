using System.Threading.Tasks;
using System.Transactions;
using Vehicle.DAL.Intefaces;
using Vehicle.Repository.Common.Interfaces;

namespace Vehicle.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private  IMakeReposiotry _makeRepository;
        private  IModelRepository _modelRepository;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public IMakeReposiotry MakeRepository 
        {
            get
            {
                if (_makeRepository == null)
                {
                    _makeRepository = new MakeRepository(_context);
                }
                return _makeRepository;
            }
        }

        public IModelRepository ModelRepository  
        {
            get
            {
                if (_modelRepository == null)
                {
                    _modelRepository = new ModelRepository(_context);
                }
                return _modelRepository;
            }
        }

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
