using System.Net;
using System.Net.Http.Json;
using RedYellowGreenBackend.Dtos;

namespace RedYellowGreenBackend.Tests;

public class EquipmentControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public EquipmentControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllEquipment_ReturnsOk()
    {
        // Arrange
        var requestUrl = "/api/equipment";
        
        // Act
        var response = await _client.GetAsync(requestUrl);

        // Assert
        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"API returned {response.StatusCode}: {errorContent}");
        }
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GetEquipmentById_ValidId()
    {
        // Arrange
        var requestUrl = "/api/equipment";
        
        // Act
        var getAllResponse = await _client.GetAsync(requestUrl);
        var equipmentList = await getAllResponse.Content.ReadFromJsonAsync<List<EquipmentDto>>();
        Assert.NotNull(equipmentList);
        var existingEquipment = equipmentList.First();
        
        var response = await _client.GetAsync($"/api/equipment/{existingEquipment.Id}");
        
        // Assert
        response.EnsureSuccessStatusCode();
        var equipment = await response.Content.ReadFromJsonAsync<EquipmentDto>();
        Assert.NotNull(equipment);
        Assert.Equal(existingEquipment.Id, equipment.Id);
        Assert.Equal(existingEquipment.Name, equipment.Name);
    }

    [Fact]
    public async Task UpdateEquipmentState_ValidRequest()
    {
        // Arrange
        var requestUrl = "/api/equipment";
        
        // Act
        var getAllEquipmentsResponse = await _client.GetAsync(requestUrl);
        var equipment = await getAllEquipmentsResponse.Content.ReadFromJsonAsync<List<EquipmentDto>>();
        Assert.NotNull(equipment);
        var firstEquipment = equipment.First();
        
        var updateRequest = new UpdateStateRequest
        {
            State = "Yellow",
            ChangedBy = "testworker"
        };
        
        var response = await _client.PutAsJsonAsync($"/api/equipment/{firstEquipment.Id}/state", updateRequest);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var updatedEquipment = await response.Content.ReadFromJsonAsync<EquipmentDto>();
        Assert.NotNull(updatedEquipment);
        Assert.Equal("Yellow", updatedEquipment.CurrentState);
    }

    [Fact]
    public async Task GetHistory()
    {
        // Arrange
        var requestUrl = "/api/equipment";
        
        // Act
        var getAllEquipmentsResponse = await _client.GetAsync(requestUrl);
        var equipment = await getAllEquipmentsResponse.Content.ReadFromJsonAsync<List<EquipmentDto>>();
        
        // Assert
        Assert.NotNull(equipment);
        var firstEquipment = equipment.First();
        var response = await _client.GetAsync($"/api/equipment/{firstEquipment.Id}/history");
        
        response.EnsureSuccessStatusCode();
        var history = await response.Content.ReadFromJsonAsync<List<StateHistoryDto>>();
        Assert.NotNull(history);
    }
}