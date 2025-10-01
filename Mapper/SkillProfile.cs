using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.SkillsDtos;

namespace HR_Carrer.Mapper
{
    public class SkillProfile:Profile
    {
        public SkillProfile()
        {
            CreateMap<Skills, SkillResponceDto>()
                .ForMember(des => des.Level, opt => opt.MapFrom(src => src.Level.ToString()));
        }

    }
}
