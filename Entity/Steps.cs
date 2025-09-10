namespace HR_Carrer.Entity
{
    public class Steps
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        //Navigaton relasionship
        public ICollection<Certificates>? Certificates { get; set; }= new List<Certificates>();

        public int RoadmapId { get; set; }  
        public Roadmap Roadmap { get; set; }  // many to one

    }
}
