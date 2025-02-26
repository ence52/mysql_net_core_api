using System.ComponentModel.DataAnnotations;

namespace mysql_net_core_api.DTOs.User
{
    public class UpdateUserDto
    {
        [StringLength(50, MinimumLength = 3)]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string LastName { get; set; }

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }

}
