namespace HR_Carrer.Entity
{
    public class Certificates
    {
        public int Id { get; set; }

        public string?  Name { get; set; }

        public string? Description { get; set; }

        public  string? Issure { get; set; }
        

        public string? ImagePath { get; set; }


        public CertificateLevel? level { get; set; }


        //Navigation relasionship
        public ICollection<Steps>? steps { get; set; }

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; }



    }

    public enum CertificateLevel
    {
        essay,
        Intermidate,
        Difficulte

    }
}
