using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.EmployeeDtos
{
    public class EmployeeRequestDto
    {

        [Required(ErrorMessage = "Postion is required")]
        public DateOnly? HiredDate { get; set; }

        [Required(ErrorMessage = "Postion is required")]
        [MaxLengthAttribute(100, ErrorMessage = "Postion can't exceed 100 characters")]
        public string? Postion { get; set; }


        [Required(ErrorMessage = "PhoneNumber is required")]
        [MaxLengthAttribute(11, ErrorMessage = "PhoneNumber can't exceed 11 characters")]
        public string? PhoneNumber { get; set; }
    }
}
