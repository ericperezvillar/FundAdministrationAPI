using FundAdmin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FundAdmin.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Fund> Funds => Set<Fund>();
    public DbSet<Investor> Investors => Set<Investor>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Fund>()
            .HasMany(f => f.Investors)
            .WithOne(i => i.Fund!)
            .HasForeignKey(i => i.FundId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Investor>()
            .HasMany(i => i.Transactions)
            .WithOne(t => t.Investor!)
            .HasForeignKey(t => t.InvestorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);
    }
}
