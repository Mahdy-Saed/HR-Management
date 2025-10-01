using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.CertificateDtos;

namespace HR_Carrer.Mapper
{
    public class CertificateProfile:Profile
    {

        public CertificateProfile()
        {
            CreateMap<CertificateRequestDto, Certificates>();

           // CreateMap<Certificates, CertificateResponceDto>().ForMember(des=>des.level,opt=>opt.MapFrom(src=>src.level.ToString()));

            CreateMap<Certificates, CertificateResponceDto>().ForMember(des => des.level, opt => opt.MapFrom("level"));

        }

    }
}
