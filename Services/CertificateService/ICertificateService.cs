using AutoMapper;
using HR_Carrer.Data.Entity;
using HR_Carrer.Data.Repositery;
using HR_Carrer.Dto.CertificateDtos;
using HR_Carrer.Dto.EmployeeDtos;
using HR_Carrer.Dto.SkillsDtos;
using HR_Carrer.Dto.UserDtos;
using HR_Carrer.Services.FileService;
using HR_Carrer.Services.Utility;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Security.Cryptography.X509Certificates;

namespace HR_Carrer.Services.CertificateService
{
    public interface ICertificateService
    {
        Task<ServiceResponce<CertificateResponceDto>> CreateCertificate(CertificateRequestDto certificateRequestDto);
       
        Task<ServiceResponce<CertificateResponceDto>> GetCertificate(int? id);

        Task<ServiceResponce<CertificateSkillsResDto>> AddSkillsToCertificate(int id , CertificateSkillsReqDto certificateSkillsReq);


        Task<ServiceResponce<PagedResultDto<CertificateResponceDto>>> GetAllCertificates(int? id = null, string? name = null,
                                                                 int pageNumber = 1, int pageSize = 10);

        Task<ServiceResponce<CertificateSkillsResDto>> GetCertificateWithSkills(int id);


        Task<ServiceResponce<CertificateResponceDto>> UpdateCertificate(int id,[FromQuery]CertificateUpdateDto certificateUpdateDto);


        Task<ServiceResponce<string>> UploadCertificateImage(int CerticateId , IFormFile CertificateImage);

        Task<ServiceResponce<string>> DeleteCertificateImage(int CerticateId);
        Task<ServiceResponce<string>> DeleteCertificate(int id);




    }

    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepo _certificateRepo;
        private readonly ISkillRepo _skillRepo;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public CertificateService(ICertificateRepo certificateRepo, IMapper mapper, IFileService fileService, ISkillRepo skillRepo)
        {
            _certificateRepo = certificateRepo;
            _mapper = mapper;
            _fileService = fileService;
            _skillRepo = skillRepo;
        }


        //.............................................(Create Certificate).......................................................
        public async Task<ServiceResponce<CertificateResponceDto>> CreateCertificate(CertificateRequestDto certificateRequestDto)
        {
            if (certificateRequestDto is null)
                return   ServiceResponce<CertificateResponceDto>.Fail("Must enter information", 400);


            var existingCertificate = await _certificateRepo.GetByName(certificateRequestDto.Name!);
            if (existingCertificate is not null)
                return ServiceResponce<CertificateResponceDto>.Fail("Certificate already exists", 409);


            var certificate = _mapper.Map<Certificates>(certificateRequestDto);
            await _certificateRepo.AddAsync(certificate);

            var responce = _mapper.Map<CertificateResponceDto>(certificate);
            return ServiceResponce<CertificateResponceDto>.success(responce, "Certificate Created Successfully", 201);

            
        
        }


        //.............................................(Get All Certificates).......................................................

        public async Task<ServiceResponce<PagedResultDto<CertificateResponceDto>>> GetAllCertificates(int? id = null, string? name = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = await _certificateRepo.GetAllWithQueryAsync(id, name);

            if (query is null || !query.Any())
                return ServiceResponce<PagedResultDto<CertificateResponceDto>>.Fail("Certificates not found", 404);

            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedCertificate = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var CertificateDtos = _mapper.Map<List<CertificateResponceDto>>(pagedCertificate.ToList());

            var responce = new PagedResultDto<CertificateResponceDto>
            {
                Items = CertificateDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };

            return ServiceResponce<PagedResultDto<CertificateResponceDto>>.success(responce, "Certificated retrieved successfully", 200);
        }
 


        //.............................................(Get Certificate).......................................................

        public async Task<ServiceResponce<CertificateResponceDto>> GetCertificate(int? id)
        {
           if(id is 0 ||  id is null)
            {
                return ServiceResponce<CertificateResponceDto>.Fail("Id must be not null or zero", 400);
            }

           var certificate = await _certificateRepo.GetByIdAsync((int)id);
            if(certificate is null)
            {
                return ServiceResponce<CertificateResponceDto>.Fail("Certificate not found", 404);
            }
            var responce = _mapper.Map<CertificateResponceDto>(certificate);
            return ServiceResponce<CertificateResponceDto>.success(responce, "Certificate found successfully", 200);

        }



        //.............................................(Delete Certificate).......................................................

        public async Task<ServiceResponce<string>> DeleteCertificate(int id)
        {
            try
            {
                var Certificate = await _certificateRepo.GetByIdAsync(id);
                if (Certificate is null)
                    return ServiceResponce<string>.Fail("Certificiate not found", 404);
                if (Certificate.ImagePath is not null || Certificate.ImagePath != string.Empty)
                {
                    _fileService.DeleteFile(Certificate.ImagePath,false);
                }

                await _certificateRepo.DeleteAsync(id);

                return ServiceResponce<string>.success("Certificate deleted successfully", "", 200);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ServiceResponce<string>.Fail("Failed to delete user", 500);
            }

        }
        //.............................................(Update Certificate).......................................................


