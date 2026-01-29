using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RedYellowGreenBackend.Data;
using RedYellowGreenBackend.Models;

namespace RedYellowGreenBackend.Tests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<TestDb>));
            if (descriptor != null)
                services.Remove(descriptor);

            // Create in-memory SQLite connection
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Add new DbContext using in-memory SQLite
            services.AddDbContext<TestDb>(options =>
            {
                options.UseSqlite(connection);
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create the database and seed data
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<TestDb>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            }
        });
    }

    private void SeedTestData(TestDb db)
    {
        // Add sample Equipment
        db.Equipment.Add(new Equipment { Name = "Test Equipment 1" });
        db.Equipment.Add(new Equipment { Name = "Test Equipment 2" });
        db.SaveChanges();
    }
}

