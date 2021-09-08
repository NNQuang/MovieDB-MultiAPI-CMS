using AuthService.Core.Entities.Concrete;
using AuthService.Core.Results.Abstract;
using AuthService.Core.Results.Concrete;
using AuthService.Core.Utilities.Security.Hashing;
using AuthService.Core.Utilities.Security.Tokens;
using AuthService.Core.Utilities.Security.Tokens.JWT;
using AuthService.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthService.Business.Abstract;

namespace AuthService.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;

        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var accessToken = _tokenHelper.CreateToken(user);
            return new DataResult<AccessToken>(accessToken, true, "Access token is generated");
        }

        public async Task<IDataResult<User>> Login(UserLoginDto userLoginDto)
        {
            var userToCheck = await _userService.GetByMailAsync(userLoginDto.Email);
            if (!userToCheck.Success)
            {
                return new DataResult<User>(null, false, $"{userLoginDto.Email} is not registered.");
            }

            var user = userToCheck.Data.User;
            if (!HashingHelper.VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new DataResult<User>(null, false, "Password is wrong.");
            }

            if (userToCheck.Data.User.IsActive == false || userToCheck.Data.User.IsDeleted == true)
            {
                return new DataResult<User>(null, false, "User is inactive or deleted.");
            }

            return new DataResult<User>(user, true);
        }

        public async Task<IDataResult<UserDto>> Register(UserRegisterDto userRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);

            return await _userService.AddAsync(userRegisterDto, passwordHash, passwordSalt);
        }

        //public async Task<IResult> UserExists(string email)
        //{
        //    var userToCheckResult = await _userService.GetByMailAsync(email);
        //    if (!userToCheckResult.Success)
        //    {
        //        return new Result(false, $"Given email is not found");
        //    }
        //    return new Result(true, "User exists.");
        //}
    }
}
