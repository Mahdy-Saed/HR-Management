using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.CertificateDtos
{
    public class CertificateUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Issure { get; set; }
        public string? ImagePath { get; set; }
        public CertificateLevel? level { get; set; }


    }
}
