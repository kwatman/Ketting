using System.Net;
using Ketting_server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DiscoveryService discoveryService = new DiscoveryService();

builder.Services.AddSingleton(discoveryService);
builder.Services.AddSingleton<BlockChainService>();
builder.Services.AddSingleton<BroadcastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Console.WriteLine(Environment.GetEnvironmentVariable("INITC"));
app.UseSwagger();
app.UseSwaggerUI();

ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
{
    return true;
};

if (Environment.GetEnvironmentVariable("initial_conection") != null)
{
    discoveryService.DiscoverConnections(Environment.GetEnvironmentVariable("initial_conection"));
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
