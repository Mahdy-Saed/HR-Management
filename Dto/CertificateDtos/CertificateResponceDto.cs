using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.CertificateDtos
{
    public class CertificateResponceDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Issure { get; set;}
        public string? ImagePath { get; set; }
        public string? level { get; set; }
    }
}
