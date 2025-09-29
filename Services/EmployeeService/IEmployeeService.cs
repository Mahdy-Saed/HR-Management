using AutoMapper;
using HR_Carrer.Authntication;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.EmployeeDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services;
using HR_Carrer.Services.Utility;
using Microsoft.AspNetCore.JsonPatch;
using System.Security.Cryptography.X509Certificates;

namespace HR_Carrer.Services.EmployeeService
{
    public interface IRoadmapService
    {
        Task<ServiceResponce<EmployeeCreateResDto>> CreateEmployee(Guid id,EmployeeRequestDto userRequestDto);

        Task<ServiceResponce<EmployeeResponceDto>> UpdateEmployee(Guid id, EmployeeUpdateDto EmployeeUpdateDto);

        Task<ServiceResponce<EmployeeResponceDto>> GetInoformation(Guid id);
            
        Task<ServiceResponce<string>> ChangePassword(Guid id, ChangePasswordDto changePasswordDto);
        Task<ServiceResponce<string>> DeleteEmployee(Guid id);

        //Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image);

        //Task<ServiceResponce<string>> DeleteImage(Guid id);

        //Task<ServiceResponce<string>> DeleteUser(Guid id);





    }

    public class EmployeeService : IRoadmapService
    {

        private readonly IEmployeeRepo _employeeRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public EmployeeService(IEmployeeRepo employeeRepo, IUserRepo userRepo, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _employeeRepo = employeeRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }
        //................................................(Create-Employee).....................................................

        public async Task<ServiceResponce<EmployeeCreateResDto>>CreateEmployee(Guid id, EmployeeRequestDto userRequestDto)
        {
            var user = await _userRepo.GetByIdAsync(id);
             if (user is null)  return ServiceResponce<EmployeeCreateResDto>.Fail("User not found", 404); 
            if(userRequestDto is null) return ServiceResponce<EmployeeCreateResDto>.Fail("Must enter information", 400);

            var employeeExist = await _employeeRepo.GetByIdAsync(id);
            if(employeeExist != null) return ServiceResponce<EmployeeCreateResDto>.Fail(id+"  Employee is already exist", 400);


            var employee = _mapper.Map<Employee>(userRequestDto);
            
            employee.User = user;
            employee.UserId = user.Id;

            await _employeeRepo.AddAsync(employee);

            var responce = _mapper.Map<EmployeeCreateResDto>(employee);

            return ServiceResponce<EmployeeCreateResDto>.success(responce, "Employee Created Successfully", 201);
        }



        //................................................(Update-Employee).....................................................


        public async Task<ServiceResponce<EmployeeResponceDto>> UpdateEmployee(Guid id, EmployeeUpdateDto EmployeeUpdateDto)
        {
            var user = await _userRepo.GetByIdAsync(id);  // return with employee data
            if(user is null) return ServiceResponce<EmployeeResponceDto>.Fail("User not found", 404);

            if(EmployeeUpdateDto is null) return ServiceResponce<EmployeeResponceDto>.Fail("Must enter information", 400);

            // extension method that will check if the property is not null or empty and then will update it

            EntityUpdater.UpdateEntity(user, EmployeeUpdateDto);
            EntityUpdater.UpdateEntity(user.Employee, EmployeeUpdateDto);

 
            await _userRepo.UpdateAsync(user);
            if(user.Employee != null)   
                await _employeeRepo.UpdateAsync(user.Employee);
            var responce = _mapper.Map<EmployeeResponceDto>(user);

            return ServiceResponce<EmployeeResponceDto>.success(responce, "Employee Updated Successfully", 200);


        }
        //................................................(Get-information).....................................................


     public async   Task<ServiceResponce<EmployeeResponceDto>> GetInoformation(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);  // return with employee data

            if (user is null) return ServiceResponce<EmployeeResponceDto>.Fail("User not found", 404);

            var responce = _mapper.Map<EmployeeResponceDto>(user);


            return ServiceResponce<EmployeeResponceDto>.success(responce, "information retrived Successfully", 200);
        }

        //................................................(Delete-Employee).....................................................


        // here i will not use the cascading delete because i want to make sure that the user is deleted first then the employee
        public async Task<ServiceResponce<string>> DeleteEmployee(Guid id)
        {
            var employee = await _employeeRepo.GetByIdAsync(id);
            if (employee is null) return ServiceResponce<string>.Fail("Employee not found", 404);

            var user = employee.User;
            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            await _userRepo.DeleteAsync(user.Id);

            await _employeeRepo.DeleteAsync(id);

            return ServiceResponce<string>.success(null, "Employee Deleted Successfully", 200);
        }

            //................................................(Delete-Employee).....................................................


       public async Task<ServiceResponce<string>> ChangePassword(Guid id, ChangePasswordDto changePasswordDto)
        {
            var user = await _userRepo.GetByIdAsync(id);

            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            if (changePasswordDto is null) return ServiceResponce<string>.Fail("Must enter information", 400);

            if (!_passwordHasher.verify(changePasswordDto.OldPassword!, user.PasswordHash!))
            {
                return ServiceResponce<string>.Fail("Old password is incorrect", 400);
            }

            if(changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                return ServiceResponce<string>.Fail("New password and confirm new password do not match", 400);
            }

            user.PasswordHash = _passwordHasher.Hash(changePasswordDto.NewPassword!);

            await _userRepo.UpdateAsync(user);

            return ServiceResponce<string>.success(null, "Password Changed Successfully", 200);



        }



        }

    }



