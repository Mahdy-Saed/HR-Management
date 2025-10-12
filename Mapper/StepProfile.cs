using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.StepDtos;

namespace HR_Carrer.Mapper
{
    public class StepProfile:Profile
    {
        public StepProfile()
        {
            CreateMap<StepsRequestDto, Steps>();

            CreateMap<Steps, StepsResponceDto>().ForMember(des => des.CertificateId, opt => opt.MapFrom(src=>src.CertificateId));
        }


    }
}
