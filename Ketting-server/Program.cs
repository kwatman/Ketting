using System.Net;
using KetKoin;
using Ketting;
using Ketting_server.Services;
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
    block.Data.Add(transaction);
    block.Timestamp = new DateTime(2022, 11, 2);
    block.PublicKey = app.Configuration["GenesisKey"];
    block.Hash = "genesis";
    block.Signature = "genesis";
    KetKoinChain.BlockChain.Add(block);    
}



app.UseHttpsRedirection();

app.MapControllers();
app.Run();
