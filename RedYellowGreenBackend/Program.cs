using Microsoft.EntityFrameworkCore;
using RedYellowGreenBackend.Data;
using RedYellowGreenBackend.Hubs;
using RedYellowGreenBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173/")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddDbContext<TestDb>(options =>
    options.UseSqlite("Data Source=./TestDatabase/RedYellowGreenBackend.db"));

var app = builder.Build();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.MapHub<EquipmentStateHub>("/hubs/states");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TestDb>();
    db.Database.EnsureCreated();
    TestData.IngestTestData(db);
}

app.Run("http://localhost:5001");

// Make Program accessible for testing
public partial class Program { }