using Microsoft.EntityFrameworkCore;
using Zad12_s30766;
using Zad12_s30766.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddDbContext<Zad122Context>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
);

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();