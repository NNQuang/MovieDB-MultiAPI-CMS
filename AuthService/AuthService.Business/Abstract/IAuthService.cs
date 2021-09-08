using AuthService.Core.Entities.Concrete;
using AuthService.Core.Results.Abstract;
using AuthService.Core.Utilities.Security.Tokens.JWT;
using AuthService.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<UserDto>> Register(UserRegisterDto userRegisterDto);
        Task<IDataResult<User>> Login(UserLoginDto userLoginDto);
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
