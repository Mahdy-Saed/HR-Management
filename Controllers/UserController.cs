using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _userService.GetAllUsers();
            if (res == null)
            {
                return StatusCode(res.StatusCode, res); ;
            }
            return StatusCode(res.StatusCode, res);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var res = await _userService.GetUser(id);
            if (res == null)
            {
                return StatusCode(res.StatusCode, res); 
            }
            return StatusCode(res.StatusCode, res);
        }


        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            var res = await _userService.CreateUser(userRequestDto);


            return CreatedAtAction(nameof(GetUsers), new { id = res.Data?.Id }, res);
            //return StatusCode(res.StatusCode, res); 
        }






    }
}
