using Ketting_server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

DiscoveryService discoveryService = new DiscoveryService();
builder.Services.AddSingleton(discoveryService);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

discoveryService.DiscoverConnections();
    
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
