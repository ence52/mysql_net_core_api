using Microsoft.EntityFrameworkCore;
using mysql_net_core_api.Core.Entitites;

namespace mysql_net_core_api
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products{ get; set; }
        public DbSet<CategoryEntity> Categories{ get; set; }
        public DbSet<OrderEntity> Orders{ get; set; }
        public DbSet<OrderItemEntity> OrderItems{ get; set; }
       
    }
}
