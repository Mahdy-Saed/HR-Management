using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.SkillsDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HR_Carrer.Services.SkillService
{
    public interface ISkillService
    {
        Task<ServiceResponce<PagedResultDto<SkillResponceDto>>> GetAllSkills(int? id=null, string? name=null,int pageNumber=1, int pageSize=10);

        Task<ServiceResponce<SkillResponceDto>> CreateSkill(SkillCreateDto skillCreateDto);


        Task<ServiceResponce<SkillResponceDto>> UpdateSkill(int id, SkillUpdateDto skillUpdateDto);

        Task<ServiceResponce<string>> DeleteSkill(int id);
    }
    public class SkillServcie : ISkillService
    {
        private readonly ISkillRepo _skillRepo;

        private readonly IMapper _mapper;


        public SkillServcie(ISkillRepo skillRepo, IMapper mapper)
        {
            _skillRepo = skillRepo;
            _mapper = mapper;
        }


        //................................................(Create-Skill).....................................................

        public async Task<ServiceResponce<SkillResponceDto>> CreateSkill(SkillCreateDto skillCreateDto)
        {
           if(skillCreateDto == null)
            {
                return ServiceResponce<SkillResponceDto>.Fail("Skill data is null", 400);
            }

           var existingSkills = await _skillRepo.GetByNameAsync(skillCreateDto.Name!);

            if(existingSkills  is not null)
            {
                 return ServiceResponce<SkillResponceDto>.Fail("Skill is  already exists", 409);
            }

            var skillEntity = _mapper.Map<Skills>(skillCreateDto);

            await _skillRepo.AddAsync(skillEntity);
            var skillResponceDto = _mapper.Map<SkillResponceDto>(skillEntity);
            return ServiceResponce<SkillResponceDto>.success(skillResponceDto, "Skill created successfully", 201  );

        }

        //................................................(Delete-Skill).....................................................

        public async Task<ServiceResponce<string>> DeleteSkill(int id)
        {
            if (id <= 0) { throw  new ArgumentOutOfRangeException(nameof(id)); }
            
            var existingSkill = await  _skillRepo.GetByIdAsync(id);
            if(existingSkill is null)
            {
                return ServiceResponce<string>.Fail("Skill not found", 404);
            }

            await _skillRepo.DeleteAsync(id);

            return ServiceResponce<string>.success( "", "Skill deleted successfully", 200);


        }
        //................................................(Get-All-Skills).....................................................

        public async Task<ServiceResponce<PagedResultDto<SkillResponceDto>>> GetAllSkills(int? id = null, string? name = null, int pageNumber = 0, int pageSize = 0)
        {
          var skills = await _skillRepo.GetAllWithQueryAsync(id, name);
            // Pagination
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;
            var totalCount = skills.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var  pagedSkill = skills.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var skillsDto = _mapper.Map<List<SkillResponceDto>>(pagedSkill.ToList());

            var responce = new PagedResultDto<SkillResponceDto>
            {
                Items = skillsDto,
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageNumber = pageNumber,
                PageSize = pageSize

            };
            return ServiceResponce<PagedResultDto<SkillResponceDto>>.success(responce, "Skills retrieved successfully", 200);
        }
        //................................................(update-Skill).....................................................

        public async Task<ServiceResponce<SkillResponceDto>> UpdateSkill(int id, SkillUpdateDto skillUpdateDto)
        {
           if(skillUpdateDto == null || id<=0) { return   ServiceResponce<SkillResponceDto>.Fail("Must Enter the information", 400); }

            var existSkill = await _skillRepo.GetByIdAsync(id);

            if(existSkill == null) return ServiceResponce<SkillResponceDto>.Fail("Skill not found", 404);

            EntityUpdater.UpdateEntity(existSkill, skillUpdateDto);

            await _skillRepo.UpdateAsync(existSkill);

            var responce = _mapper.Map<SkillResponceDto>(existSkill);

            return ServiceResponce<SkillResponceDto>.success(responce,"Skill update successfully", 200);
        }
    }





}
