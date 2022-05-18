namespace Adasit.Bootstrap.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using DomainEntity = Domain.Entity;

public class PrincipalContext : DbContext
{
    public PrincipalContext(DbContextOptions<PrincipalContext> options) : base(options)
    {

    }

    public DbSet<DomainEntity.Configuration> Configuration => Set<DomainEntity.Configuration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<DomainEntity.Configuration>((v) => {
                v.HasKey(k => k.Id);
                v.Property(k => k.Name).HasMaxLength(100);
                v.Property(k => k.Value).HasMaxLength(1000);
                v.Property(k => k.Description).HasMaxLength(1000);
                v.Property(k => k.StartDate);
                v.Property(k => k.FinalDate);
                v.Property(k => k.CreatedAt);
                v.Property(k => k.LastUpdateAt);
                v.Property(k => k.DeletedAt);
            });
    }
}