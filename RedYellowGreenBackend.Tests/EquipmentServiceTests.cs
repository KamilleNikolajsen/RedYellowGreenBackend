using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RedYellowGreenBackend.Data;
using RedYellowGreenBackend.Models;
using RedYellowGreenBackend.Services;

namespace RedYellowGreenBackend.Tests;

public class EquipmentServiceTests
{
private TestDb GetInMemoryTestDb()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<TestDb>()
            .UseSqlite(connection)
            .Options;
        var db = new TestDb(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task GetAllEquipmentAsync()
    {
        // Arrange
        var db = GetInMemoryTestDb();
        var equipment = new Equipment
        {
            Id = Guid.NewGuid(),
            Name = "Test Printer",
            CurrentState = EquipmentState.Green,
            LastStateChange = DateTime.UtcNow
        };
        db.Equipment.Add(equipment);
        await db.SaveChangesAsync();
        
        var service = new EquipmentService(db);

        // Act
        var result = await service.GetAllEquipmentAsync();
        
        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Test Printer", result.First().Name);
    }

    [Fact]
    public async Task UpdateEquipmentStateAsync()
    {
        // Arrange
        var db = GetInMemoryTestDb();
        var equipmentId = Guid.NewGuid();
        var equipment = new Equipment
        {
            Id = equipmentId,
            Name = "Test Printer",
            CurrentState = EquipmentState.Green,
            LastStateChange = DateTime.UtcNow
        };
        db.Equipment.Add(equipment);
        await db.SaveChangesAsync();
        
        var service = new EquipmentService(db);
        
        // Act
        var result = await service.UpdateEquipmentStateAsync(equipmentId, EquipmentState.Red, "worker1");
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal("Red", result.CurrentState);

        var history = await db.StateHistory.Where(h => h.EquipmentId == equipmentId).ToListAsync();
        Assert.Single(history);
        Assert.Equal(EquipmentState.Red, history[0].State);
        Assert.Equal("worker1", history[0].ChangedBy);
    }
}