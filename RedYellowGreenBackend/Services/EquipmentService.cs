using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RedYellowGreenBackend.Data;
using RedYellowGreenBackend.Dtos;
using RedYellowGreenBackend.Models;

namespace RedYellowGreenBackend.Services;


public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync();
    
    Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id);
    
    Task<EquipmentDto?> UpdateEquipmentStateAsync(Guid id, EquipmentState newState, string changedBy);
    
    Task<IEnumerable<StateHistoryDto>> GetStateHistoryAsync(Guid equipmentId, DateTime? from, DateTime? to);
}

public class EquipmentService : IEquipmentService
{
    private readonly TestDb _testDb;
    
    public EquipmentService(TestDb testDb)
    {
        _testDb = testDb;
    }
    
    public async Task<IEnumerable<EquipmentDto>> GetAllEquipmentAsync()
    {
        var equipment = await _testDb.Equipment.ToListAsync();
        return equipment.Select(MapToDto);
    }

    public async Task<EquipmentDto?> GetEquipmentByIdAsync(Guid id)
    {
        var equipment = await _testDb.Equipment.FindAsync(id);
        if (equipment == null)
        {
            return null;
        }
        return MapToDto(equipment);
    }

    public async Task<EquipmentDto?> UpdateEquipmentStateAsync(Guid id, EquipmentState newState, string changedBy)
    {
        var equipment =  await _testDb.Equipment.FindAsync(id);
        if (equipment == null)
        {
            return null;
        }

        equipment.CurrentState = newState;
        equipment.LastStateChange = DateTime.UtcNow;
        
        var history = new StateHistory
        {
            Id = Guid.NewGuid(),
            EquipmentId = id,
            State = newState,
            Timestamp = DateTime.UtcNow,
            ChangedBy = changedBy
        };
        
        _testDb.StateHistory.Add(history);
        await _testDb.SaveChangesAsync();
        
        return MapToDto(equipment);
    }

    public async Task<IEnumerable<StateHistoryDto>> GetStateHistoryAsync(Guid equipmentId, DateTime? from, DateTime? to)
    {
        var query = _testDb.StateHistory.Where(h => h.EquipmentId == equipmentId);
        
        if (from.HasValue)
        {
            query = query.Where(h => h.Timestamp >= from.Value);
        }
        
        if (to.HasValue)
        {
            query = query.Where(h => h.Timestamp <= to.Value);
        }
        
        var history = await query.OrderByDescending(h => h.Timestamp).ToListAsync();
        return history.Select(MapHistoryToDto);
    }
    
    private EquipmentDto MapToDto(Equipment equipment)
    {
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            CurrentState = equipment.CurrentState.ToString(),
            LastStateChange = equipment.LastStateChange
        };
    }
    private static StateHistoryDto MapHistoryToDto(StateHistory history)
    {
        return new StateHistoryDto
        {
            Id = history.Id,
            EquipmentId = history.EquipmentId,
            State = history.State.ToString(),
            Timestamp = history.Timestamp,
            ChangedBy = history.ChangedBy
        };
    }
}