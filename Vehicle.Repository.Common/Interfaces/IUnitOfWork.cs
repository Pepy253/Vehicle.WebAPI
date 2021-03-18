using System;
using System.Threading.Tasks;

namespace Vehicle.Repository.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMakeRepository MakeRepository { get; }
        IModelRepository ModelRepository { get; }
        Task<int>CommitAsync();
    }
}
