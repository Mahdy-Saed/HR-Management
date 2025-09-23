using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.EmployeeDtos;

namespace HR_Carrer.Mapper
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeRequestDto, Employee>()
              .ForMember(dest => dest.User, opt => opt.Ignore())  // we make it igonre because we will hash it in the service layer
              .ForMember(des => des.UserId, opt => opt.Ignore());


            CreateMap<Employee, EmployeeCreateResDto>();

            CreateMap<Employee, EmployeeResponceDto>();


            CreateMap<User, EmployeeResponceDto>()
                  .ForMember(des => des.ImagePath, opt => opt.MapFrom(src => src.ImagePath)) 
                   .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.PhoneNumber : null))
                     .ForMember(dest => dest.HiredDate, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.HiredDate : null))
                    .ForMember(dest => dest.Postion, opt => opt.MapFrom(src => src.Employee != null ? src.Employee.Postion : null));

        }
    }
}
