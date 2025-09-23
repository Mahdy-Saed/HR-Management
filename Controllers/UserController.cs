using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.FileService;
using HR_Carrer.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
         public UserController(IUserService userService)
        {
            _userService = userService;
        }
        //................................................(Create-User).....................................................

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var responce = await _userService.CreateUser(userRequestDto);

            return StatusCode(responce.StatusCode, responce);
            
        }

        //................................................(Get-All-Users).....................................................

        [Authorize(Roles = "Admin")]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers([FromQuery] Guid? id , [FromQuery] string? name ,[FromQuery] string? email,
                                                    [FromQuery] int pageNumber = 1,    [FromQuery] int pageSize = 10 )
        {

            var res = await _userService.GetAllUsers(id,name,email,pageNumber,pageSize);
            if (res == null)
            {
                return NotFound(res); 
            }
            return StatusCode(res.StatusCode, res);
        }

        //................................................(Update-User).....................................................


        [Authorize(Roles = "Admin")]
         [HttpPut("{id}")]
        public async Task<IActionResult>UpdateUser( Guid id , [FromBody] UserUpdateDto userUpdateDto)
        {
            if(userUpdateDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var responce =  await _userService.UpdateUser(id, userUpdateDto);

            return StatusCode(responce.StatusCode, responce);
        }
        //................................................(Patch-User).....................................................

        /// /// <param name="userUpdateDto">
        /// - /NewFullName → For update FullName
        /// - /NewEmail → For update Email
        /// - /NewStatus → For update status
        /// </param>
        /// /// <returns>will return the User with new information </returns>

        [Authorize(Roles = "Admin")]
         [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateSepcific(Guid id, [FromBody] JsonPatchDocument< UserUpdateDto> userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var responce = await _userService.PatchUser(id, userUpdateDto);

            return StatusCode(responce.StatusCode, responce);
        }

        //................................................(Delete-User).....................................................
        [Authorize(Roles = "Admin")]
         [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if(id == Guid.Empty)   return BadRequest("Invalid user id.");

            var responce = await _userService.DeleteUser(id);

            return StatusCode(responce.StatusCode, responce);
        }

        //................................................(Delete-All-Users).....................................................

        [Authorize(Roles = "Admin")]
         [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            var responce = await _userService.DeleteAll();
            return StatusCode(responce.StatusCode, responce);
        }

      


    }
}
