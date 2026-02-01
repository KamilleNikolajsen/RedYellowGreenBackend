using Microsoft.AspNetCore.SignalR;

namespace RedYellowGreenBackend.Hubs;

public class EquipmentStateHub : Hub
{
    private readonly ILogger<EquipmentStateHub> _logger;
    
    public  EquipmentStateHub(ILogger<EquipmentStateHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task BroadcastEquipmentStateChanged(object onStageChanged)
    {
        await Clients.All.SendAsync("EquipmentStateChanged", onStageChanged);
    }
}