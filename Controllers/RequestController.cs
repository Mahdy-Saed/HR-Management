using HR_Carrer.Dto.RequestDtos;
using HR_Carrer.Services;
using HR_Carrer.Services.RequestService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }


        // .................................(Create-Request).......................................
        [Authorize(Roles = "User")]

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRequest(Guid id, [FromForm] ReqeustCreateDto reqeustCreateDto)
        {
            Guid employeeId;

            if (reqeustCreateDto is null) { return BadRequest("Request data is null"); }
            if (id == Guid.Empty)
            {
                if (!this.TryGetUserId(out Guid userId))

                    return Unauthorized("User ID not found in claims");

                employeeId = userId;
            }
            else
            {
                employeeId = id;
            }
            var responce = await _requestService.CreateRequest(employeeId, reqeustCreateDto);

            return StatusCode(responce.StatusCode, responce);
        }

        // .................................(Get-Requests).......................................

        [Authorize(Roles = "Admin,User")]

        [HttpGet("Requests")]
        public async Task<IActionResult> GetRequests([FromQuery] int? id = null, [FromQuery] string? name = null, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (!this.TryGetUserId(out Guid nothing))
            { return Unauthorized("User ID not found in claims"); }

            var res = await _requestService.GetALlService(id, name, pageNumber, pageSize);
            if (res == null)
            {
                return NotFound(res);
            }
            return StatusCode(res.StatusCode, res);

        }

        // .................................(Update-Requests).......................................

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequestStatus(int id,[FromQuery] RequestStatusDto requestStatusDto)
        {
            if (id <= 0) return BadRequest("Invalid request ID.");

            var responce = await _requestService.UpdateRequestStatus(id, requestStatusDto);
            return StatusCode(responce.StatusCode, responce);
        }

    }
}
