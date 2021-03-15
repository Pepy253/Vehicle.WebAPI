using System;
using System.Threading.Tasks;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMakeReposiotry MakeRepository { get; }
        IModelRepository ModelRepository { get; }
        Task<int>CommitAsync();
    }
}
