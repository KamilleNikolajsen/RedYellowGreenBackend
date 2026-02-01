using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RedYellowGreenBackend.Dtos;
using RedYellowGreenBackend.Hubs;
using RedYellowGreenBackend.Models;
using RedYellowGreenBackend.Services;

namespace RedYellowGreenBackend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly ILogger<EquipmentController> _logger;
    private readonly IEquipmentService _equipmentService;
    private readonly IHubContext<EquipmentStateHub> _hubContext;
    
    public EquipmentController(ILogger<EquipmentController> logger, IEquipmentService equipmentService, IHubContext<EquipmentStateHub> hubContext)
    {
        _logger = logger;
        _equipmentService = equipmentService;
        _hubContext = hubContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EquipmentDto>>> GetAllEquipment()
    {
        var equipment = await _equipmentService.GetAllEquipmentAsync();
        return Ok(equipment);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EquipmentDto>> GetEquipmentById(Guid id)
    {
        var equipment = await _equipmentService.GetEquipmentByIdAsync(id);
        if (equipment == null)
        {
            return NotFound($"Equipment with ID {id} not found");
        }
        return Ok(equipment);
    }
    
    [HttpPut("{id}/state")]
    public async Task<ActionResult<EquipmentDto>> UpdateEquipmentState(Guid id, [FromBody] UpdateStateRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        if (!Enum.TryParse<EquipmentState>(request.State, true, out var newState))
        {
            return BadRequest($"Invalid state: {request.State}. Valid values are: Red, Yellow, Green");
        }

        var updatedEquipment = await _equipmentService.UpdateEquipmentStateAsync(id, newState, request.ChangedBy);
        if (updatedEquipment == null)
        {
            return NotFound($"Equipment with ID {id} not found");
        }
        
        await _hubContext.Clients.All.SendAsync("EquipmentStateUpdated", new
        {
            equipmentId = updatedEquipment.Id,
            equipmentName = updatedEquipment.Name,
            newState = updatedEquipment.CurrentState,
            timestamp = updatedEquipment.LastStateChange,
            changedBy = request.ChangedBy
        });
        
        _logger.LogInformation("Equipment state updated: {EquipmentId} to {NewState} by {ChangedBy}", id, newState, request.ChangedBy);
        
        return Ok(updatedEquipment);
    }

    [HttpGet("{id}/history")]
    public async Task<ActionResult<IEnumerable<StateHistoryDto>>> GetStateHistory(
        Guid id, 
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var history = await _equipmentService.GetStateHistoryAsync(id, from, to);
        return Ok(history);
    }
}