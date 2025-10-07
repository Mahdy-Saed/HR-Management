using HR_Carrer.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.SkillsDtos
{
    public class SkillCreateDto
    {
        [Required(ErrorMessage ="Name is requried")]
        [MaxLength(100, ErrorMessage = "Name can't exceed 100 characters")]
        public string? Name { get; set; }


        [Required(ErrorMessage = "Level is requried")]
        public SkillLevel Level { get; set; }


        [Required(ErrorMessage = "point is requried")]

        public int? point { get; set; } = 0;

    }
}
