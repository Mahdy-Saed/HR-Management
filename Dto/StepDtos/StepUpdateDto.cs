using HR_Carrer.Data.Entity;

namespace HR_Carrer.Dto.StepDtos
{
    public class StepUpdateDto
    {


        public string? Name { get; set; }

        public StepStatus? Status { get; set; } = StepStatus.NotStarted;

    }
}




