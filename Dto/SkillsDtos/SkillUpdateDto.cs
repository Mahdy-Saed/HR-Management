using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.SkillsDtos
{
    public class SkillUpdateDto
    {

        public string? Name { get; set; }

        public SkillLevel Level { get; set; }


        public int? point { get; set; } = 0;




    }
}
