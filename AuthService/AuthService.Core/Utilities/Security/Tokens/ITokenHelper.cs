using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuthService.Core.Entities.Concrete;
using AuthService.Core.Utilities.Security.Tokens.JWT;

namespace AuthService.Core.Utilities.Security.Tokens
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user);
    }
}
