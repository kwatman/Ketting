using KetKoin;
using Ketting;
using Ketting_server.Dto;
using Ketting_server.Services;

namespace Ketting_server.Workers;

public class MintingWorker: BackgroundService
{
    private readonly ILogger<MintingWorker> logger;
    private TransactionService TransactionService { get; set; }
    public BlockChainService BlockChainService { get; set; }
    public BroadcastService BroadcastService { get; set; }
    public DiscoveryService DiscoveryService { get; set; }

    public MintingWorker(ILogger<MintingWorker> _logger,TransactionService transactionService,BlockChainService blockChainService, BroadcastService broadcastService, DiscoveryService discoveryService)
    {
        logger = _logger;
        TransactionService = transactionService;
        BlockChainService = blockChainService;
        BroadcastService = broadcastService;
        DiscoveryService = discoveryService;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            if (KetKoinChain.TransactionPool.Count >= 5)
            {
                Console.WriteLine("Starting minting proccess");
                Console.WriteLine("Highest stake found: " + Convert.ToBase64String(Stake.GetHighestStake()));
                if (Stake.GetHighestStake().SequenceEqual(BlockChainService.KetKoinChain.NodeKeys.PublicKey))
                {
                    Console.WriteLine("Im the leader! Minting new block");
                    Block block = BlockChainService.KetKoinChain.MintBlock();
                    BroadcastService.BroadCastBlockMint(block, DiscoveryService.connections);
                }
                else
                {
                    Console.WriteLine("Im not the leader.");
                }
            }
        }
    }
}