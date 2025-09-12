using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HR_Carrer.Data
{
    public static class ModelBuilderExtension
    {
        public static  void ConfigureRelationship(this ModelBuilder modleBuilder)
        {
            modleBuilder.Entity<Employee>(builder =>
            {


                builder.HasKey(e => e.UserId);
                //Configure one-to-one relationship between Employee and User
                builder.HasOne(e => e.User).WithOne(e => e.Employee).HasForeignKey<Employee>(e=>e.UserId);

                //Configure many-to-many relationship between Employee and Skills
                builder.HasMany(e => e.Skills).WithMany(s => s.Employees).UsingEntity<Dictionary<string, object>>(
                    "Employee_Skill",
                    j => j.HasOne<Skills>().WithMany().HasForeignKey("SkillId"),
                    j => j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId"),
                    j => j.HasKey("EmployeeId", "SkillId")
                    );

                //Configure one-to-many relationship between Employee and Certificates
                builder.HasMany(e => e.Certificates).WithOne(c => c.Employee).HasForeignKey(c => c.EmployeeId);


                //Configure one-to-many relationship between Employee and Requests
                builder.HasMany(e => e.Requests).WithOne(r => r.Employee).HasForeignKey(r => r.EmployeeId);

                //configure one-to-many relationship between Roadmap and Steps
                builder.HasMany(e => e.Roadmaps).WithOne(r => r.Employee).HasForeignKey(r => r.EmployeeId);
            }
            );

            //configure one-to-many relationship between Roadmap and Steps

            modleBuilder.Entity<Roadmap>(builder =>
            { 
                builder.HasMany(r => r.Steps).WithOne(s => s.Roadmap).HasForeignKey(s => s.RoadmapId);
            });



            //configure many-to-many relationship between Certificates and Steps
            modleBuilder.Entity<Certificates>(builder =>
            {
                builder.HasMany(c => c.steps).WithMany(s => s.Certificates).UsingEntity<Dictionary<string, object>>("Certificate_step",

                j => j.HasOne<Steps>().WithMany().HasForeignKey("StepId"),
                    j => j.HasOne<Certificates>().WithMany().HasForeignKey("CertificateId"),
                    j => j.HasKey("CertificateId", "StepId")
                    );

            });
            
          
        }




    }
}
