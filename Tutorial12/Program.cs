using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.Repositories;
using Tutorial12.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<ITripsRepository, TripsRepository>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();