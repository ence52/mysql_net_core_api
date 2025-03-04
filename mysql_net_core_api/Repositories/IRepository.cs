using mysql_net_core_api.Core.Entitites;
using System.Linq.Expressions;

namespace mysql_net_core_api.Repositories
{
    public interface IRepository<T> where T : class,IEntity
    {
        Task<T> GetByPropAsync(Expression<Func<T,bool>> expression);
        Task<ICollection<T>> GetWhereAsync(Expression<Func<T, bool>> expression);
        IQueryable<T> GetByQuery();
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(T entity);

    }
}
