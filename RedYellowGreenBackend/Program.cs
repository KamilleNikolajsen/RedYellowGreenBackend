using Microsoft.EntityFrameworkCore;
using RedYellowGreenBackend.Data;
using RedYellowGreenBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add controller support
builder.Services.AddControllers();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddDbContext<TestDb>(options =>
    options.UseSqlite("Data Source=./TestDatabase/RedYellowGreenBackend.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TestDb>();
    db.Database.EnsureCreated();
    TestData.IngestTestData(db);
}

app.UseHttpsRedirection();

// Add endpoint routing for controllers
app.MapControllers();

app.Run();

// Make Program accessible for testing
public partial class Program { }