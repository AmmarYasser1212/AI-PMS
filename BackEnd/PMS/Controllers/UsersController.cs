using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.DTO.User;
using PMS.Application.Interfaces.Services;
using PMS.Application.Services.userser;
using PMS.Helpers;

namespace PMS.Controllers
{
    [Authorize(Roles ="User")]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public UsersController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        //[HttpPost]

        //public async Task<IActionResult> Create(CreateUserDto dto)
        //{
        //    var id= await _userServices.CreateUserAsync(dto);
        //    return Ok(id);
        //}

        [HttpGet("")]
        public async Task<IActionResult> GetMyProfile()
        {
           var UserId=User.GetBusinessUserId();
            var user = await _userServices.GetByIdAsync(UserId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var users = await _userServices.GetAllAsync();
        //    return Ok(users);
        //}

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            var UserId = User.GetBusinessUserId();
            var result = await _userServices.UpdateUserAsync(dto,UserId);
            if (!result)
                return BadRequest();

            return Ok();
        }

        //[HttpDelete("")]
        //public async Task<IActionResult> Delete()
        //{
        //   var UserId = User.GetBusinessUserId();
        //    var result = await _userServices.DeleteUserAsync(UserId);
        //    if (!result)
        //        return BadRequest();


        //    return Ok();
        //}
    }
}
