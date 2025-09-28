using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.RequestDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services;
using HR_Carrer.Services.FileService;
using Microsoft.AspNetCore.JsonPatch;

namespace HR_Carrer.Services.RequestService
{
    public interface IRequestService
    {
        Task<ServiceResponce<RequestResponceDto>> CreateRequest(Guid id, ReqeustCreateDto reqeustCreateDto);

        Task<ServiceResponce<string>> UploadRequestImage(int RequestId, IFormFile Image);

        Task<ServiceResponce<PagedResultDto<RequestResponceDto>>> GetALlService(int? id, string? name = null, int pageNumber = 1, int pageSize = 10);

        Task<ServiceResponce<string>> UpdateRequestStatus(int id, RequestStatusDto requestStatusDto);

        Task<ServiceResponce<string>> DeleteRequestImage(int id);

        //Task<ServiceResponce<UserResponceDto>> CreateUser(UserRequestDto userRequestDto);

        //Task<ServiceResponce<UserResponceDto>> UpdateUser(Guid id, UserUpdateDto userUpdateDto);

        //Task<ServiceResponce<UserResponceDto>> PatchUser(Guid id, JsonPatchDocument<UserUpdateDto> userUpdateDto);

        //Task<ServiceResponce<string>> UploadImage(Guid id, IFormFile Image);

        //Task<ServiceResponce<string>> DeleteImage(Guid id);

        //Task<ServiceResponce<string>> DeleteUser(Guid id);

        //Task<ServiceResponce<string>> DeleteAll();



    }



    public class ReqeustService : IRequestService
    {
        private readonly IRequestRepo _requestRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;

        public ReqeustService(IRequestRepo requestRepo, IEmployeeRepo employeeRepo, IMapper mapper, IFileService fileService)
        {

            _requestRepo = requestRepo;
            _employeeRepo = employeeRepo;
            _mapper = mapper;
            _fileService = fileService;
        }

        // .................................(Create-Request).......................................
        public async Task<ServiceResponce<RequestResponceDto>> CreateRequest(Guid id, ReqeustCreateDto reqeustCreateDto)
        {
            var Employee = await _employeeRepo.GetByIdAsync(id);

            if (Employee is null) return ServiceResponce<RequestResponceDto>.Fail("Employee Not Found", 404);

            if (reqeustCreateDto is null) return ServiceResponce<RequestResponceDto>.Fail("Request Data is null", 400);

            var request = _mapper.Map<Requests>(reqeustCreateDto);

            request.EmployeeId = Employee.UserId;
            request.Request_Date = DateTime.UtcNow;
            request.Status = RequestStatus.Pending;
            await _requestRepo.AddAsync(request);

            var responceDtp = _mapper.Map<RequestResponceDto>(request);

            return ServiceResponce<RequestResponceDto>.success(responceDtp, "Reqeust Added sucessfully", 200);

        }

        // .................................(Get-All-Request).......................................

        public async Task<ServiceResponce<PagedResultDto<RequestResponceDto>>> GetALlService(int? id, string? name = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = await _requestRepo.GetAllWithQuery(id, name);

            if (query is null || !query.Any())
                return ServiceResponce<PagedResultDto<RequestResponceDto>>.Fail("User not found", 404);

            if (pageNumber <= 0) pageNumber = 1;

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedUser = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var requests = _mapper.Map<List<RequestResponceDto>>(pagedUser.ToList());

            var responce = new PagedResultDto<RequestResponceDto>
            {
                Items = requests,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
            return ServiceResponce<PagedResultDto<RequestResponceDto>>.success(responce, "Users retrieved successfully", 200);
        }


        // .................................(Upload-Request-Image).......................................

        public async Task<ServiceResponce<string>> UploadRequestImage(int RequestId, IFormFile Image)
        {

            var requests = await _requestRepo.GetByIdAsync(RequestId);
            if (requests is null) return ServiceResponce<string>.Fail("Request not found", 404);

            if (Image is null) return ServiceResponce<string>.Fail("Image is required", 400);

            string? ImagePath = string.Empty;
            if (Image is not null)
            {
                if (!string.IsNullOrWhiteSpace(requests.ImagePath))
                    _fileService.DeleteFile(requests.ImagePath);

                ImagePath = await _fileService.SaveImage(Image);

                if (ImagePath == "File type not allowed")
                {
                    return ServiceResponce<string>.Fail("File type not allowed", 400);
                }
            }
            requests.ImagePath = ImagePath ?? requests.ImagePath;

            await _requestRepo.UpdateAsync(requests);
            return ServiceResponce<string>.success(requests.ImagePath!, "Image Uploaded ccessfully", 200);

        }

        // .................................(Delete-Request-Image).......................................

        public async Task<ServiceResponce<string>> DeleteRequestImage(int id)
        {

            var request = await _requestRepo.GetByIdAsync(id);
            if (request is null) return ServiceResponce<string>.Fail("Request not found", 404);

            if (string.IsNullOrWhiteSpace(request.ImagePath))
            {
                return ServiceResponce<string>.Fail("No image to delete", 400);
            }
            var result = _fileService.DeleteFile(request.ImagePath);
            if (result == "Invalid Path")
            {
                return ServiceResponce<string>.Fail("Invalid image path", 400);
            }
            request.ImagePath = null;
            await _requestRepo.UpdateAsync(request);
            return ServiceResponce<string>.success("Image deleted successfully", "", 200);
        }

        // .................................(Update-Request-Status).......................................
        public async Task<ServiceResponce<string>> UpdateRequestStatus(int id, RequestStatusDto requestStatusDto)
        {   
            var request = await  _requestRepo.GetByIdAsync(id);

            if(request is null) return ServiceResponce<string>.Fail("Request not found", 404);

            if(requestStatusDto is null) return ServiceResponce<string>.Fail("Status is required", 400);

            request.Status= requestStatusDto.status;
            if(requestStatusDto.status== RequestStatus.Approved)
            {
                request.Approved_Date = DateTime.UtcNow;
            }

            await _requestRepo.UpdateAsync(request);

            return ServiceResponce<string>.success("Request status updated successfully", "", 200);
        }
    }
}
