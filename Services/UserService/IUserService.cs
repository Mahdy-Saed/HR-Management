using AutoMapper;
using HR_Carrer.Authntication;
using HR_Carrer.Data;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.AuthDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.FileService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HR_Carrer.Services.UserService
{
    public interface IUserService
    {

        Task<ServiceResponce<PagedResultDto<UserResponceDto>>> GetAllUsers(Guid? id = null, string? name = null,
                                                                string? email=null   , int pageNumber=1 ,int pageSize=10);

        Task<ServiceResponce<UserResponceDto>> CreateUser(UserRequestDto userRequestDto);

        Task<ServiceResponce<UserResponceDto>> UpdateUser(Guid id,UserUpdateDto userUpdateDto);

        Task<ServiceResponce<UserResponceDto>> PatchUser(Guid id, JsonPatchDocument<UserUpdateDto> userUpdateDto);

        Task<ServiceResponce<string>> UploadProfileImage(Guid id, IFormFile Image);

         Task<ServiceResponce<string>> DeleteProfileImage(Guid id);

        Task<ServiceResponce<string>> DeleteUser(Guid id);

        Task<ServiceResponce<string>> DeleteAll();

    }
    public class UserService : IUserService
    {

        private readonly IUserRepo _userRepo;
         private readonly IPasswordHasher _passwordHasher;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        public UserService(IUserRepo userRepo, IPasswordHasher passwordHasher, IFileService fileService,IMapper mapper)
        {
            _userRepo = userRepo;
            _fileService = fileService;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }


        // ...............................................(Create-User).....................................................

        public async Task<ServiceResponce<UserResponceDto>> CreateUser(UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
                return ServiceResponce<UserResponceDto>.Fail("Must enter information", 400);

            var existingUser = await _userRepo.GetByEmailAsync(userRequestDto.Email!);
            if (existingUser is not null)
                return ServiceResponce<UserResponceDto>.Fail("User already exists", 409);


            var user = _mapper.Map<User>(userRequestDto);
            user.PasswordHash = _passwordHasher.Hash(userRequestDto.Password!);
            var role = await _userRepo.Getrole(2); // get the role of User.

            user.RoleId = role?.Id ?? 0;
            user.Role = role;

            await _userRepo.AddAsync(user);

            var responce = _mapper.Map<UserResponceDto>(user);
 
            return ServiceResponce<UserResponceDto>.success(responce, "User Created Successfully", 201);
        }



        // ...............................................(Delete-All-User).....................................................



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
// ...............................................(Delete-User).....................................................



        public async Task<ServiceResponce<string>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepo.GetByIdAsync(id);
                if (user is null)
                    return ServiceResponce<string>.Fail("User not found", 404);
                if(user.ImagePath is not null)
                {
                      _fileService.DeleteFile(user.ImagePath);
                }

                    await _userRepo.DeleteAsync(id);

                return ServiceResponce<string>.success("User deleted successfully", "", 200);
            }
            catch (Exception ex)
            {
                 Console.WriteLine(ex.Message);
                return ServiceResponce<string>.Fail("Failed to delete user", 500);
            }
        }

 // ...................................................(Get-ALl-Users).......................................................


        public async Task<ServiceResponce<PagedResultDto<UserResponceDto>>> GetAllUsers(Guid? id = null, string? name = null, string? email = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = await _userRepo.GetAllAsync(id,name,email);

            if (query is null || !query.Any())
                return ServiceResponce<PagedResultDto<UserResponceDto>>.Fail("User not found", 404);

            if(pageNumber <= 0) pageNumber = 1;

            var totalCount = query.Count();
            var totalPages =(int) Math.Ceiling(totalCount / (double)pageSize);
            var pagedUser = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var userDtos = _mapper.Map<List<UserResponceDto>>(pagedUser.ToList());

            var responce= new PagedResultDto<UserResponceDto>
            {
                Items = userDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return ServiceResponce<PagedResultDto<UserResponceDto>>.success(responce, "Users retrieved successfully", 200);
        }

  // ...............................................(UPdate-User).....................................................


        public async Task<ServiceResponce<UserResponceDto>> UpdateUser(Guid id, UserUpdateDto userUpdateDto)
        {
 
            var user = await _userRepo.GetByIdAsync(id);
            if (user  is null)
            {
                return ServiceResponce<UserResponceDto>.Fail("User not found ", 404);

            }
            user.FullName = userUpdateDto.NewFullName ?? user.FullName;  // if the value is null keep the old value
            user.Email = userUpdateDto.NewEmail ?? user.Email;
            user.Status = userUpdateDto.NewStatus ?? user.Status;

           

            await _userRepo.UpdateAsync(user);

            var responce = _mapper.Map < UserResponceDto>(user);
 
            return ServiceResponce<UserResponceDto>.success(responce, "User updated Sucessfully" ,200);


        }


        // ...............................................(Update-part-User).....................................................

    
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
                NewFullName = user.FullName,
                NewEmail= user.Email,
                 NewStatus = user.Status
            };

             patchDoc.ApplyTo(userDto);

             user.FullName = userDto.NewFullName;
             user.Email= userDto.NewEmail;
             user.Status = userDto.NewStatus;

            await _userRepo.UpdateAsync(user);

            var responce = _mapper.Map<UserResponceDto>(user);

            
            return ServiceResponce<UserResponceDto>.success(responce, "User patched successfully", 200);
        }

        //         // ...............................................(Upload-Image).....................................................

        public async Task<ServiceResponce<string>> UploadProfileImage(Guid id, IFormFile Image)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            if (Image is null) return ServiceResponce<string>.Fail("Image is required", 400);

            string? ImagePath = string.Empty;
            if (Image is not null)
            {
                if (!string.IsNullOrWhiteSpace(user.ImagePath))
                    _fileService.DeleteFile(user.ImagePath);

                ImagePath = await _fileService.SaveImage(Image);

                if (ImagePath == "File type not allowed")
                {
                    return ServiceResponce<string>.Fail("File type not allowed", 400);
                }
            }
            user.ImagePath = ImagePath ?? user.ImagePath;

            await _userRepo.UpdateAsync(user);
            return ServiceResponce<string>.success(user.ImagePath!, "Image Uploaded ccessfully", 200);
        }


        // ...............................................(Delete-Image).....................................................
        public async Task<ServiceResponce<string>> DeleteProfileImage(Guid id)
        {

            var user = await _userRepo.GetByIdAsync(id);
            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            if (string.IsNullOrWhiteSpace(user.ImagePath))
            {
                return ServiceResponce<string>.Fail("No image to delete", 400);
            }

            var result = _fileService.DeleteFile(user.ImagePath);
            if (result == "Invalid Path" || result == "File Not Found")
            {
                return ServiceResponce<string>.Fail("Invalid image path or the file not found", 400);
            }
            user.ImagePath = null;
            await _userRepo.UpdateAsync(user);

            return ServiceResponce<string>.success("Image deleted successfully", "", 200);
        }

    }



}
