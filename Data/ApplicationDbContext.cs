using HR_Carrer.Entity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;


namespace HR_Carrer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Skills> Skills { get; set; }

        public DbSet<Requests> Requests { get; set; }   

        public DbSet<Steps> Steps { get; set; }

        public DbSet<Certificates> Certificates { get; set; }

        public DbSet<Roadmap> Roadmaps { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Seeding Data for Role Table

            modelBuilder.Entity<Role>().HasData(
                new Role { Id=1,Name="Admin"},
                new Role { Id = 2, Name = "User" }
                 );



            //Configure all the relationship between te entities
            modelBuilder.ConfigureRelationship();


        }


    }
}

