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
            modelBuilder.Entity<Parent>()
                .HasMany(p => p.Children)
                .WithOne(c => c.Parent)         // <-- link back to Parent
                .HasForeignKey(c => c.ParentId) // <-- define FK
                .OnDelete(DeleteBehavior.Cascade); // <-- enable cascade delete

            base.OnModelCreating(modelBuilder);
        }


        public void EnsureDatabaseCreated() {
            Database.EnsureCreated();
        }
    }
}