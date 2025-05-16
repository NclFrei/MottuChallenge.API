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
    {
    
    }

    public DbSet<User> User { get; set; }
    
}
