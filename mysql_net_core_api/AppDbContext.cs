using Microsoft.EntityFrameworkCore;
using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
    }
}
