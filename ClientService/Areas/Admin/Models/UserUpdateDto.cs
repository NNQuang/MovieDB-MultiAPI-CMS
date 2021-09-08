using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientService.Enums;

namespace ClientService.Areas.Admin.Models
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
