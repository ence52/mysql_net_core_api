using Microsoft.EntityFrameworkCore;
using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
        public async Task DeleteAsync(Guid id) 
        {
            var entity = await GetByIdAsync(id);
            if (entity!=null)
            {
            _dbSet.Remove(entity);
            }
    }
}

}
