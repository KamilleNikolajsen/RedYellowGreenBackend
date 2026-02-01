namespace RedYellowGreenBackend.Models;

public class StateHistory
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public EquipmentState State { get; set; }
    public DateTime Timestamp { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}