using System.ComponentModel.DataAnnotations;

namespace RedYellowGreenBackend.Dtos;

public class UpdateStateRequest
{
    [Required]
    public string State { get; set; } = string.Empty;
    
    [Required]
    public string ChangedBy { get; set; } = string.Empty;
}