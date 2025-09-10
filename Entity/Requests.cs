using Org.BouncyCastle.Asn1.Mozilla;

namespace HR_Carrer.Entity
{
    public class Requests
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Type { get; set; }

        public string? Description { get; set; }

        public RequestStatus? Status { get; set; }

        public string? ImagePath { get; set; }

        public DateOnly? Request_Date { get; set; }

        public DateOnly? Approved_Date {get; set;}


        //Navigation relasionship
        public Guid EmployeeId { get; set; }    
        public Employee Employee { get; set; }  // many to one 


}
    public enum RequestStatus
        {
            Approved,
            Pending,
            Rejected

        }
}

