using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.UserDtos
{
    public class UserRequestDto
    {
    
        public string? FullName { get; set; }

        
        public string? Email { get; set; }
 
        public string? Password { get; set; }

        public string? ImagePath { get; set; }

        public UserRole Role  { get; set; }

        public bool? Status { get; set; } = true;


    }

    public enum UserRole
    {
        Admin = 1,
        Employee = 2
    }   
}
 