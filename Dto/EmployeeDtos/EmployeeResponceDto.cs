using Microsoft.Extensions.Primitives;

namespace HR_Carrer.Dto.EmployeeDtos
{
    public class EmployeeResponceDto
    {
         public string? FullName { get; set; }
        public string? Email { get; set; }

        public string? ImagePath { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Postion { get; set; }
        public DateOnly? HiredDate { get; set; }

  
    }
}