        public async Task<ServiceResponce<CertificateResponceDto>> UpdateCertificate(int id, [FromQuery] CertificateUpdateDto certificateUpdateDto)
        {
            var certificate = await _certificateRepo.GetByIdAsync(id);
            if (certificate is null)
            {
                return ServiceResponce<CertificateResponceDto>.Fail("Certificate not found ", 404);

            }
            if(certificateUpdateDto is null)
            {
                return ServiceResponce<CertificateResponceDto>.Fail("Must enter information", 400);
            }

            EntityUpdater.UpdateEntity(certificate, certificateUpdateDto);

            await _certificateRepo.UpdateAsync(certificate);


            var responce = _mapper.Map<CertificateResponceDto>(certificate);

            return ServiceResponce<CertificateResponceDto>.success(responce, "Certificate updated Sucessfully", 200);



        }

        //.............................................(Upload Certificate Image).......................................................

        public async Task<ServiceResponce<string>> UploadCertificateImage(int CerticateId, IFormFile CertificateImage)
        {
            var certificate = await _certificateRepo.GetByIdAsync(CerticateId);
            if (certificate is null)
            {
                return ServiceResponce<string>.Fail("Certificate not found", 404);
            }
            
            if (CertificateImage is null || CertificateImage.Length==0)
            {
                return ServiceResponce<string>.Fail("No file uploaded", 400);
            }
            if(certificate.ImagePath is not null)
            {
                  _fileService.DeleteFile(certificate.ImagePath,false);
            }
            string? ImagePath = await  _fileService.SaveCertificate(CertificateImage);

            if(ImagePath == "File type not allowed")
            {
                return ServiceResponce<string>.Fail("File type not allowed", 400);
            }

            certificate.ImagePath= ImagePath??certificate.ImagePath;
            await _certificateRepo.UpdateAsync(certificate);

            return ServiceResponce<string>.success("Image uploaded successfully","", 200);

        }


        //................................................(Delete-Certificate-Image).....................................................
        public async Task<ServiceResponce<string>> DeleteCertificateImage(int CerticateId)
        {
       
            if(CerticateId <= 0)
            {
                return ServiceResponce<string>.Fail("Invalid Certificate Id", 400);
            }

            var Certificate = await _certificateRepo.GetByIdAsync(CerticateId);
            if (Certificate is null) return ServiceResponce<string>.Fail("Certificate not found", 404);

            if (string.IsNullOrWhiteSpace(Certificate.ImagePath))
            {
                return ServiceResponce<string>.Fail("No image to delete", 400);
            }

            var result = _fileService.DeleteFile(Certificate.ImagePath,false);
            if (result == "Invalid Path" || result == "File Not Found")
            {
                return ServiceResponce<string>.Fail("Invalid image path or the file not found", 400);
            }
            Certificate.ImagePath = null;
            await _certificateRepo.UpdateAsync(Certificate);

            return ServiceResponce<string>.success("Image deleted successfully", "", 200);
        
    }

        //.............................................(Get Certificate With Skills).......................................................
        public async Task<ServiceResponce<CertificateSkillsResDto>> GetCertificateWithSkills(int id)
        {
           var CertificateWithSkills = await  _certificateRepo.GetCertificateWithSkill(id);
            if(CertificateWithSkills is null)
            {
                return       ServiceResponce<CertificateSkillsResDto>.Fail("Certificate not found", 404);
            }
            var CertificateDto = _mapper.Map<CertificateResponceDto>(CertificateWithSkills);
            var skillsDto = _mapper.Map<List<SkillResponceDto>>(CertificateWithSkills.Skills);

            var responce = new CertificateSkillsResDto
            {
                Certificate = CertificateDto,
                CoverdSkills = skillsDto
            };

            return (ServiceResponce<CertificateSkillsResDto>.success(responce, "Certificate with skills retrieved successfully", 200));




        }

        //.............................................(Add Skills To Certificate).......................................................

        public async Task<ServiceResponce<CertificateSkillsResDto>> AddSkillsToCertificate(int id, CertificateSkillsReqDto certificateSkillsReq)
        {

            if (id <= 0) return ServiceResponce<CertificateSkillsResDto>.Fail("Invalid Certificate Id", 400);

            var Certificate = await _certificateRepo.GetByIdAsync(id);
            if (Certificate is null) return ServiceResponce<CertificateSkillsResDto>.Fail("Certificate not found", 404);

            var CertificateWithSKill = await _certificateRepo.GetCertificateWithSkill(id);


            foreach (var skillId in certificateSkillsReq.ExsisteExistingSkillIds)
            {
                var exstingSkill = await _skillRepo.GetByIdAsync(skillId);
                if(exstingSkill is null)
                {
                    return ServiceResponce<CertificateSkillsResDto>.Fail($"The skills with Id {skillId} does not exsist", 400);

                }
                if (CertificateWithSKill.Skills.Contains(exstingSkill))
                {
                    return ServiceResponce<CertificateSkillsResDto>.Fail($"The Skill With ID : {skillId} already exsist",400);
                }
                Certificate.Skills.Add(exstingSkill);
            }

            

            await _certificateRepo.UpdateAsync(Certificate);


            var CertificateDto= _mapper.Map<CertificateResponceDto>(CertificateWithSKill);

            var skillsDto = _mapper.Map<List<SkillResponceDto>>(CertificateWithSKill.Skills);

            var Responce = new CertificateSkillsResDto()
            {
                Certificate = CertificateDto,
                CoverdSkills = skillsDto
            };


            return ServiceResponce<CertificateSkillsResDto>.success(Responce,"the added its sucessfuly", 200);



        }
        


    }



}
