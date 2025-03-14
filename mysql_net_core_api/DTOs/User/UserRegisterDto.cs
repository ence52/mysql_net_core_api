﻿using System.ComponentModel.DataAnnotations;

namespace mysql_net_core_api.DTOs.User
{
    public class UserRegisterDto
    {
        [Required]
        [StringLength(50,MinimumLength =3)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50,MinimumLength =3)]
        public string LastName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
