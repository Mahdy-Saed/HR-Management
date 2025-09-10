namespace HR_Carrer.Entity
{
    public class Role
    {

        public int Id { get; set; }


        public string? Name { get; set; }

        // Navigation Relationship  

        public User User { get; set; }  //one to one relationship

    }
}
