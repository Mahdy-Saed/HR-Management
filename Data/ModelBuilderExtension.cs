using HR_Carrer.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security.Cryptography.X509Certificates;

namespace HR_Carrer.Data
{
    public static class ModelBuilderExtension
    {
        public static  void ConfigureRelationship(this ModelBuilder modleBuilder)
        {
            modleBuilder.Entity<User>(builder =>
            {
                builder.HasKey(u => u.Id);
                //Configure one-to-one relationship between User and Employee
                builder.HasOne(u => u.Employee).WithOne(e => e.User).HasForeignKey<Employee>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
                //Configure one-to-many relationship between User and Role
                builder.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            }
            );



            modleBuilder.Entity<Employee>(builder =>
            {


                builder.HasKey(e => e.UserId);


                //Configure many-to-many relationship between Employee and Skills


                //Configure many-to-many relationship between Employee and Certificates
                builder.HasMany(e => e.Certificates).WithMany(c => c.Employees)
                .UsingEntity<Dictionary<string,object>>("Employee_Certificates",
                j=>j.HasOne<Certificates>().WithMany().HasForeignKey("CertificateId").OnDelete(DeleteBehavior.Cascade),
                j=>j.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").OnDelete(DeleteBehavior.Cascade),
                j=>j.HasKey("EmployeeId","CertificateId")
                );

                //Configure one-to-many relationship between Employee and Requests
                builder.HasMany(e => e.Requests).WithOne(r => r.Employee).HasForeignKey(r => r.EmployeeId).OnDelete(DeleteBehavior.Cascade);

                //configure one-to-many relationship between Employee and  Roadmap 
                    builder.HasMany(e => e.Roadmaps).WithOne(r => r.Employee).HasForeignKey(r => r.EmployeeId);
            }
            );

            //configure one-to-many relationship between Roadmap and Steps

            modleBuilder.Entity<Roadmap>(builder =>
            { 
                builder.HasMany(r => r.Steps).WithOne(s => s.Roadmap).HasForeignKey(s => s.RoadmapId).OnDelete(DeleteBehavior.Cascade);
            });


 

            //configure one-to-many relationship between Certificates and Steps
            modleBuilder.Entity<Steps>(builder =>
            {
                builder.HasOne(c => c.Certificate).WithOne(c=>c.step).HasForeignKey<Steps>(s => s.CertificateId);
            }
            );


        }




    }
}
