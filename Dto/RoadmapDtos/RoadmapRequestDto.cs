using HR_Carrer.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace HR_Carrer.Dto.RoadmapDtos
{
    public class RoadmapRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "TargetGoal cannot exceed 100 characters")]

        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "TargetGoal is required")]
        [MaxLength(100, ErrorMessage = "TargetGoal cannot exceed 100 characters")]
        public string? TargetGoal { get; set; }

        [Required(ErrorMessage = "EmployeePostion is required")]
        [MaxLength(100, ErrorMessage = "TargetGoal cannot exceed 100 characters")]
        public string? EmployeePostion { get; set; }

        [Required(ErrorMessage = "Estimated Time is required")]
         public DateTime? Esstimated_Completed_Time { get; set; }

        [Required(ErrorMessage = "Difficulity is required")]

        public DifficulityLevel? Difficulity { get; set; }

        [Required(ErrorMessage = "Status of Roadmap is  required")]

        public bool? IsAvailable { get; set; } = false;


        }






        public enum DifficulityLevel
        {
            easy,
            intermediate,
            hard
        }


}






 