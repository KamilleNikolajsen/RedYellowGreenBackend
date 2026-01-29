using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RedYellowGreenBackend.Dtos;
using RedYellowGreenBackend.Models;
using RedYellowGreenBackend.Services;

namespace RedYellowGreenBackend.Controllers;


[ApiController]
[Route("api/[controller]")]
public class EquipmentController : ControllerBase
{
    private readonly ILogger<EquipmentController> _logger;
    private readonly IEquipmentService _equipmentService;
    
    public EquipmentController(ILogger<EquipmentController> logger, IEquipmentService equipmentService)
    {
        _logger = logger;
        _equipmentService = equipmentService;
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