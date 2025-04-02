using EntityBenchmark.CompanyModels.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityBenchmark {
    public class CompanyBenchmarkContext : DbContext {
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<DeptLocations> DeptLocations { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorksOn> WorksOn { get; set; }
        public DbSet<Dependent> Dependents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connectionString = "Server=mssql;Database=EntityDatabase;User Id=sa;Password=Rootr00t;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Department
            modelBuilder.Entity<Department>()
                .HasKey(d => d.Dnumber);
            modelBuilder.Entity<Department>()
                .Property(d => d.Dnumber)
                .ValueGeneratedOnAdd(); // Configure Dnumber as an identity column
            modelBuilder.Entity<Department>()
                .HasIndex(d => d.Dname)
                .IsUnique();

            // Employee
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Ssn);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.Dno);
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Supervisor)
                .WithMany()
                .HasForeignKey(e => e.Superssn)
                .OnDelete(DeleteBehavior.NoAction);

            // Department Locations
            modelBuilder.Entity<DeptLocations>()
                .HasKey(dl => new { dl.Dnumber, dl.Dlocation });
            modelBuilder.Entity<DeptLocations>()
                .HasOne(dl => dl.Department)
                .WithMany(d => d.Locations)
                .HasForeignKey(dl => dl.Dnumber);

            // Project
            modelBuilder.Entity<Project>()
                .HasKey(p => p.Pnumber);
            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Pname)
                .IsUnique();
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Department)
                .WithMany(d => d.Projects)
                .HasForeignKey(p => p.Dnum);

            // WorksOn
            modelBuilder.Entity<WorksOn>()
                .HasKey(wo => new { wo.Essn, wo.Pno });
            modelBuilder.Entity<WorksOn>()
                .HasOne(wo => wo.Employee)
                .WithMany(e => e.WorksOns)
                .HasForeignKey(wo => wo.Essn)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<WorksOn>()
                .HasOne(wo => wo.Project)
                .WithMany(p => p.WorksOns)
                .HasForeignKey(wo => wo.Pno)
                .OnDelete(DeleteBehavior.NoAction);

            // Dependent
            modelBuilder.Entity<Dependent>()
                .HasKey(d => new { d.Essn, d.DependentName });
            modelBuilder.Entity<Dependent>()
                .HasOne(d => d.Employee)
                .WithMany(e => e.Dependents)
                .HasForeignKey(d => d.Essn);
        }



        public void EnsureDatabaseCreated() {
            this.Database.EnsureCreated();
        }
    }
}
