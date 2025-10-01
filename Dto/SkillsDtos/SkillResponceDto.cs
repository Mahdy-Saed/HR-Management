using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.SkillsDtos
{
    public class SkillResponceDto
    {

        public int? id { get; set;}
        public string? Name { get; set; }

        public string? Level { get; set; }


        public int? point { get; set; } = 0;

    }
}
