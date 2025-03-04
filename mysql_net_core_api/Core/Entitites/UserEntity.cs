
using mysql_net_core_api.Core.Enums;

namespace mysql_net_core_api.Core.Entitites
{
    public class UserEntity:IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<UserRoleEnum> Role{ get; set; }= new List<UserRoleEnum> {UserRoleEnum.Customer };
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderEntity> Orders { get; set; } 
    }
}
