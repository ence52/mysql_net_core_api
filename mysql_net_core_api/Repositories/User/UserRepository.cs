
using Microsoft.EntityFrameworkCore;
using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api.Repositories
{
    public class UserRepository:Repository<UserEntity>,IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context):base(context) 
        {
            _context = context;
        }

        public async Task<UserEntity> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
        }

    }
}
