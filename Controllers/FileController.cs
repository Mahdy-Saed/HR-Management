using HR_Carrer.Services.RequestService;
using HR_Carrer.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]

    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRequestService _requestService;
        public FileController( IRequestService requestService,IUserService userService)
        {
             _requestService = requestService;
            _userService= userService;
        }

        //................................................(Upload-profile-Image).....................................................

        [Authorize(Roles = "Admin,User")]
         [HttpPost("Upload-Profile-Image")]
        public async Task<IActionResult>UploadImage( IFormFile Image, [FromQuery] Guid? id = null)
        {
            Guid finalId;

             if (Image == null || Image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
             // check if the user is Admin or User

            if (User.IsInRole("Admin"))
            {
                if(id == null) return BadRequest("Id of user  is required for Admin.");

                finalId = id.Value;
            }
            
            else
            {
                if(! this.TryGetUserId(out Guid useriId))
                {
                    return Unauthorized("Authorization-Error: User ID is not valid.");
                }
                finalId = useriId; 

            }
            var responce = await _userService.UploadProfileImage(finalId, Image);

            return StatusCode(responce.StatusCode, responce);
        }

        //................................................(Delete-profile-Image).....................................................

          [Authorize(Roles = "Admin,User")]
         [HttpDelete("Delete-Profile-Image")]
        public async Task<IActionResult> Deletmage([FromQuery] Guid? id = null)
        {

            Guid finalId;
            // check if the user is Admin or User
            if (User.IsInRole("Admin"))
            {
                if (id == null) return BadRequest("Id of the User is required for Admin.");
                finalId = id.Value;
            }
            else
            {
                if (!this.TryGetUserId(out Guid useriId))
                {
                    return Unauthorized("Authorization-Error: User ID is not valid.");
                }
                finalId = useriId;
            }

            var responce = await _userService.DeleteProfileImage(finalId);
            return StatusCode(responce.StatusCode, responce);
        }
        //................................................(Upload-Request-Image).....................................................
        [Authorize(Roles = "User")]
        [HttpPost("Upload-Request-Image")]
        public async Task<IActionResult> UploadRequestImage(int id,IFormFile Image)
        {
            if (Image == null || Image.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }           
                //if (!this.TryGetUserId(out Guid EmployeeId))
                //{
                //    return Unauthorized("Authorization-Error: Employee ID is not valid.");
                //}   
            var responce = await _requestService.UploadRequestImage(id, Image);
            return StatusCode(responce.StatusCode, responce);
        }
        //...................................................(Delete-Request-Image)......................................................
        [HttpDelete("Delete-Request-Image")]
         public async Task<IActionResult> DeleteRequestImage(int id)
        {
            if ( id == 0) return BadRequest("Must Enter id");

            var result = await _requestService.DeleteRequestImage(id);

                return StatusCode(result.StatusCode, result);

        }

    }
}
