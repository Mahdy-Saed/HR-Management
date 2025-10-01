using HR_Carrer.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.CertificateDtos
{
    public class CertificateRequestDto
    {
        [Required(ErrorMessage ="Name of Certificate is required")]
        [MaxLength(100,ErrorMessage ="Name of Certificate must be less than 100 character")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Name of Certificate is required")]
        [MaxLength(200, ErrorMessage = "Name of Certificate must be less than 200 character")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Issure of Certificate is required")]
        [MaxLength(100, ErrorMessage = "Name of Certificate must be less than 100 character")]
        public string? Issure { get; set; }

        public string? ImagePath { get; set; }
        [Required(ErrorMessage = "Level of Certificate is required")]
         public CertificateLevel? level { get; set; }


       

    }

 
}





