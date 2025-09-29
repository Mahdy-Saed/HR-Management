namespace HR_Carrer.Data.Entity
{
    public class Skills
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public SkillLevel Level { get; set; }


        public int? point { get; set; } = 0;

        // Navigation Relationship
        public ICollection<Certificates> Certificates { get; set; } = new List<Certificates>(); // many to many
        

    }
    public enum SkillLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
}
