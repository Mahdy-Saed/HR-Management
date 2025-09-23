using HR_Carrer.CustomValidation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.EmployeeDtos
{
    public class EmployeeUpdateDto
    {
        [FullName]
        [MaxLength(100, ErrorMessage = "FullName can't be longer than 100 characters")]
        [DefaultValue(null)]
        public string? FullName { get; set; }


         [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DefaultValue(null)]
        public string? Email { get; set; }

         [MaxLengthAttribute(11, ErrorMessage = "PhoneNumber can't exceed 11 characters")]
        [DefaultValue(null)]
        public string? PhoneNumber { get; set; }

    }
}
