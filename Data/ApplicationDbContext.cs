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

            //Configure Employee with User one-to-one relationship
            modelBuilder.Entity<Employee>().HasKey( e=>e.UserId);
            modelBuilder.Entity<Employee>()
                .HasOne(e=>e.User)
                .WithOne(e=>e.Employee)
                .HasForeignKey<Employee>(e => e.UserId);  // the foreign key is in Employee table


            // Configure many-to-many relationship between Employee and Skills
                  modelBuilder.Entity<Employee>()
                .HasMany(e => e.Skills)
                .WithMany(e => e.Employees).
                UsingEntity<Dictionary<string, object>>("Employee_Skill",
                j=>j.HasOne<Skills>().WithMany().HasForeignKey("SkillId"),
                 j=>j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                 j=>j.HasKey("EmployeeId", "SkillId")
                );

            // Configure one-to-many relationship between Employee and Certificates
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Certificates)
             .WithOne(c=>c.Employee)
            .HasForeignKey(c => c.EmployeeId);


            // Configure one-to-many relationship between Employee and Requests
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Requests)
             .WithOne(r=>r.Employee)
            .HasForeignKey(r => r.EmployeeId);


            // Configure one-to-many relationship between Employee and Roadmap
            modelBuilder.Entity<Employee>()
            .HasMany(e => e.Roadmaps)
             .WithOne(r => r.Employee)
            .HasForeignKey(c => c.EmployeeId);


            // Configure many-to-many relationship between Certificate and steps
            modelBuilder.Entity<Steps>().HasKey(s => s.Id);
            modelBuilder.Entity<Certificates>()
                .HasMany(c => c.steps)
                .WithMany(s=>s.Certificates)
                .UsingEntity<Dictionary<string, object>>("Certificate_Steps",
                j=>j.HasOne<Steps>().WithMany().HasForeignKey("StepId"),
                j => j.HasOne<Certificates>().WithMany().HasForeignKey("CertificateId"),
                j=>j.HasKey("CertificateId", "StepId")   );

            // Configure one-to-many relationship between Roadmap and Steps
            modelBuilder.Entity<Roadmap>()
            .HasMany(r => r.Steps)
            .WithOne(s => s.Roadmap)
            .HasForeignKey(s => s.RoadmapId);

        }


    }
}

