using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class, IEntity;
        IUserRepository UserRepository { get; }
        Task<int> CompleteAsync();
    }
}
