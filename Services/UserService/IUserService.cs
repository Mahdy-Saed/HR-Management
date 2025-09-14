using HR_Carrer.Authntication;
using HR_Carrer.Data;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.AuthDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.FileService;
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

        Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image);

         Task<ServiceResponce<string>> DeleteImage(Guid id);

        Task<ServiceResponce<string>> DeleteUser(Guid id);

        Task<ServiceResponce<string>> DeleteAll();



    }
    public class UserService : IUserService
    {

        private readonly IUserRepo _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileService _fileService;
        public UserService(IUserRepo userRepo, IPasswordHasher passwordHasher, IFileService fileService)
        {
            _userRepo = userRepo;
            _fileService = fileService;
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

            string? imagePath = string.Empty;
            if (userRequestDto.Image is not null)
            {
                  imagePath = await _fileService.SaveImage( userRequestDto.Image);
                if(imagePath is null)
                {
                    return ServiceResponce<UserResponceDto>.Fail("Invalid Image format. Only .jpg and .png are allowed.", 400);
                }
            
            }

            var user = new User()
            {
                FullName = userRequestDto.FullName,
                Email = userRequestDto.Email,
                PasswordHash = _passwordHasher.Hash(userRequestDto.Password!),
                 ImagePath = imagePath,
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
        public async Task<ServiceResponce<string>> DeleteAll()
        {
            try
            {
                var count = await _userRepo.DeleteAllAsync();

                if (count == 0)
                    return ServiceResponce<string>.Fail("No users found to delete", 404);

                return ServiceResponce<string>.success($"{count} users deleted", "Users deleted successfully", 200);
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
                return ServiceResponce<string>.Fail("Failed to delete users", 500);
            }
        }

        public async Task<ServiceResponce<string>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user is null)
                    return ServiceResponce<string>.Fail("User not found", 404);

                await _userRepo.DeleteAsync(id);

                return ServiceResponce<string>.success("User deleted successfully", "", 200);
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
                return ServiceResponce<string>.Fail("Failed to delete user", 500);
            }
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
            user.FullName = userUpdateDto.FullName ?? user.FullName;  // if the value is null keep the old value
            user.Email = userUpdateDto.Email ?? user.Email;
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

            //convert the current user to userDto to apply the patch of changes.
             var userDto = new UserUpdateDto
            {
                FullName = user.FullName,
                Email= user.Email,
                 Status = user.Status
            };

             patchDoc.ApplyTo(userDto);

             user.FullName = userDto.FullName;
             user.Email= userDto.Email;
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

        public async Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image)
        {
            
            var user = await _userRepo.GetByIdAsync(id);
            if(user is null) return ServiceResponce<string>.Fail("User not found",404);

            if(Image is null) return ServiceResponce<string>.Fail("Image is required",400);

            string? ImagePath = string.Empty;
            if (Image is not null)
            {
                 _fileService.DeleteFile(user.ImagePath);

                ImagePath = await _fileService.SaveImage(Image);

            }

            user.ImagePath = ImagePath ?? user.ImagePath;

            await _userRepo.UpdateAsync(user);


            return ServiceResponce<string>.success(user.ImagePath!, "Image updated successfully", 200);

        }

        public async Task<ServiceResponce<string>> DeleteImage(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            if (string.IsNullOrWhiteSpace(user.ImagePath))
            {
                return ServiceResponce<string>.Fail("No image to delete", 400);
            }

            _fileService.DeleteFile(user.ImagePath);

            user.ImagePath = null;

            await _userRepo.UpdateAsync(user);
            return ServiceResponce<string>.success("Image deleted successfully", "", 200);


        }
    }



}
