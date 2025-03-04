using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public interface IUserRepository:IRepository<UserEntity>
    {
        Task<UserEntity> GetUserByUsernameAsync(string username);
    }
}
