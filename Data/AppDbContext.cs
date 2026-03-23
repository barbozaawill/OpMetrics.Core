using Microsoft.EntityFrameworkCore;
using OpMetrics.Core.Entities;

namespace OpMetrics.Core.Data;

public class AppDbContext : DbContext
{

    public AppDbContext (DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<Producao> Producoes { get; set; }
    public DbSet<Qualidade> Qualidades { get; set; }
    public DbSet<Oee> Oees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Producao>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Linha).IsRequired().HasMaxLength(100);
            e.Property(x => x.Turno).IsRequired().HasMaxLength(20);
            e.Ignore(x => x.PercentualAtingido);
        });
        modelBuilder.Entity<Qualidade>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Linha).IsRequired().HasMaxLength(100);
            e.Property(x => x.Turno).IsRequired().HasMaxLength(20);
            e.Ignore(x => x.IndiceQualidade);
        });
        modelBuilder.Entity<Oee>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Linha).IsRequired().HasMaxLength(100);
            e.Property(x => x.Turno).IsRequired().HasMaxLength(20);
            e.Ignore(x => x.Disponibilidade);
            e.Ignore(x => x.Performance);
            e.Ignore(x => x.IndiceQualidade);
            e.Ignore(x => x.OeePercentual);
        });
    }
}
