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

        Task<ServiceResponce<RoadmapResponceDto>> GetRoadMap(int id);

        Task<ServiceResponce<RoadmapResponceDto>> UpdateRoadmap(int id,RoadmapUpdateDto roadmapUpdateDto);

        Task<ServiceResponce<PagedResultDto<RoadmapResponceDto>>> GetAllRoadMaps(int? id = null, string? title = null,
                                                                                     int pageNumber = 1, int pageSize = 10);
        Task<ServiceResponce<string>> DeleteRoadmap(int id);

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

            var exsistRoadMap = await _roadmapRepo.GetByEmployeeId(id,roadmapRequestDto.Title);

            if(exsistRoadMap is not null)
            {
                return ServiceResponce<RoadmapResponceDto>.Fail("THe RoadMap for the Employee is ALready exsit", 409);
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

 //................................................(Get-information).....................................................

        public async Task<ServiceResponce<RoadmapResponceDto>> GetRoadMap(int id)
        {
            if(id<0) throw new ArgumentOutOfRangeException("id");

            var RoadMap = await _roadmapRepo.GetByIdAsync(id);

            if(RoadMap is null) { return ServiceResponce<RoadmapResponceDto>.Fail("Roadmap Doesnot Exsist ", 400); }


            var RoadMapDto =_mapper.Map<RoadmapResponceDto>(RoadMap);

            return   ServiceResponce<RoadmapResponceDto>.success(RoadMapDto,"Roadmap returived Successfully", 200);
        }


        //................................................(Get-All-information).....................................................

        public async Task<ServiceResponce<PagedResultDto<RoadmapResponceDto>>> GetAllRoadMaps(int? id = null, string? title = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = await _roadmapRepo.GetAllAsyncWithQuery(id, title);

            if (query is null || !query.Any())
                return ServiceResponce<PagedResultDto<RoadmapResponceDto>>.Fail("roadmaps  not found", 404);

            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedRoadmaps = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var RoadmapsDto = _mapper.Map<List<RoadmapResponceDto>>(pagedRoadmaps.ToList());

            var responce = new PagedResultDto<RoadmapResponceDto>
            {
                Items = RoadmapsDto,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return ServiceResponce<PagedResultDto<RoadmapResponceDto>>.success(responce, "Roadmap retrived successfuly", 200);




        }

        //................................................(Delete-Roadmap).....................................................

        public async Task<ServiceResponce<string>> DeleteRoadmap(int id)
        {
            if (id <= 0)  throw new ArgumentException("id");

            var exsiteRoadmap = await _roadmapRepo.GetByIdAsync(id);
            if (exsiteRoadmap == null) return ServiceResponce<string>.Fail("Roamap Does not exsist", 404);

             await _roadmapRepo.DeleteAsync(id);

            return ServiceResponce<string>.success("Deleted Sucessfully", "", 200);


        }


        //..............................................(Update-Roadmap)........................................................


        public async Task<ServiceResponce<RoadmapResponceDto>> UpdateRoadmap(int id, RoadmapUpdateDto roadmapUpdateDto )
        {
            if(id<0 || roadmapUpdateDto is null) throw new ArgumentException("Invalid arguments: ID must be positive and the update DTO cannot be null.", nameof(roadmapUpdateDto));

            var RoadMap = await _roadmapRepo.GetByIdAsync(id);
            if (RoadMap is null) return  ServiceResponce<RoadmapResponceDto>.Fail("The RoadMap does not exsist", 404);

            if(roadmapUpdateDto is null) return ServiceResponce<RoadmapResponceDto>.Fail("Must Enter Information", 400);

            EntityUpdater.UpdateEntity(RoadMap, roadmapUpdateDto);

            await _roadmapRepo.UpdateAsync(RoadMap);

            var responce = _mapper.Map<RoadmapResponceDto>(RoadMap);

            return ServiceResponce<RoadmapResponceDto>.success(responce, "Updated Sucessfully ", 200);
        }
    }
}



