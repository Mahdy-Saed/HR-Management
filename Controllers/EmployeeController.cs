using HR_Carrer.Dto.EmployeeDtos;
using HR_Carrer.Services.EmployeeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <remarks> Note:Need the privilage of "Amdin"</remarks>
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEmployee/{id}")]
        public async Task<IActionResult> CreateEmployee([FromRoute] Guid id, [FromBody] EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto == null) return BadRequest("Invalid employee data.");

            var responce = await _employeeService.CreateEmployee(id, employeeRequestDto);

            return StatusCode(responce.StatusCode, responce);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeUpdateDto employeeUpdate)
        {
            if (!this.TryGetUserId(out Guid userId))
            {
                return Unauthorized("Invalid token or user not authenticated.");
            }


            if (employeeUpdate == null) return BadRequest("Invalid employee data.");
            var responce = await _employeeService.UpdateEmployee(userId, employeeUpdate);

            return StatusCode(responce.StatusCode, responce);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetInformation()
        {
            if (!this.TryGetUserId(out Guid userId))
            {
                return Unauthorized("Invalid token or user not authenticated.");
            }

            var responce = await _employeeService.GetInoformation(userId);

            return StatusCode(responce.StatusCode, responce);
        }

        [HttpPut]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!this.TryGetUserId(out Guid userId))
            {
                return Unauthorized("Invalid token or user not authenticated.");
            }
            if (changePasswordDto == null) return BadRequest("Invalid data.");
            var responce = await _employeeService.ChangePassword(userId, changePasswordDto);
            return StatusCode(responce.StatusCode, responce);
        }



        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> DeleteEmployee()
        {
            if (!this.TryGetUserId(out Guid userId))
            {
                return Unauthorized("Invalid token or user not authenticated.");
            }
            var responce = await _employeeService.DeleteEmployee(userId);
            return StatusCode(responce.StatusCode, responce);

        }



    }
}
