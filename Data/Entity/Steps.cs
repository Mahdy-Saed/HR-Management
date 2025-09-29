namespace HR_Carrer.Data.Entity
{
    public class Steps
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public StepStatus? Status { get; set; } = StepStatus.NotStarted;


        //Navigaton relasionship

        public int CertificateId { get; set; }
        public Certificates Certificate { get; set; }  // many to one

        public int RoadmapId { get; set; }  
        public Roadmap Roadmap { get; set; }  // many to one

    }


    public enum StepStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
