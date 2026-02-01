namespace RedYellowGreenBackend.Dtos;

public class StateHistoryDto
{
    public Guid Id { get; set; }
    
    public Guid EquipmentId { get; set; }
    
    public string State { get; set; } = string.Empty;
    
    public DateTime Timestamp { get; set; }
    
    public string ChangedBy { get; set; } = string.Empty;
}