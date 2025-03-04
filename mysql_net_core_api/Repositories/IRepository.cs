using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public interface IRepository<T> where T : class,IEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);

    }
}
