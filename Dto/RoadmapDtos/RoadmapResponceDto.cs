using HR_Carrer.Dto.StepDtos;

namespace HR_Carrer.Dto.RoadmapDtos
{
    public class RoadmapResponceDto
    {

        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string? TargetGoal { get; set; }

        public string? EmployeePostion { get; set; }

        public DateOnly? Esstimated_Completed_Time { get; set; }

        public DifficulityLevel? Difficulity { get; set; }

        public bool? IsAvailable { get; set; } = false;



    }
}
