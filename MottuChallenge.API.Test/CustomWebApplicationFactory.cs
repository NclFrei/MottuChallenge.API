using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MottuChallenge.API.Infrastructure.Data;

namespace MottuChallenge.API.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing DbContext registration
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MottuChallengeContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Add in-memory database for tests
            services.AddDbContext<MottuChallengeContext>(options =>
            {
                options.UseInMemoryDatabase("TestDb");
            });

            // Build the service provider and ensure the database is created
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<MottuChallengeContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }
        });

        base.ConfigureWebHost(builder);
    }
}
