using Benchmarking.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityBenchmark.Entity {
    public class EntityContext : DbContext {
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            var connectionString = "Server=mssql;Database=EntityDatabase;User Id=sa;Password=Rootr00t;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connectionString, options => options.EnableRetryOnFailure());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            // Configure Parent-Child relationship
            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Children);

            base.OnModelCreating(modelBuilder);
        }

        public void EnsureDatabaseCreated() {
            Database.EnsureCreated();
        }
    }
}