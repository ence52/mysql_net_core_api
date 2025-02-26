using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public interface IUserRepository
    {
        Task<UserEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<UserEntity>> GetAllAsync();
        Task AddAsync(UserEntity user);
        Task<bool> DeleteAsync(Guid id);
        Task<UserEntity> GetUserByUsernameAsync(string username);
    }
}
