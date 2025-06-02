using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();