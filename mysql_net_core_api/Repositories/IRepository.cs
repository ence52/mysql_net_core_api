using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public interface IRepository<T> where T : class,IEntity<Guid>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);

    }
}
