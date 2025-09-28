using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.EmployeeDtos;
using HR_Carrer.Dto.RequestDtos;
using Org.BouncyCastle.Asn1.Ocsp;

namespace HR_Carrer.Mapper
{
    public class RequestProfile :Profile
    {

        public RequestProfile()
        {
            CreateMap<ReqeustCreateDto, Requests>();

            CreateMap<Requests, RequestResponceDto>();


        }

    }
}
