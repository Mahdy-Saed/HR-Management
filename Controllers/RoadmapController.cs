using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.RoadmapDtos;
using HR_Carrer.Dto.StepDtos;
using HR_Carrer.Services.CertificateService;
using HR_Carrer.Services.EmployeeService;
using HR_Carrer.Services.RoadmapService;
using HR_Carrer.Services.StepService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadmapController : ControllerBase
    {
        private readonly IRoadmapService _roadmapService;
        private readonly IStepService _stepService;
        public RoadmapController(ICertificateService certificateService, IRoadmapService roadmapService, IStepService stepService)
        {
            _roadmapService = roadmapService;
            _stepService = stepService;
        }


        //..........................................................(Create-RoadMap).................................................

        /// <param name="EmployeeId"> The Id of employee Required</param>
        /// <param name="roadmapRequestDto">
        /// Difficulty(easy,intermediate, hard)
        /// </param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost("{EmployeeId}")]
        public async Task<IActionResult> CreateRoadmap([FromRoute] Guid EmployeeId, RoadmapRequestDto roadmapRequestDto)
        {
            if (roadmapRequestDto is null)
            {
                return BadRequest("Must Enter Infromation");
            }

            var responce = await _roadmapService.CreateRoadmap(EmployeeId, roadmapRequestDto);

            return StatusCode(responce.StatusCode, responce);
        }

        //..........................................................(Get-All-RoadMaps).................................................

        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetRoadMaps([FromQuery] int? id, [FromQuery] string? title,
                                                  [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {

            var res = await _roadmapService.GetAllRoadMaps(id, title, pageNumber, pageSize);
            if (res == null)
            {
                return NotFound(res);
            }
            return StatusCode(res.StatusCode, res);
        }

        //..........................................................(Get-RoadMap).................................................

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoadMap([FromRoute] int id)
        {
            var RoadMap = await _roadmapService.GetRoadMap(id);

            if (RoadMap is null) return NotFound(RoadMap);

            return StatusCode(RoadMap.StatusCode, RoadMap);


        }
        //..........................................................(Update-RoadMap).................................................

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoadMap([FromRoute] int id, RoadmapUpdateDto roadmapUpdateDto)
        {
            var RoadMap = await _roadmapService.GetRoadMap(id);

            if (RoadMap is null) return NotFound(RoadMap);

            if (roadmapUpdateDto is null) return BadRequest("Invalid Value : Must Enter Correct Infomration");

            var responce = await _roadmapService.UpdateRoadmap(id, roadmapUpdateDto);

            return StatusCode(responce.StatusCode, responce);

        }
        //..........................................................(Delete-RoadMap).................................................

        [Authorize(Roles = "Admin")]

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoadMap([FromRoute] int id)
        {
            var RoadMap = await _roadmapService.GetRoadMap(id);

            if (RoadMap is null) return NotFound(RoadMap);

            var responce = await _roadmapService.DeleteRoadmap(id);

            return StatusCode(responce.StatusCode, responce);

        }

        //..........................................................(Create-Steps-For RoadMap).................................................
        [Authorize(Roles = "Admin")]

        [HttpPost("{RoadMapId}/steps")]
        public async Task<IActionResult> CreateStep([FromRoute] int RoadMapId, StepsRequestDto stepsRequest)
        {
            if (RoadMapId <= 0 || stepsRequest is null)
            {
                return BadRequest($"Must Enter Valid Information {nameof(RoadMapId)} , {nameof(stepsRequest)} ");
            }
            var responce = await _stepService.CreateSteps(RoadMapId, stepsRequest);

            return StatusCode(responce.StatusCode, responce);

        }


        //..........................................................(get-All-Steps Of RoadMap).................................................

        [Authorize(Roles = "Admin,User")]

        [HttpGet("{RoadMapId}/steps")]
        public async Task<IActionResult> GetSteps([FromRoute] int RoadMapId)
        {
            if (RoadMapId <= 0)
            {
                return BadRequest($"Must Enter Valid Information {nameof(RoadMapId)}");
            }
            var responce = await _stepService.GetAllSteps(RoadMapId);

            return StatusCode(responce.StatusCode, responce);

        }

        //..........................................................(get-on- Step).................................................

        [Authorize(Roles = "Admin,User")]

        [HttpGet("{RoadMapId}/steps/{stepId}")]
        public async Task<IActionResult> GetStep([FromRoute] int RoadMapId, [FromRoute] int stepId)
        {
            if (RoadMapId <= 0 || stepId <= 0)
            {
                return BadRequest($"Must Enter Valid Information {nameof(RoadMapId)} , {nameof(stepId)}");
            }
            var responce = await _stepService.GetStep(RoadMapId, stepId);

            return StatusCode(responce.StatusCode, responce);

        }

        //..........................................................(Update-step).................................................
        /// <param name="RoadMapId"></param>
        /// <param name="stepId"></param>
        /// <param name="stepUpdateDto">
        /// Status(   NotStarted,
        ///  InProgress,
        ///Completed)
        /// </param>
        /// <returns></returns>


        [Authorize(Roles = "Admin")]

        [HttpPut("{RoadMapId}/steps/{stepId}")]
        public async Task<IActionResult> UpdateStep([FromRoute] int RoadMapId, [FromRoute] int stepId, StepUpdateDto stepUpdateDto)
        {
            if (RoadMapId <= 0 || stepId <= 0 || stepUpdateDto is null)
            {
                return BadRequest($"Must Enter Valid Information {nameof(RoadMapId)} , {nameof(stepId)} and {nameof(stepUpdateDto)}");
            }
            var responce = await _stepService.UpdateStep(RoadMapId, stepId, stepUpdateDto);

            return StatusCode(responce.StatusCode, responce);


        }

        //..........................................................(Delete-step).................................................
        [Authorize(Roles = "Admin")]
        [HttpDelete("{RoadMapId}/steps/{stepId}")]
        public async Task<IActionResult> DelteStep([FromRoute] int RoadMapId, [FromRoute] int stepId)
        {
            if (RoadMapId <= 0 || stepId <= 0)
            {
                return BadRequest($"Must Enter Valid Information {nameof(RoadMapId)} , {nameof(stepId)}");
            }
            var responce = await _stepService.DeleteStep(RoadMapId, stepId);

            return StatusCode(responce.StatusCode, responce);


        }
    }

}
