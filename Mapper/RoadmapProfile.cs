using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.RoadmapDtos;

namespace HR_Carrer.Mapper
{
    public class RoadmapProfile:Profile
    {
        public RoadmapProfile()
        {
            CreateMap<RoadmapRequestDto, Roadmap>();

            CreateMap<Roadmap, RoadmapResponceDto>();
        }




    }
}
