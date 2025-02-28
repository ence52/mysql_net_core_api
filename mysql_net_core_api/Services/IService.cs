using mysql_net_core_api.Core.Entitites;
using mysql_net_core_api.Repositories;

namespace mysql_net_core_api.Services
{
    public interface IService<T> where T : class,IEntity<Guid>
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}
