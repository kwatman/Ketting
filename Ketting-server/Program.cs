using System.Net;
using KetKoin;
using Ketting;
using Ketting_server.Services;
using Ketting_server.Workers;
using Type = KetKoin.Type;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

DiscoveryService discoveryService = new DiscoveryService();
BlockChainService blockChainService = new BlockChainService(builder.Configuration);
builder.Services.AddSingleton(discoveryService);
builder.Services.AddSingleton(blockChainService);
builder.Services.AddSingleton<BroadcastService>();
builder.Services.AddSingleton<TransactionService>();

builder.Services.AddHostedService<ConnectionService>();
builder.Services.AddHostedService<TransactionWorker>();

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
{
    return true;
};

if (Environment.GetEnvironmentVariable("initial_conection") != null)
{
    discoveryService.DiscoverConnections(Environment.GetEnvironmentVariable("initial_conection"));
    blockChainService.GetNewChain(Environment.GetEnvironmentVariable("initial_conection"));
}
else
{
    //Setup genesis block
    Block block = new Block();
    block.PrevHash = "genesis";
    block.Version = 1;

    Console.WriteLine(app.Configuration["GenesisKey"]);
    Byte[] bytes = new Byte[0];
    Transaction transaction = new Transaction(50000,bytes, Convert.FromBase64String(app.Configuration["GenesisKey"]),1,"genesis",Type.Transaction);
    Transaction stake = new Transaction(1000,Convert.FromBase64String(app.Configuration["GenesisKey"]), bytes,1,"genesis",Type.Stake);
    block.Data.Add(transaction);
    block.Data.Add(stake);
    block.Timestamp = new DateTime(2022, 11, 2);
    block.PublicKey = app.Configuration["GenesisKey"];
    block.Hash = "genesis";
    block.Signature = "genesis";
    block.AmountOfStakers = 0;
    KetKoinChain.BlockChain.Add(block);    
}



app.UseHttpsRedirection();

app.MapControllers();
app.Run();
