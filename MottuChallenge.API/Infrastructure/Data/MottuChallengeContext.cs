using Microsoft.EntityFrameworkCore;
using MottuChallenge.API.Domain.Models;

namespace MottuChallenge.API.Infrastructure.Data;

public class MottuChallengeContext : DbContext
{
    public MottuChallengeContext(DbContextOptions<MottuChallengeContext> options) : base(options)
    { }

    public DbSet<User> User { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Patio> Patios { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Moto> Motos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Patio>()
            .HasOne(p => p.Endereco)
            .WithOne()
            .HasForeignKey<Patio>(p => p.EnderecoId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Patio>()
            .HasOne(p => p.User)
            .WithMany(u => u.Patios)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Area>()
            .HasOne(a => a.Patio)
            .WithMany(p => p.Areas)
            .HasForeignKey(a => a.PatioId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Area>()
            .HasMany(a => a.Motos)
            .WithOne(m => m.Area)
            .HasForeignKey(m => m.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
