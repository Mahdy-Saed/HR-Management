using System.Collections;

namespace HR_Carrer.Data.Entity
{
    public class Employee
    {
  

        public string? PhoneNumber { get; set; }

        public DateOnly? HiredDate { get; set; }

        public string? Postion { get; set; }


        // Navigation Relationship

        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Requests>? Requests { get; set; }=new List<Requests>(); // one to many


        public ICollection<Certificates>? Certificates { get; set; }=new List<Certificates>(); // one to many 
         
        public ICollection<Roadmap>? Roadmaps { get; set; }=new List<Roadmap>(); // one to many


 
    }
}
