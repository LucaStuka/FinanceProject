using FinanceProjectDLL;
using Microsoft.EntityFrameworkCore;

namespace DatabaseBusinessDLL
{
    public partial class DatabaseContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public DatabaseContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=FinanceProjectDB;Trusted_Connection=True;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}