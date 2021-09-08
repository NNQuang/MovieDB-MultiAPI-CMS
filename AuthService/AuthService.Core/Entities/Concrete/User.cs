using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AuthService.Core.Entities.Abstract;
using AuthService.Core.Enums;

namespace AuthService.Core.Entities.Concrete
{
    public class User:BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public Role Role { get; set; } = Role.User;
    }
}
