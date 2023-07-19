using RabbitApi.Models;
using RabbitApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddControllers();


var app = builder.Build();

app.UseAuthorization();

// Map attribute-routed API controllers
app.MapControllers();

app.Run();
