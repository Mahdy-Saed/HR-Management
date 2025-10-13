using HR_Carrer.Dto.CertificateDtos;
using HR_Carrer.Services.CertificateService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        // post , get , get{id} , put , delete , get with skills

        private readonly ICertificateService _certificateService;
        public CertificatesController(ICertificateService certificateServcie)
        {
            _certificateService = certificateServcie;
        }

        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<IActionResult> CreateCertificate([FromBody]CertificateRequestDto certificateRequestDto)
        {
            if (certificateRequestDto is null)
                return BadRequest("Must enter information");

            var responce = await _certificateService.CreateCertificate(certificateRequestDto);

            return StatusCode(responce.StatusCode, responce);


        }

        [Authorize(Roles = "Admin")]

        [HttpPost("{CertificateId}/AddSkillsToCertificate")]
        public async Task<IActionResult> AddSkillsToCertificate([FromRoute] int CertificateId,[FromBody] CertificateSkillsReqDto certificateSkillsReq)
        {
            if (CertificateId <0 || certificateSkillsReq is null)
            {
                return BadRequest("INvalid Data: Must Enter Information");
            }
               

            var responce = await _certificateService.AddSkillsToCertificate(CertificateId,certificateSkillsReq);

            return StatusCode(responce.StatusCode, responce);


        }


        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> GetAllCertificates([FromQuery]int?id=null , [FromQuery] string? name=null,
                                                            [FromQuery] int pageNumber = 1,    [FromQuery] int pageSize = 10)
        {
            var responce = await _certificateService.GetAllCertificates(id,name,pageNumber,pageSize);

            return StatusCode(responce.StatusCode, responce);

        }

        [Authorize(Roles = "Admin")]

        [HttpGet("{id}/CoveredSkills")]
        public async Task<IActionResult> GetCertificateWithSkills([FromRoute]int id)
        {
            if ( id <= 0)
                return BadRequest("Invalid Certificate Id");
            var responce = await _certificateService.GetCertificateWithSkills(id);
            return StatusCode(responce.StatusCode, responce);
        }

        [Authorize(Roles = "Admin,User")]

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCertificate([FromRoute]int? id)
        {
            if (id is null || id <= 0)
                return BadRequest("Invalid Certificate Id");

            var responce = await _certificateService.GetCertificate(id);

            return StatusCode(responce.StatusCode, responce);
        }

        [Authorize(Roles = "Admin")]

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCertificate([FromRoute]int id, [FromQuery] CertificateUpdateDto certificateRequestDto)
        {
            if (id <= 0)
                return BadRequest("Invalid Certificate Id");
            if (certificateRequestDto is null)
                return BadRequest("Must enter information");
            // You might want to add a check to see if the certificate with the given id exists before updating.
            var responce = await _certificateService.UpdateCertificate(id,certificateRequestDto);
            return StatusCode(responce.StatusCode, responce);
        }


       



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCertificate([FromRoute]int id)
        {
            if (id <= 0)
                return BadRequest("Invalid Certificate Id");
            var responce = await _certificateService.DeleteCertificate(id);
            return StatusCode(responce.StatusCode, responce);
        }







    }
}
