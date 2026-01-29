using Microsoft.EntityFrameworkCore;
using RedYellowGreenBackend.Models;

namespace RedYellowGreenBackend.Data;

public class TestDb : DbContext
{
    public TestDb(DbContextOptions<TestDb> options) : base(options)
    {
    }
    
    public DbSet<Equipment> Equipment { get; set; }
    
    public DbSet<StateHistory> StateHistory { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<StateHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EquipmentId);
            entity.HasIndex(e => e.Timestamp);
            entity.Property(e => e.ChangedBy).IsRequired().HasMaxLength(256);
        });
    }
}