using AuthService.Core.Entities.Concrete;
using AuthService.Core.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Entities.DTOs
{
    public class UserDto:IDto
    {
        public User User{ get; set; }
    }
}
