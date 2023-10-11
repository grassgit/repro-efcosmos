using Microsoft.EntityFrameworkCore;

namespace Repro_Cosmos.Model
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<DataType> Data { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DataType>()
                .ToContainer("Test")
                .HasKey(x => x.Id);
        }
    }
}
