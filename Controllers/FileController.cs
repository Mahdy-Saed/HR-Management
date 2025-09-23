using HR_Carrer.Services.AttachmentService;
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
        private readonly IAttachmentService _attachmentService;
        public FileController(IAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        //................................................(Upload-Image).....................................................

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
            var responce = await _attachmentService.UploadImage(finalId, Image);

            return StatusCode(responce.StatusCode, responce);
        }

        //................................................(Delete-Image).....................................................

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

            var responce = await _attachmentService.DeleteImage(finalId);
            return StatusCode(responce.StatusCode, responce);
        }


    }
}
