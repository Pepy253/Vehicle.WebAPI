using AutoMapper;
using System.Threading.Tasks;
using System.Transactions;
using Vehicle.DAL.Intefaces;
using Vehicle.Repository.Common.Interfaces;

namespace Vehicle.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;
        private  IMakeRepository _makeRepository;
        private  IModelRepository _modelRepository;

        public UnitOfWork(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IMakeRepository MakeRepository 
        {
            get
            {
                if (_makeRepository == null)
                {
                    _makeRepository = new MakeRepository(_context, _mapper);
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
                    _modelRepository = new ModelRepository(_context, _mapper);
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
