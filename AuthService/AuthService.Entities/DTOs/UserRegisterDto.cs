using AuthService.Core.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Entities.DTOs
{
    public class UserRegisterDto:IDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
