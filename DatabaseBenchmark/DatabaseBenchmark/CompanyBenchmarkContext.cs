using CompanyModels.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityDatabaseBenchmark {
    public class CompanyBenchmarkContext : DbContext {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DeptLocations> DeptLocations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorksOn> WorksOn { get; set; }
        public DbSet<Dependent> Dependents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EntityDatabase;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Department>()
                .HasKey(d => d.Dnumber);

            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Dname)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Ssn);

            modelBuilder.Entity<Employee>()
                .HasOne<Department>()
                .WithMany()
                .HasForeignKey(e => e.Dno);

            modelBuilder.Entity<Employee>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(e => e.Superssn);

            modelBuilder.Entity<DeptLocations>()
                .HasKey(dl => new { dl.Dnumber, dl.Dlocation });

            modelBuilder.Entity<DeptLocations>()
                .HasOne<Department>()
                .WithMany()
                .HasForeignKey(dl => dl.Dnumber);

            modelBuilder.Entity<Project>()
                .HasKey(p => p.Pnumber);

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Pname)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasOne<Department>()
                .WithMany()
                .HasForeignKey(p => p.Dnum);

            modelBuilder.Entity<WorksOn>()
                .HasKey(wo => new { wo.Essn, wo.Pno });

            modelBuilder.Entity<WorksOn>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(wo => wo.Essn);

            modelBuilder.Entity<WorksOn>()
                .HasOne<Project>()
                .WithMany()
                .HasForeignKey(wo => wo.Pno);

            modelBuilder.Entity<Dependent>()
                .HasKey(d => new { d.Essn, d.DependentName });

            modelBuilder.Entity<Dependent>()
                .HasOne<Employee>()
                .WithMany()
                .HasForeignKey(d => d.Essn);
        }
    }
}
