using HR_Carrer.Data.Repositery;
using HR_Carrer.Services.FileService;

namespace HR_Carrer.Services.AttachmentService
{
    public interface IAttachmentService
    {

        Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image);

        Task<ServiceResponce<string>> DeleteImage(Guid id);


    }


    public class AttachmentService : IAttachmentService
    {
        private readonly IUserRepo _userRepo;
        private readonly IFileService _fileService;
        public AttachmentService(IUserRepo userRepo , IFileService fileService)
        {
            _fileService = fileService;
            _userRepo = userRepo;
        }

        //................................................(Delete-Image).....................................................


        public async Task<ServiceResponce<string>> DeleteImage(Guid id)
        {

            var user = await _userRepo.GetByIdAsync(id);
            if (user is null) return ServiceResponce<string>.Fail("User not found", 404);

            if (string.IsNullOrWhiteSpace(user.ImagePath))
            {
                return ServiceResponce<string>.Fail("No image to delete", 400);
            }

            var result= _fileService.DeleteFile(user.ImagePath);
            if(result== "Invalid Path")
            {
                return ServiceResponce<string>.Fail("Invalid image path", 400);
            }
            user.ImagePath = null;
            await _userRepo.UpdateAsync(user);

            return ServiceResponce<string>.success("Image deleted successfully", "", 200);
        }
        //................................................(Upload-Image).....................................................
        public async Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image)
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
            return ServiceResponce<string>.success(user.ImagePath!, "Image updated successfully", 200);
        }
    }

}
