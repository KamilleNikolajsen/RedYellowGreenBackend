namespace RedYellowGreenBackend.Models;

public class Equipment
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public EquipmentState CurrentState { get; set; }
    public DateTime LastStateChange { get; set; }
}