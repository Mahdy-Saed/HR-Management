using HR_Carrer.Dto.AuthDtos;
using HR_Carrer.Services;
using HR_Carrer.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
namespace HR_Carrer.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public    IAuthService _authService { get; set; }

        public AuthController(IAuthService authService)
        {
             _authService= authService;
        }

        [Authorize(Roles ="Admin,User")]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult>login(AuthLogReqDto logReq)
        {
            var res = await _authService.login(logReq);

            return StatusCode(res.StatusCode,res);

        }
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> logout()
        {

            if (!this.TryGetUserId(out Guid userId)) return Unauthorized("User is not authenticated");

            var res = await _authService.logout(userId);

            return StatusCode(res.StatusCode,res);
        }


        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [Route("getNewToken")]
        public async Task<IActionResult> Refresh([FromBody] AuthRefeshDto auth)
        {
            if(string.IsNullOrEmpty(auth.RefreshToken))  return BadRequest("Refresh token is required");

            var res = await _authService.Refresh(auth.RefreshToken);

            return StatusCode(res.StatusCode,res);
        }


    }
}
