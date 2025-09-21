using HR_Carrer.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.UserDtos
{
    public class UserUpdateDto
    {


          [Required(ErrorMessage ="must Enter the New Name")]
           [MaxLength(255)]  
            public string? NewFullName { get; set; }


        [Required(ErrorMessage = "must Enter the New Email")]
        [EmailAddress(ErrorMessage ="must Enter the Right format of Email")]
            public string? NewEmail { get; set; }


            public bool? NewStatus { get; set; } = true;

          


           



        
    }

}

