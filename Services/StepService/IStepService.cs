using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.StepDtos;
using HR_Carrer.Services.Utility;
using System.Threading.Tasks;

namespace HR_Carrer.Services.StepService
{
    public interface IStepService
    {

        Task<ServiceResponce<StepsResponceDto>> CreateSteps(int roadMapId, StepsRequestDto stepsRequestDto);

        Task<ServiceResponce<List<StepsResponceDto>>> GetAllSteps(int roadMapId);

        Task<ServiceResponce<StepsResponceDto>> GetStep(int roadMapId, int StepId);

        Task<ServiceResponce<StepsResponceDto>> UpdateStep (int roadmapId , int stepId , StepUpdateDto stepUpdateDto);


        Task<ServiceResponce<string>> DeleteStep(int roadMapId, int StepId);


    }








    public class StepService : IStepService
    {
        private readonly IRoadmapRepo _roadmapRepo;
        private readonly IMapper _mapper;
        private readonly IStepsRepo _stepsRepo;
        private readonly ICertificateRepo _CertificateRepo;
        public StepService(IRoadmapRepo roadmapRepo, IStepsRepo stepsRepo, IMapper mapper, ICertificateRepo certificateRepo)
        {
            _roadmapRepo = roadmapRepo;
            _stepsRepo = stepsRepo;
            _mapper = mapper;
            _CertificateRepo = certificateRepo;
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

            var certificate = await _CertificateRepo.GetByIdAsync(stepsRequestDto.CertificatedId);
            if (certificate is null)
                return ServiceResponce<StepsResponceDto>.Fail($"The Certificate with Id{stepsRequestDto.CertificatedId} not found ", 404);

            // link the step with the roadmap and the certificate
            step.RoadmapId = roadMapId;
            step.CertificateId = stepsRequestDto.CertificatedId;

            await _stepsRepo.AddAsync(step);

            var responce = _mapper.Map<StepsResponceDto>(step);

            return ServiceResponce<StepsResponceDto>.success(responce, "Step Created Successfully", 201);
        }


        //................................................(Get All Steps).....................................................


        public async Task<ServiceResponce<List<StepsResponceDto>>> GetAllSteps(int roadMapId)
        {

            if (roadMapId <= 0) throw new ArgumentException("The id is null here");

            var roadMap = await _roadmapRepo.GetByIdAsync(roadMapId);

            if (roadMap is null) return ServiceResponce<List<StepsResponceDto>>.Fail("Roadmap Not found", 404);

            var steps = await _stepsRepo.GetStepsByRoamapIdAsync(roadMapId);

            if (!steps.Any()) return ServiceResponce<List<StepsResponceDto>>.Fail("Steps Not found", 404);

            var stepsDto = _mapper.Map<List<StepsResponceDto>>(steps);

            return ServiceResponce<List<StepsResponceDto>>.success(stepsDto, "data retrived sucessfuly", 200);
        }



        //................................................(Get-Step).....................................................

        public async Task<ServiceResponce<StepsResponceDto>> GetStep(int roadMapId, int StepId)
        {

            if (roadMapId <= 0 || StepId <= 0) throw new ArgumentException("Invalid Data: muste Enter information");


            var (exsist, Message) = await IsExsist(roadMapId, StepId);
            if (!exsist)
                return ServiceResponce<StepsResponceDto>.Fail(Message, 404);

            var step = await _stepsRepo.GetByIdAsync(StepId);

            var stepDto = _mapper.Map<StepsResponceDto>(step);
            return ServiceResponce<StepsResponceDto>.success(stepDto, "Data retrived Sucessfuly", 200);
        }


        //................................................(Delete-Step).....................................................
        public async Task<ServiceResponce<string>> DeleteStep(int roadMapId, int StepId)
        {
            
            var (exsist,Message)= await IsExsist(roadMapId,StepId);
            if (!exsist)
                return ServiceResponce<string>.Fail(Message, 404);

            await _stepsRepo.DeleteAsync(StepId);

            return ServiceResponce<string>.success("Deleted Step Sucessfuly", "" ,200);

        }







        //................................................(Update-Step).....................................................


        public async Task<ServiceResponce<StepsResponceDto>> UpdateStep(int roadmapId, int stepId, StepUpdateDto stepUpdateDto)
        {

            if (roadmapId <= 0 || stepId <= 0)
                throw new ArgumentException("The Id of {Step , RoadMap} not Correct here");

            var (exisit, message) = await IsExsist(roadmapId, stepId);

            if (!exisit)
                return ServiceResponce<StepsResponceDto>.Fail(message, 404);

            var step = await _stepsRepo.GetByIdAsync(stepId);

            EntityUpdater.UpdateEntity(step, stepUpdateDto);

            await _stepsRepo.UpdateAsync(step!);

            var StepDto = _mapper.Map<StepsResponceDto>(step);

            return ServiceResponce<StepsResponceDto>.success(StepDto, "Data Updateed Sucessfully", 200);

        }








        //helper method....
        private async Task<(bool,string)> IsExsist(int? RoamapId = null, int? stepId = null )
        {



            if (RoamapId is not null)
            {
                var Roadmap = await _roadmapRepo.GetByIdAsync(RoamapId.Value);

                if (Roadmap is null) return (false, "Roamap NotFound");
            }
            if (stepId is not null)
            {
                var step = await _stepsRepo.GetByIdAsync(stepId.Value);

                if (step is null) return (false,"step not found");
            }
          
            return (true,"");

        }

    }
}





