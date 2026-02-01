namespace RedYellowGreenBackend.Dtos;

public class EquipmentDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string CurrentState { get; set; } = string.Empty;
    
    public DateTime LastStateChange { get; set; }
}