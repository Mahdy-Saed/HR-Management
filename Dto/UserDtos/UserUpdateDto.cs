using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.UserDtos
{
    public class UserUpdateDto
    { 


            public string? FullName { get; set; }

            public string? Email { get; set; }

            public string? ImagePath { get; set; }

            public bool? Status { get; set; } = true;


           



        
    }

}

