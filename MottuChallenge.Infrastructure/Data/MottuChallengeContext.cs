using Microsoft.EntityFrameworkCore;
using MottuChallenge.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MottuChallenge.Infrastructure.Data;

public class MottuChallengeContext : DbContext
{
    public MottuChallengeContext(DbContextOptions<MottuChallengeContext> options) : base(options)
    { }

    public DbSet<User> User { get; set; }
    public DbSet<Endereco> Enderecos { get; set; }
    public DbSet<Patio> Patios { get; set; }

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

        base.OnModelCreating(modelBuilder);
    }
}
