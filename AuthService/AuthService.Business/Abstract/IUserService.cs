using AuthService.Core.Entities.Concrete;
using AuthService.Core.Results.Abstract;
using AuthService.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<UserListDto>> GetAllAsync();
        Task<IDataResult<UserListDto>> GetAllActiveAsync();
        Task<IDataResult<UserDto>> GetByMailAsync(string email);
        Task<IDataResult<UserDto>> GetByUsernameAsync(string userName);
        Task<IDataResult<UserDto>> GetByIdAsync(int UserId);
        Task<IDataResult<UserDto>> AddAsync(UserRegisterDto userRegister, byte[] passwordHash, byte[] passwordSalt);
        Task<IResult> UpdateAsync(UserUpdateDto userUpdate);
        Task<IResult> DeleteAsync(int userId);
        Task<IResult> HardDeleteAsync(int userId);
    }
}
