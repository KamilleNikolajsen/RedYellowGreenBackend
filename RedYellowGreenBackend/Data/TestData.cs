using RedYellowGreenBackend.Models;

namespace RedYellowGreenBackend.Data;

public static class TestData
{
    public static void IngestTestData(TestDb testDb)
    {
        if (testDb.Equipment.Any())
        {
            return;
        }

        var equipment = new List<Equipment>
        {
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Printer A", CurrentState = EquipmentState.Green, LastStateChange = DateTime.UtcNow },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111112"), Name = "Printer B", CurrentState = EquipmentState.Yellow, LastStateChange = DateTime.UtcNow },
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111113"), Name = "Printer C", CurrentState = EquipmentState.Red, LastStateChange = DateTime.UtcNow },
        };
        
        testDb.Equipment.AddRange(equipment);
        testDb.SaveChanges();
        

        var random = new Random();
        var stateValues = Enum.GetValues<EquipmentState>();
        var workers = new[] { "worker1", "worker2", "supervisor1" };
        var history = GenerateStateHistory(equipment, random, stateValues, workers);
        
        testDb.StateHistory.AddRange(history);
        testDb.SaveChanges();
    }

    private static List<StateHistory> GenerateStateHistory(List<Equipment> equipment, Random random, EquipmentState[] stateValues, string[] workers)
    {
        var history = new List<StateHistory>();
        foreach (var e in equipment)
        {
            for (int i = 0; i < 3; i++)
            {
                var daysAgo = random.Next(1, 8);
                var hoursOffset = random.Next(0, 24);
                var minutesOffset = random.Next(0, 60);

                history.Add(new StateHistory
                {
                    Id = Guid.NewGuid(),
                    EquipmentId =  e.Id,
                    State = stateValues[random.Next(stateValues.Length)],
                    Timestamp = DateTime.UtcNow.AddDays(-daysAgo).AddHours(-hoursOffset).AddMinutes(-minutesOffset),
                    ChangedBy = workers[random.Next(workers.Length)]
                });
            }
        }
        return history;
    }
}