namespace HR_Carrer.Data.Entity
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
 
         public ICollection<Employee> Employees { get; set; }

         public Steps step { get; set; }

        public ICollection<Skills> Skills { get; set; }=new List<Skills>(); // many to many

    }

    public enum CertificateLevel
    {
        essay,
        Intermidate,
        Difficulte

    }
}
