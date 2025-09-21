using HR_Carrer.CustomValidation;
using Microsoft.AspNetCore.Http.Metadata;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.UserDtos
{
    public class UserRequestDto
    {
        [FullName]
        [Required(ErrorMessage = "FullName is required")]
        [MaxLength(100, ErrorMessage = "FullName can't be longer than 100 characters")]
        public string? FullName { get; set; }

        
        [Required(ErrorMessage = "FullName is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string? Password { get; set; }


    }

  
}
 