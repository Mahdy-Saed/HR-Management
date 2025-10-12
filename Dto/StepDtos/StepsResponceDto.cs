using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.StepDtos
{
    public class StepsResponceDto
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public int CertificateId { get; set; }

        public StepStatus? Status { get; set; }


    }
}
