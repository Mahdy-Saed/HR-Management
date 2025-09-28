using HR_Carrer.Data.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;
namespace HR_Carrer.Dto.RequestDtos
{
    public class ReqeustCreateDto
    {
        [Required(ErrorMessage = "The Title is Required")]
        [MaxLength(100, ErrorMessage = "The max length is 100 charcter")]

        public string? Title { get; set; }

         public RequestType Type { get; set; }



        [Required(ErrorMessage = "The Description is Required")]
        [MaxLength(255, ErrorMessage = "The max length is 255 charcter")]
        public string? Description { get; set; }

}
         

    
  
}





