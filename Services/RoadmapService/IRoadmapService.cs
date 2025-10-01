using AutoMapper;
using HR_Carrer.Authntication;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.EmployeeDtos;
using HR_Carrer.Dto.RoadmapDtos;
using HR_Carrer.Dto.StepDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services;
using HR_Carrer.Services.Utility;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Cryptography.X509Certificates;

namespace HR_Carrer.Services.RoadmapService
{
    public interface IRoadmapService
    {
        Task<ServiceResponce<RoadmapResponceDto>> CreateRoadmap(Guid id, RoadmapRequestDto roadmapRequestDto);

        Task<ServiceResponce<StepsResponceDto>> CreateSteps (int roadMapId , StepsRequestDto stepsRequestDto );

        //Task<ServiceResponce<EmployeeResponceDto>> GetInoformation(Guid id);

        //Task<ServiceResponce<string>> ChangePassword(Guid id, ChangePasswordDto changePasswordDto);
        //Task<ServiceResponce<string>> DeleteEmployee(Guid id);

        //Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image);

        //Task<ServiceResponce<string>> DeleteImage(Guid id);

        //Task<ServiceResponce<string>> DeleteUser(Guid id);





    }

    public class RoadmapService : IRoadmapService
    {

        private readonly IEmployeeRepo _employeeRepo;
        private readonly IRoadmapRepo _roadmapRepo;
        private readonly IRequestRepo _requestRepo;
        private readonly IStepsRepo _stepsRepo;
        private readonly IMapper _mapper;

        public RoadmapService(IEmployeeRepo employeeRepo, IRoadmapRepo roadmapRepo, IMapper mapper, IRequestRepo requestRepo, IStepsRepo stepsRepo)
        {

            _employeeRepo = employeeRepo;
            _roadmapRepo = roadmapRepo;
            _mapper = mapper;
            _requestRepo = requestRepo;
            _stepsRepo = stepsRepo;
        }
        //................................................(Create-Roadmap).....................................................

        public async Task<ServiceResponce<RoadmapResponceDto>> CreateRoadmap(Guid id, RoadmapRequestDto roadmapRequestDto)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee is null) return ServiceResponce<RoadmapResponceDto>.Fail("Employee not found", 404);

            if (roadmapRequestDto is null)
            {
                return ServiceResponce<RoadmapResponceDto>.Fail("Must enter information", 400);
            
            }
            
              var requestOfEmployee = await _requestRepo.promotionRequest(id);

            if (requestOfEmployee == null)
            {
                return ServiceResponce<RoadmapResponceDto>.Fail("The employee Did Not send a request for promotion.", 400);
            
            }
            if (requestOfEmployee.Status != RequestStatus.Approved)
            {
                return ServiceResponce<RoadmapResponceDto>.Fail("The employee's promotion request has not been accepted yet.", 400);
            }

            var Roadmap = _mapper.Map<Roadmap>(roadmapRequestDto); 
            Roadmap.EmployeeId = id;

            await _roadmapRepo.AddAsync(Roadmap);

            var responce = _mapper.Map<RoadmapResponceDto>(Roadmap);
            return ServiceResponce<RoadmapResponceDto>.success(responce, "Roadmap Created Successfully", 201);
        }



        //................................................(Create-Steps).....................................................

        public async Task<ServiceResponce<StepsResponceDto>> CreateSteps(int roadMapId, StepsRequestDto stepsRequestDto)
        {
            var roadmap = await _roadmapRepo.GetByIdAsync(roadMapId);
            if (roadmap is null) return ServiceResponce<StepsResponceDto>.Fail("Roadmap not found or must create RoadMap", 404);
            if (stepsRequestDto is null)
            {
                return ServiceResponce<StepsResponceDto>.Fail("Must enter information", 400);
            }
            var step = _mapper.Map<Steps>(stepsRequestDto);

            // link the step with the roadmap and the certificate
            step.RoadmapId = roadMapId;
            step.CertificateId = stepsRequestDto.CertificatedId; 

            await _stepsRepo.AddAsync(step);
            var responce = _mapper.Map<StepsResponceDto>(roadmap.Steps);
            return ServiceResponce<StepsResponceDto>.success(responce, "Step Created Successfully", 201);
        }


        //   //................................................(Get-information).....................................................


        //public async   Task<ServiceResponce<EmployeeResponceDto>> GetInoformation(Guid id)
        //   {
        //       var user = await _userRepo.GetByIdAsync(id);  // return with employee data

        //       if (user is null) return ServiceResponce<EmployeeResponceDto>.Fail("User not found", 404);

        //       var responce = _mapper.Map<EmployeeResponceDto>(user);


        //       return ServiceResponce<EmployeeResponceDto>.success(responce, "information retrived Successfully", 200);
        //   }

        //   //................................................(Delete-Employee).....................................................


        //   // here i will not use the cascading delete because i want to make sure that the user is deleted first then the employee
        //   public async Task<ServiceResponce<string>> DeleteEmployee(Guid id)
        //   {
        //       var employee = await _employeeRepo.GetByIdAsync(id);
        //       if (employee is null) return ServiceResponce<string>.Fail("Employee not found", 404);

        //       var user = employee.User;
        //       if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

        //       await _userRepo.DeleteAsync(user.Id);

        //       await _employeeRepo.DeleteAsync(id);

        //       return ServiceResponce<string>.success(null, "Employee Deleted Successfully", 200);
        //   }

        //       //................................................(Delete-Employee).....................................................


        //  public async Task<ServiceResponce<string>> ChangePassword(Guid id, ChangePasswordDto changePasswordDto)
        //   {
        //       var user = await _userRepo.GetByIdAsync(id);

        //       if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

        //       if (changePasswordDto is null) return ServiceResponce<string>.Fail("Must enter information", 400);

        //       if (!_passwordHasher.verify(changePasswordDto.OldPassword!, user.PasswordHash!))
        //       {
        //           return ServiceResponce<string>.Fail("Old password is incorrect", 400);
        //       }

        //       if(changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
        //       {
        //           return ServiceResponce<string>.Fail("New password and confirm new password do not match", 400);
        //       }

        //       user.PasswordHash = _passwordHasher.Hash(changePasswordDto.NewPassword!);

        //       await _userRepo.UpdateAsync(user);

        //       return ServiceResponce<string>.success(null, "Password Changed Successfully", 200);



        //   }



        //   }

    }
}



