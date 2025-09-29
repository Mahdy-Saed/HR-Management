using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.RoadmapDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoadmapController : ControllerBase
    {
        
        public RoadmapController()
        {
            
        }


        [HttpPost]
        public async Task<IActionResult>CreateRoadmap(Guid EmployeeId , RoadmapRequestDto roadmapRequestDto)
        {
            throw new NotImplementedException();
        }





    }
}
