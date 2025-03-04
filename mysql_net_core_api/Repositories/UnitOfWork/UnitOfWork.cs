using mysql_net_core_api.Core.Entitites;
using System.Collections.Concurrent;
namespace mysql_net_core_api.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly ConcurrentDictionary<Type, object> _repositories = new();

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class, IEntity
        {

            return (IRepository<T>)_repositories.GetOrAdd(typeof(T), _ => new Repository<T>(_context));
        }

        public IUserRepository UserRepository => new UserRepository(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();


        public void Dispose() => _context.Dispose();


    }
}
