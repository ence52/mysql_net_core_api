using System.ComponentModel.DataAnnotations;

namespace mysql_net_core_api.DTOs.User
{
    public class UserAuthDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
