namespace HR_Carrer.Entity
{
    public class Roadmap
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? TargetGoal { get; set; }

        public string? EmployeePostion { get; set; }

        public DateOnly? Esstimated_Completed_Time { get; set; }

        public DifficulityLevel? Difficulity { get; set; }

        public bool? IsAvailable { get; set; } = false;


        //Navigation relasionship
        
        public ICollection<Steps>? Steps { get; set; } = new List<Steps>(); // many to one

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }  // many to one

    }





    public enum DifficulityLevel
    {
        easy,
        middle,
        hard
    }


}
