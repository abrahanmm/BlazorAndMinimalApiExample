using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Peman.Model;

namespace Peman.Api
{
    public class PemanContexDb : DbContext
    {
        public PemanContexDb()
        {
            Stations = Set<WheaterStation>();
        }

        public PemanContexDb(DbContextOptions<PemanContexDb> options)
            : base(options)
        {
            Stations = Set<WheaterStation>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WheaterStationEntityTypeConfiguration());
        }

        public DbSet<WheaterStation> Stations { get; set; }
    }
}


internal class WheaterStationEntityTypeConfiguration : IEntityTypeConfiguration<WheaterStation>
{
    public void Configure(EntityTypeBuilder<WheaterStation> builder)
    {
        builder.HasKey(x => x.Indicativo);
        builder.ToTable("Stations");
    }
}