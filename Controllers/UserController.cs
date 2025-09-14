using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.FileService;
using HR_Carrer.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
         public UserController(IUserService userService,IFileService fileService)
        {
            _userService = userService;
        }

        [ApiExplorerSettings(GroupName ="1")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromForm] UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var responce = await _userService.CreateUser(userRequestDto);

            return CreatedAtAction(nameof(GetUsers), new { id = responce.Data?.Id }, responce);
            
        }

        [ApiExplorerSettings(GroupName = "2")]
        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _userService.GetAllUsers();
            if (res == null)
            {
                return NotFound(res); 
            }
            return StatusCode(res.StatusCode, res);
        }


        [ApiExplorerSettings(GroupName = "3")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var res = await _userService.GetUser(id);
            if (res == null)
            {
                return NotFound(res); 
            }
            return StatusCode(res.StatusCode, res);
        }


        [ApiExplorerSettings(GroupName = "4")]
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



        [ApiExplorerSettings(GroupName = "5")]

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

        [ApiExplorerSettings(GroupName = "6")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if(id == Guid.Empty)   return BadRequest("Invalid user id.");

            var responce = await _userService.DeleteUser(id);

            return StatusCode(responce.StatusCode, responce);
        }

        [ApiExplorerSettings(GroupName = "7")]
        [HttpDelete("DeleteAll")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            var responce = await _userService.DeleteAll();
            return StatusCode(responce.StatusCode, responce);
        }

        [ApiExplorerSettings(GroupName = "8")]
        [HttpPost("{id}/Upload-Image")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile Image)
        {
            if (id == Guid.Empty || Image == null)
            {
                return BadRequest("Invalid data.");
            }
            var responce = await _userService.UploadImage(id, Image);

            return StatusCode(responce.StatusCode, responce);

        }



        [ApiExplorerSettings(GroupName = "9")]
        [HttpDelete("{id}/Delete-Image")]        
        public async Task<IActionResult> DeleteImage([Required] Guid id, [FromBody, Required] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("Invalid user data.");
            }
            var responce = await _userService.UpdateUser(id, userUpdateDto);

            return StatusCode(responce.StatusCode, responce);
        }


    }
}
