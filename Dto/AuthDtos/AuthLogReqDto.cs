using HR_Carrer.CustomValidation;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.AuthDtos
{
    public class AuthLogReqDto
    {
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(255)]
        [Password]
        public string? Password { get; set; }   

    }
}