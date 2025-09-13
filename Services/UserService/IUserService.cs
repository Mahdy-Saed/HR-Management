using HR_Carrer.Authntication;
using HR_Carrer.Data;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.AuthDtos;
using HR_Carrer.Dto.UserDtos;
using Microsoft.AspNetCore.JsonPatch;

namespace HR_Carrer.Services.UserService
{
    public interface IUserService
    {

        Task<ServiceResponce<List<UserResponceDto>>> GetAllUsers();

        Task<ServiceResponce<UserResponceDto>> GetUser(Guid id);


        Task<ServiceResponce<UserResponceDto>> CreateUser(UserRequestDto userRequestDto);

        Task<ServiceResponce<UserResponceDto>> UpdateUser(Guid id,UserUpdateDto userUpdateDto);

        Task<ServiceResponce<UserResponceDto>> PatchUser(Guid id, JsonPatchDocument<UserUpdateDto> userUpdateDto);


        Task<ServiceResponce<UserResponceDto>> DeleteUser(Guid id);

        Task<ServiceResponce<string>> DeleteAll();



    }
    public class UserService : IUserService
    {

        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepo userRepo, IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;

            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResponce<UserResponceDto>> CreateUser(UserRequestDto userRequestDto)
        {
 

            if (userRequestDto == null)
            {
                return ServiceResponce<UserResponceDto>.Fail("Must enter information ", 400);
            }

            var existingUser = await _userRepo.GetByEmailAsync(userRequestDto.Email!);
            if( existingUser is not null)
            {

                return ServiceResponce<UserResponceDto>.Fail("User is already Exist", 409);
 
            }

            var user = new User()
            {
                FullName = userRequestDto.FullName,
                Email = userRequestDto.Email,
                PasswordHash = _passwordHasher.Hash(userRequestDto.Password!),
                 ImagePath = userRequestDto.ImagePath,
                Status = userRequestDto.Status

            };
            user.Employee = new Employee()
            {
                UserId = user.Id,
            };
            await _userRepo.AddAsync(user);

            var responce = new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            };
            return ServiceResponce<UserResponceDto>.success(responce,"User created successfully ", 201);
             
        }

        public async Task<ServiceResponce<UserResponceDto>> Delete(Guid id)
        {
 
            var user = await _userRepo.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("User not found ", 404);

            }
            await _userRepo.DeleteAsync(id);
            var responce = new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            };

            return ServiceResponce<UserResponceDto>.success(responce, "User deleted successfully",200);


        }

        public async Task<ServiceResponce<string>> DeleteAll()
         {
 
            var count = await _userRepo.DeleteAllAsync();

            if (count == 0)
            {
                return ServiceResponce<string>.Fail("No users found to delete", 404);

            }
            
                return ServiceResponce<string>.success($"{count} users deleted", " Users deleted successfully", 200);
 
        }

        public Task<ServiceResponce<UserResponceDto>> DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponce<List<UserResponceDto>>> GetAllUsers()
        {
 
            var users = await   _userRepo.GetAllAsync();

            if (users is null || !users.Any())
            {
                return ServiceResponce<List<UserResponceDto>>.Fail("User not found ", 404);


            }
            var userDtos = users.Select(user => new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            }).ToList();

            return ServiceResponce<List<UserResponceDto>>.success(userDtos,"Users retrieved successfully", 200);
        }




        public async Task<ServiceResponce<UserResponceDto>> GetUser(Guid id)
        {
 
            var user = await _userRepo.GetByIdAsync(id);
            if(user is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("User not found ", 404);


                 
            }
            var responce = new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            };
             return ServiceResponce<UserResponceDto>.success(responce,"User retrieved successfully", 200);

        }

        public async Task<ServiceResponce<UserResponceDto>> UpdateUser(Guid id, UserUpdateDto userUpdateDto)
        {
 
            var user = await _userRepo.GetByIdAsync(id);
            if (user  is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("User not found ", 404);

            }
            user.FullName = userUpdateDto.FullName ?? user.FullName;
            user.ImagePath = userUpdateDto.ImagePath ?? user.ImagePath;
            user.Status = userUpdateDto.Status ?? user.Status;
            await _userRepo.UpdateAsync(user);
            
            var responce = new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            };

            return ServiceResponce<UserResponceDto>.success(responce, "User updated Sucessfully" ,200);


        }


        public async Task<ServiceResponce<UserResponceDto>> PatchUser(Guid id, JsonPatchDocument<UserUpdateDto> patchDoc)
        {
 
            if (patchDoc is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("Invalid patch document", 400);
            }

            var user = await _userRepo.GetByIdAsync(id);
            if (user is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("User not found", 404);
            }

             var userDto = new UserUpdateDto
            {
                FullName = user.FullName,
                ImagePath = user.ImagePath,
                Status = user.Status
            };

             patchDoc.ApplyTo(userDto);

             user.FullName = userDto.FullName;
            user.ImagePath = userDto.ImagePath;
            user.Status = userDto.Status;

            await _userRepo.UpdateAsync(user);

            var responce = new UserResponceDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                ImagePath = user.ImagePath,
                Status = user.Status,
                Role = new RoleResponseDto
                {
                    Id = user.Role!.Id,
                    Name = user.Role.Name
                }
            };
             

            return ServiceResponce<UserResponceDto>.success(responce, "User patched successfully", 200);
        }

    }



}
