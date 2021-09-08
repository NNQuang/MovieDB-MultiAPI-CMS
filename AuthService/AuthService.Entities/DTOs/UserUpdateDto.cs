using AuthService.Core.Entities.Concrete;
using AuthService.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Entities.DTOs
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedByName { get; set; }
    }
}
