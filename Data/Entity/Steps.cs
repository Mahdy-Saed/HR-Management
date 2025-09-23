namespace HR_Carrer.Data.Entity
{
    public class Steps
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        //Navigaton relasionship
 
        public int RoadmapId { get; set; }  
        public Roadmap Roadmap { get; set; }  // many to one

    }
}
