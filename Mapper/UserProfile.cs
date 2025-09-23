using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Dto.UserDtos;

namespace HR_Carrer.Mapper
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<UserRequestDto, User>()
              .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // we make it igonre because we will hash it in the service layer
              //.ForMember(dest => dest.Employee, opt => opt.MapFrom(src=>new Employee()))
              .ForMember(des=>des.RoleId,opt=>opt.MapFrom(src=>2))
              .ForMember(des => des.Status, opt => opt.MapFrom(src =>true));

            CreateMap<User, UserResponceDto>()
              .ForMember(des => des.Role, opt => opt.MapFrom(src => src.Role == null
          ? new RoleResponseDto { Id = src.RoleId, Name = "Employee" } // fallback
           : new RoleResponseDto { Id = src.Role.Id, Name = src.Role.Name }));

        }




    }
}
