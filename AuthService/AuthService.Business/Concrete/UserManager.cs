using AutoMapper;
using AuthService.Core.Entities.Concrete;
using AuthService.Core.Results.Abstract;
using AuthService.Core.Results.Concrete;
using AuthService.Data.UnitOfWork.Abstract;
using AuthService.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuthService.Business.Abstract;

namespace AuthService.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserManager(IUnitOfWork unitOfWork, IMapper mapper = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<UserDto>> AddAsync(UserRegisterDto userRegister, byte[] passwordHash, byte[] passwordSalt)
        {
            if (await _unitOfWork.Users.AnyAsync(u => u.Email == userRegister.Email))
            {
                return new DataResult<UserDto>(new UserDto { User = null }, false, $"{userRegister.Email} already exists.");
            }
            var user = _mapper.Map<User>(userRegister);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _unitOfWork.Users.AddAsync(user);
            try
            {
                await _unitOfWork.SaveAsync();
                return new DataResult<UserDto>(new UserDto { User = user }, true, $"{userRegister.UserName} is successfully registered.");
            }
            catch (Exception)
            {
                return new DataResult<UserDto>(null, false);
            }
        }

        public async Task<IResult> DeleteAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == userId);
            if (user != null)
            {
                user.IsActive = false;
                user.IsDeleted = true;
                user.ModifiedDate = DateTime.Now;
                await _unitOfWork.Users.UpdateAsync(user);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"User is updated.");
            }
            return new Result(false, $"User is not found.");

        }

        public async Task<IDataResult<UserListDto>> GetAllActiveAsync()
        {
            var activeUsers = await _unitOfWork.Users.GetAllAsync(u => u.IsActive == true);
            return new DataResult<UserListDto>(new UserListDto { Users = activeUsers }, true);
        }

        public async Task<IDataResult<UserListDto>> GetAllAsync()
        {
            var activeUsers = await _unitOfWork.Users.GetAllAsync();
            return new DataResult<UserListDto>(new UserListDto { Users = activeUsers }, true);
        }

        public async Task<IDataResult<UserDto>> GetByIdAsync(int UserId)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == UserId);
            if (user != null)
            {
                return new DataResult<UserDto>(new UserDto { User = user, }, true);
            }
            return new DataResult<UserDto>(null, false, $"{UserId} is not found");
        }

        public async Task<IDataResult<UserDto>> GetByMailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Email == email);
            if (user != null)
            {
                return new DataResult<UserDto>(new UserDto { User = user, }, true);
            }
            return new DataResult<UserDto>(null, false, $"{email} is not found");
        }

        public async Task<IDataResult<UserDto>> GetByUsernameAsync(string userName)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.UserName == userName);
            if (user != null)
            {
                return new DataResult<UserDto>(new UserDto { User = user, }, true);
            }
            return new DataResult<UserDto>(null, false, $"{userName} is not found");
        }

        public async Task<IResult> HardDeleteAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetAsync(u => u.Id == userId);
            if (user != null)
            {
                await _unitOfWork.Users.DeleteAsync(user);
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{userId} is deleted");
            }
            return new Result(false, $"{userId} is not found.");
        }

        public async Task<IResult> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            var oldUser = await _unitOfWork.Users.GetAsync(u => u.Id == userUpdateDto.Id);
            if (oldUser != null)
            {//password hashlerinin null olmaması için automapper kullanmaktan kaçındık.
                oldUser.Role = userUpdateDto.Role;
                oldUser.UserName = userUpdateDto.UserName;
                oldUser.IsActive = userUpdateDto.IsActive;
                oldUser.IsDeleted = userUpdateDto.IsDeleted;
                oldUser.ModifiedByName = userUpdateDto.ModifiedByName;
                oldUser.ModifiedDate = DateTime.Now;
            }
            try
            {
                await _unitOfWork.SaveAsync();
                return new Result(true, $"{userUpdateDto.UserName} is updated");
            }
            catch (Exception)
            {
                return new Result(false, "Something went wrong when updating the user.");
            }
        }
    }
}

