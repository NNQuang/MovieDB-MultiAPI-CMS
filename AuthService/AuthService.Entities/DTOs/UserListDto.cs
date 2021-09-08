using AuthService.Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Entities.DTOs
{
    public class UserListDto
    {
        public IList<User> Users { get; set; }
    }
}
