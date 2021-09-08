using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthService.Business.Abstract;
using AuthService.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get/{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            var result = await _userService.GetByIdAsync(userId);
            if (result.Success)
            {
                return Ok(result.Data.User);
            }
            return NotFound(result.Message);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAllAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        [HttpGet("getallactive")]
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _userService.GetAllActiveAsync();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var updateResult = await _userService.UpdateAsync(userUpdateDto);
            if (updateResult.Success)
            {
                return Ok(updateResult);
            }
            return NotFound(updateResult.Message);
        }

        [HttpPost("delete/{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            var deleteResult = await _userService.DeleteAsync(userId);
            if (deleteResult.Success)
            {
                return Ok(deleteResult);
            }
            return NotFound(deleteResult.Message);
        }

        [HttpPost]
        public async Task<IActionResult> HardDelete(int id)
        {
            var hardDeleteResult = await _userService.HardDeleteAsync(id);
            if (hardDeleteResult.Success)
            {
                return Ok(hardDeleteResult);
            }
            return NotFound(hardDeleteResult.Message);
        }

    }
}
