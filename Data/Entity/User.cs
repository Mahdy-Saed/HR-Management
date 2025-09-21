namespace HR_Carrer.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? RefreshToken { get; set; }


        public DateTime? RefreshTokenExpiryTime { get; set; }   

        public string? ImagePath { get; set; }

        public bool? Status { get; set; } = true;


        // Navigation Relationship
        public Employee? Employee { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }      // Id =1 admin, Id=2 employee                ||     spongpop roldie=Id   rainmaster roleid=id



    }
}
