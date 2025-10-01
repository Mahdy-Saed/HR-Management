using HR_Carrer.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.StepDtos
{
    public class StepsRequestDto
    {

        [Required(ErrorMessage = "StepTitle is required")]
        [MaxLength(100, ErrorMessage = "StepTitle cannot exceed 100 characters")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "CertificatedId is required")]
        public int CertificatedId { get; set; }

  
    }






}

