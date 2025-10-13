using HR_Carrer.Dto.SkillsDtos;
using HR_Carrer.Services.SkillService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllSkills([FromQuery] int? id, [FromQuery] string? name,
                                                    [FromQuery] int pageNumber = 1,    [FromQuery] int pageSize = 10 )
        {
            var responce = await _skillService.GetAllSkills(id,name, pageNumber, pageSize);

            return StatusCode(responce.StatusCode, responce);

        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> CereateSkill(SkillCreateDto  skillCreateDto)
        {
            

            if (skillCreateDto is null) { return BadRequest("Must Enter the information"); }

            var responce = await _skillService.CreateSkill(skillCreateDto);

            return StatusCode(responce.StatusCode, responce);

        }

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateSkill([FromRoute] int id ,[FromBody]SkillUpdateDto skillUpdateDto)
        {
            if (skillUpdateDto is null) { return BadRequest("Must Enter the infromation"); }

            var responce = await _skillService.UpdateSkill(id, skillUpdateDto);

            return StatusCode(responce.StatusCode, responce);
        }



        [Authorize(Roles = "Admin")]


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkills([FromRoute]int id)
        {

            if (id <= 0) { return BadRequest("Must Enter the id"); }

            var responce = await _skillService.DeleteSkill(id);

            return StatusCode(responce.StatusCode, responce);

        }



    }
}
